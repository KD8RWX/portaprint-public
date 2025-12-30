import ctypes
import os
import sys
import time
import logging
from ctypes import *

# Configure logging
logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
logger = logging.getLogger("KojakWrapper")

# --- Constants & Enums (from IBScanUltimateApi_defs.h) ---
IBSU_STATUS_OK = 0
IBSU_ERR_INVALID_PARAM_VALUE = -1
IBSU_ERR_MEM_ALLOC = -2
IBSU_ERR_NOT_SUPPORT = -3
IBSU_ERR_HEX_DUMP_FAILED = -4
IBSU_ERR_DEVICE_BUSY = -5
IBSU_ERR_DEVICE_NOT_FOUND = -6
IBSU_ERR_DEVICE_NOT_INITIALIZED = -7
IBSU_ERR_DEVICE_INVALID_STATE = -8
IBSU_ERR_DEVICE_IO = -9
IBSU_ERR_DEVICE_NOT_AUTHORIZED = -207

# Image Types (Kojak is usually valid for these)
ENUM_IBSU_TYPE_NONE = 0
ENUM_IBSU_ROLL_SINGLE_FINGER = 1
ENUM_IBSU_FLAT_SINGLE_FINGER = 2
ENUM_IBSU_FLAT_TWO_FINGERS = 3
ENUM_IBSU_FLAT_FOUR_FINGERS = 4
ENUM_IBSU_FLAT_THREE_FINGERS = 5

# Image Resolutions
IBSU_IMG_RES_500 = 500

# Capture Options
IBSU_OPTION_AUTO_CONTRAST = 1
IBSU_OPTION_AUTO_CAPTURE = 2
IBSU_OPTION_IGNORE_FINGER_COUNT = 4

# --- Event Enums ---
ENUM_IBSU_ESSENTIAL_EVENT_RESULT_IMAGE_EX = 12

# --- Structures ---
class IBSU_ImageData(Structure):
    _fields_ = [
        ("Buffer", c_void_p),
        ("Width", c_uint),
        ("Height", c_uint),
        ("ResolutionX", c_double),
        ("ResolutionY", c_double),
        ("FrameTime", c_double),
        ("Pitch", c_int),
        ("BitsPerPixel", c_ubyte),
        ("Format", c_int),
        ("IsFinal", c_int),
        ("ProcessThres", c_uint)
    ]

class IBSU_SegmentPosition(Structure):
    _fields_ = [
        ("x1", c_short), ("y1", c_short),
        ("x2", c_short), ("y2", c_short),
        ("x3", c_short), ("y3", c_short),
        ("x4", c_short), ("y4", c_short)
    ]

# Callback Type Definition
# typedef void (CALLBACK *IBSU_CallbackResultImageEx)(...)
CALLBACK_RESULT_IMAGE_EX_TYPE = CFUNCTYPE(None, 
                                          c_int,          # deviceHandle
                                          c_void_p,       # pContext
                                          c_int,          # imageStatus
                                          IBSU_ImageData, # image (passed by value)
                                          c_int,          # imageType
                                          c_int,          # detectedFingerCount
                                          c_int,          # segmentImageArrayCount
                                          POINTER(IBSU_ImageData), # pSegmentImageArray
                                          POINTER(IBSU_SegmentPosition) # pSegmentPositionArray
                                          )

# --- Helper to load library ---
def load_library():
    cwd = os.getcwd()
    logger.info(f"Current working directory: {cwd}")
    
    # 1. Search recursively for the library file
    lib_name = "libIBScanUltimate.so"
    found_path = None
    
    # ... (search logic remains per previous edit, abbreviated for brevity in this replacement block if possible, but I will include it all to be safe)
    
    logger.info(f"Searching for {lib_name}...")
    for root, dirs, files in os.walk(cwd):
        if lib_name in files:
            found_path = os.path.join(root, lib_name)
            logger.info(f"Found library at: {found_path}")
            break
            
    if not found_path:
        standards = ["/usr/lib/libIBScanUltimate.so", "/usr/local/lib/libIBScanUltimate.so"]
        for p in standards:
            if os.path.exists(p):
                found_path = p
                break
    
    if not found_path:
        logger.error(f"Could not find {lib_name} in {cwd} or standard paths.")
        return None

    # 2. Try to load ALL dependencies (Explicitly)
    lib_dir = os.path.dirname(found_path)
    dependencies = ["libusb-1.0.so", "libuvc.so", "libAKXUS.so.2"]
    
    for dep_name in dependencies:
        dep_path = os.path.join(lib_dir, dep_name)
        if not os.path.exists(dep_path):
             if "libAKXUS" in dep_name:
                 for f in os.listdir(lib_dir):
                     if f.startswith("libAKXUS.so"):
                         dep_path = os.path.join(lib_dir, f)
                         break
        if os.path.exists(dep_path):
            try:
                CDLL(dep_path, mode=RTLD_GLOBAL)
            except Exception as e:
                logger.warning(f"Failed to load dependency {dep_name}: {e}")

    # 3. Load the main library
    try:
        logger.info(f"Loading IBScanUltimate from: {found_path}")
        lib = CDLL(found_path, mode=RTLD_GLOBAL)
        logger.info("Library loaded successfully!")
        return lib
    except OSError as e:
        logger.error(f"Failed to load library: {e}")
        return None

# --- Wrapper Class ---
class KojakScanner:
    def __init__(self):
        self.lib = load_library()
        if not self.lib:
            raise Exception("Failed to load IBScanUltimate library")
        
        self.device_handle = c_int(-1)
        self.is_open = False
        self.image_callback = None # Keep reference to prevent GC
        self.captured_image_data = None
        
        # Define function signatures
        self.lib.IBSU_GetDeviceCount.argtypes = [POINTER(c_int)]
        self.lib.IBSU_GetDeviceCount.restype = c_int
        
        self.lib.IBSU_OpenDevice.argtypes = [c_int, POINTER(c_int)]
        self.lib.IBSU_OpenDevice.restype = c_int
        
        self.lib.IBSU_CloseDevice.argtypes = [c_int]
        self.lib.IBSU_CloseDevice.restype = c_int
        
        self.lib.IBSU_BeginCaptureImage.argtypes = [c_int, c_int, c_int, c_int]
        self.lib.IBSU_BeginCaptureImage.restype = c_int
        
        self.lib.IBSU_CancelCaptureImage.argtypes = [c_int]
        self.lib.IBSU_CancelCaptureImage.restype = c_int
        
        self.lib.IBSU_IsCaptureActive.argtypes = [c_int, POINTER(c_int)]
        self.lib.IBSU_IsCaptureActive.restype = c_int
        
        self.lib.IBSU_RegisterCallbacks.argtypes = [c_int, c_int, c_void_p, c_void_p]
        self.lib.IBSU_RegisterCallbacks.restype = c_int

    # ... (get_device_count, open, close remain similar) ...
    def get_device_count(self):
        count = c_int(0)
        res = self.lib.IBSU_GetDeviceCount(byref(count))
        if res < 0: return 0
        return count.value

    def open(self, index=0):
        if self.is_open: return True
        handle = c_int(0)
        logger.info(f"Opening device index {index}...")
        res = self.lib.IBSU_OpenDevice(index, byref(handle))
        if res == IBSU_STATUS_OK:
            self.device_handle = handle
            self.is_open = True
            self.register_callback() # Register callback immediately
            logger.info(f"Device opened! Handle: {handle.value}")
            return True
        else:
            logger.error(f"OpenDevice failed: {res}")
            return False

    def close(self):
        if self.is_open:
            logger.info("Closing device...")
            try:
                self.lib.IBSU_CloseDevice(self.device_handle)
            except: pass
            self.is_open = False

    # Helper to generate BMP bytes in memory
    def get_bmp_bytes(self, data, width, height):
        file_size = 14 + 40 + 1024 + len(data)
        offset = 14 + 40 + 1024
        
        header = b'BM' + file_size.to_bytes(4, 'little') + (0).to_bytes(4, 'little') + offset.to_bytes(4, 'little')
        info = (40).to_bytes(4, 'little') + width.to_bytes(4, 'little') + height.to_bytes(4, 'little') + \
               (1).to_bytes(2, 'little') + (8).to_bytes(2, 'little') + (0).to_bytes(4, 'little') + \
               len(data).to_bytes(4, 'little') + (2835).to_bytes(4, 'little') * 2 + (0).to_bytes(4, 'little') * 2
        
        palette = b''.join([bytes([i, i, i, 0]) for i in range(256)])
        return header + info + palette + data

    def save_bmp(self, filename, data, width, height):
        bmp_data = self.get_bmp_bytes(data, width, height)
        with open(filename, 'wb') as f:
            f.write(bmp_data)
            
    def on_image_callback(self, deviceHandle, pContext, imageStatus, image, imageType, fingerCount, segmentCount, pSegmentArray, pSegmentPos):
        if imageStatus >= 0: # IBSU_STATUS_OK
            if image.IsFinal:
                logger.info(f"FINAL IMAGE RECEIVED! Size: {image.Width}x{image.Height}, Pitch: {image.Pitch}")
                try:
                    pitch = abs(image.Pitch)
                    buffer_size = image.Height * pitch
                    
                    if buffer_size > 0 and image.Buffer:
                        data = string_at(image.Buffer, buffer_size)
                        self.captured_image_data = data
                        
                        # Store BMP data for external use
                        self.last_bmp_data = self.get_bmp_bytes(data, image.Width, image.Height)
                        
                        # Save for debug
                        self.save_bmp(f"capture_{int(time.time())}.bmp", data, image.Width, image.Height)
                        
                except Exception as e:
                    logger.error(f"Error processing image: {e}")
            else:
                pass
        else:
            logger.warning(f"Callback Error Status: {imageStatus}")

    def register_callback(self):
        # Create callback wrapper
        self.image_callback = CALLBACK_RESULT_IMAGE_EX_TYPE(self.on_image_callback)
        res = self.lib.IBSU_RegisterCallbacks(self.device_handle, 
                                              ENUM_IBSU_ESSENTIAL_EVENT_RESULT_IMAGE_EX, 
                                              self.image_callback, 
                                              None)
        if res != IBSU_STATUS_OK:
            logger.error(f"Failed to register callback: {res}")
        else:
            logger.info("Callback registered successfully")

    def start_capture(self, image_type=ENUM_IBSU_FLAT_SINGLE_FINGER):
        if not self.is_open: return False
        opts = IBSU_OPTION_AUTO_CONTRAST | IBSU_OPTION_AUTO_CAPTURE
        logger.info(f"Starting capture with Type: {image_type}")
        res = self.lib.IBSU_BeginCaptureImage(self.device_handle, image_type, IBSU_IMG_RES_500, opts)
        if res == IBSU_STATUS_OK:
            logger.info("Capture started successfully")
            return True
        else:
            logger.error(f"BeginCaptureImage failed: {res}")
            return False

    def is_capturing(self):
        if not self.is_open: return False
        active = c_int(0)
        res = self.lib.IBSU_IsCaptureActive(self.device_handle, byref(active))
        return active.value != 0 if res == IBSU_STATUS_OK else False

if __name__ == "__main__":
    scanner = KojakScanner()
    cnt = scanner.get_device_count()
    logger.info(f"Devices found: {cnt}")
    
    if cnt > 0:
        if scanner.open(0):
            try:
                # Need to give time for the prompt to appear or user to react
                if scanner.start_capture():
                    logger.info("\n=== PLEASE PLACE FINGER ON SCANNER ===\n")
                    
                    # Poll for completion or timeout (10 seconds)
                    # The callback handles the image saving
                    for i in range(100): # 10 seconds (0.1s sleep)
                        if scanner.is_capturing():
                            # logger.debug("Waiting for finger...")
                            pass
                        elif scanner.captured_image_data:
                            logger.info("Capture completed and data received!")
                            break
                        else:
                            # Not capturing, and no data? Maybe just started or finished without data?
                            # Continue waiting a bit
                            pass
                        time.sleep(0.1)
            except KeyboardInterrupt:
                logger.info("Interrupted by user")
            finally:
                scanner.close()
