//
//  VideoFrame.h
//  CraftAR-sdk
//
//  Created by Luis Martinell Andreu on 5/29/13.
//  Copyright (c) 2013 Catchoom. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreMedia/CoreMedia.h>

@interface VideoFrame : NSObject {
    NSInteger _width;
    NSInteger _height;
    NSInteger _bytesPerRow;
}

- (id) initFromCMSampleBufferRef: (CMSampleBufferRef) sampleBuffer;
- (unsigned char *) getRawBytes;
- (void) drawInTexture: (GLuint) texture;

@property (nonatomic, readonly) NSInteger _width;
@property (nonatomic, readonly) NSInteger _height;
@property (nonatomic, readonly) NSInteger _bytesPerRow;

@end;