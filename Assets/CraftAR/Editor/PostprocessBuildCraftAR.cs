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
using UnityEditor;
using UnityEditor.Callbacks;
using System.Diagnostics;
using System.IO;

public class PostprocessBuildCraftAR : MonoBehaviour {
	
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {

		#if UNITY_IOS
		Process proc = new Process();
		proc.EnableRaisingEvents=false;
		proc.StartInfo.FileName = Application.dataPath + "/Plugins/CraftAR-iOS/PostBuildIOSScript";
		CDebug.Log("process filename: "+proc.StartInfo.FileName);
		proc.StartInfo.Arguments = "'" + pathToBuiltProject + "'";

		// Add the Unity version as an argument to the postbuild script, use 'Unity3' for all 3.x versions and for
		// 4 and up use the API to get it
		string unityVersion;
		#if UNITY_3_5 || UNITY_3_4 || UNITY_3_3 || UNITY_3_2 || UNITY_3_1 || UNITY_3_0_0 || UNITY_3_0
		unityVersion = "Unity3";
		#else
		unityVersion = Application.unityVersion;
		#endif
		proc.StartInfo.Arguments += " '" + unityVersion + "'";
		proc.Start();
		proc.WaitForExit();
		#endif
	}

}