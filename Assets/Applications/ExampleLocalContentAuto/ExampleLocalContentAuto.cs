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
This example shows how to load a content pre-loaded in the app Resources/ folder. By doing this, you can skip network delays
and save network usage, but your application will be bigger. 

To use it, run the App, point to your reference image. When any of your AR items in your collection is found,
this app will show the AR experience with the pre-loaded content.
 */
public class ExampleLocalContentAuto : MonoBehaviour, CraftARSDK.CraftARSDKCallbacks{

	public string CollectionToken = "Put your token here";

	void Start () {
		//Set this app as Callback handler in the SDK.
		CraftARSDK.instance.setCraftARSDKCallbacksHandler(this);
	}
	
	//CraftARSDK events:
	void CraftARSDK.CraftARSDKCallbacks.CraftARReady() { 
		//#warning Set your collection token!
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
			Debug.Log ("Found AR Item with name: "+bestMatch.itemName);
			//Add item to the ARScene, start tracking and load item content.
			bool itemAdded = CraftARSDK.instance.AddSceneARItem(bestMatch,true);
			if (itemAdded) {
				CraftARSDK.instance.startTracking();
				GameObject content = Resources.Load ("ARItem_"+bestMatch.itemName,typeof(GameObject)) as GameObject;
				bestMatch.contentInstance =  GameObject.Instantiate(content,content.transform.position, content.transform.rotation) as GameObject;
			}
			break;
		case  CraftARItem.CraftARItemType.IMAGE_RECOGNITION_ITEM:
			Debug.Log ("Found IR Item with name: "+bestMatch.itemName);
			break;
		} 
	}
	void CraftARSDK.CraftARSDKCallbacks.SearchError(CraftARError error) {
		Debug.Log("Search error: "+error.errorMessage);
	}
	void CraftARSDK.CraftARSDKCallbacks.TokenValidated() {
		Debug.Log("token OK");
		CraftARSDK.instance.startFinderMode ();
	}
	void CraftARSDK.CraftARSDKCallbacks.TokenValidationError(CraftARError error) {
		Debug.Log("Token validation error: " + error.errorMessage);
	}
}
