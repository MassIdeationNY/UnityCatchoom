// This software is free software. You may use it under the MIT license, which is copied
// below and available at http://opensource.org/licenses/MIT
//
// Copyright (c) 2015 Catchoom Technologies S.L.
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/**
This example shows how to modify the frames from the SDK. In this case, we will draw the frames in grayscale.
 */
public class ExampleOverrideFrame : MonoBehaviour,CraftARSDK.CraftARFrameDrawer {
	
	Texture2D cameraTexture; //We will store here texture in which the camera draws the frames and the contents
	void Start () {
		bool keepDrawingFrames = false; //We will draw the frames, the SDK will not.
		CraftARSDK.instance.setCraftARFrameHandler (this, keepDrawingFrames);
	}

	//Frame events:
	void CraftARSDK.CraftARFrameDrawer.newFrame(byte[] frameData,int frameWidth,int frameHeight){
		if (cameraTexture != null) {
			VideoFrameSettings videoFrameSettings = CraftARSDK.instance.GetVideoFrameSettings();
			//Example: Convert the frame to grayscale
			// Note that this can be done more efficiently using other tools!)
			int B,G,R,mean;
			switch(videoFrameSettings.Format){
			case TextureFormat.RGB24:
				//Android
				for(int pixel=0;pixel<frameWidth*frameHeight;pixel++){
					R = (int) frameData[3*pixel];
					G = (int) frameData[3*pixel + 1];
					B = (int) frameData[3*pixel + 2];
					mean = (R+G+B)/3;
					frameData[3*pixel]= frameData[3*pixel + 1] = frameData[3*pixel + 2] = (byte)mean;		
				}
				break;
			case TextureFormat.BGRA32:
				//iOS
				for(int pixel=0;pixel<frameWidth*frameHeight;pixel++){
					B = (int) frameData[4*pixel];
					G = (int) frameData[4*pixel + 1];
					R = (int) frameData[4*pixel + 2];
					mean = (B+G+R)/3;
					frameData[4*pixel]= frameData[4*pixel + 1] = frameData[4*pixel + 2] = (byte)mean;		
				}
				break;
			}
			//Apply the modified frame to the camera texture!
			cameraTexture.LoadRawTextureData(frameData);	
			cameraTexture.Apply();
		}
	}
	void CraftARSDK.CraftARFrameDrawer.textureReady(Texture2D cameraTexture){
		this.cameraTexture = cameraTexture;
	}
}
