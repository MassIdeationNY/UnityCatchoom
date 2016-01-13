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

public class ExampleOffTracking : MonoBehaviour, CraftARSDK.CraftARSDKCallbacks {

	public string CollectionToken = "Put your token here";

	float sliderValue=0.0f;
	CraftARItem lastItemAdded;
	bool isUpdatingWithTracking=true;
	Texture2D cameraTexture;
	Quaternion contentRotation;

	void Start () {
		//Set this app as Callback handler in the SDK.
		CraftARSDK.instance.setCraftARSDKCallbacksHandler(this);
	}
	public void OnGUI()
	{
		if (lastItemAdded == null) {
			//When there are no ARItems added, don't show anything in the GUI.
			return;
		}

		GUILayout.BeginVertical ();
		//Button to toogle Off tracking
		if (GUILayout.Button("Toggle OFF-Tracking", GUILayout.Height (Screen.height / 8))) {
			isUpdatingWithTracking = !isUpdatingWithTracking;
			contentRotation = lastItemAdded.contentInstance.transform.rotation;
		}
		lastItemAdded.SetUpdateWithTracking (isUpdatingWithTracking);
		//Add an slider to modify item's rotation in direction Vector3.up
		//The slider is enabled just when isUpdatingWithTracking = false;
		if (!isUpdatingWithTracking) {
			
			GUIStyle sliderStyle = new GUIStyle ("horizontalSlider");
			sliderStyle.fixedHeight = 30;
			sliderStyle.fixedWidth = 300;
			GUIStyle sliderStyleThumb = new GUIStyle ("horizontalSliderThumb");
			sliderStyleThumb.fixedHeight = 30;
			sliderStyleThumb.fixedWidth = 30;

			sliderValue = GUI.HorizontalSlider (new Rect (50, 200, 600, 100), sliderValue, 0.0f, 360.0f, sliderStyle, sliderStyleThumb);
			Quaternion newRotation = Quaternion.AngleAxis (sliderValue, Vector3.up);
			lastItemAdded.contentInstance.transform.rotation = contentRotation*newRotation;
		}
		GUILayout.EndVertical ();
	}
	
	//CraftARSDK events:
	void CraftARSDK.CraftARSDKCallbacks.CraftARReady() { 
		//#warning Configure the SDK to point to your servers:
        CraftARSDK.instance.setToken(CollectionToken);
	}
	void CraftARSDK.CraftARSDKCallbacks.SearchResults(List<CraftARItem> results){
		if (results.Count <= 0) {
			//Nothing found!
			Debug.Log("Results is empty");
			return;
		}
		//Something was found! We stop the finder mode.
		CraftARSDK.instance.stopFinderMode();
		
		//As an example, we take only the best match (the first in the list of matches). 
		CraftARItem bestMatch = results [0];
		switch (bestMatch.ItemType) {
		case CraftARItem.CraftARItemType.AUGMENTED_REALITY_ITEM:
			Debug.Log ("Found AR Item with name:"+bestMatch.itemName);
			//Add item to the ARScene
			bool itemAdded = CraftARSDK.instance.AddSceneARItem(bestMatch,true);
			//Start tracking and download item contents.
			if (itemAdded) {
				lastItemAdded = bestMatch;
				CraftARSDK.instance.startTracking();
				//Download the bundle from this ARItem, and automatically enable it when download finishes.
				CraftARSDK.instance.DownloadItemContents(bestMatch, true);
			}
			break;
		case  CraftARItem.CraftARItemType.IMAGE_RECOGNITION_ITEM:
			Debug.Log ("Found IR Item with name:"+bestMatch.itemName);
			break;
		} 
	}
	void CraftARSDK.CraftARSDKCallbacks.SearchError(CraftARError error) {
		Debug.Log("Search error: "+error.errorMessage);
	}
	void CraftARSDK.CraftARSDKCallbacks.TokenValidated() {
		CraftARSDK.instance.startFinderMode ();
		Debug.Log("token OK");
	}
	void CraftARSDK.CraftARSDKCallbacks.TokenValidationError(CraftARError error) {
		Debug.Log("Token validation error: " + error.errorMessage);
	}
}
