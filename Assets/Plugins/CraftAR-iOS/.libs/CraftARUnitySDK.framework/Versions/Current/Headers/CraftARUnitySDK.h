//
//  CraftARUnitySDK.h
//  CraftARUnitySDK
//
//  Created by Luis Martinell Andreu on 15/10/14.
//  Copyright (c) 2014 Luis Martinell Andreu. All rights reserved.
//

#import <UIKit/UIKit.h>

//#define HAVE_SIGNED_TRACKING_DATA

//! Project version number for CraftARUnitySDK.
FOUNDATION_EXPORT double CraftARUnitySDKVersionNumber;

//! Project version string for CraftARUnitySDK.
FOUNDATION_EXPORT const unsigned char CraftARUnitySDKVersionString[];

// In this header, you should import all the public headers of your framework using statements like #import <CraftARUnitySDK/PublicHeader.h>

#import <CraftARUnitySDK/VideoFrame.h>
#import <CraftARUnitySDK/CraftARUnitySDKConstants.h>

@protocol CraftARUnityProtocol;

@interface CraftARUnitySDK : NSObject
@property (nonatomic, weak) id <CraftARUnityProtocol> delegate;
@property (nonatomic, readwrite) NSString * craftARSDKLicenseKey;


- (void) startCapture;
- (void) stopCapture;
- (void) triggerFocusAtPoint: (CGPoint)pointOfinterest;

- (void) startFinderMode;
- (void) stopFinderMode;
- (void) singleShotSearch;
- (void) setToken: (NSString*) token;
- (void) setConnectURL: (NSString*) url;
- (void) setSearchURL: (NSString*) url;

- (void) startTracking;
- (void) stopTracking;
- (int) addARItem: (NSString*) arItemData;
- (void) removeARItem: (int) arItemId;

- (bool) isDrawingWatermark;

@end

@protocol CraftARUnityProtocol <NSObject>

- (void) didInitializeSDK;
- (void) didProcessVideoFrame: (VideoFrame*) frame withResults: (NSArray*) results;
- (void) didGetSearchResults: (NSString*) results;
- (void) didValidateToken: (NSString*) result;

@end
