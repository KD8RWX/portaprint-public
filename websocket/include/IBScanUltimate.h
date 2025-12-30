/**
****************************************************************************************************
* @file
*       IBScanUltimate.h
*
* @brief
*       Definition of image data structures for IBScanUltimate.
*				https://integratedbiometrics.com/
* @author
*       Integrated Biometrics, LLC
*
* @copyright
*       Copyright (c) Integrated Biometrics, 2009-2022 \n
*       http://www.integratedbiometrics.com
*
* @page page_File_Revision_History Revision History for the files
* @section section_IBScanUltimate IBScanUltimate.h
* @li @par  2015/04/07
*                       Added enumeration value to IBSU_ImageData.
*                       (ProcessThres)
* @li @par  2013/08/03
*                       Reformatted.
* @li @par  2012/04/12
*                       Created.
****************************************************************************************************
*/

#pragma once

#ifdef __linux__
#include "LinuxPort.h"
#endif

#ifdef __cplusplus
extern "C" {
#endif 


    /**
    ****************************************************************************************************
    * @defgroup group_Enumeration_ImageFormat Enumeration - ImageFormat
    * @brief    Enumeration of image formats.
    * @{
    ****************************************************************************************************
    */
    typedef enum
    {
        /** Gray-scale image. */
        IBSU_IMG_FORMAT_GRAY,
        /** 24-bit color image. */
        IBSU_IMG_FORMAT_RGB24,
        /** True-color RGB image. */
        IBSU_IMG_FORMAT_RGB32,
        /** Unknown format. */
        IBSU_IMG_FORMAT_UNKNOWN
    }
    IBSU_ImageFormat;
    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */



    /**
    ****************************************************************************************************
    * @defgroup group_Structure_ImageData Structure - ImageData
    * @brief    Container for image data and metadata.
    * @{
    ****************************************************************************************************
    */
    typedef struct 
    {
        /** Pointer to image buffer.  If this structure is supplied by a callback function, this pointer 
        * must not be retained; the data should be copied to an application buffer for any processing
        * after the callback returns. */
        void                *Buffer;                           

        /** Image horizontal size (in pixels). */
        DWORD               Width;                            

        /** Image vertical size (in pixels). */
        DWORD               Height;                           

        /** Horizontal image resolution (in pixels/inch). */
        double              ResolutionX;                      

        /** Vertical image resolution (in pixels/inch). */
        double              ResolutionY;                      

        /** Image acquisition time, excluding processing time (in seconds). */
        double              FrameTime;                        

        /** Image line pitch (in bytes).  A positive value indicates top-down line order; a negative 
        * value indicates bottom-up line order. */
        int                 Pitch;                            

        /* Number of bits per pixel. */
        BYTE                BitsPerPixel;

        /** Image color format. */
        IBSU_ImageFormat    Format;

        /** Marks image as the final processed result from the capture.  If this is FALSE, the image is
        * a preview image or a preliminary result. */
        BOOL                IsFinal;                    

        /** Threshold of image processing. */
        DWORD               ProcessThres;                    
    }IBSU_ImageData;
    /**
    ****************************************************************************************************
    * @}
    ****************************************************************************************************
    */

#ifdef __cplusplus
} // extern "C"
#endif 