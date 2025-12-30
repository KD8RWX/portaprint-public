/*
****************************************************************************************************
* ReservedApi.h
*
* DESCRIPTION:
*     Private API for IBScanUltimate.
*     http://www.integratedbiometrics.com
*
* NOTES:
*     Copyright (c) Integrated Biometrics, 2009-2022
*
* HISTORY:
*     2013/02/03  1.x.x  Created.
*     2013/08/03  1.6.9  Reformatted.
*     2013/10/14  1.7.0  Add a function to read EEPROM.
*     2015/03/26  1.8.4  Added API function to enhance the preview image
*                        (RESERVED_GetEnhancedImage)
*     2016/09/22  1.9.4  Added enumeration value to RESERVED_DeviceInfo
*                        (reserved1)
*     2017/04/27  1.9.7  Added enumeration value to RESERVED_DeviceInfo
*                        (reserved2)
*     2017/08/22  1.9.9  Added API function to get final image by native for Columbo
*                        (RESERVED_GetFinalImageByNative)
*     2020/01/09  3.2.0  Added API function and enumerations to support Spoof
*                         (RESERVED_GetSpoofScore)
*     2020/10/06  3.7.0  Added API function to support locking feature
*     2022/11/14  3.9.3  1. Added API function to support matcher function.
*                        2. Added API function to control analysis image save options.
*                        (RESERVED_RegisterImageToDB, RESERVED_AddImageToRegistered,
*                         RESERVED_DeleteImageFromDB, RESERVED_ClearAllImageFromDB,
*                         RESERVED_MatchImageWithDB,  RESERVED_SaveDBToFile,
*                         RESERVED_LoadDBFromFile,    RESERVED_UpdateDBList,
*                         RESERVED_GetSecurityLevel,  RESERVED_SetSecurityLevel,
*                         RESERVED_GetMatchedPoint,   RESERVED_GetMinutiaeInfo)
*     2023/07/10  3.9.4  Added API function to support Curve-Size-Image (HumanIntech)
*     2024/03/06  3.9.4  Added API function to get FPGA version function. 
*                        (RESERVED_GetFpgaVersion)
*     2024/03/06  3.9.4  Added API function to Enable Device count with Polling
*                        (RESERVED_DeviceDetectionByPolling)
*     2025/04/18  4.2.0  1. Added API function to set DAC, Get final image data & information
*                        2. Added API function to write & read the uniformityMask.
*                        3. Added API function to ControlTransfer. (Use WinUSB, libusb)
*                        (RESERVED_Set_DAC_Register,    RESERVED_Set_DAC_Register_Reference_DAC_Table,
*                         RESERVED_GetFinalBitmapImage, RESERVED_GetFinalImageInformation,
*                         RESERVED_ReadUniformityMask,  RESERVED_WriteUniformityMask,
*                         RESERVED_ControlTransfer)
****************************************************************************************************
*/

#pragma once

#include "IBScanUltimateApi_defs.h"
#include "IBScanUltimateApi_err.h"
#include "IBScanUltimate.h"

#ifdef __cplusplus
extern "C" { 
#endif

/*
****************************************************************************************************
* GLOBAL DEFINES
****************************************************************************************************
*/

/* Minimum LE voltage value. */
#define RESERVED_MIN_LE_VOLTAGE_VALUE   0

/* Maximum LE voltage value. */
#define RESERVED_MAX_LE_VOLTAGE_VALUE  15

/*  */
#define RESERVED_SAVE_FINAL_IMAGE_RESULT_IMAGE            0
#define RESERVED_SAVE_FINAL_IMAGE_INPUT_IMAGE             1

/*  */
#define RESERVED_GET_FINAL_IMAGE_BRIGHTNESS_MEAN          0
#define RESERVED_GET_FINAL_IMAGE_DAC                      1

/*
****************************************************************************************************
* GLOBAL TYPES
****************************************************************************************************
*/

/*
****************************************************************************************************
* RESERVED_DeviceInfo
*
* DESCRIPTION:
*     Container for device information.
****************************************************************************************************
*/
typedef struct tagRESERVED_DeviceInfo
{
    /* Serial number. */
    char serialNumber[IBSU_MAX_STR_LEN];
    
    /* Product name. */
    char productName[IBSU_MAX_STR_LEN];
    
    /* Interface type (USB). */
    char interfaceType[IBSU_MAX_STR_LEN];
    
    /* Firmare version. */
    char fwVersion[IBSU_MAX_STR_LEN];

    /* Firmare version. */
    char fwVersion2[IBSU_MAX_STR_LEN];
    
    /* Revision. */
    char devRevision[IBSU_MAX_STR_LEN];
    
    /* Device handle. */
    int  handle;
    
    /* Indicates whether device is open. */
    BOOL IsHandleOpened;
    
    /* Manufacturer identifier. */
    char vendorID[IBSU_MAX_STR_LEN];
    
    /* Production date. */
    char productionDate[IBSU_MAX_STR_LEN];
    
    /* Last service date. */
    char serviceDate[IBSU_MAX_STR_LEN];

	/* Reserved 1 */
	char reserved1[IBSU_MAX_STR_LEN];

	/* Reserved 2 */
	char reserved2[IBSU_MAX_STR_LEN];
}
RESERVED_DeviceInfo;

/*
****************************************************************************************************
* IBSM_MatchedInfo
*
* DESCRIPTION:
*     This Stcture use for PAD Test/Match test 
****************************************************************************************************
*/
typedef struct tag_IBSM_MatchedInfo
{
    unsigned char       MatchedCount;
    unsigned short      MatchedScore[256];
    unsigned short		MatchedPosX[256];
    unsigned short		MatchedPosY[256];
    unsigned short		MatchedAngle[256];
}
IBSM_MatchedInfo;

/*
****************************************************************************************************
* GLOBAL FUNCTIONS
****************************************************************************************************
*/

/*
****************************************************************************************************
* RESERVED_GetDeviceInfo()
* 
* DESCRIPTION:
*     Retrieve detailed device information about a particular scanner by its logical index.
*
* ARGUMENTS:
*     deviceIndex   Zero-based index of the scanner.
*     pReservedKey  Key to unlock reserved functionality.
*     pDeviceInfo   Pointer to structure that will receive description of device.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetDeviceInfo
    (const int            deviceIndex,
     const char          *pReservedKey,
     RESERVED_DeviceInfo *pDeviceInfo);

/*
****************************************************************************************************
* RESERVED_OpenDevice()
* 
* DESCRIPTION:
*     Initialize a device and obtain a handle for subsequent function calls.  Any initialized device
*     must be released with IBSU_CloseDevice() or IBSU_CloseAllDevice() before shutting down the 
*     application.
*
* ARGUMENTS:
*     deviceIndex   Zero-based index of the scanner.
*     pReservedKey  Key to unlock reserved functionality.
*     pHandle       Pointer to variable that will receive device handle for subsequent function calls.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_OpenDevice
    (const int   deviceIndex,
     const char *pReservedKey,
     int        *pHandle);

/*
****************************************************************************************************
* RESERVED_WriteEEPROM()
* 
* DESCRIPTION:
*     Write to Cypress EEPROM.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     addr          Address of EEPROM.
*     pData         Pointer to data buffer.
*     len           Length of data buffer.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_WriteEEPROM
    (const int   handle,
     const char *pReservedKey,
     const WORD  addr,
     const BYTE *pData,
     const int   len);

/*
****************************************************************************************************
* RESERVED_ReadEEPROM()
* 
* DESCRIPTION:
*     Read from Cypress EEPROM.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     addr          Address of EEPROM.
*     pData         Pointer to data buffer.
*     len           Length of data buffer.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_ReadEEPROM
    (const int   handle,
     const char *pReservedkey,
     const WORD  addr,
     BYTE       *pData,
     const int   len);

/*
****************************************************************************************************
* RESERVED_SetProperty()
* 
* DESCRIPTION:
*     Set the value of a property for a device.  For descriptions of properties and values, see 
*     definition of 'IBSU_PropertyId'.  This function can set reserved properties such as serial 
*     number, production date, and service date.
*
* ARGUMENTS:
*     handle         Device handle.
*     pReservedKey   Key to unlock reserved functionality.
*     propertyId     Property for which value will be set.
*     propertyValue  Value of property to set.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_SetProperty
    (const int             handle,
     const char           *pReservedKey,
     const IBSU_PropertyId propertyId,
     LPCSTR                propertyValue);

/*
****************************************************************************************************
* RESERVED_GetLEVoltage()
* 
* DESCRIPTION:
*     Get the LE voltage value for a device.
*
* ARGUMENTS:
*     handle         Device handle.
*     pReservedKey   Key to unlock reserved functionality.
*     pVoltageValue  Pointer to variable that will receive LE voltage value.  Value will be between
*                    RESERVED_MIN_LE_VOLTAGE_VALUE and RESERVED_MAX_LE_VOLTAGE_VALUE, inclusive.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetLEVoltage
    (const int   handle,
     const char *pReservedKey,
     int        *pVoltageValue);

/*
****************************************************************************************************
* RESERVED_SetLEVoltage()
* 
* DESCRIPTION:
*     Set the LE voltage value for a device.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     voltageValue  LE voltage value.  Value must be between RESERVED_MIN_LE_VOLTAGE_VALUE and 
*                   RESERVED_MAX_LE_VOLTAGE_VALUE, inclusive.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_SetLEVoltage
    (const int   handle,
     const char *pReservedKey,
     const int   voltageValue);

/*
****************************************************************************************************
* IBSU_BeginCaptureImage()
* 
* DESCRIPTION:
*     Begin acquiring an image from a device, without using capture thread.
*
* ARGUMENTS:
*     handle           Device handle.
*     pReservedKey     Key to unlock reserved functionality.
*     imageType        Type of capture.
*     imageResolution  Resolution of capture.
*     captureOptions   Bit-wise OR of capture options:
*                          IBSU_OPTION_AUTO_CONTRAST - automatically adjust contrast to optimal value
*                          IBSU_OPTION_AUTO_CAPTURE - complete capture automatically when a good-
*                              quality image is available
*                          IBSU_OPTION_IGNORE_FINGER_COUNT - ignore finger count when deciding to 
*                              complete capture
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_BeginCaptureImage
    (const int                  handle,
     const char                *pReservedKey,
     const IBSU_ImageType       imageType,
     const IBSU_ImageResolution imageResolution,
     const DWORD                captureOptions);

/*
****************************************************************************************************
* RESERVED_GetOneFrameImage()
* 
* DESCRIPTION:
*     Get image from a device, without using capture thread.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     pRawImage     Pointer to buffer that will receive raw image data.
*     imageLength   Length of image buffer.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetOneFrameImage
    (const int      handle,
     const char    *pReservedKey,
     unsigned char *pRawImage,
     const int      imageLength);

/*
****************************************************************************************************
* RESERVED_GetFpgaRegister()
* 
* DESCRIPTION:
*     Get the value of an FPGA register for a device.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     address       Address of FPGA register.
*     pValue        Pointer to variable that will receive FPGA register value.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetFpgaRegister
    (const int           handle,
     const char         *pReservedKey,
     const unsigned char address,
     unsigned char      *pValue);

/*
****************************************************************************************************
* RESERVED_SetFpgaRegister()
* 
* DESCRIPTION:
*     Set the value of an FPGA register for a device.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     address       Address FPGA register.
*     value         FPGA register value.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_SetFpgaRegister
    (const int           handle,
     const char         *pReservedKey,
     const unsigned char address,
     const unsigned char value);

/*
****************************************************************************************************
* RESERVED_CreateClientWindow()
* 
* DESCRIPTION:
*     Create client window associated with device.  (Available only on Windows.)
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     hWindow       Windows handle to draw.
*     left          Coordinate of left edge of rectangle.
*     top           Coordinate of top edge of rectangle.
*     right         Coordinate of right edge of rectangle.
*     bottom        Coordinate of bottom edge of rectangle.
*     rawImgWidth   Width of raw image.
*     rawImgHeight  Height of raw image.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_CreateClientWindow
    (const int       handle,
     const char     *pReservedKey,
     const IBSU_HWND hWindow,
     const DWORD     left,
     const DWORD     top,
     const DWORD     right,
     const DWORD     bottom,
     const DWORD     rawImgWidth,
     const DWORD     rawImgHeight);

/*
****************************************************************************************************
* RESERVED_DrawClientWindow()
* 
* DESCRIPTION:
*     Draw image on client window associated with device.  (Available only on Windows.)
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     drawImage     Pointer to buffer containing image to draw.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_DrawClientWindow
    (const int      handle,
     const char    *pReservedKey,
     unsigned char *drawImage);

/*
****************************************************************************************************
* RESERVED_UsbBulkOutIn()
* 
* DESCRIPTION:
*     Perform raw USB bulk transaction to device.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     outEp         OUT endpoint to send data to.
*     uiCommand     Command to send.
*     outData       Pointer to buffer with data to send.  To send no data, pass NULL.
*     outDataLen    Length of data to send.
*     inEp          IN endpoint to receive data from.
*     inData        Pointer to buffer which will receive data.  To receive no data, pass NULL.
*     inDataLen     Length of data to receive.
*     nBytesRead    Pointer to variable that will receive number of bytes received.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_UsbBulkOutIn
    (const int           handle,
     const char         *pReservedKey,
     const int           outEp,
     const unsigned char uiCommand,
     unsigned char      *outData,
     const int           outDataLen,
     const int           inEp,
     unsigned char      *inData,
     const int           inDataLen,
     int                *nBytesRead);

/*
****************************************************************************************************
* RESERVED_InitializeCamera()
* 
* DESCRIPTION:
*     Initialize camera on device.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_InitializeCamera
    (const int   handle,
     const char *pReservedKey);

/*
****************************************************************************************************
* RESERVED_GetEnhancedImage()
* 
* DESCRIPTION:
*     Enhanced image from the input preview image.
*
* ARGUMENTS:
*     handle                 Device handle.
*     pReservedKey           Key to unlock reserved functionality.
*     inImage                Input image data which is returned from preview callback.
*     enhandedImage          Pointer to structure that will receive data of enhanced preview image.
*                            The buffer in this structure points to an internal image buffer; the
*                            data should be copied to an application buffer if desired for future processing.
*     segmentImageArrayCount Pointer to variable that will receive number of finger images split from input image.
*     segmentImageArray     The buffer in this structure points to an internal image buffer; the
*                            data should be copied to an application buffer if desired for future processing.
*     segmentPositionArray  Array of structures with position data for individual fingers split from input image.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetEnhancedImage
    (const int              handle,
     const char            *pReservedKey,
     const IBSU_ImageData   inImage,
     IBSU_ImageData        *enhancedImage,
     int                   *segmentImageArrayCount,
     IBSU_ImageData        *segmentImageArray,
     IBSU_SegmentPosition  *segmentPositionArray);


/*
****************************************************************************************************
* RESERVED_GetFinalImageByNative()
* 
* DESCRIPTION:
*     get a native image for the final capture.
*
* ARGUMENTS:
*     handle                 Device handle.
*     pReservedKey           Key to unlock reserved functionality.
*     finalImage             Pointer to structure that will receive data of final image by native
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetFinalImageByNative
    (const int              handle,
     const char            *pReservedKey,
     IBSU_ImageData        *finalImage);


/*
****************************************************************************************************
* RESERVED_GetSpoofScore()
* 
* DESCRIPTION:
*     get a spoof score for the finger.
*
* ARGUMENTS:
*     pReservedKey           Key to unlock reserved functionality.
*     deviceID               Device ID captured the image.
*     pImage                 Pointer to fingerprint image
*     Width                  Width of pImage
*     Height                 Height of pImage
*     Pitch                  Image line pitch (in bytes).  A positive value indicates top-down line order; a
*                            negative value indicates bottom-up line order.
*     pScore                 Pointer to return spoof score (the score range is from 0 to 1000)
*                            The closer to 1000 score, it means Live finger.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetSpoofScore
    (const char            *pReservedKey,
     const BYTE            *pImage,
     const int             Width,
	 const int             Height,
	 const int             Pitch,
	 int                   *pScore);

/*
****************************************************************************************************
* RESERVED_GetEncyptedImage()
* 
* DESCRIPTION:
*     Get encrypted image from a device, without using capture thread.
*
* ARGUMENTS:
*     handle        Device handle.
*     pReservedKey  Key to unlock reserved functionality.
*     pEncKey       Key to be encrypted.
*     pEncRawImage  Pointer to structure that will receive raw encrypted image data.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetEncryptedImage
    (const int          handle,
     const char         *pReservedKey,
     unsigned char      *pEncKey,
     IBSU_ImageData     *pRawEncImage);

/*
****************************************************************************************************
* RESERVED_ConvetToDecryptImage()
* 
* DESCRIPTION:
*     Convert to decrypted image from an encrypted image.
*
* ARGUMENTS:
*     pReservedKey  Key to unlock reserved functionality.
*     pEncKey       Key to be encrypted.
*     pEncRawImage  Pointer to structure that was received raw encrypted image data.
*     pDecRawImage  Decrypted image from an encrypted image.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_ConvertToDecryptImage
    (const char         *pReservedKey,
     unsigned char      *pEncKey,
     IBSU_ImageData     rawEncImage,
     IBSU_ImageData     *pRawDecImage);
/*
****************************************************************************************************
* RESERVED_GetHashCode()
* 
* DESCRIPTION:
*     Get Hash Code From the PlainText
* 
* ARGUMENTS:
*     pReservedKey        Key to unlock reserved functionality.
*     nHashType           Type of Hash
*     pPlainText          PlainText to get hash code
*     nPlainTextSize      size of the PlainText
*     pHashCode           Hash code result
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetHashCode
    (const char         *pReservedKey,
     const int          nHashType,
     const char         *pPlainText,
     const int          nPlainTextSize,
     char               *pHashCode);

/*
****************************************************************************************************
* RESERVED_Set_DeviceLockInfo()
* 
* DESCRIPTION:
*     It writes lock information to a device. After this, use IBSU_Set_DeviceLock to lock the device.
* 
* ARGUMENTS:
*     handle              Handle for device associated with this event (if appropriate).
*     pReservedKey        Key to unlock reserved functionality.
*     nHashType           Type of Hash
*     pCustomerKey        Customer Key to match lock info written in the locked device.
*     pHashCode           Hash Code
*     pCustomerString     Customer String to return when Customer Key is matched after opendevice
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_Set_DeviceLockInfo
    (const int              handle,
     const char             *pReservedKey,
     const int              nHashType,
     const char             *pCustomerKey,
     const unsigned char    *pHashCode,
     const char             *pCustomerString);

/*
****************************************************************************************************
* RESERVED_Erase_DeviceLockInfo()
* 
* DESCRIPTION:
*     It erases lock information of device.
* 
* ARGUMENTS:
*     handle              Handle for device associated with this event (if appropriate).
*     pReservedKey        Key to unlock reserved functionality.
*     nHashType           Type of Hash
*     pCustomerKey        Customer Key to match lock info written in the locked device.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_Erase_DeviceLockInfo
    (const int             handle,
     const char            *pReservedKey,
     const int             nHashType,
     const char*           pCustomerKey);



/*
****************************************************************************************************
* RESERVED_Set_DeviceLock()
* 
* DESCRIPTION:
*     It locks or unlocks a device.
* 
* ARGUMENTS:
*     handle              Handle for device associated with this event (if appropriate).
*     pReservedKey        Key to unlock reserved functionality.
*     nHashType           Type of Hash
*     pCustomerKey        Customer Key to match lock info written in the locked device.
*     bLock               Lock(TRUE) or Unlock(FALSE) 
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_Set_DeviceLock
    (const int            handle,
     const char           *pReservedKey,
     const int            nHashType,
     const char*          pCustomerKey,
     const BOOL           bLock);



/*
****************************************************************************************************
* RESERVED_SetAdminMode()
* 
* DESCRIPTION:
*     It sets Administrator mode for Locking Kojak.
* 
* ARGUMENTS:
*     pReservedKey        Key to unlock reserved functionality.
*     bEnable             Enable(TRUE) or Disable(FALSE) 
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_SetAdminMode
    (const char           *pReservedKey,
     const BOOL           bEnable);


int WINAPI RESERVED_GetSpoofScoreEx
    (const char            *pReservedKey,
     const BYTE            *pImage,
     const int             Width,
	 const int             Height,
	 const int             Pitch,
     const int             bgWidth, 
     const int             bgHeight,
	 int                   *pScore);


/*
****************************************************************************************************
* RESERVED_RegisterImageToDB()
*
* DESCRIPTION:
*     Subject name and register the fingerprint captured in DB.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     pSubjectName              Entered subject name.
*     image                     Input image data.
*     imageType                 Type of capture.
*     pMinutiaeInfo             Minutiae info. (Count, Score, X, Y, Angel)
*     nfiq                      NIST fingerprint image quality (NFIQ)
*     pMinutiaeCount            Minutiae count
*     pTotalMinutiaeCount       Total minutiae count
*     pEnrollScore              Enroll score
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_RegisterImageToDB(
    const int                 handle,
    const char*               pReservedKey,
    const char*               pSubjectName,
    const IBSU_ImageData      image,
    const IBSU_ImageType      imageType,
    IBSM_MatchedInfo*         pMinutiaeInfo,
    int					      nfiq,
    int*                      pMinutiaeCount,
    int*                      pTotalMinutiaeCount,
    int*                      pEnrollScore);

/*
****************************************************************************************************
* RESERVED_AddImageToRegistered()
*
* DESCRIPTION:
*     Register the image in DB.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     pSubjectName              Entered subject name.
*     image                     Input image data.
*     imageType                 Type of capture.
*     pMinutiaeInfo             Minutiae info. (Count, Score, X, Y, Angel)
*     nfiq                      NIST fingerprint image quality (NFIQ)
*     pMinutiaeCount            Minutiae count
*     pTotalMinutiaeCount       Total minutiae count
*     pEnrollScore              Enroll score
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_AddImageToRegistered(
    const int                 handle,
    const char*               pReservedKey,
    const IBSU_ImageData      image,
    const IBSU_ImageType      imageType,
    IBSM_MatchedInfo*         pMinutiaeInfo,
    int					      nfiq,
    int*                      pMinutiaeCount,
    int*                      pTotalMinutiaeCount,
    int*                      pEnrollScore);



/*
****************************************************************************************************
* RESERVED_DeleteImageFromDB()
*
* DESCRIPTION:
*     Delete the data of the corresponding index from the DB.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     index                     DB index of the data to be deleted.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_DeleteImageFromDB(
    const int                 handle,
    const char*               pReservedKey,
    const int				  index);



/*
****************************************************************************************************
* RESERVED_ClearAllImageFromDB()
*
* DESCRIPTION:
*     Delete all data in DB.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_ClearAllImageFromDB(
    const int                 handle,
    const char*               pReservedKey);



/*
****************************************************************************************************
* RESERVED_MatchImageWithDB()
*
* DESCRIPTION:
*     Find the index matching the input fingerprint image in DB.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     image                     Input image data.
*     pMinutiaeInfo             Minutiae count.
*     nMatchingAngle            Matching angle.
*     nMatchingScore            Matching score.
*     pMatchedIndex             Matched index in DB.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_MatchImageWithDB(
    const int                 handle,
    const char*               pReservedKey,
    const IBSU_ImageData      image,
    IBSM_MatchedInfo*         pMinutiaeInfo,
    const unsigned short      nMatchingAngle,
    const unsigned int        nMatchingScore,
    int*                      pMatchedIndex);



/*
****************************************************************************************************
* RESERVED_SaveDBToFile()
*
* DESCRIPTION:
*     Save the DB as a file.
*
* ARGUMENTS:
*     pReservedKey              Key to unlock reserved functionality.
*     pFilePath                 Path to save DB file.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_SaveDBToFile(
    const char* pReservedKey,
    const char* pFilePath);



/*
****************************************************************************************************
* RESERVED_LoadDBFromFile()
*
* DESCRIPTION:
*     Load from DB file.
*
* ARGUMENTS:
*     pReservedKey              Key to unlock reserved functionality.
*     pFilePath                 Path of DB file to be load.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_LoadDBFromFile(
    const char* pReservedKey,
    const char* pFilePath);



/*
****************************************************************************************************
* RESERVED_UpdateDBList()
*
* DESCRIPTION:
*     Update the DB file.
*
* ARGUMENTS:
*     pReservedKey              Key to unlock reserved functionality.
*     pFilePath                 File path of DB to be update.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_UpdateDBList(
    const char* pReservedKey,
    void*       pList);



/*
****************************************************************************************************
* RESERVED_GetSecurityLevel()
*
* DESCRIPTION:
*     Get the security level.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     pSecurityLevel            Currently security level.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetSecurityLevel(
    const int                 handle,
    const char*               pReservedKey,
    int*                      pSecurityLevel);



/*
****************************************************************************************************
* RESERVED_SetSecurityLevel()
*
* DESCRIPTION:
*     Set the security level.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     SecurityLevel             Value to set as security level.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_SetSecurityLevel(
    const int                 handle,
    const char*               pReservedKey,
    int					      SecurityLevel);



/*
****************************************************************************************************
* RESERVED_GetMatchedPoint()
*
* DESCRIPTION:
*     Get the matchedpoints.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     pMatchedInfo              Matched information.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetMatchedPoint(
    const int           handle,
    const char*         pReservedKey,
    IBSM_MatchedInfo*   pMatchedInfo);



/*
****************************************************************************************************
* RESERVED_GetMinutiaeInfo()
*
* DESCRIPTION:
*     Get the minutiae infomation.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     image                     Image data.
*     pMatchedInfo              Matched information.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetMinutiaeInfo(
    const int                 handle,
    const char*               pReservedKey,
    const IBSU_ImageData      image,
    IBSM_MatchedInfo*         pMinutiaeInfo);



/*
****************************************************************************************************
* RESERVED_SetSaveImageLog()
*
* DESCRIPTION:
*
*
* ARGUMENTS:
*     pReservedKey              Key to unlock reserved functionality.
*     bUse
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_SetSaveImageLog(
    const char* pReservedKey,
    const BOOL bEnable);



/*
****************************************************************************************************
* RESERVED_GetSaveImageLog()
*
* DESCRIPTION:
*     Get the
*
* ARGUMENTS:
*     pReservedKey              Key to unlock reserved functionality.
*     bUse
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetSaveImageLog(
    const char* pReservedKey,
    BOOL* bOutEnabled);
/*
****************************************************************************************************
* RESERVED_ePADLoadExternalSettings()
*
* DESCRIPTION:
*     Load ePAD setting from the file
*
* ARGUMENTS:
*     pReservedKey              Key to unlock reserved functionality.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_ePADLoadExternalSettings(
    const int handle,
    const char* pReservedKey);

/**
    ****************************************************************************************************
    * @brief    Convert images of IBScanUltimate from 1 FP scanner(Curve/Columo/Danno) to the IBISDK image type.
    *           This API is used for IBISDK users want to use Columbo/Danno scanner with IBISDK's Matcher feature.
    *           The following shows the difference of the image type between IBScanUltimate and IBISDK.
    * 
    *           1. IBScanUltimate's Image Type
    *             1) Background Color / FP Color : Whight / Black(0-254 Gray Scale)
    *             2) Image Size for each scanner : Curve (288w*352h), Columbo(400w*500h), Danno(400w*500h)
    *             3) Direction of FP Image : Top is on the up side.
    *           
    *           2. IBISDK's Image Type
    *             1) Background Color / FP Color : Black / White(1-255 Gray Scale)
    *             2) Image Size : Curve (352w*288h)
    *             3) Direction of FP Image : Top is on the right side.
    * 
    *           3. Usage
    *             Example) RESERVED_ConvertToIBISDKImage("ibkorea1120!", (BYTE*)inImage, inImage_Width, inImage_Height, TRUE, TRUE, (BYTE*)outImage);
    *
    * @param    [in]    pReservedKey  Key to unlock reserved functionality.
    * @param    [in]    inImage       Input Image buffer (Curve/Columbo/Danno images from IBScanUltimate)
    * @param    [in]    width         Width of the inImage
    * @param    [in]    height        Height of the inImage
    * @param    [in]    bRotation     TRUE(90�� Rotation to the right), FALSE(No Rotation)    
    * @param    [in]    bInvert       TRUE(Invert Image), FALSE(No Invert)
    * @param    [out]   outImage      Output Image buffer in 352(w)*288(h) size
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
int WINAPI RESERVED_ConvertToIBISDKImage(
    const char*     pReservedKey,
    BYTE*           inImage,
    int             width,
    int             height,
    BOOL            bRotation,
    BOOL            bInvert,
    BYTE*			outImage);


/**
    ****************************************************************************************************
    * RESERVED_IsValidFingerGeometryEx()
    *
    * @brief
    *     Check for hand and finger geometry based on position array and return whether it is correct or not.
    *
    * @param    [in] handle         Device handle obtained by OpenDevice functions.
    * @param
    *     pSegmentPositionArray     Pointer to array of four structures that will receive position data for
    *                               individual fingers split from output image.
    * @param
    *     fIndex              Bit-pattern of finger index of input image.
    *                         ex) IBSU_FINGER_LEFT_LITTLE | IBSU_FINGER_LEFT_RING in IBScanUltimateApi_defs.h
    * @param
    *     pValid              Pointer to variable that will receive whether it is valid or not.  TRUE to valid; FALSE to invalid.
    *
    * @return
    *           ::IBSU_STATUS_OK, if successful.\n
    *           Error code < 0, otherwise.  @see See error codes in 'IBScanUltimateApi_err.h'. \n
    ****************************************************************************************************
    */
int WINAPI RESERVED_IsValidFingerGeometryEx(
    const char*             pReservedKey, 
    const int               handle,
    IBSU_SegmentPosition*   segmentPositionArray,
    int                     segmentPositionArrayCount,
    const DWORD             fIndex,
    BOOL*                   pValid);

    /*
    ****************************************************************************************************
    * RESERVED_GetCBPPreviewInfo()
    *
    * DESCRIPTION:
    *     get a cbp information for preview image.
    *
    * ARGUMENTS:
    *     handle                             Device handle.
    *     pReservedKey                       Key to unlock reserved functionality.
    *     segmentImageArray                  The buffer in this structure points to an internal image buffer; the
    *                                        data should be copied to an application buffer if desired for future processing.
    *     segmentPositionArray               Array of structures with position data for individual fingers split from input image.
    *     segmentPositionArray_for_geo       Array of structures with position data for individual fingers split from input image.
    *     IsFingerDetected                   Indicate if finger is detected
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
    */
int WINAPI RESERVED_CBP_GetPreviewInfo(
    const int               handle,
    const char*             pReservedkey,
    IBSU_SegmentPosition*   segmentPositionArray,
    IBSU_SegmentPosition*   segmentPositionArray_for_geo,
    int*                    segmentPositionArrayCount,
    BOOL*                   IsFingerDetected,
    IBSU_FingerQualityState* segmentPositionQuality);

int WINAPI RESERVED_CBP_CleanUp
(const int              handle);

int WINAPI RESERVED_CBP_IsFingerOn
(const int              handle,
    BOOL* IsNoFinger);

int WINAPI RESERVED_CBP_PrevCapture_FingerCount
(const int              handle,
    int* nFingerCnt);

int WINAPI RESERVED_CBP_FingerRemovedAfterScan
(const int              handle,
    BOOL* bIsRemoved);

int WINAPI RESERVED_FPGA_Set_Capture_Mode
(const int              handle,
    const char* pReservedKey,
    const int              nCaptureMode);

int WINAPI RESERVED_Image_Binarization
(const int              handle,
    const char* pReservedKey,
    BYTE* pInImage,
    BYTE* pOutImage,
    const int              nWidth,
    const int              nHeight);

    /*
    ****************************************************************************************************
    * RESERVED_GetFpgaVersion()
    *
    * DESCRIPTION:
    *     Get FPGA version for a device.
    *
    * ARGUMENTS:
    *     handle                             Device handle.
    *     pReservedKey                       Key to unlock reserved functionality.
    *     pFpgaVersion                       Pointer to variable that will receive FPGA version value.
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_GetFpgaVersion(
    const int handle,
    const char* pReservedKey,
    int* pFpgaVersion);

/*
    ****************************************************************************************************
    * RESERVED_DeviceDetectionByPolling()
    *
    * DESCRIPTION:
    *     Enable Device count with Polling
    *     Currently Android support only. (Fallback purpose)
    * ARGUMENTS:
    *     pReservedKey                       Key to unlock reserved functionality.
    *     bEnable                            Polling every 250ms for device count check
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_DeviceDetectionByPolling(
    const char* pReservedKey);


/*
    ****************************************************************************************************
    * RESERVED_Set_DAC_Register()
    *
    * DESCRIPTION:
    *     Sets the DAC hex value.
    *
    * ARGUMENTS:
    *     pReservedKey                       Key to unlock reserved functionality.
    *     nDacHexValue                       DAC hex value to set.
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_Set_DAC_Register(
    const int       handle,
    const char* pReservedKey,
    const USHORT    nDacHexValue);


/*
    ****************************************************************************************************
    * RESERVED_Set_DAC_Register_Reference_DAC_Table()
    *
    * DESCRIPTION:
    *     Set the DAC using the DAC table index value.
    *
    * ARGUMENTS:
    *     handle                             Handle for device associated with this event (if appropriate).
    *     pReservedKey                       Key to unlock reserved functionality.
    *     nDacTableIndex                     Enter the index of the DAC table. (0 ~ 34)
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_Set_DAC_Register_Reference_DAC_Table(
    const int       handle,
    const char* pReservedKey,
    const int       nDacTableIndex);


/*
****************************************************************************************************
* RESERVED_GetFinalBitmapImage()
*
* DESCRIPTION:
*       Get the final image that matches the type.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     pFilePath                 Path to save image file.
*     nFinalImageType           Specifies the image type to get.
*                               (RESERVED_SAVE_FINAL_IMAGE_RESULT_IMAGE, RESERVED_SAVE_FINAL_IMAGE_INPUT_IMAGE, ...)
*     pOutImageData             Returns an image data pointer.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetFinalBitmapImage(
    const int       handle,
    const char* pReservedKey,
    const int       nFinalImageType,
    IBSU_ImageData* pOutImageData);


/*
****************************************************************************************************
* RESERVED_GetFinalImageInformation()
*
* DESCRIPTION:
*       Retrieves information about the final image that matches the type.
*
* ARGUMENTS:
*     handle                    Handle for device associated with this event (if appropriate).
*     pReservedKey              Key to unlock reserved functionality.
*     nInfoDefineType           Identifier to get value for.
                                (RESERVED_GET_FINAL_IMAGE_BRIGHTNESS_MEAN, RESERVED_GET_FINAL_IMAGE_DAC, ...)
*     pOutStrValue              String returning value. Memory must be provided by caller.
*                               This buffer should be able to hold ::IBSU_MAX_STR_LEN characters.
*
* RETURNS:
*     IBSU_STATUS_OK, if successful.
*     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
****************************************************************************************************
*/
int WINAPI RESERVED_GetFinalImageInformation(
    const int       handle,
    const char* pReservedKey,
    const int       nInfoDefineType,
    LPSTR           pOutStrValue);


/*
    ****************************************************************************************************
    * RESERVED_ReadUniformityMask()
    *
    * DESCRIPTION:
    *     Read a uniform mask on the device.
    *
    * ARGUMENTS:
    *     handle                             Device handle.
    *     pReservedKey                       Key to unlock reserved functionality.
    *
    *     [OUT] pMaskData                    Mask data
    *     nMaskDataLen                       Mask data lenth
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_ReadUniformityMask(
    const int    handle,
    const char*  pReservedKey,
    BYTE*        pOutMaskData,
    const int    nMaskDataLen);

/*
    ****************************************************************************************************
    * RESERVED_WriteUniformityMask()
    *
    * DESCRIPTION:
    *     Write a uniform mask on the device.
    *
    * ARGUMENTS:
    *     handle                             Device handle.
    *     pReservedKey                       Key to unlock reserved functionality.
    *
    *     [IN] pMaskData                     Mask data
    *     nMaskDataLen                       Mask data lenth
    *     [OUT] pProgress                    Progress
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_WriteUniformityMask(
    const int    handle,
    const char*  pReservedKey,
    const BYTE*  pMaskData,
    const int    nMaskDataLen,
    int*         pProgress);

/*
    ****************************************************************************************************
    * RESERVED_ControlTransfer()
    *
    * DESCRIPTION:
    *     RESERVED_ControlTransfer function transmits control data over a default control endpoint. (WinUSB, libusb)
    *
    * ARGUMENTS:
    *     handle                             Device handle.
    *     pReservedKey                       Key to unlock reserved functionality.
    *
    *     requestType                        The request type.
    *                                        The values that are assigned to this member are defined in Table 9.2 of section 9.3 of the Universal Serial Bus (USB) specification
    *     request                            The device request.
    *                                        The values that are assigned to this member are defined in Table 9.3 of section 9.4 of the Universal Serial Bus (USB) specification.
    *     value                              The meaning of this member varies according to the request.
    *     index                              The meaning of this member varies according to the request.
    *     length                             The number of bytes to transfer.
    *     [OUT] pBuffer                      A caller-allocated buffer that contains the data to transfer.
    *                                        The length of this buffer must not exceed 4KB.
    *     nBufferLen                         The number of bytes to transfer, not including the setup packet.
    *                                        This number must be less than or equal to the size, in bytes, of Buffer.
    *     [OUT] pTransferred                 A pointer to a ULONG variable that receives the actual number of transferred bytes.
    *                                        If the application does not expect any data to be transferred during the data phase (BufferLength is zero), LengthTransferred can be NULL.
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_ControlTransfer(
    const int     handle,
    const char*   pReservedKey,
    UCHAR         requestType,
    UCHAR         request,
    USHORT        value,
    USHORT        index,
    USHORT        length,
    UCHAR*        pBuffer,
    const ULONG   nBufferLen,
    ULONG*        pTransferred);

/*
    ****************************************************************************************************
    * RESERVED_invokeAndroidDestructor()
    *
    * DESCRIPTION:
    *     Explicitly invokes the destructor of the IBScanUltimate library on Android.
    *     This is required because the library's destructor is not automatically called
    *     when the Android application terminates.
	*
    * ARGUMENTS: None
    *
    * RETURNS:
    *     IBSU_STATUS_OK, if successful.
    *     Error code < 0, otherwise.  See error codes in 'IBScanUltimateApi_err'.
    ****************************************************************************************************
*/
int WINAPI RESERVED_invokeAndroidDestructor(void);

#ifdef __cplusplus
} // extern "C"
#endif
