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
This example uses setAutoTrackingOnSearchResults(). When enabled, the SDK will automatically manage AR references,
and start tracking them. However, if you want to use IR items you still have to check them in the SearchResults() method.

To use it, run the App, point to your reference image, and the AR content will be managed by the SDK automatically.
 */
public class ExampleAutoTracking : MonoBehaviour, CraftARSDK.CraftARSDKCallbacks {
	public string CollectionToken = "Put your token here";

	void Start () {
		//Set this app as Callback handler in the SDK.
		CraftARSDK.instance.setCraftARSDKCallbacksHandler(this);
	}
	
	//CraftARSDK events:
	void CraftARSDK.CraftARSDKCallbacks.CraftARReady() { 
		CraftARSDK.instance.setAutoTrackingOnSearchResults (true); //The SDK will automatically manage AR references, and start tracking them.
		CraftARSDK.instance.setToken (CollectionToken);
	}
	void CraftARSDK.CraftARSDKCallbacks.SearchResults(List<CraftARItem> results){

		if (results.Count <= 0) {
			//Nothing found!
			Debug.Log ("Results is empty!");
			return;
		}
		//Something was found!
		//Note that when AutoTrackingOnSearchResults is enabled, the SDK automatically stops the finderMode when AR item is found.
		//If you want to use IR items, you will still have to check them here.
		Debug.Log ("Found item with name:"+results[0].itemName);
	}
	void CraftARSDK.CraftARSDKCallbacks.SearchError(CraftARError error) {
		Debug.Log("Search error: "+error.errorMessage);
	}
	void CraftARSDK.CraftARSDKCallbacks.TokenValidated() {
		Debug.Log("Token OK");
		CraftARSDK.instance.startFinderMode ();
	}
	void CraftARSDK.CraftARSDKCallbacks.TokenValidationError(CraftARError error) {
		Debug.Log("Token validation error: " + error.errorMessage);
	}

}
