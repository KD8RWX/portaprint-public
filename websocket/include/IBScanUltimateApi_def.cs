/*
****************************************************************************************************
* IBScanUltimateApi_defs.cs
*
* DESCRIPTION:
*     API structures and constants for IBScanUltimate.
*     http://www.integratedbiometrics.com
*
* NOTES:
*     Copyright (c) Integrated Biometrics, 2009-2017
*
* HISTORY:
*     2012/04/06  1.0.0  Created.
*     2012/05/29  1.1.0  Added callback function (IBSU_CallbackAsyncOpenDevice).
*     2012/09/05  1.3.0  Added enumeration value to IBSU_PropertyId 
*                            (ENUM_IBSU_PROPERTY_ENABLE_POWER_SAVE_MODE)
*                        Modified IBSU_DeviceDesc structure for Android.
*     2012/11/06  1.4.1  Added enumeration for RollingState (IBSU_RollingState).
*     2013/02/01  1.5.0  Added enumeration value to IBSU_ImageType (ENUM_IBSU_FLAT_FOUR_FINGERS)
*                        Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_RETRY_WRONG_COMMUNICATION)
*                        Added callback function (IBSU_CallbackNotifyMessage)
*     2013/03/20  1.6.0  Added new enumerations & structures to support IBScanMatcher integration
*                            (IBSM_ImageFormat, IBSM_ImpressionType, IBSM_FingerPosition, 
*                             IBSM_CaptureDeviceTechID, IBSM_CaptureDeviceTypeID, 
*                             IBSM_CaptureDeviceVendorID, IBSM_ImageData)
*     2013/07/31  1.6.8  Added new defines to support Curve (TBN240) LEDs (IBSU_LED_SCAN_CURVE_RED, 
*                             IBSU_LED_SCAN_CURVE_GREEN, IBSU_LED_SCAN_CURVE_BLUE)
*     2013/08/03  1.6.9  Reformatted.
*     2013/10/14  1.7.0  Added new defines to support image segmentation and bitmap header. 
*                            (IBSU_MAX_SEGMENT_COUNT, IBSU_BMP_GRAY_HEADER_LEN, IBSU_BMP_RGB24_HEADER_LEN,                
*                             IBSU_BMP_RGB32_HEADER_LEN) 
*                        Added a new structure to support image segmentation.
*                            (IBSU_SegmentPosition)
*                        Added new items in existing enumerations to support capture timeout, 
*                        roll-figerprint's width, draw arrow, getting the scale of the displayed 
*                        image, getting the left/top margin of the display, showing invalid finger 
*                        position area of top/left/right, and identifying the extended result image 
*                        available callback.
*                        Added new enumeration to make overlay object on client window.
*                            (IBSU_OverlayShapePattern)
*                        Added new callback function (IBSU_CallbackResultImageEx)
*     2014/02/25  1.7.1  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_ROLL_MODE,ENUM_IBSU_PROPERTY_ROLL_LEVEL )
*     2014/06/19  1.7.3  Added enumeration value to IBSU_ClientWindowPropertyId
*                            (ENUM_IBSU_WINDOW_PROPERTY_ROLL_GUIDE_LINE_WIDTH )
*     2014/07/23  1.8.0  Reformatted.
*     2014/09/17  1.8.1  Change wrong properties of IBSM_ImageData (ScaleUnit, BitDepth, ImageDataLength)
*     2015/03/04  1.8.3  Reformatted to support UNICODE for WinCE
*                        Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_CAPTURE_AREA_THRESHOLD, ENUM_IBSU_PROPERTY_ENABLE_DECIMATION )
*                        Added enumeration value to IBSU_ClientWindowPropertyId
*                            (ENUM_IBSU_WINDOW_PROPERTY_SCALE_FACTOR_EX )
*                        Added reserved feature on ENUM_IBSU_PROPERTY_RESERVED_1
*     2015/04/07  1.8.4  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_ENABLE_CAPTURE_ON_RELEASE, ENUM_IBSU_PROPERTY_DEVICE_INDEX,
*                             ENUM_IBSU_PROPERTY_DEVICE_ID )
*                        Added reserved enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_RESERVED_IMAGE_PROCESS_THRESHOLD )
*     2015/08/05  1.8.5  Added enumeration value to IBSU_ClientWindowPropertyId
*                            (ENUM_IBSU_WINDOW_PROPERTY_KEEP_REDRAW_LAST_IMAGE )
*                        Added new enumerations to support Image combine
*                            (IBSU_CombineImageWhichHand ) 
*     2015/12/11  1.9.0  Added additional LED definitions for Kojak.
*                        Added enumeration value to IBSU_ImageType
*                            (ENUM_IBSU_FLAT_THREE_FINGERS )
*                        Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_SUPER_DRY_MODE )
*                        Added enumeration value to IBSU_LedType
*                            (ENUM_IBSU_LED_TYPE_FSCAN )
*                        Added enumeration for beeper
*                            (IBSU_BeeperType, IBSU_BeepPattern )
*                        Added enumeration value to IBSM_CaptureDeviceTypeID
*                            (IBSM_CAPTURE_DEVICE_TYPE_ID_KOJAK )
*                        Added new callback function (IBSU_CallbackKeyButtons)
*     2016/01/21  1.9.2  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_MIN_CAPTURE_TIME_IN_SUPER_DRY_MODE )
*     2016/04/20  1.9.3  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_ROLLED_IMAGE_WIDTH, ENUM_IBSU_PROPERTY_ROLLED_IMAGE_HEIGHT )
*     2016/09/22  1.9.4  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_NO_PREVIEW_IMAGE, ENUM_IBSU_PROPERTY_ROLL_IMAGE_OVERRIDE )
*     2017/04/27  1.9.7  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_WARNING_MESSAGE_INVALID_AREA, ENUM_IBSU_PROPERTY_ENABLE_WET_FINGER_DETECT,
*                             ENUM_IBSU_PROPERTY_WET_FINGER_DETECT_LEVEL) 
*                        Added enumeration value to IBSU_FingerQualityState
*                            (ENUM_IBSU_QUALITY_INVALID_AREA_BOTTOM )
*     2017/06/16  1.9.8  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_WET_FINGER_DETECT_LEVEL_THRESHOLD, 
*                             ENUM_IBSU_PROPERTY_START_POSITION_OF_ROLLING_AREA,
*	                          ENUM_IBSU_PROPERTY_START_ROLL_WITHOUT_LOCK,
*                             ENUM_IBSU_PROPERTY_ENABLE_TOF,
*                             ENUM_IBSU_PROPERTY_RESERVED_ENABLE_TOF_FOR_ROLL, 
*                             ENUM_IBSU_PROPERTY_RESERVED_CAPTURE_BRIGHTNESS_THRESHOLD_FOR_FLAT,
*                             ENUM_IBSU_PROPERTY_RESERVED_CAPTURE_BRIGHTNESS_THRESHOLD_FOR_ROLL,
*                             ENUM_IBSU_PROPERTY_RESERVED_ENHANCED_RESULT_IMAGE)
*     2017/12/05  1.10.0  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_ENABLE_ENCRYPTION)
*     2018/04/27  2.0.1   Deprecated enumeration
*                            (IBSM_FingerPosition)
*     2019/06/21  3.0.0   Added IBSU_EncryptionMode.
*                         Added enumeration value to IBSU_PropertyId
                             (ENUM_IBSU_PROPERTY_VIEW_ENCRYPTION_IMAGE_MODE)
*     2019/10/25  3.1.1   Added enumeration value to IBSU_PropertyId
                             (ENUM_IBSU_PROPERTY_FINGERPRINT_SEGMENTATION_MODE)
*     2020/01/09  3.2.0   Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_IS_SPOOF_SUPPORTED,
*                             ENUM_IBSU_PROPERTY_ENABLE_SPOOF,
*                             ENUM_IBSU_PROPERTY_SPOOF_LEVEL)
*     2020/04/01  3.3.0   Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_ROLL_METHOD,
*                             ENUM_IBSU_PROPERTY_RENEWAL_OPPOSITE_IMGAE_LEVEL,
*                             ENUM_IBSU_PROPERTY_PREVIEW_IMAGE_QUALITY_FOR_KOJAK,
*                             ENUM_IBSU_PROPERTY_RESERVED_ENHANCED_RESULT_IMAGE_LEVEL,
*                             ENUM_IBSU_PROPERTY_RESERVED_ENABLE_SLIP_DETECTION,
*                             ENUM_IBSU_PROPERTY_RESERVED_SLIP_DETECTION_LEVEL)
*     2020/04/01  3.3.0   Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_RESERVED_ENABLE_TRICK_CAPTURE)
*     2020/08/19  3.5.0  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_ADAPTIVE_CAPTURE_MODE,
*                             ENUM_IBSU_PROPERTY_ENABLE_KOJAK_BEHAVIOR_2_6)
*     2020/10/06  3.7.0  Modified IBSU_DeviceDesc structure for new locking feature.
*                        Added IBSU_HashType
*     2021/07/16  3.7.2  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_VERTICAL_DIRECTION_SEGMENT,
*                             ENUM_IBSU_PROPERTY_RESERVED_SPOOF_LEVEL_THRESHOLD)
*	  2022/02/18  3.8.1  Modified enumeration name changed from 
*                            ENUM_IBSU_PROPERTY_VERTICAL_DIRECTION_SEGMENT to
*                            ENUM_IBSU_PROPERTY_DISABLE_SEGMENT_ROTATION
*                        ENUM_IBSU_PROPERTY_ADAPTIVE_CAPTURE_MODE 
*                            property value set not permitted
*     2022/04/13  3.9.0  Added new enumerations & structures to support ISO/ANSI template integration
*                            (IBSM_TemplateVersion, IBSM_Template, IBSM_StandardFormat, 
*                             IBSM_StandardFormatData)                 
*     2022/05/30  3.9.1  Removed ENUM_IBSU_PROPERTY_RESERVED_SPOOF_LEVEL_THRESHOLD
*     2024/03/26  4.0.0  Added enumeration value to IBSU_PropertyId
*                            (ENUM_IBSU_PROPERTY_DR_MODE_ZOOM_IN)
****************************************************************************************************
*/


using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace IBscanUltimate
{
    public partial class DLL
    {
        /*
        ****************************************************************************************************
        * GLOBAL DEFINES
        ****************************************************************************************************
        */

        /* Required length of buffer for return string parameters. */
        public const int IBSU_MAX_STR_LEN = 128;

        /* Minimum value of contrast. */
        public const int IBSU_MIN_CONTRAST_VALUE = 0;

        /* Maximum value of contrast. */
        public const int IBSU_MAX_CONTRAST_VALUE = 34;

        /* Required buffer length for segment parameters. */
        public const int IBSU_MAX_SEGMENT_COUNT = 5;
        /* Required buffer length for the 8bit bitmap header. */
        public const int IBSU_BMP_GRAY_HEADER_LEN = 1078;
        /* Required buffer length for the 24bit bitmap header. */
        public const int IBSU_BMP_RGB24_HEADER_LEN = 54;
        /* Required buffer length for the 32bit bitmap header. */
        public const int IBSU_BMP_RGB32_HEADER_LEN = 54;

        /* Capture options.  For more information, see IBSU_BeginCaptureImage(). */
        public const int IBSU_OPTION_AUTO_CONTRAST = 1;
        public const int IBSU_OPTION_AUTO_CAPTURE = 2;
        public const int IBSU_OPTION_IGNORE_FINGER_COUNT = 4;

        /* LED bit defines. */
        public const uint IBSU_LED_NONE = 0x0;  /* All LEDs off. */
        public const uint IBSU_LED_ALL = 0xFFFFFFFF;  /* All LEDs on. */
        public const uint IBSU_LED_INIT_BLUE = 0x00000001;  /* Reserved for vendor (user cannot control it). */
        public const uint IBSU_LED_SCAN_GREEN = 0x00000002;  /* OK key - green blink phase 1. */
        public const uint IBSU_LED_SCAN_CURVE_RED = 0x00000010;  /* Red LED for Curve (TBN240). */
        public const uint IBSU_LED_SCAN_CURVE_GREEN = 0x00000020;  /* Green LED for Curve (TBN240). */
        public const uint IBSU_LED_SCAN_CURVE_BLUE = 0x00000040;  /* Blue LED for Curve (TBN240). */

        /* Specific LED bit defines with LED type = ENUM_IBSU_LED_TYPE_FSCAN (e.g four finger scanner, Kojak). */
        public const uint IBSU_LED_F_BLINK_GREEN            = 0x10000000;  /* All Green LEDs blink. */
        public const uint IBSU_LED_F_BLINK_RED              = 0x20000000;  /* All Red LEDs blink. */
        public const uint IBSU_LED_F_LEFT_LITTLE_GREEN      = 0x01000000;  /* Green LED for left little finger. */
        public const uint IBSU_LED_F_LEFT_LITTLE_RED        = 0x02000000;  /* Red LED for left little finger. */
        public const uint IBSU_LED_F_LEFT_RING_GREEN        = 0x04000000;  /* Green LED for left ring finger. */
        public const uint IBSU_LED_F_LEFT_RING_RED          = 0x08000000;  /* Red LED for left ring finger. */
        public const uint IBSU_LED_F_LEFT_MIDDLE_GREEN      = 0x00100000;  /* Green LED for left middle finger. */
        public const uint IBSU_LED_F_LEFT_MIDDLE_RED        = 0x00200000;  /* Red LED for left middle finger. */
        public const uint IBSU_LED_F_LEFT_INDEX_GREEN       = 0x00400000;  /* Green LED for left index finger. */
        public const uint IBSU_LED_F_LEFT_INDEX_RED         = 0x00800000;  /* Red LED for left index finger. */
        public const uint IBSU_LED_F_LEFT_THUMB_GREEN       = 0x00010000;  /* Green LED for left thumb finger. */
        public const uint IBSU_LED_F_LEFT_THUMB_RED         = 0x00020000;  /* Red LED for left thumb finger. */
        public const uint IBSU_LED_F_RIGHT_THUMB_GREEN      = 0x00040000;  /* Green LED for right thumb finger. */
        public const uint IBSU_LED_F_RIGHT_THUMB_RED        = 0x00080000;  /* Red LED for right thumb finger. */
        public const uint IBSU_LED_F_RIGHT_INDEX_GREEN      = 0x00001000;  /* Green LED for right index finger. */
        public const uint IBSU_LED_F_RIGHT_INDEX_RED        = 0x00002000;  /* Red LED for right index finger. */
        public const uint IBSU_LED_F_RIGHT_MIDDLE_GREEN     = 0x00004000;  /* Green LED for right middle finger. */
/* 0x00008000 cannot be used at VB6 (Negative value) */
//        public const uint IBSU_LED_F_RIGHT_MIDDLE_RED       = 0x00008000;  /* Red LED for right middle finger. */
        public const uint IBSU_LED_F_RIGHT_MIDDLE_RED       = 0x40000000;  /* Red LED for right middle finger. */
        public const uint IBSU_LED_F_RIGHT_RING_GREEN       = 0x00000100;  /* Green LED for right ring finger. */
        public const uint IBSU_LED_F_RIGHT_RING_RED         = 0x00000200;  /* Red LED for right ring finger. */
        public const uint IBSU_LED_F_RIGHT_LITTLE_GREEN     = 0x00000400;  /* Green LED for right little finger. */
        public const uint IBSU_LED_F_RIGHT_LITTLE_RED       = 0x00000800;  /* Red LED for right little finger. */
        public const uint IBSU_LED_F_PROGRESS_ROLL          = 0x00000010;  /* Green LED for right ring finger. */
        public const uint IBSU_LED_F_PROGRESS_LEFT_HAND     = 0x00000020;  /* Red LED for right ring finger. */
        public const uint IBSU_LED_F_PROGRESS_TWO_THUMB     = 0x00000040;  /* Green LED for right little finger. */
        public const uint IBSU_LED_F_PROGRESS_RIGHT_HAND    = 0x00000080;  /* Red LED for right little finger. */

        /* Bit-pattern of finger index for IBSU_RemoveFingerImage, IBSU_AddFingerImage, IBSU_IsFingerDuplicated and IBSU_IsValidFingerGeometry */
        public const uint IBSU_FINGER_NONE = 0x00000000;
        public const uint IBSU_FINGER_LEFT_LITTLE = 0x00000001;
        public const uint IBSU_FINGER_LEFT_RING = 0x00000002;
        public const uint IBSU_FINGER_LEFT_MIDDLE = 0x00000004;
        public const uint IBSU_FINGER_LEFT_INDEX = 0x00000008;
        public const uint IBSU_FINGER_LEFT_THUMB = 0x00000010;
        public const uint IBSU_FINGER_RIGHT_THUMB = 0x00000020;
        public const uint IBSU_FINGER_RIGHT_INDEX = 0x00000040;
        public const uint IBSU_FINGER_RIGHT_MIDDLE = 0x00000080;
        public const uint IBSU_FINGER_RIGHT_RING = 0x00000100;
        public const uint IBSU_FINGER_RIGHT_LITTLE = 0x00000200;
        public const uint IBSU_FINGER_LEFT_HAND = (IBSU_FINGER_LEFT_INDEX | IBSU_FINGER_LEFT_MIDDLE | IBSU_FINGER_LEFT_RING | IBSU_FINGER_LEFT_LITTLE);
        public const uint IBSU_FINGER_RIGHT_HAND = (IBSU_FINGER_RIGHT_INDEX | IBSU_FINGER_RIGHT_MIDDLE | IBSU_FINGER_RIGHT_RING | IBSU_FINGER_RIGHT_LITTLE);
        public const uint IBSU_FINGER_BOTH_THUMBS = (IBSU_FINGER_RIGHT_THUMB | IBSU_FINGER_LEFT_THUMB);
        public const uint IBSU_FINGER_ALL = (IBSU_FINGER_LEFT_HAND | IBSU_FINGER_RIGHT_HAND | IBSU_FINGER_BOTH_THUMBS);
        public const uint IBSU_FINGER_LEFT_LITTLE_RING = (IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING);
        public const uint IBSU_FINGER_LEFT_MIDDLE_INDEX = (IBSU_FINGER_LEFT_MIDDLE | IBSU_FINGER_LEFT_INDEX);
        public const uint IBSU_FINGER_RIGHT_INDEX_MIDDLE = (IBSU_FINGER_RIGHT_INDEX | IBSU_FINGER_RIGHT_MIDDLE);
        public const uint IBSU_FINGER_RIGHT_RING_LITTLE = (IBSU_FINGER_RIGHT_RING | IBSU_FINGER_RIGHT_LITTLE);

        /* Maximum value of Minutiae size. (For support ISO Template control) */
        public const int IBSU_MAX_MINUTIAE_SIZE = (255+2);

        [StructLayout(LayoutKind.Explicit)]
        public struct IBSU_RECT
        {
            [FieldOffset(0)] public int left;
            [FieldOffset(4)] public int top;
            [FieldOffset(8)] public int right;
            [FieldOffset(12)] public int bottom;
        }

        /*
        ****************************************************************************************************
        * GLOBAL TYPES
        ****************************************************************************************************
        */

        /*
        ****************************************************************************************************
        * IBSU_SdkVersion
        *
        * DESCRIPTION:
        *     Container to hold SDK version.
        ****************************************************************************************************
        */
        public struct IBSU_SdkVersion
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string Product;                    /* Product version string */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string File;                       /* File version string */
        }

        /*
        ****************************************************************************************************
        * IBSU_DeviceDesc
        *
        * DESCRIPTION:
        *     Basic device description.
        ****************************************************************************************************
        */
        public struct IBSU_DeviceDesc
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string serialNumber;               /* Device serial number */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string productName;                /* Device product name */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string interfaceType;              /* Device interface type (USB, Firewire) */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string fwVersion;                  /* Device firmware version */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string devRevision;                /* Device revision */
            public int    handle;                     /* Return device handle */
            public bool   IsHandleOpened;             /* Check if device handle is opened */
            public bool   IsDeviceLocked;             /* Check if device is locked */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = IBSU_MAX_STR_LEN)]
            public string customerString;                /* Customer string */
        }

        /*
        ****************************************************************************************************
        * IBSU_SegmentPosition
        *
        * DESCRIPTION:
        *     Container to hold segment information.
        ****************************************************************************************************
        */
        [StructLayout(LayoutKind.Explicit)]
        public struct IBSU_SegmentPosition
        {
            [FieldOffset(0)]
            public short x1;										/* X coordinate of starting point of the finger segment. */
            [FieldOffset(2)]
            public short y1;										/* Y coordinate of starting point of the finger segment. */
            [FieldOffset(4)]
            public short x2;										/* X coordinate of 1st corner of the finger segment. */
            [FieldOffset(6)]
            public short y2;										/* Y coordinate of 1st corner of the finger segment. */
            [FieldOffset(8)]
            public short x3;										/* X coordinate of 2nd corner of the finger segment. */
            [FieldOffset(10)]
            public short y3;										/* Y coordinate of 2nd corner of the finger segment. */
            [FieldOffset(12)]
            public short x4;										/* X coordinate of 3rd corner of the finger segment. */
            [FieldOffset(14)]
            public short y4;										/* Y coordinate of 3rd corner of the finger segment. */
        }

        /*
        ****************************************************************************************************
        * IBSU_ImageType
        *
        * DESCRIPTION:
        *     Enumeration of image types.  The image type is passed as a parameter to API functions 
        *     IBSU_BeginCaptureImage() and IBSU_IsCaptureAvailable().
        ****************************************************************************************************
        */
        public enum IBSU_ImageType
        {
            /* Unspecified type. */
            ENUM_IBSU_TYPE_NONE,

            /* One-finger rolled fingerprint. */
            ENUM_IBSU_ROLL_SINGLE_FINGER,

            /* One-finger flat fingerprint. */
            ENUM_IBSU_FLAT_SINGLE_FINGER,

            /* Two-finger flat fingerprint. */
            ENUM_IBSU_FLAT_TWO_FINGERS,

            /* Four-finger flat fingerprint. */
            ENUM_IBSU_FLAT_FOUR_FINGERS,

            /* Three-finger flat fingerprint. */
            ENUM_IBSU_FLAT_THREE_FINGERS,

            /* Palm flat writer's palm. */
            ENUM_IBSU_FLAT_SINGLE_WRITERS_PALM,

            /* Palm flat upper plam. */
            ENUM_IBSU_FLAT_SINGLE_UPPER_PALM,

            /* Palm flat lower palm. */
            ENUM_IBSU_FLAT_SINGLE_LOWER_PALM

        }

        /*
        ****************************************************************************************************
        * IBSU_ImageResolution
        *
        * DESCRIPTION:
        *     Enumeration of image resolutions.  The image resolution is passed as a parameter to API  
        *     functions IBSU_BeginCaptureImage() and IBSU_IsCaptureAvailable().
        ****************************************************************************************************
        */
        public enum IBSU_ImageResolution
        {
            /* 500 pixels/inch. */
            ENUM_IBSU_IMAGE_RESOLUTION_500 = 500,

            /* 1000 pixels/inch. */
            ENUM_IBSU_IMAGE_RESOLUTION_1000 = 1000
        }

        /*
        ****************************************************************************************************
        * IBSU_PropertyId
        *
        * DESCRIPTION:
        *     Enumeration of property IDs.  Properties can be gotten with IBSU_GetProperty(); some properties
        *     can also be set with IBSU_SetProperty().
        ****************************************************************************************************
        */
        public enum IBSU_PropertyId
        {
            /* Product name string (e.g., "Watson").  [Get only.] */
            ENUM_IBSU_PROPERTY_PRODUCT_ID,

            /* Serial number string.  [Get only.] */
            ENUM_IBSU_PROPERTY_SERIAL_NUMBER,

            /* Device manufacturer identifier.  [Get only.] */
            ENUM_IBSU_PROPERTY_VENDOR_ID,

            /* IBIA vendor ID.  [Get only.] */
            ENUM_IBSU_PROPERTY_IBIA_VENDOR_ID,

            /* IBIA version information.  [Get only.] */
            ENUM_IBSU_PROPERTY_IBIA_VERSION,

            /* IBIA device ID.  [Get only.] */
            ENUM_IBSU_PROPERTY_IBIA_DEVICE_ID,

            /* Firmware version string.  [Get only.] */
            ENUM_IBSU_PROPERTY_FIRMWARE,

            /* Device revision string.  [Get only.] */
            ENUM_IBSU_PROPERTY_REVISION,

            /* Production date string.  [Get only.] */
            ENUM_IBSU_PROPERTY_PRODUCTION_DATE,

            /* Last service date string.  [Get only.] */
            ENUM_IBSU_PROPERTY_SERVICE_DATE,

            /* Image width value.  [Get only.] */
            ENUM_IBSU_PROPERTY_IMAGE_WIDTH,

            /* Image height value.  [Get only.] */
            ENUM_IBSU_PROPERTY_IMAGE_HEIGHT,

            /* Time to acquire fingerprint in the auto capture regardless of number of fingers.  The option
             * IBSU_OPTION_AUTO_CAPTURE must be used.  The valid range is between 2000- and 4000-ms, 
             * inclusive, with the default of 4000-ms. [Get and set.] */
            ENUM_IBSU_PROPERTY_IGNORE_FINGER_TIME,

            /* Auto contrast level value. [Get and set.] */
            ENUM_IBSU_PROPERTY_RECOMMENDED_LEVEL,

            /* Polling time for IBSU_BGetImage(). [Get only.] */
            ENUM_IBSU_PROPERTY_POLLINGTIME_TO_BGETIMAGE,

            /* Enable power save mode (TRUE to enable or FALSE to disable). [Get and set.] */
            ENUM_IBSU_PROPERTY_ENABLE_POWER_SAVE_MODE,

            /* Retry count for communication error.  The valid range is between 0 and 120, inclusive, with 
             * the default of 6. [Get and set.] */
            ENUM_IBSU_PROPERTY_RETRY_WRONG_COMMUNICATION,

            /* The maximum wait time for image capture, in seconds.  Must use IBSU_CallbackResultImageEx instead
             * of IBSU_CallbackResultImage.  If -1, the timeout is infinite.  Otherwise, the valid
             * range is between 10- and 3600-seconds, inclusive.  The default is -1.  [Get and set.] */
            ENUM_IBSU_PROPERTY_CAPTURE_TIMEOUT,

            /* Minimum distance of rolled fingerprint, in millimeters.  The valid range is between 10-
             * and 30-mm, inclusive.  The default is 15-mm.  [Get and set.] */
            ENUM_IBSU_PROPERTY_ROLL_MIN_WIDTH,

            /* roll mode. The valid range is between 0 ~ 1.  The default is 1.  [Get and set.] 
             * 0 : no use smear
               1 : use notice */
            ENUM_IBSU_PROPERTY_ROLL_MODE,

            /* roll level. The valid range is between 0 ~ 2.  The default is 1.  [Get and set.] 
             * 0 : low level
               1 : medium level
               2 : high level */
            ENUM_IBSU_PROPERTY_ROLL_LEVEL,

            /* The area threshold for image capture for flat fingers and
             * The area threshold for beginning rolled finger.
             * The valid range is between 0 and 12, inclusive, with the default of 6. [Get and set.] */
            ENUM_IBSU_PROPERTY_CAPTURE_AREA_THRESHOLD,

            /* Enable decimation mode (TRUE to enable or FALSE to disable).
             * Some of devices (or firmware version) does not support this feature.	[Get and set.]*/
            ENUM_IBSU_PROPERTY_ENABLE_DECIMATION,

            /* Enable capture on release (TRUE to enable or FALSE to disable). The default is FALSE.  [Get and set.]
             * TRUE  : the result callback will be called when user release the finger from the sensor.
             * FALSE : the result callback will be called when the quality of finger become good */
            ENUM_IBSU_PROPERTY_ENABLE_CAPTURE_ON_RELEASE,

            /* The device index. [Get only.] */
            ENUM_IBSU_PROPERTY_DEVICE_INDEX,

            /* The device ID which has same information with UsbDevice class of Android. [Get only.] */
            ENUM_IBSU_PROPERTY_DEVICE_ID,

            /* It can be used for dry finger
             * Some of devices (or firmware version) does not support this feature.
             * The default is FALSE. [Get and set.]
             * TRUE  : Enable dry mode.
             * FALSE : Disable dry mode */
            ENUM_IBSU_PROPERTY_SUPER_DRY_MODE,

            /* It is a minimum capture time when the dry mode is enabled with the property ENUM_IBSU_PROPERTY_SUPER_DRY_MODE
             * Some of devices (or firmware version) does not support this feature.
             * The valid range is between 600- and 3000-ms, 
             * inclusive, with the default of 2000-ms. [Get and set.]*/
            ENUM_IBSU_PROPERTY_MIN_CAPTURE_TIME_IN_SUPER_DRY_MODE,

            /* Rolled image width value.  [Get only.] */
            ENUM_IBSU_PROPERTY_ROLLED_IMAGE_WIDTH,

            /* Rolled image height value.  [Get only.] */
            ENUM_IBSU_PROPERTY_ROLLED_IMAGE_HEIGHT,

			/* Enable the drawing for preview image (TRUE to enable or FALSE to disable). 
		     * The default is TRUE.  [Get and set.] */
			ENUM_IBSU_PROPERTY_NO_PREVIEW_IMAGE,
		
			/* Enable to override roll image (TRUE to enable or FALSE to disable). 
		     * The default is FALSE.  [Get and set.] */
			ENUM_IBSU_PROPERTY_ROLL_IMAGE_OVERRIDE,

            /* Enable the warning message for invalid area for result image (TRUE to enable or FALSE to disable). 
             * The default is FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_WARNING_MESSAGE_INVALID_AREA,

            /* Enable wet detect function.
             * The default is FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_ENABLE_WET_FINGER_DETECT,

            /* Change wet detect level.
	         * The valid range is between 1 and 5. The default is 3.  [Get and set.]
	         * 1 : Lowest level for detect wet finger : less sensitive
	         * 5 : Highest level for detect wet finger : more sensitive */
            ENUM_IBSU_PROPERTY_WET_FINGER_DETECT_LEVEL,

            /* Change threshold for each wet detect level.
             * The valid range is between 10 and 1000. The default is "50 100 150 200 250"  [Get and set.]
             * 50 : Threshold of lowest level for detect wet finger
             * 250 : Threshold of highest level for detect wet finger */
            ENUM_IBSU_PROPERTY_WET_FINGER_DETECT_LEVEL_THRESHOLD,

            /* Control rolling area vertically.
             * The valid range is between 0 and 9. The default is 0.  [Get and set.]
             * 0 : minimum position
             * 9 : maximum position */
            ENUM_IBSU_PROPERTY_START_POSITION_OF_ROLLING_AREA,

            /* Enable rolling without lock.
             * The default is FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_START_ROLL_WITHOUT_LOCK,

            /* Enable TOF function.
             * The default is set depending on the devices.  [Get and set.] */
            ENUM_IBSU_PROPERTY_ENABLE_TOF,
            
            /* Enable Encryption for capture images
			 * The default is FALSE.  [Get and set.] */
			ENUM_IBSU_PROPERTY_ENABLE_ENCRYPTION,

            /* Check if the device supprort spoof function or not */
            ENUM_IBSU_PROPERTY_IS_SPOOF_SUPPORTED,

            /* Enable spoof function
             * The default is FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_ENABLE_SPOOF,

            /* Change spoof level.
             * The valid range is between 1 and 5. The default is 3.  [Get and set.]
             * 1 : Lowest level for spoof finger : less sensitive
             * 5 : Highest level for spoof finger : more sensitive */
            ENUM_IBSU_PROPERTY_SPOOF_LEVEL,

            /* View encrypt Image
	        * The default is FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_VIEW_ENCRYPTION_IMAGE_MODE,

            /* Select fingerprint segmentation mode
             * The default is 0.  [Get and set.] */
            ENUM_IBSU_PROPERTY_FINGERPRINT_SEGMENTATION_MODE,

            /* Enhanced roll Method
             * The default values are 0.  [Get and set.] */
            ENUM_IBSU_PROPERTY_ROLL_METHOD,

            /* Select a level of oppisite image value during roll
             * The default values are 0.  [Get and set.] 
             * 0 : No use
             * 1 : renewal if roll image is moved as 1.2mm.
             * 2 : renewal if roll image is moved as 2.4mm.
             * 3 : renewal if roll image is moved as 3.6mm.
             */
            ENUM_IBSU_PROPERTY_RENEWAL_OPPOSITE_IMGAE_LEVEL,

             /* Enable to High quality preview image for Kojak
              * The default values are 0.  [Get and set.] */
            ENUM_IBSU_PROPERTY_PREVIEW_IMAGE_QUALITY_FOR_KOJAK,

            /* Enable Adaptive Capture
             * The default values are TRUE. [Get only.] */
            ENUM_IBSU_PROPERTY_ADAPTIVE_CAPTURE_MODE,

            /* Enable to Kojak 2.6 behavior
             * The default values are FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_ENABLE_KOJAK_BEHAVIOR_2_6,

            /* Disable to Segment rectangles rotation
             * The default values are FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_DISABLE_SEGMENT_ROTATION,

            /* Enable Zoom-In mode : It sets Zoom-In when fingers start being detected in DR mode.
			* The default values are TRUE. This property only set FALSE in WINDOWS.
			* Can't set FALSE in Linux or Android or Windows JNI-JAVA environments. [Get and set.] */
            ENUM_IBSU_PROPERTY_DR_MODE_ZOOM_IN,

            /* Reserved for manufacturer strings. [Need a reserve code]*/
            ENUM_IBSU_PROPERTY_RESERVED_1 = 200,
            ENUM_IBSU_PROPERTY_RESERVED_2,
            ENUM_IBSU_PROPERTY_RESERVED_100,

            /* The previmage processing threshold. [Need a partner or reserve code] 
             * The valid range is between 0 and 2, inclusive, 
             * with the default of 0 on embedded processor (ARM, Android and Windows Mobile),
             * and with the default of 2 on PC. [Get and set.]
             * 0  : IMAGE_PROCESS_LOW
             * 1  : IMAGE_PROCESS_MEDIUM
             * 2  : IMAGE_PROCESS_HIGH */
            ENUM_IBSU_PROPERTY_RESERVED_IMAGE_PROCESS_THRESHOLD = 400,

            /* Enable TOF for roll capture
             * The default is FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_RESERVED_ENABLE_TOF_FOR_ROLL,

            /* Change brightness threshold for flat capture
             * The default values are depending on the scanner.  [Get and set.] */
            ENUM_IBSU_PROPERTY_RESERVED_CAPTURE_BRIGHTNESS_THRESHOLD_FOR_FLAT,

            /* Change brightness threshold for roll capture
             * The default values are depending on the scanner.  [Get and set.] */
            ENUM_IBSU_PROPERTY_RESERVED_CAPTURE_BRIGHTNESS_THRESHOLD_FOR_ROLL,

            /* Change result image to be enhanced
             * The default values are FALSE.  [Get and set.] */
            ENUM_IBSU_PROPERTY_RESERVED_ENHANCED_RESULT_IMAGE,
            
            /* Select a level of image processing
             * The default values are 0.  [Get and set.] 
               0 : Minimum enhancement
               5 : Maximum enhancement */
            ENUM_IBSU_PROPERTY_RESERVED_ENHANCED_RESULT_IMAGE_LEVEL,

            /* Enable Slip detection
            * The default values are FALSE. [Get and set.] */
            ENUM_IBSU_PROPERTY_RESERVED_ENABLE_SLIP_DETECTION,

            /* Change level of Slip detection
            * The default values are 3. The Ragne is from 1 to 10.  [Get and set.] */
            ENUM_IBSU_PROPERTY_RESERVED_SLIP_DETECTION_LEVEL,

            /* Enable Trick Capture
            * The default values are TRUE. [Get and set.] */
            ENUM_IBSU_PROPERTY_RESERVED_ENABLE_TRICK_CAPTURE,

        }

        /*
        ****************************************************************************************************
        * IBSU_ClientWindowPropertyId
        *
        * DESCRIPTION:
        *     Enumeration of client window property IDs.  Properties can be gotten with IBSU_GetClientWindowProperty(); 
        *     some properties can also be set with IBSU_SetClientWindowProperty().
        ****************************************************************************************************
        */
        public enum IBSU_ClientWindowPropertyId
        {
            /* Background color of display window.  The valid range is between 0x00000000 and 0xFFFFFFFF, 
             * inclusive, with the default of 0x00D8E9EC (the button face color on Windows). [Get and set.] */
            ENUM_IBSU_WINDOW_PROPERTY_BK_COLOR,

            /* Indicates whether guide line should be drawn for rolling print capture (TRUE or FALSE).  The
             * default is TRUE.  [Get and set.] */
            ENUM_IBSU_WINDOW_PROPERTY_ROLL_GUIDE_LINE,

            /* Draw arrow to display invalid area (TRUE or FALSE).  The default is FALSE.  [Get and set.] */
            ENUM_IBSU_WINDOW_PROPERTY_DISP_INVALID_AREA,

            /* Get the scale of the display image on client window, as a floating point value.  */
            ENUM_IBSU_WINDOW_PROPERTY_SCALE_FACTOR,

            /* Get the left margin of the displayed image in relation to the client window, as an integer. */
            ENUM_IBSU_WINDOW_PROPERTY_LEFT_MARGIN,

            /* Get the top margin of the displayed image in relation to the client window, as an integer. */
            ENUM_IBSU_WINDOW_PROPERTY_TOP_MARGIN,

            /* Thickness of ENUM_IBSU_WINDOW_PROPERTY_ROLL_GUIDE_LINE The valid range is between 1 and 6 pixels,
             * inclusive, with the default of 2 pixels.  [Get and set.] */
            ENUM_IBSU_WINDOW_PROPERTY_ROLL_GUIDE_LINE_WIDTH,

            /* Get the extended scale of the display image on client window, as a integer value.  */
            ENUM_IBSU_WINDOW_PROPERTY_SCALE_FACTOR_EX,
    
            /* Keep the last image for redrawing of the display image when calling IBSU_RedrawClientWindow.
            (TRUE to enable or FALSE to disable).  The default is TRUE.  [Get and set.]  */
            ENUM_IBSU_WINDOW_PROPERTY_KEEP_REDRAW_LAST_IMAGE,

            /* Color of the roll guide line.  The valid range is between 0x000000 and 0xFFFFFF,
                 inclusive, with the default of 0x0000FF (COLORREF:0xBBGGRR). [Get and set.]  */
            ENUM_IBSU_WINDOW_PROPERTY_ROLL_GUIDE_LINE_COLOR,
        }

        /*
        ****************************************************************************************************
        * IBSU_FingerCountState
        *
        * DESCRIPTION:
        *     Enumeration of finger count states.
        ****************************************************************************************************
        */
        public enum IBSU_FingerCountState
        {
            ENUM_IBSU_FINGER_COUNT_OK,
            ENUM_IBSU_TOO_MANY_FINGERS,
            ENUM_IBSU_TOO_FEW_FINGERS,
            ENUM_IBSU_NON_FINGER,
        }

        /*
        ****************************************************************************************************
        * IBSU_FingerQualityState
        *
        * DESCRIPTION:
        *     Enumeration of finger quality states.
        ****************************************************************************************************
        */
        public enum IBSU_FingerQualityState
        {
            ENUM_IBSU_FINGER_NOT_PRESENT,
            ENUM_IBSU_QUALITY_GOOD,
            ENUM_IBSU_QUALITY_FAIR,
            ENUM_IBSU_QUALITY_POOR,
            ENUM_IBSU_QUALITY_INVALID_AREA_TOP,						///< Finger position is not valid area of top side
            ENUM_IBSU_QUALITY_INVALID_AREA_LEFT,					///< Finger position is not valid area of left side
            ENUM_IBSU_QUALITY_INVALID_AREA_RIGHT,					///< Finger position is not valid area of right side
            ENUM_IBSU_QUALITY_INVALID_AREA_BOTTOM,					///< Finger position is not valid area of bottom side
        }

        /*
        ****************************************************************************************************
        * IBSU_LEOperationMode
        *
        * DESCRIPTION:
        *     Enumeration of light emitting (LE) file operation modes.
        ****************************************************************************************************
        */
        public enum IBSU_LEOperationMode
        {
            ENUM_IBSU_LE_OPERATION_AUTO,
            ENUM_IBSU_LE_OPERATION_ON,
            ENUM_IBSU_LE_OPERATION_OFF,
        }


        /*
        ****************************************************************************************************
        * IBSU_PlatenState
        *
        * DESCRIPTION:
        *     Enumeration of platen states.
        ****************************************************************************************************
        */
        public enum IBSU_PlatenState
        {
            ENUM_IBSU_PLATEN_CLEARD,
            ENUM_IBSU_PLATEN_HAS_FINGERS,
        }

        /*
        ****************************************************************************************************
        * IBSU_Events
        *
        * DESCRIPTION:
        *     Enumeration of events that can trigger callbacks.
        ****************************************************************************************************
        */
        public enum IBSU_Events
        {
            /* Callback when device count changes. */
            ENUM_IBSU_ESSENTIAL_EVENT_DEVICE_COUNT,

            /* Callback when communication with a device is interrupted. */
            ENUM_IBSU_ESSENTIAL_EVENT_COMMUNICATION_BREAK,

            /* Callback when a new preview image is available from a device. */
            ENUM_IBSU_ESSENTIAL_EVENT_PREVIEW_IMAGE,

            /* Callback for rolled print acquisition when rolling should begin. */
            ENUM_IBSU_ESSENTIAL_EVENT_TAKING_ACQUISITION,

            /* Callback for rolled print acquisition when rolling completes. */
            ENUM_IBSU_ESSENTIAL_EVENT_COMPLETE_ACQUISITION,

            /* Callback when result image is available for a capture (deprecated). */
            ENUM_IBSU_ESSENTIAL_EVENT_RESULT_IMAGE,

            /* Callback when a finger quality changes. */
            ENUM_IBSU_OPTIONAL_EVENT_FINGER_QUALITY,

            /* Callback when the finger count changes. */
            ENUM_IBSU_OPTIONAL_EVENT_FINGER_COUNT,

            /* Callback when initialization progress changes for a device. */
            ENUM_IBSU_ESSENTIAL_EVENT_INIT_PROGRESS,

            /* Callback when the platen was not clear when capture started or has since become clear. */
            ENUM_IBSU_OPTIONAL_EVENT_CLEAR_PLATEN_AT_CAPTURE,

            /* Callback when asynchronous device initialization completes. */
            ENUM_IBSU_ESSENTIAL_EVENT_ASYNC_OPEN_DEVICE,

            /* Callback when a warning message is generated. */
            ENUM_IBSU_OPTIONAL_EVENT_NOTIFY_MESSAGE,

            /* Callback when result image is available for a capture (with extended information). */
            ENUM_IBSU_ESSENTIAL_EVENT_RESULT_IMAGE_EX,

            /* Callback when key buttons are pressed */
            ENUM_IBSU_ESSENTIAL_EVENT_KEYBUTTON
        }

        /*
        ****************************************************************************************************
        * IBSU_LedType
        *
        * DESCRIPTION:
        *     Enumeration of LED types.
        ****************************************************************************************************
        */
        public enum IBSU_LedType
        {
            /* No LED field. */
            ENUM_IBSU_LED_TYPE_NONE,

            /* Two-scanner type (e.g., Watson). */
            ENUM_IBSU_LED_TYPE_TSCAN,

            /* four-scanner type (e.g., Kojak). */
            ENUM_IBSU_LED_TYPE_FSCAN
        }

        /*
        ****************************************************************************************************
        * IBSU_RollingState
        *
        * DESCRIPTION:
        *     Enumeration of rolling print acquisition states.
        ****************************************************************************************************
        */
        public enum IBSU_RollingState
        {
	        ENUM_IBSU_ROLLING_NOT_PRESENT,
	        ENUM_IBSU_ROLLING_TAKE_ACQUISITION,
            ENUM_IBSU_ROLLING_COMPLETE_ACQUISITION,
            ENUM_IBSU_ROLLING_RESULT_IMAGE,
        }

        /*
        ****************************************************************************************************
        * IBSU_OverlayShapePattern
        *
        * DESCRIPTION:
        *     Enumeration of the shape pattern to use for the overlay on client window.
        ****************************************************************************************************
        */
        public enum IBSU_OverlayShapePattern
        {
	        ENUM_IBSU_OVERLAY_SHAPE_RECTANGLE,
	        ENUM_IBSU_OVERLAY_SHAPE_ELLIPSE,
	        ENUM_IBSU_OVERLAY_SHAPE_CROSS,
	        ENUM_IBSU_OVERLAY_SHAPE_ARROW,
        }

        /*
        ****************************************************************************************************
        * IBSU_BeeperType
        *
        * DESCRIPTION:
        *     Enumeration of Beeper types.
        ****************************************************************************************************
        */
        public enum IBSU_BeeperType
        {
            /* No Beeper field. */
            ENUM_IBSU_BEEPER_TYPE_NONE,
            
            /* Monotone type. */
            ENUM_IBSU_BEEPER_TYPE_MONOTONE,
        } 

        /*
        ****************************************************************************************************
        * IBSU_BeepPattern
        *
        * DESCRIPTION:
        *     Enumeration of the beep pattern.
        ****************************************************************************************************
        */
        public enum IBSU_BeepPattern
        {
            ENUM_IBSU_BEEP_PATTERN_GENERIC,
            ENUM_IBSU_BEEP_PATTERN_REPEAT
        }

		/*
		****************************************************************************************************
		* IBSU_EncryptionMode
		*
		* DESCRIPTION:
		*     Enumeration of the encryption mode.
		****************************************************************************************************
		*/
		public enum IBSU_EncryptionMode
		{
		    /* Random Key generated by own library. */
		    ENUM_IBSU_ENCRYPTION_KEY_RANDOM,
		
		    /* Custom Key provided by user. */
		    ENUM_IBSU_ENCRYPTION_KEY_CUSTOM
		}

        /*
        ****************************************************************************************************
        * IBSU_CombineImageWhichHand
        *
        * DESCRIPTION:
        *     Enumeration of hand to use for combining two images into one.
        ****************************************************************************************************
        */
        public enum IBSU_CombineImageWhichHand
        {
            ENUM_IBSU_COMBINE_IMAGE_LEFT_HAND,
            ENUM_IBSU_COMBINE_IMAGE_RIGHT_HAND
        }

        /*
        ****************************************************************************************************
        * IBSU_HashType
        *
        * DESCRIPTION:
        *     Enumeration of the beep pattern.
        ****************************************************************************************************
        */
        public enum IBSU_HashType
        {
            /* SHA256 */
            ENUM_IBSU_HASH_TYPE_SHA256,

            /* Reserved */
            ENUM_IBSU_HASH_TYPE_RESERVED
        }

        /*
        ****************************************************************************************************
        * IBSM_ImageFormat
        *
        * DESCRIPTION:
        *     Enumeration of image formats to support IBScanMatcher integration.
        ****************************************************************************************************
        */
        public enum IBSM_ImageFormat
        {
	        IBSM_IMG_FORMAT_NO_BIT_PACKING=0,
	        IBSM_IMG_FORMAT_BIT_PACKED,
	        IBSM_IMG_FORMAT_WSQ,
	        IBSM_IMG_FORMAT_JPEG_LOSSY,
	        IBSM_IMG_FORMAT_JPEG2000_LOSSY,
	        IBSM_IMG_FORMAT_JPEG2000_LOSSLESS,
	        IBSM_IMG_FORMAT_PNG,
	        IBSM_IMG_FORMAT_UNKNOWN,
        }

        /*
        ****************************************************************************************************
        * IBSM_ImpressionType
        *
        * DESCRIPTION:
        *     Enumeration of image impression types to support IBScanMatcher integration.
        ****************************************************************************************************
        */
        public enum IBSM_ImpressionType
        {
	        IBSM_IMPRESSION_TYPE_LIVE_SCAN_PLAIN=0,
	        IBSM_IMPRESSION_TYPE_LIVE_SCAN_ROLLED,
	        IBSM_IMPRESSION_TYPE_NONLIVE_SCAN_PLAIN,
	        IBSM_IMPRESSION_TYPE_NONLIVE_SCAN_ROLLED,
	        IBSM_IMPRESSION_TYPE_LATENT_IMPRESSION,
	        IBSM_IMPRESSION_TYPE_LATENT_TRACING,
	        IBSM_IMPRESSION_TYPE_LATENT_PHOTO,
	        IBSM_IMPRESSION_TYPE_LATENT_LIFT,
	        IBSM_IMPRESSION_TYPE_LIVE_SCAN_SWIPE,
	        IBSM_IMPRESSION_TYPE_LIVE_SCAN_VERTICAL_ROLL,
	        IBSM_IMPRESSION_TYPE_LIVE_SCAN_PALM,
	        IBSM_IMPRESSION_TYPE_NONLIVE_SCAN_PALM,
	        IBSM_IMPRESSION_TYPE_LATENT_PALM_IMPRESSION,
	        IBSM_IMPRESSION_TYPE_LATENT_PALM_TRACING,
	        IBSM_IMPRESSION_TYPE_LATENT_PALM_PHOTO,
	        IBSM_IMPRESSION_TYPE_LATENT_PALM_LIFT,
	        IBSM_IMPRESSION_TYPE_LIVE_SCAN_OPTICAL_CONTRCTLESS_PLAIN=24,
	        IBSM_IMPRESSION_TYPE_OTHER=28,
	        IBSM_IMPRESSION_TYPE_UNKNOWN=29,
        }

        /*
        ****************************************************************************************************
        * IBSM_FingerPosition (Deprecated)
        *
        * DESCRIPTION:
        *     Enumeration of finger positions to support IBScanMatcher integration.
        ****************************************************************************************************
        */
        public enum IBSM_FingerPosition
        {
	        IBSM_FINGER_POSITION_UNKNOWN=0,
	        IBSM_FINGER_POSITION_RIGHT_THUMB,
	        IBSM_FINGER_POSITION_RIGHT_INDEX_FINGER,
	        IBSM_FINGER_POSITION_RIGHT_MIDDLE_FINGER,
	        IBSM_FINGER_POSITION_RIGHT_RING_FINGER,
	        IBSM_FINGER_POSITION_RIGHT_LITTLE_FINGER,
	        IBSM_FINGER_POSITION_LEFT_THUMB,
	        IBSM_FINGER_POSITION_LEFT_INDEX_FINGER,
	        IBSM_FINGER_POSITION_LEFT_MIDDLE_FINGER,
	        IBSM_FINGER_POSITION_LEFT_RING_FINGER,
	        IBSM_FINGER_POSITION_LEFT_LITTLE_FINGER,
	        IBSM_FINGER_POSITION_PLAIN_RIGHT_FOUR_FINGERS=13,
	        IBSM_FINGER_POSITION_PLAIN_LEFT_FOUR_FINGERS,
	        IBSM_FINGER_POSITION_PLAIN_THUMBS,
	        IBSM_FINGER_POSITION_UNKNOWN_PALM=20,
	        IBSM_FINGER_POSITION_RIGHT_FULL_PALM,
	        IBSM_FINGER_POSITION_RIGHT_WRITERS_PALM,
	        IBSM_FINGER_POSITION_LEFT_FULL_PALM,
	        IBSM_FINGER_POSITION_LEFT_WRITERS_PALM,
	        IBSM_FINGER_POSITION_RIGHT_LOWER_PALM,
	        IBSM_FINGER_POSITION_RIGHT_UPPER_PALM,
	        IBSM_FINGER_POSITION_LEFT_LOWER_PALM,
	        IBSM_FINGER_POSITION_LEFT_UPPER_PALM,
	        IBSM_FINGER_POSITION_RIGHT_OTHER,
	        IBSM_FINGER_POSITION_LEFT_OTHER,
	        IBSM_FINGER_POSITION_RIGHT_INTERDIGITAL,
	        IBSM_FINGER_POSITION_RIGHT_THENAR,
	        IBSM_FINGER_POSITION_RIGHT_HYPOTHENAR,
	        IBSM_FINGER_POSITION_LEFT_INTERDIGITAL,
	        IBSM_FINGER_POSITION_LEFT_THENAR,
	        IBSM_FINGER_POSITION_LEFT_HYPOTHENAR,
	        IBSM_FINGER_POSITION_RIGHT_INDEX_AND_MIDDLE=40,
	        IBSM_FINGER_POSITION_RIGHT_MIDDLE_AND_RING,
	        IBSM_FINGER_POSITION_RIGHT_RING_AND_LITTLE,
	        IBSM_FINGER_POSITION_LEFT_INDEX_AND_MIDDLE,
	        IBSM_FINGER_POSITION_LEFT_MIDDLE_AND_RING,
	        IBSM_FINGER_POSITION_LEFT_RING_AND_LITTLE,
	        IBSM_FINGER_POSITION_RIGHT_INDEX_AND_LEFT_INDEX,
	        IBSM_FINGER_POSITION_RIGHT_INDEX_AND_MIDDLE_AND_RING,
	        IBSM_FINGER_POSITION_RIGHT_MIDDLE_AND_RING_AND_LITTLE,
	        IBSM_FINGER_POSITION_LEFT_INDEX_AND_MIDDLE_AND_RING,
	        IBSM_FINGER_POSITION_LEFT_MIDDLE_AND_RING_AND_LITTLE,

            // HALF PALM
            IBSM_FINGER_POSITION_UNKNOWN_HALF_PALM,
            IBSM_FINGER_POSITION_RIGHT_HALF_PALM,
            IBSM_FINGER_POSITION_LEFT_HALF_PALM,
            IBSM_FINGER_POSITION_RIGHT_LOWER_HALF_PALM,
            IBSM_FINGER_POSITION_RIGHT_UPPER_HALF_PALM,
            IBSM_FINGER_POSITION_LEFT_LOWER_HALF_PALM,
            IBSM_FINGER_POSITION_LEFT_UPPER_HALF_PALM,

            IBSM_FINGER_POSITION_LEFT_CENTER_JOINT,
            IBSM_FINGER_POSITION_RIGHT_CENTER_JOINT,
            IBSM_FINGER_POSITION_LEFT_SIDE_JOINT,
            IBSM_FINGER_POSITION_RIGHT_SIDE_JOINT,
            IBSM_FINGER_POSITION_LEFT_ROLL_JOINT,
            IBSM_FINGER_POSITION_RIGHT_ROLL_JOINT,
            IBSM_FINGER_POSITION_LEFT_ROLL_UP,
            IBSM_FINGER_POSITION_RIGHT_ROLL_UP

        }

        /*
        ****************************************************************************************************
        * IBSM_CaptureDeviceTechID
        *
        * DESCRIPTION:
        *     Enumeration of capture device technology IDs to support IBScanMatcher integration.
        ****************************************************************************************************
        */
        public enum IBSM_CaptureDeviceTechID
        {
	        IBSM_CAPTURE_DEVICE_UNKNOWN_OR_UNSPECIFIED=0,
	        IBSM_CAPTURE_DEVICE_WHITE_LIGHT_OPTICAL_TIR,
	        IBSM_CAPTURE_DEVICE_WHITE_LIGHT_OPTICAL_DIRECT_VIEW_ON_PLATEN,
	        IBSM_CAPTURE_DEVICE_WHITE_LIGHT_OPTICAL_TOUCHLESS,
	        IBSM_CAPTURE_DEVICE_MONOCHROMATIC_VISIBLE_OPTICAL_TIR,
	        IBSM_CAPTURE_DEVICE_MONOCHROMATIC_VISIBLE_OPTICAL_DIRECT_VIEW_ON_PLATEN,
	        IBSM_CAPTURE_DEVICE_MONOCHROMATIC_VISIBLE_OPTICAL_TOUCHLESS,
	        IBSM_CAPTURE_DEVICE_MONOCHROMATIC_IR_OPTICAL_TIR,
	        IBSM_CAPTURE_DEVICE_MONOCHROMATIC_IR_OPTICAL_DIRECT_VIEW_ON_PLATEN,
	        IBSM_CAPTURE_DEVICE_MONOCHROMATIC_IR_OPTICAL_TOUCHLESS,
	        IBSM_CAPTURE_DEVICE_MULTISPECTRAL_OPTICAL_TIR,
	        IBSM_CAPTURE_DEVICE_MULTISPECTRAL_OPTICAL_DIRECT_VIEW_ON_PLATEN,
	        IBSM_CAPTURE_DEVICE_MULTISPECTRAL_OPTICAL_TOUCHLESS,
	        IBSM_CAPTURE_DEVICE_ELECTRO_LUMINESCENT,
	        IBSM_CAPTURE_DEVICE_SEMICONDUCTOR_CAPACITIVE,
	        IBSM_CAPTURE_DEVICE_SEMICONDUCTOR_RF,
	        IBSM_CAPTURE_DEVICE_SEMICONDUCTOR_THEMAL,
	        IBSM_CAPTURE_DEVICE_PRESSURE_SENSITIVE,
	        IBSM_CAPTURE_DEVICE_ULTRASOUND,
	        IBSM_CAPTURE_DEVICE_MECHANICAL,
	        IBSM_CAPTURE_DEVICE_GLASS_FIBER,
        }

        /*
        ****************************************************************************************************
        * IBSM_CaptureDeviceTypeID
        *
        * DESCRIPTION:
        *     Enumeration of capture device type IDs to support IBScanMatcher integration.
        ****************************************************************************************************
        */
        public enum IBSM_CaptureDeviceTypeID
        {
	        IBSM_CAPTURE_DEVICE_TYPE_ID_UNKNOWN=0x0000,
	        IBSM_CAPTURE_DEVICE_TYPE_ID_CURVE=0x1004,
	        IBSM_CAPTURE_DEVICE_TYPE_ID_WATSON=0x1005,
	        IBSM_CAPTURE_DEVICE_TYPE_ID_SHERLOCK=0x1010,
	        IBSM_CAPTURE_DEVICE_TYPE_ID_WATSON_MINI=0x1020,
	        IBSM_CAPTURE_DEVICE_TYPE_ID_COLUMBO=0x1100,
	        IBSM_CAPTURE_DEVICE_TYPE_ID_HOLMES=0x1200,
            IBSM_CAPTURE_DEVICE_TYPE_ID_KOJAK = 0x1300,
            IBSM_CAPTURE_DEVICE_TYPE_ID_FIVE0 = 0x1500
        }

        /*
        ****************************************************************************************************
        * IBSM_CaptureDeviceVendorID
        *
        * DESCRIPTION:
        *     Enumeration of capture device vendor IDs to support IBScanMatcher integration.
        ****************************************************************************************************
        */
        public enum IBSM_CaptureDeviceVendorID
        {
	        IBSM_CAPTURE_DEVICE_VENDOR_ID_UNREPORTED=0x0000,
	        IBSM_CAPTURE_DEVICE_VENDOR_INTEGRATED_BIOMETRICS=0x113F,
        }

        /*
        ****************************************************************************************************
        * IBSM_ImageData
        *
        * DESCRIPTION:
        *     Container for image information to support IBScanMatcher integration.
        ****************************************************************************************************
        */
        public struct IBSM_ImageData
        {
	        public IBSM_ImageFormat  			ImageFormat;
	        public IBSM_ImpressionType			ImpressionType;
	        public IBSM_FingerPosition			FingerPosition;
	        public IBSM_CaptureDeviceTechID	    CaptureDeviceTechID;
	        public ushort 				        CaptureDeviceVendorID;
	        public ushort 				        CaptureDeviceTypeID;
	        public ushort				        ScanSamplingX;
	        public ushort				        ScanSamplingY;
	        public ushort				        ImageSamplingX;
	        public ushort				        ImageSamplingY;
	        public ushort 				        ImageSizeX;
	        public ushort 				        ImageSizeY;
	        public byte				            ScaleUnit;
	        public byte				            BitDepth;
	        public uint				            ImageDataLength;
	        public IntPtr 				        ImageData;
        }

        /*
        ****************************************************************************************************
        * IBSM_TemplateVersion
        *
        * DESCRIPTION:
        *     Enumeration of Template versions to support ISO/ANSI integration.
        ****************************************************************************************************
        */
        public enum IBSM_TemplateVersion
        {
            IBSM_TEMPLATE_VERSION_IBISDK_0=0x00,    // TEMPLATE_OLD_VERSION
            IBSM_TEMPLATE_VERSION_IBISDK_1,         // TEMPLATE_MIX_VERSION
            IBSM_TEMPLATE_VERSION_IBISDK_2,         // TEMPLATE_FAST_VERSION
            IBSM_TEMPLATE_VERSION_IBISDK_3,         // Secuest 1nd Algorithm
            IBSM_TEMPLATE_VERSION_NEW_0=0x10        // IBSM_NEW_TEMPLATE
        }

        /*
        ****************************************************************************************************
        * IBSM_Template
        *
        * DESCRIPTION:
        *     Container for Template information to support ISO/ANSI integration.
        ****************************************************************************************************
        */
        public struct IBSM_Template
        {
            IBSM_TemplateVersion        Version;
            uint		        FingerPosition;
            IBSM_ImpressionType         ImpressionType;
            IBSM_CaptureDeviceTechID    CaptureDeviceTechID;
            ushort              CaptureDeviceVendorID;  // Integrated Biometrics ( 0x007B )
            ushort              CaptureDeviceTypeID;    // WATSON : 0x0001, SHERLOCK : 0x0010, WATSON_MINI : 0x0020, COLUMBO : 0x0030, HOLMES : 0x0040
            ushort              ImageSamplingX;         // 500 dpi
            ushort              ImageSamplingY;         // 500 dpi
            ushort              ImageSizeX;
            ushort              ImageSizeY;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = IBSU_MAX_MINUTIAE_SIZE)]
            uint[]              Minutiae;
            uint                Reserved;
        }
        

        /*
        ****************************************************************************************************
        * IBSM_ImageData
        *
        * DESCRIPTION:
        *     Enumeration of ISO/ANSI Standard Format types to support ISO/ANSI integration.
        ****************************************************************************************************
        */
        public enum IBSM_StandardFormat
        {
            /* ISO 19794-2:2005 */
            ENUM_IBSM_STANDARD_FORMAT_ISO_19794_2_2005,

	        /* ISO 19794-4:2005 */
            ENUM_IBSM_STANDARD_FORMAT_ISO_19794_4_2005,

	        /* ISO 19794-2:2011 */
            ENUM_IBSM_STANDARD_FORMAT_ISO_19794_2_2011,

	        /* ISO 19794-4:2011 */
            ENUM_IBSM_STANDARD_FORMAT_ISO_19794_4_2011,

	        /* ANSI/INCITS 378:2004 */
            ENUM_IBSM_STANDARD_FORMAT_ANSI_INCITS_378_2004,

	        /* ANSI/INCITS 381:2004 */
            ENUM_IBSM_STANDARD_FORMAT_ANSI_INCITS_381_2004,
        }

        /*
        ****************************************************************************************************
        * IBSM_StandardFormatData
        *
        * DESCRIPTION:
        *     Container for Standard format data information to support ISO/ANSI integration.
        ****************************************************************************************************
        */
        public struct IBSM_StandardFormatData
        {
            /* Pointer to data buffer.  If this structure is supplied by a callback function, this pointer 
             * must not be retained; the data should be copied to an application buffer for any processing
             * after the callback returns. */
            public IntPtr   Data;
            
            /* Data Length (in bytes). */
            public uint     DataLength;
            
	        /* Standard Format (ISO 19794-2:2005, ISO 19794-4:2005, ISO 19794-2:2011, ISO 19794-4:2011, ANSI/INCITS 378:2004, ANSI/INCITS 381:2004) */
	        public IBSM_StandardFormat     Format;
        }

    }
}
