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
This example shows how to use the OnItemDownloadProgress and OnItemDownloadFinished callbacks 
to show a 3DText label updating with the download percentage when the contents are being downloaded.
The 3DText label is an ARitem content created in the editor (CraftAR-> Create -> Empty AR Item) and 
stored in the Resources/ folder of the Unity project. 

To use it, run the App, point to your reference image. While the content bundle is being downloaded, 
you will see a 3DText label tracking the reference image.
 */
public class ExampleDownloadContent : MonoBehaviour, CraftARSDK.CraftARSDKCallbacks, CraftARSDK.CraftARItemEvents, CraftARSDK.CraftARFreeTrialEventsHandler{

	public string CollectionToken = "Put your token here";

	GameObject loading3DtextARItem;
	TextMesh loadingText;
	void Start () {
		//Set this app as Callback handler in the SDK.
		CraftARSDK.instance.setCraftARSDKCallbacksHandler(this);
		//Set this app as Callback handler for the CraftARItem events in the SDK
		CraftARSDK.instance.setCraftARItemEventsHandler(this);
		CraftARSDK.instance.setCraftARFreeTrialEventsHandler (this);
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
			Debug.Log ("Found AR Item with name:"+bestMatch.itemName);
			//Add item to the ARScene
			bool itemAdded = CraftARSDK.instance.AddSceneARItem(bestMatch,true);
			//Start tracking and download item contents. During download, we change the ARItem content for a text label we have pre-loaded.
			if (itemAdded) {
				CraftARSDK.instance.startTracking();
				//Change the ARItem content to a 3DText content during download.
				loading3DtextARItem = Resources.Load ("ARItem_Loading3DTextContent",typeof(GameObject)) as GameObject;
				bestMatch.contentInstance =  GameObject.Instantiate(loading3DtextARItem,loading3DtextARItem.transform.position, loading3DtextARItem.transform.rotation) as GameObject;
				loadingText = GameObject.Find ("Loading3DText").GetComponent<TextMesh>();
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
		Debug.Log("token OK");
		CraftARSDK.instance.startFinderMode ();
	}
	void CraftARSDK.CraftARSDKCallbacks.TokenValidationError(CraftARError error) {
		Debug.Log("Token validation error: " + error.errorMessage);
	}
	
	// Item events:
	void CraftARSDK.CraftARItemEvents.TrackingStarted(CraftARItem item) {
		Debug.Log("TrackingStarted: "+ item.itemName);
	}
	void CraftARSDK.CraftARItemEvents.TrackingLost(CraftARItem item) {
		Debug.Log("TrackingLost: "+ item.itemName);
	}
	
	void CraftARSDK.CraftARItemEvents.AddItemError(CraftARItem item, CraftARError error) {
		Debug.Log("Error Adding item: " + item.itemName + " Message: " + error.errorMessage);
	}
	
	void CraftARSDK.CraftARItemEvents.OnItemContentDownloadProgress(CraftARItem item, float progress) {
		Debug.Log("Download progress: " + progress + " - "+ item.itemName);
		loadingText.text = "Loading... " + (int)(progress * 100) + "%";
	}
	
	void CraftARSDK.CraftARItemEvents.OnItemContentDownloadFinished(CraftARItem item) {
		Debug.Log("Download Finished!  "+ item.itemName);
		if (loading3DtextARItem != null) {
			loadingText.text = "";
		}
	}
	
	void CraftARSDK.CraftARItemEvents.OnItemContentDownloadFailed(CraftARItem item, CraftARError error) {
		Debug.Log("Download error: "+ error.errorMessage);
	}

	void CraftARSDK.CraftARFreeTrialEventsHandler.OnFreeTrialExpired(){
		Debug.Log ("App: Free trial expired!!!!");
		loadingText.text = "Free trial expired!";
	}
}
