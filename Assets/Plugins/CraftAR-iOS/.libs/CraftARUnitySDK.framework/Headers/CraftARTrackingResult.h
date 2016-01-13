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
#import <CoreGraphics/CoreGraphics.h>
#import <QuartzCore/QuartzCore.h>

///@cond
typedef struct matrix33f_t {
    float m11, m12, m13;
    float m21, m22, m23;
    float m31, m32, m33;
} Matrix33f;


struct MyCATransform3D
{
    float m11, m12, m13, m14;
    float m21, m22, m23, m24;
    float m31, m32, m33, m34;
    float m41, m42, m43, m44;
};

///@endcond

typedef struct CraftARTrackingResultRaw_t {
    int32_t referenceID;
    float referenceWidth;
    float referenceHeight;
    float tx, ty, tz;
    struct MyCATransform3D rotation;
} CraftARTrackingResultRaw;

/*
 * The Tracking result contains the information of a reference found in a given video frame.
 * It provides information about the reference and its pose inside the scene.
 */
@interface CraftARTrackingResult : NSObject {
    ///@cond
    NSInteger referenceId;                  /// Identifies the detected reference
    CGSize resultReferenceSize;    /// Dimensions of the detected reference
    CGSize videoFrameSize;         /// Dimensions of the video frame
    CGPoint TL;                    /// Top left corner of the rerefence in video frame coordinates
    CGPoint TR;                    /// Top right corner of the reference in video frame coordinates
    CGPoint BR;                    /// Bottom right corner of the reference in video frame coordinates
    CGPoint BL;                    // Bottom left corner of the reference in video frame coordinates
    NSInteger _matchesCount;        /// Number of key points of the reference found (indicates the quality of the tracking)
    CATransform3D translation;     /// Translation component of the reference's pose in the video frame (rel to geometric center)
    CATransform3D rotation;        /// Rotation component of the reference's pose
    Matrix33f homography;          // Homography matrix (converts points from the scene to screen (video frame) coords).
}
///@endcond

/// Size of the reference corresponding to this result
@property (nonatomic, readonly) CGSize resultReferenceSize;
/// Size of the video frame
@property (nonatomic, readonly) CGSize videoFrameSize;
/// Top-left vertex coords of the reference image in the video frame
@property (nonatomic, readonly) CGPoint TL;
/// Top-right vertex coords of the reference image in the video frame
@property (nonatomic, readonly) CGPoint TR;
/// Bottom-right vertex coords of the reference image in the video frame
@property (nonatomic, readonly) CGPoint BR;
/// Bottom-left vertex coords of the reference image in the video frame
@property (nonatomic, readonly) CGPoint BL;
/// identifier of the matched reference
@property (nonatomic, readonly) NSInteger referenceId;
/// Translation transformation to position the contents on top of the reference
@property (nonatomic, readonly) CATransform3D translation;
/// Rotation transformation to orientate the contents on top of the reference
@property (nonatomic, readonly) CATransform3D rotation;
///@cond
@property (nonatomic, readonly) Matrix33f homography;
///@endcond

- (CraftARTrackingResultRaw) rawVersion;

@end
