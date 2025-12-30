/**
****************************************************************************************************
* @file
* IBScanUltimateApi_err.h
*
* @brief
*       Error codes for IBScanUltimate.
*
* @author
*       Integrated Biometrics, LLC
*
* @copyright
*       Copyright (c) Integrated Biometrics, 2009-2022 \n
*       http://www.integratedbiometrics.com
*
* @page page_File_Revision_History Revision History for the files
* @section section_IBScanUltimate_err IBScanUltimateApi_err.h
* @li @par  2023/02/16  3.9.3   Added error codes for KISA Matcher feature
*                               (IBSU_WRN_MATCHER_NO_MATCH, IBSU_WRN_MATCHER_ALREADY_REGISTERED)
* @li @par  2021/06/23  3.7.2
*								Added error codes
*								(IBSU_ERR_DEVICE_LOCK_INVALID_SERIAL_FORMAT, IBSU_ERR_PAD_PROPERTY_DISABLED)
* @li @par  2020/10/06  3.7.0
*								Added error codes
*								(IBSU_ERR_DEVICE_LOCK_INVALID_BUFF, IBSU_ERR_DEVICE_LOCK_INFO_EMPTY,
*								IBSU_ERR_DEVICE_LOCK_INFO_NOT_MATCHED, IBSU_ERR_DEVICE_LOCK_INVALID_CHECKSUM,
*								IBSU_ERR_DEVICE_LOCK_INVALID_KEY, IBSU_ERR_DEVICE_LOCK_LOCKED,
*								IBSU_ERR_DEVICE_LOCK_ILLEGAL_DEVICE)
* @li @par  2020/04/01  3.3.0
*								Added warning codes
*								(IBSU_WRN_ROLLING_SLIP_DETECTED, IBSU_WRN_SPOOF_INIT_FAILED)
* @li @par  2020/01/09  3.2.0
*								Added warning codes
*								(IBSU_WRN_SPOOF_DETECTED)
* @li @par  2018/04/27  2.0.1
*                               Added error codes
*                               (IBSU_ERR_DEVICE_INVALID_CALIBRATION_DATA, IBSU_ERR_DUPLICATE_EXTRACTION_FAILED,
*                               IBSU_ERR_DUPLICATE_ALREADY_USED, IBSU_ERR_DUPLICATE_SEGMENTATION_FAILED,
*                               IBSU_ERR_DUPLICATE_MATCHING_FAILED)
* @li @par  2017/06/16  1.9.8
*                               Added error codes
*                               (IBSU_ERR_DEVICE_NEED_CALIBRATE_TOF, IBSU_WRN_MULTIPLE_FINGERS_DURING_ROLL)
* @li @par  2017/04/27  1.9.7
*                               Added warning codes
*                               (IBSU_WRN_QUALITY_INVALID_AREA, IBSU_WRN_INVALID_BRIGHTNESS_FINGERS,
*                               IBSU_WRN_WET_FINGERS)
* @li @par  2015/04/07  1.8.4
*                               Added error codes
*                               (IBSU_ERR_LIBRARY_UNLOAD_FAILED )
*                               \n
*                               Added warning codes
*                               (IBSU_WRN_ALREADY_ENHANCED_IMAGE )
* @li @par  2014/09/17  1.8.1
*                               Added error codes for JPEG2000 and PNG
*                               (IBSU_ERR_NBIS_PNG_ENCODE_FAILED,IBSU_ERR_NBIS_JP2_ENCODE_FAILED )
* @li @par  2014/07/23  1.8.0
*                               Added error codes for WSQ
*                               (IBSU_ERR_NBIS_WSQ_ENCODE_FAILED,IBSU_ERR_NBIS_WSQ_DECODE_FAILED )
* @li @par  2014/02/25  1.7.1
*                               Added warning to check incorrect fingers/smear
*                               (IBSU_WRN_ROLLING_SHIFTED_HORIZONTALLY,IBSU_WRN_ROLLING_SHIFTED_VERTICALLY )
* @li @par  2013/10/14  1.7.0
*                               Added error codes to check update firmware, invalid overlay handle
*                               (IBSU_ERR_DEVICE_NEED_UPDATE_FIRMWARE,IBSU_ERR_INVALID_OVERLAY_HANDLE )
*                               \n
*                               Added warning codes to deprecate API functions and detect no finger/
*                               incorrect fingers/smear in result image.
*                               (IBSU_WRN_API_DEPRECATED, IBSU_WRN_NO_FINGER, IBSU_WRN_INCORRECT_FINGERS,
*                               IBSU_WRN_ROLLING_SMEAR)
* @li @par  2013/08/03  1.6.9
*                               Reformatted.
* @li @par  2013/03/20  1.6.0
*                               Added error and warning codes to support IBScanMatcher integration
*                               (IBSU_ERR_NBIS_NFIQ_FAILED, IBSU_WRN_EMPTY_IBSM_RESULT_IMAGE)
* @li @par  2012/11/06  1.4.1
*                               Added warning codes (IBSU_WRN_ROLLING_NOT_RUNNING)
* @li @par  2012/09/05  1.3.0
*                               Added error and warning codes (IBSU_ERR_DEVICE_ENABLED_POWER_SAVE_MODE,
*                               IBSU_WRN_CHANNEL_IO_SLEEP_STATUS, IBSU_WRN_BGET_IMAGE)
* @li @par  2012/04/06  1.0.0
*                               Created.
****************************************************************************************************
*/

#pragma once

/**
****************************************************************************************************
* @defgroup group_Def_Error_Codes_General Definition - Error Code - General
* @brief    Error Code from 0 to -11
* @{
****************************************************************************************************
*/
/** Function completed successfully. */
#define IBSU_STATUS_OK                                0
/** Invalid parameter value. */
#define IBSU_ERR_INVALID_PARAM_VALUE                 -1
/** Insufficient memory. */
#define IBSU_ERR_MEM_ALLOC                           -2
/** Requested functionality isn't supported. */
#define IBSU_ERR_NOT_SUPPORTED                       -3
/** File (USB handle, pipe, or image file) open failed. */
#define IBSU_ERR_FILE_OPEN                           -4
/** File (USB handle, pipe, or image file) read failed. */
#define IBSU_ERR_FILE_READ                           -5
/** Failure due to a locked resource. */
#define IBSU_ERR_RESOURCE_LOCKED                     -6
/** Failure due to a missing resource (e.g. DLL file). */
#define IBSU_ERR_MISSING_RESOURCE                    -7
/** Invalid access pointer address. */
#define IBSU_ERR_INVALID_ACCESS_POINTER              -8
/** Thread creation failed. */
#define IBSU_ERR_THREAD_CREATE                       -9
/** Generic command execution failed. */
#define IBSU_ERR_COMMAND_FAILED                     -10
/** The library unload failed. */
#define IBSU_ERR_LIBRARY_UNLOAD_FAILED              -11
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/

/**
****************************************************************************************************
* @defgroup group_Def_Error_Codes_Low_Level_IO Definition - Error Code - Low Level I/O
* @brief    Error Code from -100 to -107
* @{
****************************************************************************************************
*/
/** Command execution failed. */
#define IBSU_ERR_CHANNEL_IO_COMMAND_FAILED         -100
/** Input communication failed. */
#define IBSU_ERR_CHANNEL_IO_READ_FAILED            -101
/** Output communication failed. */
#define IBSU_ERR_CHANNEL_IO_WRITE_FAILED           -102
/** Input command execution timed out, but device communication is alive. */
#define IBSU_ERR_CHANNEL_IO_READ_TIMEOUT           -103
/** Output command execution timed out, but device communication is alive. */
#define IBSU_ERR_CHANNEL_IO_WRITE_TIMEOUT          -104
/** Unexpected communication failed. (Only used on IBTraceLogger.) */
#define IBSU_ERR_CHANNEL_IO_UNEXPECTED_FAILED      -105
/** I/O handle state is invalid; reinitialization (close then open) required. */
#define IBSU_ERR_CHANNEL_IO_INVALID_HANDLE         -106
/** I/O pipe index is invalid; reinitialization (close then open) required. */
#define IBSU_ERR_CHANNEL_IO_WRONG_PIPE_INDEX       -107

/**
****************************************************************************************************
* @}
****************************************************************************************************
*/

/**
****************************************************************************************************
* @defgroup group_Def_Error_Codes_Device_Related Definition - Error Code - Device Related
* @brief    Error Code from -200 to -222
* @{
****************************************************************************************************
*/
/** Device communication failed. */
#define IBSU_ERR_DEVICE_IO                         -200
/** No device is detected/active. */
#define IBSU_ERR_DEVICE_NOT_FOUND                  -201
/** No matching device is detected. */
#define IBSU_ERR_DEVICE_NOT_MATCHED                -202
/** Initialization failed because it is in use by another thread/process. */
#define IBSU_ERR_DEVICE_ACTIVE                     -203
/** Device needs to be initialized. */
#define IBSU_ERR_DEVICE_NOT_INITIALIZED            -204
/** Device state is invalid; reinitialization (exit then initialization) required. */
#define IBSU_ERR_DEVICE_INVALID_STATE              -205
/** Another thread is currently using device functions. */
#define IBSU_ERR_DEVICE_BUSY                       -206
/** No hardware support for requested function. */
#define IBSU_ERR_DEVICE_NOT_SUPPORTED_FEATURE      -207
/** The license is invalid or does not match the device. */
#define IBSU_ERR_INVALID_LICENSE                   -208
/** Device is connected to a full-speed USB port but high-speed is required. */
#define IBSU_ERR_USB20_REQUIRED                    -209
/** Device has enabled the power save mode. */
#define IBSU_ERR_DEVICE_ENABLED_POWER_SAVE_MODE    -210
/** Need to update firmware. */
#define IBSU_ERR_DEVICE_NEED_UPDATE_FIRMWARE       -211
/** Need to calibrate TOF. */
#define IBSU_ERR_DEVICE_NEED_CALIBRATE_TOF         -212
/** Invalid calibration data from the device. */
#define IBSU_ERR_DEVICE_INVALID_CALIBRATION_DATA   -213
/** Device is required to connect higher SDK version for running */
#define IBSU_ERR_DEVICE_HIGHER_SDK_REQUIRED	       -214
/** The Lock-info Buff is not valid.*/
#define IBSU_ERR_DEVICE_LOCK_INVALID_BUFF	       -215
/** The Lock-info Buff is empty.*/
#define IBSU_ERR_DEVICE_LOCK_INFO_EMPTY            -216
/** The Customer Key to the devices is not registered.*/
#define IBSU_ERR_DEVICE_LOCK_INFO_NOT_MATCHED      -217
/** Checksums between buffer and calculated are different. */
#define IBSU_ERR_DEVICE_LOCK_INVALID_CHECKSUM	   -218
/** When Customer key is invalid. */
#define IBSU_ERR_DEVICE_LOCK_INVALID_KEY	       -219
/** The device is locked. */
#define IBSU_ERR_DEVICE_LOCK_LOCKED	               -220
/** The device is not valid from the license file */
#define IBSU_ERR_DEVICE_LOCK_ILLEGAL_DEVICE        -221
/** The serial number format is not valid */
#define IBSU_ERR_DEVICE_LOCK_INVALID_SERIAL_FORMAT -222
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/

/**
****************************************************************************************************
* @defgroup group_Def_Error_Code_Image_Capture_Related Definition - Error Code - Image Capture Related
* @brief    Error Code from -300 to -308
* @{
****************************************************************************************************
*/
/** Image acquisition failed. */
#define IBSU_ERR_CAPTURE_COMMAND_FAILED            -300
/** Stop capture failed. */
#define IBSU_ERR_CAPTURE_STOP                      -301
/** Timeout during capturing. */
#define IBSU_ERR_CAPTURE_TIMEOUT                   -302
/** A capture is still running. */
#define IBSU_ERR_CAPTURE_STILL_RUNNING             -303
/** A capture is not running. */
#define IBSU_ERR_CAPTURE_NOT_RUNNING               -304
/** Capture mode is not valid or not supported. */
#define IBSU_ERR_CAPTURE_INVALID_MODE              -305
/** Generic algorithm processing failure. */
#define IBSU_ERR_CAPTURE_ALGORITHM                 -306
/** Image processing failure at rolled finger print processing. */
#define IBSU_ERR_CAPTURE_ROLLING                   -307
/** No roll start detected within a defined timeout period. */
#define IBSU_ERR_CAPTURE_ROLLING_TIMEOUT           -308
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/

/**
****************************************************************************************************
* @defgroup group_Def_Error_Code_Client_Window_Related Definition - Error Code - Client Window Related
* @brief    Error Code from -400 to -402
* @{
****************************************************************************************************
*/
/** Generic client window failure. */
#define IBSU_ERR_CLIENT_WINDOW                     -400
/** No client window has been created. */
#define IBSU_ERR_CLIENT_WINDOW_NOT_CREATE          -401
/** Invalid overlay handle. */
#define IBSU_ERR_INVALID_OVERLAY_HANDLE            -402
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/

/**
****************************************************************************************************
* @defgroup group_Def_Error_Code_NIBS_Related Definition - Error Code - NBIS Related
* @brief    Error Code from -500 to -504
* @{
****************************************************************************************************
*/
/** Getting NFIQ score failed. */
#define IBSU_ERR_NBIS_NFIQ_FAILED                  -500
/** WSQ encode failed. */
#define IBSU_ERR_NBIS_WSQ_ENCODE_FAILED            -501
/** WSQ decode failed. */
#define IBSU_ERR_NBIS_WSQ_DECODE_FAILED            -502
/** PNG encode failed. */
#define IBSU_ERR_NBIS_PNG_ENCODE_FAILED            -503
/** JP2 encode failed. */
#define IBSU_ERR_NBIS_JP2_ENCODE_FAILED            -504
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/


/**
****************************************************************************************************
* @defgroup group_Def_Error_Code_Matcher_Related Definition - Error Code - Matcher Related
* @brief    Error Code from -600 to -603
* @{
****************************************************************************************************
*/
/** The extraction from the fingerimage failed in IBSU_ADDFingerImage and DLL_IsFingerDuplicated */
#define IBSU_ERR_DUPLICATE_EXTRACTION_FAILED		-600
/** The image of the fingerposition is already in use. in IBSU_ADDFingerImage */
#define IBSU_ERR_DUPLICATE_ALREADY_USED				-601
/** Found segment fingercounts are not two or more in IBSU_IsValidFingerGeometry */
#define IBSU_ERR_DUPLICATE_SEGMENTATION_FAILED		-602
/** Found small extractions in IBSM_MatchingTemplate */
#define IBSU_ERR_DUPLICATE_MATCHING_FAILED			-603
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/

/**
****************************************************************************************************
* @defgroup group_Def_Error_Code_PAD_Related Definition - Error Code - PAD Related
* @brief    Error Code -700
* @{
****************************************************************************************************
*/
/** PAD Property is not enabled. */
#define IBSU_ERR_PAD_PROPERTY_DISABLED		        -700
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/

/**
****************************************************************************************************
* @defgroup group_Def_Error_Code_ISO_ANSI Definition - Error Code - ISO/ANSI
* @brief    Error Code -800
* @{
****************************************************************************************************
*/
/** Standard data(ISO or ANSI) is incorrect. */
#define IBSU_ERR_INCORRECT_STANDARD_FORMAT	        -800
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/


/**
****************************************************************************************************
* @defgroup group_Def_Warning_Code_General Definition - Warning Code - General
* @brief    General Warning Codes 
* @{
****************************************************************************************************
*/
/** Missing an image frame. (Only used on IBTraceLogger.) */
#define IBSU_WRN_CHANNEL_IO_FRAME_MISSING           100
/** Camera work is wrong. reset is requied (Only used on IBTraceLogger.) */
#define IBSU_WRN_CHANNEL_IO_CAMERA_WRONG            101
/** Camera work is wrong. IO Write failure or IO Read failure */
#define IBSU_WRN_CHANNEL_IO_SLEEP_STATUS            102
/** Device firmware version outdated. */
#define IBSU_WRN_OUTDATED_FIRMWARE                  200
/** Device/component has already been initialized and is ready to be used. */
#define IBSU_WRN_ALREADY_INITIALIZED                201
/** API function was deprecated. */
#define IBSU_WRN_API_DEPRECATED                     202
/** Image is already enhanced. */
#define IBSU_WRN_ALREADY_ENHANCED_IMAGE             203
/** Device still has not received the first image frame. */
#define IBSU_WRN_BGET_IMAGE                         300
/** Rolling has not started. */
#define IBSU_WRN_ROLLING_NOT_RUNNING                301
/** No finger detected in result image. */
#define IBSU_WRN_NO_FINGER                          302
/** Incorrect fingers detected in result image. */
#define IBSU_WRN_INCORRECT_FINGERS                  303
/** Empty result image. */
#define IBSU_WRN_EMPTY_IBSM_RESULT_IMAGE            400
/** When a finger doesn't meet image brightness criteria */
#define IBSU_WRN_INVALID_BRIGHTNESS_FINGERS			600
/** Wet finger detected */
#define IBSU_WRN_WET_FINGERS						601
/** Detected multiple fingers during roll */
#define IBSU_WRN_MULTIPLE_FINGERS_DURING_ROLL		602
/** Detected spoof finger */
#define IBSU_WRN_SPOOF_DETECTED						603
/** Detected slip finger */
#define IBSU_WRN_ROLLING_SLIP_DETECTED				604
/** Spoof initalize failed */
#define IBSU_WRN_SPOOF_INIT_FAILED					605
/* No match from Matcher DB */
#define IBSU_WRN_MATCHER_NO_MATCH			        700  
/* Finger is already registered */
#define IBSU_WRN_MATCHER_ALREADY_REGISTERED			701  
/*
****************************************************************************************************
* @}
****************************************************************************************************
*/


/**
****************************************************************************************************
* @defgroup group_Def_Warning_Code_Smear Definition - Warning Code - Smear
* @brief    Warning Codes related to Smear detection from roll-finger image
* @{
****************************************************************************************************
*/
/** Smear detected in rolled result image. */
#define IBSU_WRN_ROLLING_SMEAR                      304
/** Rolled finger was shifted horizontally. */
#define IBSU_WRN_ROLLING_SHIFTED_HORIZONTALLY		(IBSU_WRN_ROLLING_SMEAR | 1)
/** Rolled finger was shifted vertically. */
#define IBSU_WRN_ROLLING_SHIFTED_VERTICALLY			(IBSU_WRN_ROLLING_SMEAR | 2)
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/


/**
****************************************************************************************************
* @defgroup group_Def_Warning_Code_Invalid_Area Definition - Warning Code - Invalid Area
* @brief    Warning Codes related to invalid area which occurs when user put fingers on the area close to the edge of the active area.
* @{
****************************************************************************************************
*/
/** When a finger is located in the invalid area */
#define IBSU_WRN_QUALITY_INVALID_AREA	    		512
/** Finger was located on the horizontal invalid area */
#define IBSU_WRN_QUALITY_INVALID_AREA_HORIZONTALLY			(IBSU_WRN_QUALITY_INVALID_AREA | 1)
/** Finger was located on the vertical invalid area */
#define IBSU_WRN_QUALITY_INVALID_AREA_VERTICALLY			(IBSU_WRN_QUALITY_INVALID_AREA | 2)
/**
****************************************************************************************************
* @}
****************************************************************************************************
*/
