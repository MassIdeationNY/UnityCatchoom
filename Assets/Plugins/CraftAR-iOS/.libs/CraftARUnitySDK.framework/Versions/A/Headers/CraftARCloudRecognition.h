// This file is free software. You may use it under the MIT license, which is copied
// below and available at http://opensource.org/licenses/MIT
//
// Copyright (c) 2014 Catchoom Technologies S.L.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the
// Software, and to permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

#import <Foundation/Foundation.h>

@protocol CraftARCloudRecognitionProtocol;

/**
  The CatchooomCloudRecognition class offers an interface
  for performing searches to the CRS. <br>
  It allows to search with frames from the attached video capture sending
  as much frames as possible to the CRS limited to 2 searches per second. <br>
  It will also allow to perform direct quieries with a snap shot taken from
  the preview.
 @see CraftARSDK::takeSnapshot
 */
@interface CraftARCloudRecognition :  NSObject

/// Delegate to handle CraftARCloudRecognition responses
@property (nonatomic, weak) id <CraftARCloudRecognitionProtocol> delegate;

/// Retrieve the bounding boxes for each item found
/**
 Default value: false
 @see http://catchoom.com/documentation/api/bounding-boxes/
 */
@property (nonatomic, readwrite) bool retrieveBoundingBox;

/// Retrieve the custom field for each item found
/**
 Default value: false
 @see CraftARCloudRecognitionItem
 @see http://catchoom.com/documentation/api/custom-data/
 */
@property (nonatomic, readwrite) bool DEPRECATED_ATTRIBUTE retrieveCustomField;

/// Embed the custom field for each item found
/**
 Default value: false
 @see CraftARCloudRecognitionItem
 @see http://catchoom.com/documentation/api/custom-data/
 */
@property (nonatomic, readwrite) bool embedCustomField;

/**
 Set the collection token
 @param collectionToken NSString containing the 16-char long token of a CRS colleciton.
 @see http://catchoom.com/documentation/where-do-i-get-my-token/
 */
- (void) setToken: (NSString*) collectionToken;


- (void) setConnectURL: (NSString*) url;
- (void) setSearchURL: (NSString*) url;

/**
 Set the maximum number of searches per second.
 @param maxSearchesPerSecond float value in range 0.0 < value <= 2.0
 @note This maximum is always limited to 2 searches per second.
 */
- (void) setMaxSearchesPerSecond: (float) maxSearchesPerSecond;

/**
 Start continuous search (finder mode)
 The SDK will send the capture's video frames to catchoom.com for recognition on
 the collection with the set token.
 @see CraftARSDK
 @see http://catchoom.com/documentation/faq/#continuous-scan
 */
- (void) startFinderMode;

/**
 Stop continuous search (finder mode) and ignore results of in-flight searches.
 */
- (void) stopFinderMode;

/**
 Capture a photo and start a search in single shot mode
 The SDK will capture a photo with the camera and query the CRS with the image taken. The camera preview will
 be frozen you can unfreeze it by calling [[CraftARSDK sharedCraftARSDK] unfreezeCapture];
 When a response is obtained the didGetSearchResults or didFailWithError messages will be sent to the delegate.
 */
- (void) singleShotSearch;

@end

@protocol CraftARCloudRecognitionProtocol <NSObject>

/**
 Callback for successful CRS request,
 @param resultItems contains a list of CraftARCloudRecognitionItem objects.
 */
- (void)didGetSearchResults: (NSString *) result;


/**
 Callback for successful token validation,
 */
- (void)didValidateToken: (NSString*) result;


@end
