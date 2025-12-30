/**
****************************************************************************************************
* @file
*       IBScanUltimateApi.h
*
* @brief
*       API functions for IBScanUltimate.
*
* @author
*       Integrated Biometrics, LLC
*
* @copyright
*       Copyright (c) Integrated Biometrics, 2009-2022 \n
*       http://www.integratedbiometrics.com
*
* @page page_File_Revision_History Revision History for the files
* @section section_IBScanUltimateApi IBScanUltimateApi.h
* @li @par  2024/05/23  4.0.1
*                               IBSU_UnloadLibrary() Not supported. Return value changed to IBSU_ERR_NOT_SUPPORT(-3) \n
*                               IBSU_SetEncryptionKey() Not supported. Return value changed to IBSU_ERR_NOT_SUPPORT(-3) \n
*                               Deprecated API function of IBSU_GetImageWidth() \n
*                               Deprecated API function of IBSU_CheckWetFinger() \n
*                               
* @li @par  2022/04/13  3.9.0
*                               Added API function to ISO(ISO 19794, ANSI/INCITS) template create
*                               (IBSU_ConvertImageToISOANSI)  \n
*                               Re-added IBSU_GetIBSM_ResultImageInfo() to support IBSU_ConvertImageToISOANSI API \n
* @li @par  2021/12/17  3.8.0
*                               Removed the code for WinCE
* @li @par  2021/08/04  3.7.2
*                               Added API function to enhance NFIQ function and support new Spoof(PAD) function
*                               (IBSU_GetNFIQScoreEx, IBSU_IsSpoofFingerDetected)
* @li @par  2020/10/06  3.7.0
*                               Added API function to support new locking feature
*                               (IBSU_SetCustomerKey, IBSU_GetErrorString)
* @li @par  2019/06/21  3.0.0
*                               Added API function
*                               (IBSU_SetEncryptionKey)
* @li @par  2019/02/19  2.1.0
*                               Added API function
*                               (IBSU_GetRequiredSDKVersion)
* @li @par  2018/04/27  2.0.1
*                               Added API function to improve the display speed on Embedded System
*                               (IBSU_RemoveFingerImage(), IBSU_AddFingerImage(), IBSU_IsFingerDuplicated(),
*                               IBSU_IsValidFingerGeometry()) \n
*                               Deprecated the API function IBScanMater(IBSM) 
*                               (IBSU_GetIBSM_ResultImageInfo())
* @li @par  2018/03/06  2.0.0
*                               Added API function to improve display speed on Embedded System
*                               (IBSU_GenerateDisplayImage())
* @li @par  2017/06/17  1.9.8
*                               Added API function to support an improved feature for CombineImage
*                               (IBSU_CheckWetFinger(), IBSU_BGetRollingInfoEx(), IBSU_GetImageWidth(),
*                               IBSU_IsWritableDirectory())
* @li @par  2017/04/27  1.9.7
*                               Added API function to support an improved feature for CombineImage
*                               (IBSU_CombineImageEx())
* @li @par  2015/12/11  1.9.0
*                               Added an API function to support Kojak devices
*                               (IBSU_GetOperableBeeper(), IBSU_SetBeeper())
* @li @par  2015/08/05  1.8.5
*                               Added an API function to combine two images into one
*                               (IBSU_CombineImage())
* @li @par  2015/04/07  1.8.4
*                               Added an API function to unload the library manually 
*                               (IBSU_UnloadLibrary())
* @li @par  2015/03/04  1.8.3
*                               Reformatted to support UNICODE for WinCE
*                               \n
*                               Added an API function relelated to ClientWindow
*                               (IBSU_RedrawClientWindow())
*                               \n
*                               Bug Fixed, and added a new parameter (pitch) to WSQ functions
*                               (IBSU_WSQEncodeMem(), IBSU_WSQEncodeToFile(), IBSU_WSQDecodeMem(),
*                               IBSU_WSQDecodeFromFile())
* @li @par  2014/09/17  1.8.1
*                               Added API functions relelated to JPEG2000 and PNG
*                               (IBSU_SavePngImage(), IBSU_SaveJP2Image())
* @li @par  2014/07/23  1.8.0
*                               Added API functions relelated to WSQ
*                               (IBSU_WSQEncodeMem(), IBSU_WSQEncodeToFile(), IBSU_WSQDecodeMem(),
*                               IBSU_WSQDecodeFromFile(), IBSU_FreeMemory())
* @li @par  2013/10/14  1.7.0
*                               Added API functions to acquire an image from a device (blocking for resultEx),
                                deregister a callback function, show (or remove) an overlay object, show (or remove)
                                all overlay objects, add an overlay text, modify an existing overlay text, add an overlay line,
                                modify an existing line, add an overlay quadrangle, modify an existing quadrangle,
                                add an overlay shape, modify an overlay shape, save image to bitmap memory 
*                               (IBSU_BGetImageEx(), IBSU_ReleaseCallbacks(), IBSU_ShowOverlayObject,
*                               IBSU_ShowAllOverlayObject(), IBSU_RemoveOverlayObject(), IBSU_RemoveAllOverlayObject(),
*                               IBSU_AddOverlayText(), IBSU_ModifyOverlayText(), IBSU_AddOverlayLine(),
*                               IBSU_ModifyOverlayLine(), IBSU_AddOverlayQuadrangle(), IBSU_ModifyOverlayQuadrangle(),
*                               IBSU_AddOverlayShape(), IBSU_ModifyOverlayShape(), IBSU_SaveBitmapMem())
* @li @par  2013/08/03  1.6.9
*                               Reformatted.
* @li @par  2013/04/05  1.6.2
*                               Added an API function to enable or disable trace log at run-time
*                               (IBSU_EnableTraceLog()).
* @li @par  2013/03/20  1.6.0
*                               Added an API function to support IBScanMatcher integration
*                               (IBSU_GetIBSM_ResultImageInfo(), IBSU_GetNFIQScore()).
* @li @par  2012/11/06  1.4.1
*                               Added rolling and extended open API functions 
*								(IBSU_BGetRollingInfo(), IBSU_OpenDeviceEx()).
* @li @par  2012/05/29  1.1.0
*                               Added blocking API functions
*                               (IBSU_AsyncOpenDevice(), IBSU_BGetImage(),
*                            	IBSU_BGetInitProgress(), IBSU_BGetClearPlatenAtCapture()).
* @li @par  2012/04/06  1.0.0
*                               Created.
****************************************************************************************************
*/

#pragma once

#include "IBScanUltimateApi_defs.h"
#include "IBScanUltimateApi_err.h"
#include "IBScanUltimate.h"
#include "ReservedApi.h"

#ifdef __cplusplus
extern "C" {
#endif

    /**
    ****************************************************************************************************
    * @defgroup group_API_Device_Open_Close API - Device - Open/Close
    * @brief    These API functions are related to opening or closing a device
    * @{
    * @page page_API_Device_Open_Close
    ****************************************************************************************************
    */

    /**
    ****************************************************************************************************
    * @brief
    *           Initializes a device, given a particular device index.
    *
    * @param    [in] deviceIndex    Zero-based device index for device to init.
    * @param    [out] pHandle       Function returns device handle to be used for subsequent function calls.
    *                               Memory must be provided by caller.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *
    * @remark   Any initialized device must be released before closing the host application!
    *           (call IBSU_CloseDevice() or IBSU_CloseAllDevice())
    ****************************************************************************************************
    */
    int WINAPI IBSU_OpenDevice
        (const int  deviceIndex,
        int* pHandle);

    /**
    ****************************************************************************************************
    * @brief
    *           Extension of initialize device(fast mode), given by a particular device index.
    *
    * @param    [in] deviceIndex        Zero-based device index for device to init.
    * @param    [in] uniformityMaskPath Uniformity mask path in your computer.
    *                                   If the file does not exist or path differs, the DLL makes a new file in path.
    * @param    [in] ayncnOpen          async open device(TRUE) or sync open device(FALSE).
    * @param    [out] pHandle           Function returns device handle to be used for subsequent function calls.
    *                                   Memory must be provided by caller.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *
    * @remark   Any initialized device must be released before closing the host application!
    *           (call IBSU_CloseDevice() or IBSU_CloseAllDevice())
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_OpenDeviceEx
        (const int  deviceIndex,
        LPCSTR      uniformityMaskPath,
        const BOOL  asyncOpen,
        int* pHandle);

#else  // UNICODE
    int WINAPI IBSU_OpenDeviceExW
        (const int      deviceIndex,
        const wchar_t* uniformityMaskPath,
        const BOOL      asyncOpen,
        int* pHandle);

#define IBSU_OpenDeviceEx IBSU_OpenDeviceExW
#endif 

    /**
    ****************************************************************************************************
    * @brief
    *           Asynchronous Initialize device, given by a particular device index.
    *
    * @param    [in] deviceIndex    Zero-based device index for device to init.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *
    * @remark   Any initialized device must be released before closing the host application!
    *           (call IBSU_CloseDevice() or IBSU_CloseAllDevice())
    ****************************************************************************************************
    */
    int WINAPI IBSU_AsyncOpenDevice
        (const int  deviceIndex);

    /**
    ****************************************************************************************************
    * @brief
    *           Releases a device (by device handle).
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle     Device handle obtained by OpenDevice functions.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *           ::IBSU_ERR_RESOURCE_LOCKED: A callback is still active. \n
    *           ::IBSU_ERR_DEVICE_NOT_INITIALIZED: device(s) in use are identified by index;
    *           so either device has already been released or is unknown. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_CloseDevice
        (const int  handle);

    /**
    ****************************************************************************************************
    * @brief
    *           Releases all currently initialized devices (particular device handle not needed).
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *           ::IBSU_ERR_RESOURCE_LOCKED: A callback is still active.
    *
    * @remark   This function should be called upon closing the host application to free allocated resources
    ****************************************************************************************************
    */
    int WINAPI IBSU_CloseAllDevice();

    /**
    ****************************************************************************************************
    * @brief
    *           Check if a particular device is opened/initialized.
    *
    * @param    [in] handle     Device handle obtained by OpenDevice functions.
    *
    * @return
    *           ::IBSU_STATUS_OK, if device is ready to be used.
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *           ::IBSU_ERR_INVALID_PARAM_VALUE: if handle value is out of valid range.
    *           ::IBSU_ERR_DEVICE_NOT_INITIALIZED: device is not initialized.
    *           ::IBSU_ERR_DEVICE_IO: device is initialized, but there was a communication problem.
    ****************************************************************************************************
    */
    int WINAPI IBSU_IsDeviceOpened
        (const int  handle);

    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */

    /**
    ****************************************************************************************************
    * @defgroup group_API_Device_Information API - Device - Infomation
    * @brief    These API functions are related to retrieving device information
    * @{
    * @page page_API_Device_Information
    ****************************************************************************************************
    */

    /**
    ****************************************************************************************************
    * @brief
    *           Retrieve the number of connected IB USB devices.
    *           Device count function trigger for Android SDK (Reduce polling count)
    *
    * @param    [out] pDeviceCount  Number of connected devices. Memory must be provided by caller.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetDeviceCount
        (int* pDeviceCount);

  
	/**
	****************************************************************************************************
	* @brief
	*           Retrieve detailed device information about a particular scanner by its logical index.
	*
	* @param    [in] deviceIndex    Zero-based index for device to lookup.
	* @param    [out] pDeviceDesc   Basic device information. Memory must be provided by caller.
	*
	* @return
	*           ::IBSU_STATUS_OK, if successful.\n
	*           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
	****************************************************************************************************
	*/
#ifndef WINCE
    int WINAPI IBSU_GetDeviceDescription
        (const int          deviceIndex,
        IBSU_DeviceDesc* pDeviceDesc);

#else  // UNICODE
    int WINAPI IBSU_GetDeviceDescriptionW
        (const int          deviceIndex,
        IBSU_DeviceDescW* pDeviceDesc);

#define IBSU_GetDeviceDescription IBSU_GetDeviceDescriptionW
#endif 

    /**
    ****************************************************************************************************
    * IBSU_GetRequiredSDKVersion()
    *
    * @brief
    *     Get minimum SDK version required for running.
    *
    * @param    [in] deviceIndex					Device index.
    * @param    [out] deviceIndex         Minimum SDK Version to be returned.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetRequiredSDKVersion
        (const int  deviceIndex,
        LPSTR       minSDKVersion);

    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */

    /**
    ****************************************************************************************************
    * @defgroup group_API_Device_Property API - Device - Property
    * @brief    These API functions are related to Set or Get properties of a device.
    * @{
    * @page page_API_Device_Property
    ****************************************************************************************************
    */
    /**
   ****************************************************************************************************
   * @brief
   *           Set a device's property value (by handle).
   *
   * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
   *
   * @param    [in] handle         Device handle obtained by OpenDevice functions.
   * @param    [in] propertyId     Property identifier to set value for.
   * @param    [in] propertyValue  String containing property value.
   *
   * @return
   *           ::IBSU_STATUS_OK, if successful.\n
   *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
   *
   * @remark   Only specific property values can be set.
   ****************************************************************************************************
   */
#ifndef WINCE
    int WINAPI IBSU_SetProperty
        (const int              handle,
        const IBSU_PropertyId   propertyId,
        LPCSTR                  propertyValue);

#else  // UNICODE
    int WINAPI IBSU_SetPropertyW
        (const int              handle,
        const IBSU_PropertyId   propertyId,
        const wchar_t* propertyValue);

#define IBSU_SetProperty IBSU_SetPropertyW
#endif 

    /**
    ****************************************************************************************************
    * @brief
    *           Retrieves a particular device's property value (by handle).
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] propertyId     Property identifier to get value for.
    * @param    [out] propertyValue String returning property value. Memory must be provided by a caller.
    *                               This buffer should be able to hold ::IBSU_MAX_STR_LEN characters.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_GetProperty
        (const int              handle,
        const IBSU_PropertyId   propertyId,
        LPSTR                   propertyValue);

#else  // UNICODE
    int WINAPI IBSU_GetPropertyW
        (const int              handle,
        const IBSU_PropertyId   propertyId,
        wchar_t* propertyValue);

#define IBSU_GetProperty IBSU_GetPropertyW
#endif 

    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */

    /**
    ****************************************************************************************************
    * @defgroup group_API_Device_Image_Aquisition API - Device - Image Aquisition
    * @brief    These API functions are related to Image Captures.
    * @{
    * @page page_API_Device_Image_Aquisition
    ****************************************************************************************************
    */
    /**
    ****************************************************************************************************
    * @brief
    *           Check if a requested capture mode is supported by the device.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle             Device handle obtained by OpenDevice functions.
    * @param    [in] imageType          Image type to verify.
    * @param    [in] imageResolution    Requested capture resolution.
    * @param    [out] pIsAvailable      Returns TRUE if mode is available. Memory must be provided by caller
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_IsCaptureAvailable
        (const int                  handle, 
        const IBSU_ImageType        imageType, 
        const IBSU_ImageResolution  imageResolution, 
        BOOL                        *pIsAvailable);
    
    /**
    ****************************************************************************************************
    * @brief
    *           Starts image acquisition for a particular device (by handle).
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle           Device handle obtained by OpenDevice functions.
    * @param    [in] imageType        Image type to capture.
    * @param    [in] imageResolution  Requested capture resolution.
    * @param    [in] captureOptions   Bit coded capture options to use:
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *           ::IBSU_ERR_CAPTURE_STILL_RUNNING: an acquisition is currently pending and needs to be completed first. \n
    *           ::IBSU_ERR_CAPTURE_INVALID_MODE: acquisition mode needs to be set as a prerequisite. \n
    *
    * @remark
    *           Once image acquisition is completed, image streaming will continue in the background 
    *           (to minimize delays when restarting acquisition). To stop communication traffic 
    *           on the PC bus system, streaming can be stopped by setting the capture mode to ::ENUM_IBSU_TYPE_NONE. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_BeginCaptureImage
        (const int                  handle, 
        const IBSU_ImageType        imageType, 
        const IBSU_ImageResolution  imageResolution, 
        const DWORD                 captureOptions);

    /**
    ****************************************************************************************************
    * @brief
    *           Abort image acquisition on a device that is currently scanning.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    * @pre      IBSU_BeginCaptureImage() called successfully
    *
    * @param    [in] handle     Device handle obtained by OpenDevice functions.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    *           ::IBSU_ERR_CAPTURE_NOT_RUNNING: no active acquisition to be aborted. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_CancelCaptureImage
        (const int  handle);

    /**
    ****************************************************************************************************
    * @brief
    *           Check if a particular device is actively scanning for image acquisition.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle     Device handle obtained by OpenDevice functions.
    * @param    [out] Returns   TRUE if acquisition is in progress (preview or result image acquisition). 
    *                           Memory must be provided by caller..
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_IsCaptureActive
        (const int  handle, 
        BOOL        *pIsActive);    

    /**
    ****************************************************************************************************
    * @brief
    *           Start image acquisition for a particular device (by handle) with image gain manually set.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    * @pre      IBSU_BeginCaptureImage() called successfully
    *
    * @param    [in] handle     Device handle obtained by OpenDevice functions.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_TakeResultImageManually
        (const int  handle);

    /**
    ****************************************************************************************************
    * @brief
    *     Get the result image information.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] fingerPosition Finger position.
    * @param    [out] pResultImage  Pointer to structure that will receive data of preview or result image.
    *                               The buffer in this structure points to an internal image buffer; the
    *                               data should be copied to an application buffer if desired for future
    *                               processing.
    * @param    [out] pSplitResultImage Pointer to array of four structures that will receive individual finger
    *                                   images split from result image.  The buffers in these structures point
    *                                   to internal image buffers; the data should be copied to application
    *                                   buffers if desired for future processing.
    * @param    [out] pSplitResultImageCount    Pointer to variable that will receive the number of finger images split
    *                                           from result image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetIBSM_ResultImageInfo
        (const int          handle,
        IBSM_FingerPosition fingerPosition,
        IBSM_ImageData*     pResultImage,
        IBSM_ImageData*     pSplitResultImage,
        int*                pSplitResultImageCount);

    
    /**
    ****************************************************************************************************
    * @brief
    *           Queries a particular scanner to determine if a finger is currently detected.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    * @pre      IBSU_BeginCaptureImage() called successfully
    *
    * @param    [in] handle             Device handle obtained by OpenDevice functions.
    * @param    [out] pTouchInValue     TouchValue value (0 : touch off, 1 : touch on). Memory must be provided by caller
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_IsTouchedFinger
        (const int  handle,
        int*        pTouchInValue);
    /**
   ****************************************************************************************************
   * IBSU_CheckWetFinger() (Deprecated)
   *
   * @brief
   *     Check if the image is wet or not.
   *
   * @param
   * @param    [in] handle         Device handle obtained by OpenDevice functions.
   * @param
   *     inImage                Input image data which is returned from result callback.
   *
   * @return
   *           ::IBSU_STATUS_OK, if successful.\n
   *     Error code < 0, otherwise.  See warning codes in 'IBScanUltimateApi_err'.
   ****************************************************************************************************
   */
    int WINAPI IBSU_CheckWetFinger
        (const int              handle,
        const IBSU_ImageData    inImage);

    /**
    ****************************************************************************************************
    * IBSU_GetImageWidth() (Deprecated)
    *
    * @brief
    *     Get the image width of input image by millimeter(mm).
    *
    * @param
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     inImage                Input image data which is returned from result callback.
    * @param
    *     Width_MM				 Output millimeter (width) of Input image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *     Error code < 0, otherwise.  See warning codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetImageWidth
        (const int              handle,
        const IBSU_ImageData    inImage,
        int* Width_MM);

    /**
    ****************************************************************************************************
    * IBSU_ConvertImageToISOANSI()
    *
    * @brief
    *           Convert Image Data to Standard Format for write file.
    *						(ISO 19794-2:2005, ISO 19794-4:2005, ISO 19794-2:2011, ISO 19794-4:2011, ANSI/INCITS 378:2004, ANSI/INCITS 381:2004)
    *
    * @pre      ...
    *
    * @param    [in] handle								Device handle obtained by OpenDevice functions.
    * @param    [in] image								Input image data for roll to slap comparison.
    * @param    [in] imageCount						Number of image.
    * @param    [in] imageFormat					Image compression format of output data.
    * @param    [in] STDformat						ISO format of output data.
    * @param    [out] pdata								Pointer to output data.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_ConvertImageToISOANSI
        (const int                  handle,
        const IBSM_ImageData*       image,
        const int                   imageCount,
        const IBSM_ImageFormat      imageFormat,
        const IBSM_StandardFormat   STDformat,
        IBSM_StandardFormatData*    pdata);

    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */

    /**
    ****************************************************************************************************
    * @defgroup group_API_Device_General API - Device - General
    * @brief    These API functions are related to opening & closing a device.
    * @{
    * @page page_API_Device_General
    ****************************************************************************************************
    */
    /**
    ****************************************************************************************************
    * @brief
    *           Get the contrast value for a particular scanner.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    * @pre      IBSU_BeginCaptureImage() called successfully
    *
    * @param    [in] handle             Device handle obtained by OpenDevice functions.
    * @param    [out] pContrastValue    Contrast value (range: 0 <= value <= ::IBSU_MAX_CONTRAST_VALUE).
    *                                   Memory must be provided by caller.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetContrast
        (const int  handle, 
        int         *pContrastValue);

    /**
    ****************************************************************************************************
    * @brief
    *           Set the contrast value for a particular scanner.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    * @pre      IBSU_BeginCaptureImage() called successfully
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] contrastValue  Contrast value (range: 0 <= value <= ::IBSU_MAX_CONTRAST_VALUE).
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_SetContrast
        (const int  handle, 
        const int   contrastValue);

    /**
    ****************************************************************************************************
    * @brief
    *           Sets the LE operation mode (On, Off, or Auto) for a particular scanner.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle             Device handle obtained by OpenDevice functions.
    * @param    [in] leOperationMode    LE film operation mode.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_SetLEOperationMode
        (const int                  handle,
        const IBSU_LEOperationMode  leOperationMode);

    /**
    ****************************************************************************************************
    * @brief
    *           Get the light-emitting (LE) film operation mode for a device.
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle             Device handle obtained by OpenDevice functions.
    * @param    [out] pLeOperationMode  Pointer to variable that will receive LE film operation mode.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetLEOperationMode
    (const int                  handle,
    IBSU_LEOperationMode* pLeOperationMode);

    /**
    ****************************************************************************************************
    * @brief
    *           Get operable status LED's. 
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pLedType      Type of LED's. Memory must be provided by caller
    * @param    [out] pLedCount     Number of LED's. Memory must be provided by caller.
    * @param    [out] pOperableLEDs Bit pattern of operable LED's. Memory must be provided by caller.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetOperableLEDs
        (const int      handle,
        IBSU_LedType    *pLedType,
        int             *pLedCount,
        DWORD           *pOperableLEDs);

    /**
    ****************************************************************************************************
    * @brief
    *           Get operable status LED's. 
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pLedType      Type of LED's. Memory must be provided by caller
    * @param    [out] pLedCount     Number of LED's. Memory must be provided by caller.
    * @param    [out] pOperableLEDs Bit pattern of operable LED's. Memory must be provided by caller.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetOperableLEDs
        (const int      handle,
        IBSU_LedType    *pLedType,
        int             *pLedCount,
        DWORD           *pOperableLEDs);

    /**
    ****************************************************************************************************
    * @brief
    *     Get active status LED's for a particular scanner. 
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pActiveLEDs   Get active LEDs. Memory must be provided by a caller.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetLEDs
        (const int  handle,
        DWORD       *pActiveLEDs);

    /**
    ****************************************************************************************************
    * @brief
    *     Set active status LED's on a particular scanner. 
    *
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] activeLEDs     Set active LEDs.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_SetLEDs
        (const int  handle,
        const DWORD activeLEDs);


    /*****************************************************************************************************
    * IBSU_GetOperableBeeper()
    * 
    * @brief
    *     Get characteristics of operable Beeper on a device. 
    *
    * @param
    *     handle         Device handle.
    * @param
    *     pBeeperType    Pointer to variable that will receive the type of Beeper.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetOperableBeeper
        (const int      handle,
        IBSU_BeeperType *pBeeperType);

    /**
    ****************************************************************************************************
    * IBSU_SetBeeper()
    * 
    * @brief
    *     Set the value of Beeper on a device. 
    *
    * @param
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     beepPattern     Pattern of beep.
    * @param
    *     soundTone       The frequency of the sound, using a specific value. The parameter must be
    *                     in the range of 0 through 2.
    * @param
    *     duration        The duration of the sound, in 25 miliseconds intervals. The parameter must be
    *                     in the range of 1 through 200 at ENUM_IBSU_BEEP_PATTERN_GENERIC,
    *                     in the range of 1 through 7 at ENUM_IBSU_BEEP_PATTERN_REPEAT.
    * @param
    *     reserved_1      Reserved
    * @param
    *     reserved_2      Reserved
    *                     If you set beepPattern to ENUM_IBSU_BEEP_PATTERN_REPEAT
    *                     reserved_1 can use the sleep time after duration of the sound, in 25 miliseconds.
    *                     The parameter must be in the range of 1 through 8
    *                     reserved_2 can use the operation(start/stop of pattern repeat), 1 to start; 0 to stop 
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_SetBeeper
        (const int              handle,
        const IBSU_BeepPattern  beepPattern,
        const DWORD             soundTone,
        const DWORD             duration,
        const DWORD             reserved_1,
        const DWORD             reserved_2);

    /**
    ****************************************************************************************************
    * IBSU_BGetImage()
    *
    * @brief
    *     Acquire an image from a device, blocking for result.  The split image array will only be
    *     populated if the image is a result image, i.e., if the 'IsFinal' member of 'pImage' is set to
    *     TRUE.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pImage        Pointer to structure that will receive data of preview or result image.
    *                               The buffer in this structure points to an internal image buffer; the
    *                               data should be copied to an application buffer if desired for future
    *                               processing.
    * @param    [out] pImageType    Pointer to variable that will receive image type.
    * @param    [out] pSplitImageArray  Pointer to array of four structures that will receive individual finger
    *                                   images split from result image.  The buffers in these structures point
    *                                   to internal image buffers; the data should be copied to application
    *                                   buffers if desired for future processing.
    * @param    [out] pSplitImageArrayCount     Pointer to variable that will receive number of finger images split
    *                                           from result images.
    * @param    [out] pFingerCountState     Pointer to variable that will receive finger count state.
    * @param    [out] pQualityArray         Pointer to array of four variables that will receive quality states for
    *                                       finger images.
    * @param    [out] pQualityArrayCount    Pointer to variable that will receive number of finger qualities.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_BGetImage
    (const int                  handle,
        IBSU_ImageData* pImage,
        IBSU_ImageType* pImageType,
        IBSU_ImageData* pSplitImageArray,
        int* pSplitImageArrayCount,
        IBSU_FingerCountState* pFingerCountState,
        IBSU_FingerQualityState* pQualityArray,
        int* pQualityArrayCount);

    /**
    ****************************************************************************************************
    * IBSU_BGetImageEx()
    *
    * @brief
    *     Acquire an image from a device, blocking for result.  The segment image array will only be
    *     populated if the image is a result image, i.e., if the 'IsFinal' member of 'pImage' is set to
    *     TRUE.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pImageStatus  Pointer to variable that will receive status from result image acquisition.
    *                               See error codes in 'IBScanUltimateApi_err'.
    * @param    [out] pImage        Pointer to structure that will receive data of preview or result image.
    *                               The buffer in this structure points to an internal image buffer; the
    *                               data should be copied to an application buffer if desired for future
    *                               processing.
    * @param    [out] pImageType                Pointer to variable that will receive image type.
    * @param    [out] pDetectedFingerCount      Pointer to variable that will receive detected finger count.
    * @param    [out] pSegmentImageArray        Pointer to array of four structures that will receive individual finger
    *                                           image segments from result image.  The buffers in these structures point
    *                                           to internal image buffers; the data should be copied to application
    *                                           buffers if desired for future processing.
    * @param    [out] pSegmentPositionArray     Pointer to array of four structures that will receive position data for
    *                                           individual fingers split from result image.
    * @param    [out] pSegmentImageArrayCount   Pointer to variable that will receive number of finger images split
    *                                           from result image.
    * @param    [out] pFingerCountState         Pointer to variable that will receive finger count state.
    * @param    [out] pQualityArray             Pointer to array of four variables that will receive quality states for
    *                                           finger images.
    * @param    [out] pQualityArrayCount        Pointer to variable that will receive number of finger qualities.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_BGetImageEx
    (const int                  handle,
        int* pImageStatus,
        IBSU_ImageData* pImage,
        IBSU_ImageType* pImageType,
        int* pDetectedFingerCount,
        IBSU_ImageData* pSegmentImageArray,
        IBSU_SegmentPosition* pSegmentPositionArray,
        int* pSegmentImageArrayCount,
        IBSU_FingerCountState* pFingerCountState,
        IBSU_FingerQualityState* pQualityArray,
        int* pQualityArrayCount);

    /**
    ****************************************************************************************************
    * IBSU_BGetInitProgress()
    *
    * @brief
    *     Get initialization progress of a device.  If initialization is complete, the handle for
    *     subsequent function calls will be returned to the application.
    *
    * @param    [in]  deviceIndex       Zero-based index of the scanner.
    * @param    [out] pIsComplete       Pointer to variable that will receive indicator of initialization completion.
    * @param    [out] pHandle           Pointer to variable that will receive device handle for subsequent function
    *                                   calls, if 'pIsComplete' receives the value TRUE.
    * @param    [out] pProgressValue    Pointer to variable that will receive initialize progress, as a percentage
    *                                   between 0 and 100, inclusive.
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_BGetInitProgress
    (const int deviceIndex,
        BOOL* pIsComplete,
        int* pHandle,
        int* pProgressValue);

    /**
    ****************************************************************************************************
    * IBSU_BGetClearPlatenAtCapture()
    *
    * @brief
    *     Determine whether the platen was clear when capture was started or has since become clear.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pPlatenState  Pointer to variable that will receive platen state.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_BGetClearPlatenAtCapture
    (const int          handle,
        IBSU_PlatenState* pPlatenState);

    /**
    ****************************************************************************************************
    * IBSU_BGetRollingInfo()
    *
    * @brief
    *     Get information about the status of the rolled print capture for a device.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pRollingState Pointer to variable that will receive rolling state.
    * @param    [out] pRollingLineX Pointer to variable that will receive x-coordinate of current "rolling line"
    *                               for display as a guide.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_BGetRollingInfo
    (const int          handle,
        IBSU_RollingState* pRollingState,
        int* pRollingLineX);

    /**
    ****************************************************************************************************
    * IBSU_BGetRollingInfoEx()
    *
    * @brief
    *     Get information about the status of the rolled print capture for a device.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [out] pRollingState     Pointer to variable that will receive rolling state.
    * @param    [out] pRollingLineX     Pointer to variable that will receive x-coordinate of current "rolling line"
    *                                   for display as a guide.
    * @param    [out] pRollDirection    Pointer to variable that will receive rolling direction
    *                                   0 : can't determine yet
    *                                   1 : left to right  --->
    *                                   2 : right to left  <---
    * @param    [out] pRollWidth        Pointer to vairable that will receive rolling width (mm)
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_BGetRollingInfoEx
    (const int          handle,
        IBSU_RollingState* pRollingState,
        int* pRollingLineX,
        int* pRollingDirection,
        int* pRollingWidth);

    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */


    /**
    ****************************************************************************************************
    * @defgroup group_API_Util_Image_Related API - Util - Image Related
    * @brief    These API functions are related to Common uses for Images.
    * @{
    * @page page_API_Util_Image_Related
    ****************************************************************************************************
    */
    /**
    ****************************************************************************************************
    * IBSU_GenerateZoomOutImage()
    *
    * @brief
    *     Generate scaled version of an image.
    *
    * @param    [in] inImage    Original image.
    * @param    [out outImage   Pointer to buffer that will receive output image.  This buffer must hold at least
    *                           'outWidth' x 'outHeight' bytes.
    * @param    [in] outWidth   Width of output image.
    * @param    [in] outHeight  Height of output image.
    * @param    [in] bkColor    Background color of output image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GenerateZoomOutImage
        (const IBSU_ImageData   inImage,
        BYTE* outImage,
        const int               outWidth,
        const int               outHeight,
        const BYTE              bkColor);

    /**
    ****************************************************************************************************
    * IBSU_GenerateZoomOutImageEx()
    *
    * @brief
    *     Generate scaled version of an image.
    *
    * @param    [in] inImage    Original image data.
    * @param    [in] inWidth    Width of input image.
    * @param    [in] in Height  Height of input image.
    * @param    [out] outImage  Pointer to buffer that will receive output image.  This buffer must hold at least
    *                           'outWidth' x 'outHeight' bytes.
    * @param    [in] outWidth   Width of output image.
    * @param    [in] outHeight  Height of output image.
    * @param    [in] bkColor    Background color of output image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GenerateZoomOutImageEx
        (const BYTE* pInImage,
        const int   inWidth,
        const int   inHeight,
        BYTE* outImage,
        const int   outWidth,
        const int   outHeight,
        const BYTE  bkColor);

    /**
    ****************************************************************************************************
    * IBSU_SaveBitmapImage()
    *
    * @brief
    *     Save image to bitmap file.
    *
    * @param    [in] filePath   Path of file for output image.
    * @param    [in] imgBuffer  Pointer to image buffer.
    * @param    [in] width      Image width (in pixels).
    * @param    [in] height     Image height (in pixels).
    * @param    [in] pitch      Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                           negative value indicates bottom-up line order.
    * @param    [in] resX       Horizontal image resolution (in pixels/inch).
    * @param    [in] resY       Vertical image resolution (in pixels/inch).
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_SaveBitmapImage
        (LPCSTR         filePath,
        const BYTE* imgBuffer,
        const DWORD     width,
        const DWORD     height,
        const int       pitch,
        const double    resX,
        const double    resY);

#else  // UNICODE
    int WINAPI IBSU_SaveBitmapImageW
        (const wchar_t* filePath,
        const BYTE* imgBuffer,
        const DWORD     width,
        const DWORD     height,
        const int       pitch,
        const double    resX,
        const double    resY);

#define IBSU_SaveBitmapImage IBSU_SaveBitmapImageW
#endif

    /**
    ****************************************************************************************************
    * IBSU_SaveBitmapMem()
    *
    * @brief
    *     Save image to bitmap memory.
    *
    * @param    [in] inImage            Point to image data (gray scale image).
    * @param    [in] inWidth            Image width (in pixels).
    * @param    [in] inHeight           Image height (in pixels).
    * @param    [in] inPitch            Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                                   negative value indicates bottom-up line order.
    * @param    [in] inResX             Horizontal image resolution (in pixels/inch).
    * @param    [in] inResY             Vertical image resolution (in pixels/inch).
    * @param    [out] outBitmapBuffer   Pointer to output image data buffer which is a set image format and zoom-out factor.
    *                                   Required memory buffer size depends upon the output image format (outImageFormat):
    *                                   for IBSU_IMG_FORMAT_GRAY:  IBSU_BMP_GRAY_HEADER_LEN  + outWidth * outHeight bytes
    *                                   for IBSU_IMG_FORMAT_RGB24: IBSU_BMP_RGB24_HEADER_LEN + 3 * outWidth * outHeight bytes
    *                                   for IBSU_IMG_FORMAT_RGB32: IBSU_BMP_RGB32_HEADER_LEN + 4 * outWidth * outHeight bytes
    * @param    [in] outImageFormat     Set Image color format for output image
    * @param    [in] outWidth           Width for zoom-out image
    * @param    [in] outHeight          Height for zoom-out image
    * @param    [in] bkColor            Background color for remaining area from zoom-out image
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_SaveBitmapMem
        (const BYTE* inImage,
        const DWORD             inWidth,
        const DWORD             inHeight,
        const int               inPitch,
        const double            inResX,
        const double            inResY,
        BYTE* outBitmapBuffer,
        const IBSU_ImageFormat  outImageFormat,
        const DWORD             outWidth,
        const DWORD             outHeight,
        const BYTE              bkColor);

    /**
    ****************************************************************************************************
    * IBSU_WSQEncodeMem()
    *
    * @brief
    *     WSQ compresses a grayscale fingerprint image.
    *
    * @param
    *     image             Original image.
    * @param
    *     width             Width of original image (in pixels).
    * @param
    *     height            Height of original image (in pixels).
    * @param
    *     pitch             Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                       negative value indicates bottom-up line order.
    * @param
    *     bitsPerPixel      Bits per pixel of original image.
    * @param
    *     pixelPerInch      Pixel per inch of original image.
    * @param
    *     bitRate           Determines the amount of lossy compression.
    Suggested settings:
    bitRate = 2.25 yields around 5:1 compression
    bitRate = 0.75 yields around 15:1 compression
    * @param
    *     commentText       Comment to write compressed data.
    * @param
    *     compressedData    Pointer of image which is compressed from original image by WSQ compression.
    *                       This pointer is deallocated by IBSU_FreeMemory() after using it.
    * @param
    *     compressedLength  Length of image which is compressed from original image by WSQ compression.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_WSQEncodeMem
        (const BYTE* image,
        const int       width,
        const int       height,
        const int       pitch,
        const int       bitsPerPixel,
        const int       pixelPerInch,
        const double    bitRate,
        const char* commentText,
        BYTE** compressedData,
        int* compressedLength);

    /**
    ****************************************************************************************************
    * IBSU_WSQEncodeToFile()
    *
    * @brief
    *     Save WSQ compressed grayscale fingerprint image to a specific file path.
    *
    * @param
    *     filePath          File path to save image which is compressed from original image by WSQ compression.
    * @param
    *     image             Original image.
    * @param
    *     width             Width of original image (in pixels).
    * @param
    *     height            Height of original image (in pixels).
    * @param
    *     pitch             Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                       negative value indicates bottom-up line order.
    * @param
    *     bitsPerPixel      Bits per pixel of original image.
    * @param
    *     pixelPerInch      Pixel per inch of original image.
    * @param
    *     bitRate           Determines the amount of lossy compression.
    Suggested settings:
    bitRate = 2.25 yields around 5:1 compression
    bitRate = 0.75 yields around 15:1 compression
    * @param
    *     commentText       Comment to write compressed data.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_WSQEncodeToFile
        (LPCSTR         filePath,
        const BYTE* image,
        const int       width,
        const int       height,
        const int       pitch,
        const int       bitsPerPixel,
        const int       pixelPerInch,
        const double    bitRate,
        const char* commentText);

#else  // UNICODE
    int WINAPI IBSU_WSQEncodeToFileW
        (const wchar_t* filePath,
        const BYTE* image,
        const int       width,
        const int       height,
        const int       pitch,
        const int       bitsPerPixel,
        const int       pixelPerInch,
        const double    bitRate,
        const wchar_t* commentText);

#define IBSU_WSQEncodeToFile IBSU_WSQEncodeToFileW
#endif

    /**
    ****************************************************************************************************
    * IBSU_WSQDecodeMem()
    *
    * @brief
    *     Decompress a WSQ-encoded grayscale fingerprint image.
    *
    * @param
    *     compressedImage   WSQ-encoded image.
    * @param
    *     compressedLength  Length of WSQ-encoded image.
    * @param
    *     decompressedImage Pointer of image which is decompressed from a WSQ-encoded image.
    *                       This pointer is deallocated by IBSU_FreeMemory() after using it.
    * @param
    *     outWidth          Width of decompressed image (in pixels).
    * @param
    *     outHeight         Height of decompressed image (in pixels).
    * @param
    *     outPitch          Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                       negative value indicates bottom-up line order.
    * @param
    *     outBitsPerPixel   Bits per pixel of decompressed image.
    * @param
    *     outPixelPerInch   Pixel per inch of decompressed image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_WSQDecodeMem
        (const BYTE* compressedImage,
        const int   compressedLength,
        BYTE** decompressedImage,
        int* outWidth,
        int* outHeight,
        int* outPitch,
        int* outBitsPerPixel,
        int* outPixelPerInch);

    /**
    ****************************************************************************************************
    * IBSU_WSQDecodeFromFile()
    *
    * @brief
    *     Decompress a WSQ-encoded grayscale fingerprint image from a specific file path.
    *
    * @param
    *     filePath          File path of WSQ-encoded image.
    * @param
    *     decompressedImage Pointer of image which is decompressed from a WSQ-encoded image.
    *                       This pointer is deallocated by IBSU_FreeMemory() after using it.
    * @param
    *     outWidth          Width of decompressed image (in pixels).
    * @param
    *     outHeight         Height of decompressed image (in pixels).
    * @param
    *     outPitch          Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                       negative value indicates bottom-up line order.
    * @param
    *     outBitsPerPixel   Bits per pixel of decompressed image.
    * @param
    *     outPixelPerInch   Pixel per inch of decompressed image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_WSQDecodeFromFile
        (LPCSTR filePath,
        BYTE** decompressedImage,
        int* outWidth,
        int* outHeight,
        int* outPitch,
        int* outBitsPerPixel,
        int* outPixelPerInch);

#else  // UNICODE
    int WINAPI IBSU_WSQDecodeFromFileW
        (const wchar_t* filePath,
        BYTE** decompressedImage,
        int* outWidth,
        int* outHeight,
        int* outPitch,
        int* outBitsPerPixel,
        int* outPixelPerInch);

#define IBSU_WSQDecodeFromFile IBSU_WSQDecodeFromFileW
#endif

    /**
    ****************************************************************************************************
    * IBSU_FreeMemory()
    *
    * @brief
    *     Release the allocated memory block from the internal heap of the library.
    *     This is obtained by IBSU_WSQEncodeMem(), IBSU_WSQDecodeMem, IBSU_WSQDecodeFromFile() and other API functions.
    *
    * @param
    *     memblock          Previously allocated memory block to be freed.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_FreeMemory
        (void* memblock);

    /**
    ****************************************************************************************************
    * IBSU_SavePngImage()
    *
    * @brief
    *     Save image to PNG file.
    *
    * @param
    *     filePath   Path of file for output image.
    * @param
    *     imgBuffer  Pointer to image buffer.
    * @param
    *     width      Image width (in pixels).
    * @param
    *     height     Image height (in pixels).
    * @param
    *     pitch      Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                negative value indicates bottom-up line order.
    * @param
    *     resX       Horizontal image resolution (in pixels/inch).
    * @param
    *     resY       Vertical image resolution (in pixels/inch).
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_SavePngImage
        (LPCSTR         filePath,
        const BYTE* image,
        const DWORD     width,
        const DWORD     height,
        const int       pitch,
        const double    resX,
        const double    resY
    );

#else  // UNICODE
    int WINAPI IBSU_SavePngImageW
        (const wchar_t* filePath,
        const BYTE* image,
        const DWORD     width,
        const DWORD     height,
        const int       pitch,
        const double    resX,
        const double    resY
    );

#define IBSU_SavePngImage IBSU_SavePngImageW
#endif

    /**
    ****************************************************************************************************
    * IBSU_SaveJP2Image()
    *
    * @brief
    *     Save image to JPEG-2000 file.
    *
    * @param
    *     filePath   Path of file for output image.
    * @param
    *     imgBuffer  Pointer to image buffer.
    * @param
    *     width      Image width (in pixels).
    * @param
    *     height     Image height (in pixels).
    * @param
    *     pitch      Image line pitch (in bytes).  A positive value indicates top-down line order; a
    *                negative value indicates bottom-up line order.
    * @param
    *     resX       Horizontal image resolution (in pixels/inch).
    * @param
    *     resY       Vertical image resolution (in pixels/inch).
    * @param
    *     fQuality   Quality level for JPEG2000, the valid range is between 0 and 100
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_SaveJP2Image
        (LPCSTR         filePath,
        const BYTE* image,
        const DWORD     width,
        const DWORD     height,
        const int       pitch,
        const double    resX,
        const double    resY,
        const int       fQuality
    );

#else  // UNICODE
    int WINAPI IBSU_SaveJP2ImageW
        (const wchar_t* filePath,
        const BYTE* image,
        const DWORD     width,
        const DWORD     height,
        const in        pitch,
        const double    resX,
        const double    resY,
        const int       fQuality
    );

#define IBSU_SaveJP2Image IBSU_SaveJP2ImageW
#endif

    /**
    ****************************************************************************************************
    * IBSU_CombineImage()
    *
    * @brief
    *     Combine two images (2 flat fingers) into a single image (left/right hands)
    *
    * @param
    *     inImage1		  Pointer to IBSU_ImageData ( index and middle finger )
    * @param
    *     inImage2		  Pointer to IBSU_ImageData ( ring and little finger )
    * @param
    *	  whichHand		  Information of left or right hand
    * @param
    *     outImage		  Pointer to IBSU_ImageData ( 1600 x 1500 fixed size image )
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_CombineImage
        (const IBSU_ImageData       inImage1,
        const IBSU_ImageData        inImage2,
        IBSU_CombineImageWhichHand  whichHand,
        IBSU_ImageData* outImage
    );

    /**
    ****************************************************************************************************
    * IBSU_CombineImageEx()
    *
    * @brief
    *     Combine two images (2 flat fingers) into a single image (left/right hands)
    *     and return segment information
    *
    * @param
    *     inImage1					Pointer to IBSU_ImageData ( index and middle finger )
    * @param
    *     inImage2					Pointer to IBSU_ImageData ( ring and little finger )
    * @param
    *	  whichHand					Information of left or right hand
    * @param
    *     outImage					Pointer to IBSU_ImageData ( 1600 x 1500 fixed size image )
    * @param
    *     pSegmentImageArray        Pointer to array of four structures that will receive individual finger
    *                               image segments from output image.  The buffers in these structures point
    *                               to internal image buffers; the data should be copied to application
    *                               buffers if desired for future processing.
    * @param
    *     pSegmentPositionArray     Pointer to array of four structures that will receive position data for
    *                               individual fingers split from output image.
    * @param
    *     pSegmentImageArrayCount   Pointer to variable that will receive number of finger images split
    *                               from output image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_CombineImageEx
        (const IBSU_ImageData       InImage1,
        const IBSU_ImageData        InImage2,
        IBSU_CombineImageWhichHand  WhichHand,
        IBSU_ImageData*             OutImage,
        IBSU_ImageData*             pSegmentImageArray,
        IBSU_SegmentPosition*       pSegmentPositionArray,
        int*                        pSegmentImageArrayCount
    );

    /**
    ****************************************************************************************************
    * IBSU_GenerateDisplayImage()
    *
    * @brief
    *     Generate scaled image in various formats for fast image display on canvas.
    *     This can be used instead of IBSU_GenerateZoomOutImageEx()
    *
    * @param
    *     inImage     Original grayscale image data.
    * @param
    *     inWidth     Width of input image.
    * @param
    *     in Height   Height of input image.
    * @param
    *     outImage    Pointer to buffer that will receive output image.  This buffer must hold at least
    *                 'outWidth' x 'outHeight' x 'bitsPerPixel' bytes.
    * @param
    *     outWidth    Width of output image.
    * @param
    *     outHeight   Height of output image.
    * @param
    *     outBkColor     Background color of output image.
    * @param
    *     outFormat   IBSU_ImageFormat of output image.
    * @param
    *     outQualityLevel  Image quality of output image. The parameter must be in the range of 0 through 2
    * @param
    *     outVerticalFlip  Enable/disable vertical flip of output image.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GenerateDisplayImage
    (const BYTE* pInImage,
        const int               inWidth,
        const int               inHeight,
        BYTE* outImage,
        const int               outWidth,
        const int               outHeight,
        const BYTE              outBkColor,
        const IBSU_ImageFormat  outFormat,
        const int               outQualityLevel,
        const BOOL              outVerticalFlip);


    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */



    /**
    ****************************************************************************************************
    * @defgroup group_API_Util_Matcher API - Util - Matcher
    * @brief    This is API functions related to Matcher
    * @{
    * @page page_API_Util_Matcher
    ****************************************************************************************************
    * /
    /**
    ****************************************************************************************************
    * IBSU_RemoveFingerImages()
    *
    * @brief
    *     Remove finger images.
    *
    * @param
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     fIndex              Bit-pattern of finger index of input image.
    *                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_RemoveFingerImage
        (const int  handle,
        const DWORD fIndex);

    /**
    ****************************************************************************************************
    * IBSU_AddFingerImage()
    *
    * @brief
    *     Add a finger image for the fingerprint duplicate check and roll to slap comparison.
    *     It can have only ten prints
    *
    * @param
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     image               Input image data.
    * @param
    *     fIndex              Bit-pattern of finger index of input image.
    *                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
    * @param
    *     imageType           Image type of input image.
    * @param
    *     flagForce           Indicates whether input image should be saved even if another image is already stord or not.  TRUE to be stored force; FALSE to
    *                         be not stored force.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_AddFingerImage
        (const int              handle,
        const IBSU_ImageData    image,
        const DWORD             fIndex,
        const IBSU_ImageType    imageType,
        const BOOL              flagForce);

    /**
    ****************************************************************************************************
    * IBSU_IsFingerDuplicated()
    *
    * @brief
    *     Checks for a fingerprint duplicate from the stored prints by IBSU_AddFingerImage().
    *
    * @param
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     image               Input image data for the fingerprint duplicate check.
    * @param
    *     fIndex              Bit-pattern of finger index of input image.
    *                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
    * @param
    *     imageType           Image type of input image.
    * @param
    *     securityLevel       Security level for the duplicate checks.
    * @param
    *     pMatchedPosition    Pointer to variable that will receive result of duplicate.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_IsFingerDuplicated
        (const int              handle,
        const IBSU_ImageData    image,
        const DWORD             fIndex,
        const IBSU_ImageType    imageType,
        const int               securityLevel,
        DWORD*                  pMatchedPosition);

    /**
    ****************************************************************************************************
    * IBSU_IsValidFingerGeometry()
    *
    * @brief
    *     Check for hand and finger geometry whether it is correct or not.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     image               Input image data for roll to slap comparison.
    * @param
    *     fIndex              Bit-pattern of finger index of input image.
    *                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
    * @param
    *     imageType           Image type of input image.
    * @param
    *     pValid              Pointer to variable that will receive whether it is valid or not.  TRUE to valid; FALSE to invalid.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_IsValidFingerGeometry
        (const int              handle,
        const IBSU_ImageData    image,
        const DWORD             fIndex,
        const IBSU_ImageType    imageType,
        BOOL*                   pValid);


    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */



    /**
    ****************************************************************************************************
    * @defgroup group_API_Util_NFIQ API - Util - NFIQ
    * @brief    These are API functions related to measuring NFIQ Scores.
    * @{
    * @page page_API_Util_NFIQ
    ****************************************************************************************************
    */
    /**
    ****************************************************************************************************
    * IBSU_GetNFIQScore()
    *
    * @brief
    *     Calculate NFIQ score for an image.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] imgBuffer      Pointer to image buffer.
    * @param    [in] width          Image width (in pixels).
    * @param    [in] height         Image height (in pixels).
    * @param    [in] bitsPerPixel   Bits per pixel.
    * @param    [out] pScore        Pointer to variable that will receive NFIQ score.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetNFIQScore
        (const int  handle,
        const BYTE* imgBuffer,
        const DWORD width,
        const DWORD height,
        const BYTE  bitsPerPixel,
        int* pScore);

    /*
    ****************************************************************************************************
    * IBSU_GetNFIQScoreEx()
    *
    * DESCRIPTION:
    *     Calculate NFIQ score for an image. (Pitch argument added)
    *
    * ARGUMENTS:
    *     handle        Device handle.
    *     imgBuffer     Pointer to image buffer.
    *     width         Image width (in pixels).
    *     height        Image height (in pixels).
    *     pitch         Image pitch (in pixels).
    *     bitsPerPixel  Bits per pixel.
    *     pScore        Pointer to variable that will receive NFIQ score.
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetNFIQScoreEx
        (const int  handle,
        const BYTE* imgBuffer,
        const DWORD width,
        const DWORD height,
        const int   pitch,
        const BYTE  bitsPerPixel,
        int* pScore);
    
    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */


    /**
     ****************************************************************************************************
     * @defgroup    group_API_Util_PAD API - Util - PAD
     * @brief       These are API functions related to Encryption Mode.
     * @{
     * @page page_API_Util_PAD
     ****************************************************************************************************
     */
    /**
     ****************************************************************************************************
     * IBSU_IsSpoofFingerDetected()
     *
     * @brief
     *           Detect if the finger print is Live or Fake.
     *
     * @pre      ...
     *
     * @param    [in] handle								Device handle obtained by OpenDevice functions.
     * @param    [in] image								Input image data.
     * @param    [out] pIsSpoof						Pointer to variable that will receive whether it is Spoof or Live.  TRUE to Spoof; FALSE to Live.
     *
     * @return
     *           ::IBSU_STATUS_OK, if successful.\n
     *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
     ****************************************************************************************************
     */
    int WINAPI IBSU_IsSpoofFingerDetected
        (const int              handle,
        const IBSU_ImageData    image,
        BOOL*                   pIsSpoof);

    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */


    /**
    ****************************************************************************************************
    * @defgroup group_API_Util_Encryption API - Util - Encryption
    * @brief    These are API functions related to Encryption Mode.
    * @{
    * @page page_API_Util_Encryption
    ****************************************************************************************************
    */
    /**
     ****************************************************************************************************
     * IBSU_SetEncryptionKey()
     *
     * @brief
     *           Set encryption key and mode. (Currently not supported)
     *
     * @pre      ...
     *
     * @param    [in] handle         Device handle obtained by OpenDevice functions.
     * @param    [in] pEncyptionKey  Input data for encryption key (should be 32 bytes).
     * @param    [in] encMode 				Input data for encryption mode. (random, custom)
     *
     * @return
     *           ::IBSU_ERR_NOT_SUPPORTED , (this API not support currently)
     ****************************************************************************************************
     */
        int WINAPI IBSU_SetEncryptionKey
            (const int                  handle,
            const unsigned char*        pEncyptionKey,
            const IBSU_EncryptionMode   encMode);
    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */


    /**
    ****************************************************************************************************
    * @defgroup group_API_Util_Lock_and_Key API - Util - Lock and Key
    * @brief    These are API functions related to Encryption Mode.
    * @{
    * @page page_API_Util_Lock_and_Key
    ****************************************************************************************************
    */
    /**
    ****************************************************************************************************
    * IBSU_SetCustomerKey()
    *
    * @brief    Set CustomerKey to use locked devices, This must be perfomed on locked devices before IBSU_OpenDevice.
    *
    * @pre      The device should be locked using the IBDeviceLockWizard SW
    *
    * @param    [in] deviceIndex					Device index.
    * @param    [in] nHashType						Type of Hash.
    * @param    [in] pCustomerKey 				Customer Key to match lock info written in the locked device.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_SetCustomerKey
        (const int          deviceIndex,
        const IBSU_HashType hashType,
        LPCSTR              pCustomerKey);
    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */


    /**
    ****************************************************************************************************
    * @defgroup group_API_General API - General
    * @brief    These are API functions related to Common use.
    * @{
    * @page page_API_General
    ****************************************************************************************************
    */
    /**
     ****************************************************************************************************
     * @brief
     *           Gets a structure holding product and software version information (::IBSU_SdkVersion).
     *
     * @param    [out] pVerinfo  API version information. Memory must be provided by a caller.
     *
     * @return
     *           ::IBSU_STATUS_OK, if successful.\n
     *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
     ****************************************************************************************************
     */
#ifndef WINCE
    int WINAPI IBSU_GetSDKVersion
        (IBSU_SdkVersion* pVerinfo);

#else  // UNICODE
    int WINAPI IBSU_GetSDKVersionW
        (IBSU_SdkVersionW* pVerinfo);

#define IBSU_GetSDKVersion IBSU_GetSDKVersionW
#endif

    /**
    ****************************************************************************************************
    * IBSU_UnloadLibrary() (Not supported)
    *
    * @brief
    *     The library is unmapped from the address space explicitly, and the library is no longer valid
    *
    * @return
    *           ::IBSU_ERR_NOT_SUPPORTED , (this API not support currently)
    ****************************************************************************************************
    */
    int WINAPI IBSU_UnloadLibrary();

    /**
    ****************************************************************************************************
    * IBSU_IsWritableDirectory()
    *
    * @brief
    *     Check whether a directory is writable.
    *
    * @param
    *     dirpath                Directory path.
    * @param
    *     needCreateSubFolder	 Check the need to create a subfolder in the directory path.
    *
    * @return
    *     IBSU_STATUS_OK, if a directory is writable.
    *     Error code < 0, otherwise.  See warning codes in 'IBScanUltimateApi_err'.
    *         IBSU_ERR_CHANNEL_IO_WRITE_FAILED: Directory does not writable.
    ****************************************************************************************************
    */
    int WINAPI IBSU_IsWritableDirectory
        (LPCSTR     dirpath,
        const BOOL  needCreateSubFolder);

    /**
    ****************************************************************************************************
    * IBSU_GetErrorString()
    *
    * @brief
    *           Returns a string description of the error code.
    *
    * @pre      ...
    *
    * @param    [in] errorCode						Device index.
    * @param    [out] errorString					Buffer in which value of error string description will be stored.  This buffer should be
    *																			able to hold IBSU_MAX_STR_LEN characters.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_GetErrorString
        (const int  errorCode,
        LPSTR       errorString);

    /**
    ****************************************************************************************************
    * @brief
    *           Enable or disable trace log.  The trace log is enabled by default on both Windows and
    *           Android, and disabled by default on Linux.
    *
    * @param    [in] on     TRUE to enable trace log; FALSE to disable it.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_EnableTraceLog
    (const BOOL on);


    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */


    /**
    ****************************************************************************************************
    * @defgroup group_API_Client_Window API - Client Window
    * @brief    These are API functions related to drawing on Client Window.
    * @{
    * @page page_API_Client_Window
    ****************************************************************************************************
    */

    /**
    ****************************************************************************************************
    * IBSU_CreateClientWindow()
    *
    * @brief
    *     Create a client window associated with a device.  (Available only on Windows.)
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] hWindow    Windows handle to draw.
    * @param    [in] left       Coordinate of left edge of rectangle.
    * @param    [in] top        Coordinate of top edge of rectangle.
    * @param    [in] right      Coordinate of right edge of rectangle.
    * @param    [in] bottom     Coordinate of bottom edge of rectangle.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_CreateClientWindow
        (const int      handle,
        const IBSU_HWND hWindow,
        const DWORD     left,
        const DWORD     top,
        const DWORD     right,
        const DWORD     bottom);

    /**
    ****************************************************************************************************
    * IBSU_DestroyClientWindow()
    *
    * @brief
    *     Destroy client window associated with a device.  (Available only on Windows.)
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] clearExistingInfo  Indicates whether the existing display information, including display
    *                                   properties and overlay text, will be cleared.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_DestroyClientWindow
        (const int  handle,
        const BOOL  clearExistingInfo);

    /**
    ****************************************************************************************************
    * IBSU_GetClientWindowProperty()
    *
    * @brief
    *     Get the value of a property for the client window associated with a device.  For descriptions
    *     of properties and values, see definition of 'IBSU_ClientWindowPropertyId'.  (Available only on
    *     Windows.)
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] propertyId     Property for which value will be set.
    * @param    [out] propertyValue Buffer in which value of property will be stored.  This buffer should be
    *                               able to hold IBSU_MAX_STR_LEN characters.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_GetClientWindowProperty
        (const int                          handle,
        const IBSU_ClientWindowPropertyId   propertyId,
        LPSTR                               propertyValue);

#else  // UNICODE
    int WINAPI IBSU_GetClientWindowPropertyW
        (const int                          handle,
        const IBSU_ClientWindowPropertyId   propertyId,
        wchar_t* propertyValue);
#endif

    /**
    ****************************************************************************************************
    * IBSU_SetClientDisplayProperty()
    *
    * @brief
    *     Set the value of a property for the client window associated with a device.  For descriptions
    *     of properties and values, see definition of 'IBSU_ClientWindowPropertyId'.  (Available only on
    *     Windows.)
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] propertyId     Property for which value will be set.
    * @param    [in] propertyValue  Value of property to set.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_SetClientDisplayProperty
        (const int                          handle,
        const IBSU_ClientWindowPropertyId   propertyId,
        LPCSTR                              propertyValue);

#else  // UNICODE
    int WINAPI IBSU_SetClientDisplayPropertyW
        (const int                          handle,
        const IBSU_ClientWindowPropertyId   propertyId,
        const wchar_t* propertyValue);

#define IBSU_SetClientDisplayProperty IBSU_SetClientDisplayPropertyW
#endif 

    /**
    ****************************************************************************************************
    * IBSU_SetClientWindowOverlayText()
    *
    * @brief
    *     Set the overlay text for the client window associated with a device.  (Available only on
    *     Windows.)  (Deprecated)
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] fontName   Font name.
    * @param    [in] fontSize   Font size.
    * @param    [in] fontBold   Indicates if the font will be bold.
    * @param    [in] text       Text to display.
    * @param    [in] posX       X-coordinate of text.
    * @param    [in] poxY       Y-coordinate of text.
    * @param    [in] textColor  Color of text.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_SetClientWindowOverlayText
        (const int  handle,
        const char* fontName,
        const int   fontSize,
        const BOOL  fontBold,
        const char* text,
        const int   posX,
        const int   posY,
        const DWORD textColor);

#else  // UNICODE
    int WINAPI IBSU_SetClientWindowOverlayTextW
        (const int      handle,
        const wchar_t*  fontName,
        const int       fontSize,
        const BOOL      fontBold,
        const wchar_t*  text,
        const int       posX,
        const int       posY,
        const DWORD     textColor);

#define IBSU_SetClientWindowOverlayText IBSU_SetClientWindowOverlayTextW
#endif




    /**
    ****************************************************************************************************
    * IBSU_ShowOverlayObject()
    *
    * @brief
    *     Show or hide an overlay object.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] overlayHandle  Overlay handle obtained by overlay functions.
    * @param    [in] show           If TRUE, the overlay will be shown on client window.
    *                               If FALSE, the overlay will be hidden on client window.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_ShowOverlayObject
        (const int  handle,
        const int   overlayHandle,
        const BOOL  show);

    /**
    ****************************************************************************************************
    * IBSU_ShowAllOverlayObject()
    *
    * @brief
    *     Show or hide all overlay objects.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] show       If TRUE, the overlay will be shown on client window.
    *                           If FALSE, the overlay will be hidden on client window.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_ShowAllOverlayObject
        (const int  handle,
        const BOOL  show);

    /**
    ****************************************************************************************************
    * IBSU_RemoveOverlayObject()
    *
    * @brief
    *     Remove an overlay object.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] overlayHandle  Overlay handle obtained by overlay functions.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_RemoveOverlayObject
        (const int  handle,
        const int   overlayHandle);

    /**
    ****************************************************************************************************
    * IBSU_RemoveAllOverlayObject()
    *
    * @brief
    *     Remove all overlay objects.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_RemoveAllOverlayObject
        (const int  handle);

    /**
    ****************************************************************************************************
    * IBSU_AddOverlayText()
    *
    * @brief
    *     Add an overlay text for display on the window.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] pOverlayHandle Function returns overlay handle to be used for client windows function call.
    * @param    [in] fontName       Name of font.
    * @param    [in] fontSize       Font size.
    * @param    [in] fontBold       Indicates if the font is bold.
    * @param    [in] text           Text for display on window.
    * @param    [in] posX           X coordinate of text for display on window.
    * @param    [in] posY           Y coordinate of text for display on window.
    * @param    [in] textColor      Text color.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_AddOverlayText
        (const int  handle,
        int*        pOverlayHandle,
        const char* fontName,
        const int   fontSize,
        const BOOL  fontBold,
        const char* text,
        const int   posX,
        const int   posY,
        const DWORD textColor);

#else  // UNICODE
    int WINAPI IBSU_AddOverlayTextW
        (const int      handle,
        int*            pOverlayHandle,
        const wchar_t*  fontName,
        const int       fontSize,
        const BOOL      fontBold,
        const wchar_t*  text,
        const int       posX,
        const int       posY,
        const DWORD     textColor);

#define IBSU_AddOverlayText IBSU_AddOverlayTextW
#endif

    /**
    ****************************************************************************************************
    * IBSU_ModifyOverlayText()
    *
    * @brief
    *     Modify an existing overlay text for display on the window.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param    [in] overlayHandle  Handle of overlay to modify.
    * @param    [in] fontName       Name of font.
    * @param    [in] fontSize       Font size.
    * @param    [in] fontBold       Indicates if the font is bold.
    * @param    [in] text           Text for display on window.
    * @param    [in] posX           X coordinate of text for display on window.
    * @param    [in] posY           Y coordinate of text for display on window.
    * @param    [in] textColor      Text color.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
#ifndef WINCE
    int WINAPI IBSU_ModifyOverlayText
        (const int  handle,
        const int   overlayHandle,
        const char* fontName,
        const int   fontSize,
        const BOOL  fontBold,
        const char* text,
        const int   posX,
        const int   posY,
        const DWORD textColor);

#else  // UNICODE
    int WINAPI IBSU_ModifyOverlayTextW
        (const int      handle,
        const int       overlayHandle,
        const wchar_t*  fontName,
        const int       fontSize,
        const BOOL      fontBold,
        const wchar_t*  text,
        const int       posX,
        const int       posY,
        const DWORD     textColor);

#define IBSU_ModifyOverlayText IBSU_ModifyOverlayTextW
#endif


    /**
    ****************************************************************************************************
    * IBSU_AddOverlayLine()
    *
    * @brief
    *     Add an overlay line for display on the window.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     pOverlayHandle  Function returns overlay handle to be used for client windows function calls.
    * @param
    *     x1              X coordinate of start point of line.
    * @param
    *     y1              Y coordinate of start point of line.
    * @param
    *     x2              X coordinate of end point of line.
    * @param
    *     y2              Y coordinate of end point of line.
    * @param
    *     lineWidth       Line width.
    * @param
    *     lineColor       Line color.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */

    int WINAPI IBSU_AddOverlayLine
    (const int      handle,
        int*        pOverlayHandle,
        const int   x1,
        const int   y1,
        const int   x2,
        const int   y2,
        const int   lineWidth,
        const DWORD lineColor);

    /**
    ****************************************************************************************************
    * IBSU_ModifyOverlayLine()
    *
    * @brief
    *     Modify an existing line for display on the window
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    *     overlayHandle  Handle of overlay to modify.
    *     x1             X coordinate of start point of line.
    *     y1             Y coordinate of start point of line.
    *     x2             X coordinate of end point of line.
    *     y2             Y coordinate of end point of line.
    *     lineWidth      Line width.
    *     lineColor      Line color.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_ModifyOverlayLine
    (const int      handle,
        const int   overlayHandle,
        const int   x1,
        const int   y1,
        const int   x2,
        const int   y2,
        const int   lineWidth,
        const DWORD lineColor);

    /**
    ****************************************************************************************************
    * IBSU_AddOverlayQuadrangle()
    *
    * @brief
    *     Add an overlay quadrangle for display on the window
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    *     pOverlayHandle  Function returns overlay handle to be used for client windows function calls.
    *     x1              X coordinate of 1st vertex of quadrangle.
    *     y1              Y coordinate of 1st vertex of quadrangle.
    *     x2              X coordinate of 2nd vertex of quadrangle.
    *     y2              Y coordinate of 2nd vertex of quadrangle.
    *     x3              X coordinate of 3rd vertex of quadrangle.
    *     y3              Y coordinate of 3rd vertex of quadrangle.
    *     x4              X coordinate of 4th vertex of quadrangle.
    *     y4              Y coordinate of 4th vertex of quadrangle.
    *     lineWidth       Line width.
    *     lineColor       Line color.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_AddOverlayQuadrangle
    (const int  handle,
        int*        pOverlayHandle,
        const int   x1,
        const int   y1,
        const int   x2,
        const int   y2,
        const int   x3,
        const int   y3,
        const int   x4,
        const int   y4,
        const int   lineWidth,
        const DWORD lineColor);

    /**
    ****************************************************************************************************
    * IBSU_ModifyOverlayQuadrangle()
    *
    * @brief
    *     Modify an existing quadrangle for display on the window
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    *     overlayHandle  Handle of overlay to be modified.
    *     x1             X coordinate of 1st vertex of quadrangle.
    *     y1             Y coordinate of 1st vertex of quadrangle.
    *     x2             X coordinate of 2nd vertex of quadrangle.
    *     y2             Y coordinate of 2nd vertex of quadrangle.
    *     x3             X coordinate of 3rd vertex of quadrangle.
    *     y3             Y coordinate of 3rd vertex of quadrangle.
    *     x4             X coordinate of 4th vertex of quadrangle.
    *     y4             Y coordinate of 4th vertex of quadrangle.
    *     lineWidth      Line width.
    *     lineColor      Line color.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_ModifyOverlayQuadrangle
        (const int  handle,
        const int   overlayHandle,
        const int   x1,
        const int   y1,
        const int   x2,
        const int   y2,
        const int   x3,
        const int   y3,
        const int   x4,
        const int   y4,
        const int   lineWidth,
        const DWORD lineColor);

    /**
    ****************************************************************************************************
    * IBSU_AddOverlayShape()
    *
    * @brief
    *     Add an overlay shape for display on the window
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    *     pOverlayHandle  Function returns overlay handle to be used for client windows function calls.
    *     shapePattern    Pattern of shape.  If ENUM_IBSU_OVERLAY_SHAPE_ARROW, reserved_1 should be
    *                     the width (in pixels) of the full base of the arrowhead, and reserved_1
    *                     should be the angle (in radians) at the arrow tip between the two sides
    *                     of the arrowhead.
    *     x1              X coordinate of start point of overlay shape.
    *     y1              Y coordinate of start point of overlay shape.
    *     x2              X coordinate of end point of overlay shape.
    *     y2              Y coordinate of end point of overlay shape.
    *     lineWidth       Line width.
    *     lineColor       Line color.
    *     reserved_1      X reserved.
    *     reserved_2      Y reserved.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_AddOverlayShape
        (const int                      handle,
        int*                            pOverlayHandle,
        const IBSU_OverlayShapePattern  shapePattern,
        const int                       x1,
        const int                       y1,
        const int                       x2,
        const int                       y2,
        const int                       lineWidth,
        const DWORD                     lineColor,
        const int                       reserved_1,
        const int                       reserved_2);

    /**
    ****************************************************************************************************
    * IBSU_ModifyOverlayShape()
    *
    * @brief
    *     Modify an overlay shape for display on the window
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     overlayHandle  Handle of overlay to modify.
    * @param
    *     shapePattern   Pattern of shape.  If ENUM_IBSU_OVERLAY_SHAPE_ARROW, reserved_1 should be
    *                    the width (in pixels) of the full base of the arrowhead, and reserved_1
    *                    should be the angle (in radians) at the arrow tip between the two sides
    *                    of the arrowhead.
    * @param
    *     x1             X coordinate of start point of overlay shape.
    * @param
    *     y1             Y coordinate of start point of overlay shape.
    * @param
    *     x2             X coordinate of end point of overlay shape.
    * @param
    *     y2             Y coordinate of end point of overlay shape.
    * @param
    *     lineWidth      Line width.
    * @param
    *     lineColor      Line color.
    * @param
    *     reserved_1     X reserved.
    * @param
    *     reserved_2     Y reserved.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_ModifyOverlayShape
        (const int                      handle,
        const int                       overlayHandle,
        const IBSU_OverlayShapePattern  shapePattern,
        const int                       x1,
        const int                       y1,
        const int                       x2,
        const int                       y2,
        const int                       lineWidth,
        const DWORD                     lineColor,
        const int                       reserved_1,
        const int                       reserved_2);


    /**
    ****************************************************************************************************
    * IBSU_RedrawClientWindow()
    *
    * @brief
    *     Update the specified client window which is defined by IBSU_CreateClientWindow().  (Available only on Windows.)
    *
    * @param
    *     handle          Device handle.
    * @param
    *     flags           Bit-pattern of redraw flags.  See flag codes in 'IBScanUltimateApi_def
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_RedrawClientWindow
        (const int  handle);

    /**
   ****************************************************************************************************
   * @}
   ****************************************************************************************************
   */



   /**
   ****************************************************************************************************
   * @defgroup  group_API_Callbacks API - Callbacks
   * @brief     These are API functions related to Callback functions.
   * @{
   * @page page_API_Callbacks
   ****************************************************************************************************
   */
   /**
    ****************************************************************************************************
    * @brief
    *           This function is used to register callback methods,
    *           utilizing event-driven programming when the state of the scanner changes.
    *
    * @pre
    * @pre      IBSU_OpenDevice() or IBSU_OpenDeviceEx() or IBSU_AsyncOpenDevice() called successfully
    *
    * @param    [in] handle     Device handle obtained by OpenDevice functions.
    * @param    [in] event      Event for which the callback is being registered.
    * @param    [in] pCallbackFunction  Pointer to the callback function
    * @param    [in] pContext   Pointer to user context that will be passed to callback function.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_RegisterCallbacks
        (const int          handle,
        const IBSU_Events   event,
        void*               pCallbackFunction,
        void*               pContext);

    /**
    ****************************************************************************************************
    * @brief
    *           Unregister a callback function for a particular event.
    *
    * @param    [in] handle     Device handle obtained by OpenDevice functions.
    * @param    [in] event      Event for which the callback is being unregistered.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
    int WINAPI IBSU_ReleaseCallbacks
        (const int          handle,
        const IBSU_Events   events);

    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */

#ifdef __cplusplus
} // extern "C"
#endif
