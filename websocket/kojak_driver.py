
import usb.core
import usb.util
import time
import struct
import logging

class KojakDriver:
    """
    Python driver for Integrated Biometrics Kojak Scanner.
    Based on IBScanUltimate.dll.
    Protocol: USB Bulk Transfer.
    VID: 0x113F
    PID: 0x1300
    """
    
    VID = 0x113F
    PID = 0x1300
    
    CMD_INIT = 0x2A
    CMD_CAPTURE = 0x77
    CMD_GET_VERSION = 0x05
    
    # Sequence 1
    # Includes preamble (0b...0d) and 0x05 status polls.
    # This sequence leads to the 4MB data read in the trace.
    CAPTURE_SEQ = [
        bytes.fromhex('0b'),
        bytes.fromhex('1402'),
        bytes.fromhex('0e'),
        bytes.fromhex('0d'),
        bytes.fromhex('05f001'),
        bytes.fromhex('05f101'),
        bytes.fromhex('05f201'),
        bytes.fromhex('05f301'),
        bytes.fromhex('06000101'),
        bytes.fromhex('06010100'),
        bytes.fromhex('02'),
        bytes.fromhex('06070140'),
        bytes.fromhex('0608017c'),
        bytes.fromhex('0609013c'),
        bytes.fromhex('06000100'),
        bytes.fromhex('066f0103'),
        bytes.fromhex('080d00020001'),
        bytes.fromhex('080d00020000'),
        bytes.fromhex('082000020047'),
        bytes.fromhex('084100020000'),
        bytes.fromhex('084200020003'),
        bytes.fromhex('084300020003'),
        bytes.fromhex('087f00020000'),
        bytes.fromhex('081e00028006'),
        bytes.fromhex('089f00023070'),
        bytes.fromhex('089e0002111e'),
        bytes.fromhex('080b00020001'),
        bytes.fromhex('080b00020000'),
        bytes.fromhex('081000020051'),
        bytes.fromhex('081000020050'),
        bytes.fromhex('081000020051'),
        bytes.fromhex('081100021e01'),
        bytes.fromhex('081200020001'),
        bytes.fromhex('081000020053'),
        bytes.fromhex('082b00020010'),
        bytes.fromhex('082c00020010'),
        bytes.fromhex('082d00020010'),
        bytes.fromhex('082e00020010'),
        bytes.fromhex('080100020053'),
        bytes.fromhex('08020002011a'),
        bytes.fromhex('080300020787'),
        bytes.fromhex('080400020807'),
        bytes.fromhex('080900020bb8'),
        bytes.fromhex('080800020000'),
        bytes.fromhex('080c00020000'),
        bytes.fromhex('08350002005f'),
        bytes.fromhex('080b00020001'),
        bytes.fromhex('080b00020000'),
        bytes.fromhex('080600020000'),
        bytes.fromhex('0870000200ff'),
        bytes.fromhex('08710002a700'),
        bytes.fromhex('08720002a700'),
        bytes.fromhex('087300020c00'),
        bytes.fromhex('087400020600'),
        bytes.fromhex('087500025617'),
        bytes.fromhex('087600026b57'),
        bytes.fromhex('087700026b57'),
        bytes.fromhex('08780002a500'),
        bytes.fromhex('08790002ab00'),
        bytes.fromhex('087a0002a904'),
        bytes.fromhex('087b0002a700'),
        bytes.fromhex('087c0002a700'),
        bytes.fromhex('087e0002a900'),
        bytes.fromhex('087f00020000'),
        bytes.fromhex('082900020480'),
        bytes.fromhex('083e000280c7'),
        bytes.fromhex('083f00020004'),
        bytes.fromhex('084100020000'),
        bytes.fromhex('084800020010'),
        bytes.fromhex('085f0002231d'),
        bytes.fromhex('085700020004'),
        bytes.fromhex('082a00021086'),
        bytes.fromhex('05f001'),
        bytes.fromhex('05f101'),
        bytes.fromhex('05f201'),
        bytes.fromhex('05f301'),
        bytes.fromhex('06010101'),
        bytes.fromhex('05f001'),
        bytes.fromhex('05f101'),
        bytes.fromhex('05f201'),
        bytes.fromhex('05f301'),
        bytes.fromhex('1c'),
        bytes.fromhex('05f001'),
        bytes.fromhex('05f101'),
        bytes.fromhex('05f201'),
        bytes.fromhex('05f301'),
        bytes.fromhex('06330101'),
        bytes.fromhex('062f0101'),
        bytes.fromhex('02'),
        bytes.fromhex('062e013e'),
        bytes.fromhex('062d0182'),
        bytes.fromhex('062c0100'),
        bytes.fromhex('06280101'),
    ]

    def __init__(self):
        self.dev = None
        self.bp_in = None  # Bulk IN Endpoint (0x81) - Responses
        self.bp_out = None # Bulk OUT Endpoint (0x01) - Commands
        self.bp_in_img = None # Bulk IN Endpoint (0x88) - Image Data
        self.logger = logging.getLogger("KojakDriver")
        logging.basicConfig(level=logging.DEBUG)

    def connect(self):
        """Finds and claims the USB device."""
        self.logger.info("Looking for Kojak scanner...")
        self.dev = usb.core.find(idVendor=self.VID, idProduct=self.PID)
        
        if self.dev is None:
            raise ValueError("Kojak Scanner not found.")
            
        self.logger.info("Device found.")
        
        # Reset device to clear state
        try:
            self.dev.reset()
        except Exception as e:
            self.logger.warning(f"Device reset warning: {e}")

        # Detach kernel driver if active
        if self.dev.is_kernel_driver_active(0):
            try:
                self.dev.detach_kernel_driver(0)
            except usb.core.USBError as e:
                self.logger.warning(f"Could not detach kernel driver: {e}")

        # Set configuration
        try:
            self.dev.set_configuration()
        except Exception as e:
            self.logger.warning(f"Set Configuration failed (might already be set): {e}")

        # Claim Interface
        try:
            usb.util.claim_interface(self.dev, 0)
        except Exception as e:
            self.logger.warning(f"Claim Interface failed: {e}")
            
        # Basic Life Check (Get Device Descriptor via Control Pipe)
        try:
            desc = self.dev.ctrl_transfer(0x80, 0x06, 0x0100, 0x0000, 18, timeout=1000)
            self.logger.info(f"Control Pipe Life Check: Alive! (Desc len: {len(desc)})")
        except Exception as e:
            self.logger.error(f"Control Pipe Check Failed: {e}")

        # Get Endpoints
        cfg = self.dev.get_active_configuration()
        intf = cfg[(0,0)] # Interface 0, Alt 0
        
        self.bp_out = usb.util.find_descriptor(
            intf,
            custom_match = lambda e: \
                usb.util.endpoint_direction(e.bEndpointAddress) == \
                usb.util.ENDPOINT_OUT and e.bEndpointAddress == 0x01)
                
        self.bp_in = usb.util.find_descriptor(
            intf,
            custom_match = lambda e: \
                usb.util.endpoint_direction(e.bEndpointAddress) == \
                usb.util.ENDPOINT_IN and e.bEndpointAddress == 0x81)
        
        # Image Endpoint (0x88 based on trace)
        self.bp_in_img = usb.util.find_descriptor(
            intf,
            custom_match = lambda e: \
                usb.util.endpoint_direction(e.bEndpointAddress) == \
                usb.util.ENDPOINT_IN and e.bEndpointAddress == 0x88)
                
        if not self.bp_out or not self.bp_in:
            raise ValueError("Could not find Bulk Command Endpoints (0x01 / 0x81).")
        
        if not self.bp_in_img:
            self.logger.warning("Could not find Image Endpoint (0x88). Capture might fail.")
            
        self.logger.info(f"Endpoints Found. CMD_IN: {self.bp_in.bEndpointAddress}, CMD_OUT: {self.bp_out.bEndpointAddress}, IMG_IN: {self.bp_in_img.bEndpointAddress if self.bp_in_img else 'N/A'}")
        
        # Try to set alternate interface for EP 0x88
        try:
            cfg = self.dev.get_active_configuration()
            intf = cfg[(0,0)]
            self.logger.debug(f"Current interface: {intf.bInterfaceNumber}, alt: {intf.bAlternateSetting}")
            
            # Check if there are alternate settings
            for alt_intf in cfg.interfaces():
                if alt_intf.bInterfaceNumber == 0:
                    self.logger.debug(f"  Alt setting {alt_intf.bAlternateSetting} available")
                    # Try setting alternate interface 1 if it exists
                    if alt_intf.bAlternateSetting == 1:
                        try:
                            self.dev.set_interface_altsetting(0, 1)
                            self.logger.info("Set alternate interface 1 for EP 0x88")
                        except Exception as e:
                            self.logger.debug(f"Could not set alt interface 1: {e}")
        except Exception as e:
            self.logger.debug(f"Alt interface check failed: {e}")
        
        # Flush buffers
        self.flush_buffers()
        
        # Explicitly claim interface 0 to ensure we can read
        try:
            usb.util.claim_interface(self.dev, 0)
            self.logger.debug("Interface 0 claimed explicitly")
        except:
            self.logger.debug("Interface 0 already claimed or could not claim")

        # Test endpoint reads (from new trace analysis)
        # The official driver tests both EP 0x81 and 0x88 before initialization
        self.logger.debug("Testing endpoint 0x81 (Wake-up read)...")
        try:
            # Increased timeout to 500ms to ensure it hits the wire
            test_read = self.dev.read(0x81, 4, timeout=500)
            self.logger.debug(f"EP 0x81 test read: {bytes(test_read).hex()}")
        except usb.core.USBError as e:
            self.logger.debug(f"EP 0x81 test read failed (expected): {e}")
        
        self.logger.debug("Testing endpoint 0x88 (Wake-up read)...")
        try:
            # Increased timeout to 500ms to ensure it hits the wire
            test_read = self.dev.read(0x88, 4, timeout=500)
            self.logger.debug(f"EP 0x88 test read: {bytes(test_read).hex()}")
        except usb.core.USBError as e:
            self.logger.debug(f"EP 0x88 test read failed (expected): {e}")

    def flush_buffers(self):
        try:
            self.bp_in.read(1024, timeout=10)
        except:
             pass
        try:
            if self.bp_in_img: self.bp_in_img.read(1024, timeout=10)
        except:
             pass

    def initialize(self):
        """Sends initialization sequence."""
        if not self.dev: self.connect()
        
        self.logger.info("Initializing... (Legacy 0x2A removed, using Probe)")
        
        # 1. Get Name (0F 00)
        try:
            self.send_cmd(bytes([0x0F, 0x00]), "Get Name")
        except:
            pass
            
        # 2. Get Serial (0F 01)
        try:
            self.send_cmd(bytes([0x0F, 0x01]), "Get Serial")
        except:
            pass
            
        # 3. Init Calibration / Setup (Line 29 of trace)
        try:
             # Trace says 61 bytes (Line 31: S Bi:1:008:1 -115 61 <)
             self.send_cmd(bytes.fromhex('03c01f3d'), "Init Calib", read_len=61)
        except Exception as e:
             self.logger.warning(f"Init Calib failed: {e}")

        # 4. Mode Setups (0f04 ... 0f0d)
        for c in ['0f04', '0f0a', '0f0b', '0f0c', '0f0d']:
             try:
                 self.send_cmd(bytes.fromhex(c), f"Init {c}")
             except: pass
        
        # 5. Status Checks (Missing in previous version)
        # Trace lines 53-68: 05f001, 05f101...
        for c in ['05f001', '05f101', '05f201', '05f301']:
             try:
                 # These expect a 1-byte response
                 self.send_cmd(bytes.fromhex(c), f"Init Poll {c}", read_len=1)
             except Exception as e:
                 self.logger.warning(f"Init Poll failed: {e}")

        # 6. Second Init Calibration (Line 69 of trace)
        try:
             # Trace says 61 bytes again
             self.send_cmd(bytes.fromhex('03c01f3d'), "Init Calib 2", read_len=61)
        except Exception as e:
             self.logger.warning(f"Init Calib 2 failed: {e}")

        # 7. Final Mode Setup (0f04)
        try:
             self.send_cmd(bytes.fromhex('0f04'), "Init 0f04-Final")
        except: pass

        # Verify connection with Version command
        self.check_connection()

    def check_connection(self):
        """Verifies communication by reading version."""
        try:
            v = self.get_version()
            self.logger.info(f"Connection Verified. Firmware: {v}")
        except Exception as e:
            self.logger.warning(f"Could not verify connection (Version Check Failed): {e}")

    def get_version(self):
        """Gets firmware version."""
        # Cmd 0x0F 0x06
        resp = self.send_cmd(bytes([0x0F, 0x06]), "Get Version")
        if resp:
            # Filter non-printable characters to get the string
            resp_str = "".join([chr(c) for c in resp if 32 <= c <= 126])
            return resp_str
        else:
            raise Exception("No response for get_version")

    def send_cmd(self, cmd_bytes, label="Cmd", read_len=32):
        """Sends a command and reads response."""
        self.logger.debug(f"{label} Send: {cmd_bytes.hex()}")
        try:
            self.bp_out.write(cmd_bytes, timeout=1000)
            resp = self.bp_in.read(read_len, timeout=1000)
            self.logger.debug(f"{label} Resp: {bytes(resp).hex()}")
            return resp
        except usb.core.USBError as e:
            self.logger.error(f"{label} failed: {e}")
            raise

    def capture_image(self, scan_type=1):
        """
        Captures an image using the Sequence 1.
        """
        self.logger.info("Starting Capture Sequence (Seq 1 + Preamble)...")
        
        if not self.bp_in_img:
            raise ValueError("Image endpoint not found")

        # Replay Setup Sequence
        total = len(self.CAPTURE_SEQ)
        for i, cmd in enumerate(self.CAPTURE_SEQ):
            try:
                # Special handling for 0x1c (Begin Capture command)
                if cmd[0] == 0x1c:
                    print("\n[USER ACTION REQUIRED] Please place your LEFT THUMB on the scanner now.")
                    print("Waiting 2 seconds for finger placement...")
                    time.sleep(2)  # Match the ~357ms gap in trace (we use 2s for user)
                    self.logger.info("Sending Begin Capture command (0x1c)...")
                
                self.bp_out.write(cmd, timeout=100)
                
                # Handling interleaved reads
                # If command starts with 0x05 (Status Poll), must read a response (usually 1 byte) for validity.
                if cmd[0] == 0x05:
                    try:
                        resp = self.bp_in.read(1, timeout=100)
                        self.logger.debug(f"Poll {cmd.hex()} Resp: {bytes(resp).hex()}")
                    except usb.core.USBError as e:
                         self.logger.warning(f"Poll Read Timeout/Error for {cmd.hex()}: {e}")

            except usb.core.USBError as e:
                self.logger.warning(f"Capture Cmd {i}/{total} failed: {e}")
                
        self.logger.info(f"Capture sequence sent. Reading image data... (Waiting 0.5s for buffer)")
        
        # Try to clear any halt condition on the image endpoint
        try:
            self.dev.clear_halt(0x88)
            self.logger.debug("Cleared halt on EP 0x88")
        except Exception as e:
            self.logger.debug(f"Clear halt failed (might be OK): {e}")
        
        time.sleep(0.5)


        # Read Data from Endpoint 0x88
        data = bytearray()
        
        # Diagnostic: Check endpoint properties
        self.logger.debug(f"EP 0x88 Address: {self.bp_in_img.bEndpointAddress}")
        self.logger.debug(f"EP 0x88 Max Packet Size: {self.bp_in_img.wMaxPacketSize}")
        
        # Try reading just 1 byte first to test endpoint
        self.logger.debug("Testing EP 0x88 with 1-byte read...")
        try:
            test_byte = self.bp_in_img.read(1, timeout=5000)
            self.logger.info(f"✓ Got test byte from EP 0x88: {bytes(test_byte).hex()}")
        except usb.core.USBError as e:
            self.logger.error(f"✗ Test byte read failed: {e} (errno: {e.errno if hasattr(e, 'errno') else 'N/A'})")
            self.logger.error(f"  This suggests EP 0x88 is not receiving data from the device.")
            return bytes(data)
        
        # Based on trace, we expect ~4MB
        expected_min = 3 * 1024 * 1024 
        chunk_size = 1024 * 64 
        
        start_time = time.time()
        timeout = 5.0
        
        while len(data) < expected_min:
            if time.time() - start_time > timeout:
                self.logger.warning(f"Capture timed out. Got {len(data)} bytes.")
                break
                
            try:
                chunk = self.bp_in_img.read(chunk_size, timeout=1000)
                if chunk:
                    data.extend(chunk)
                    # Reset timeout on data
                    start_time = time.time()
                else:
                    # Empty read?
                    pass
            except usb.core.USBError as e:
                if e.errno == 110: # Timeout
                    # Maybe finished?
                    if len(data) > 0:
                        self.logger.info("Read Timeout (likely end of stream).")
                        break
                    else:
                        continue # Keep trying if we have nothing
                else:
                    self.logger.error(f"Image Read Error: {e}")
                    break
                    
        self.logger.info(f"Capture Finished. Read {len(data)} bytes.")
        
        # Optional: Save for inspection
        try:
             with open("capture.local.bin", "wb") as f:
                 f.write(data)
             self.logger.info("Dummy Saved to capture.local.bin")
        except:
             pass

        return bytes(data)

if __name__ == "__main__":
    # Test stub
    drv = KojakDriver()
    try:
        drv.connect()
        drv.initialize()
        print(f"Version: {drv.get_version()}")
        print("Scaning...")
        img = drv.capture_image()
        print(f"Got {len(img)} bytes.")
    except Exception as e:
        print(f"Error: {e}")
