#pragma strict

var anim:String;

function Start () {

}

function OnMouseDown () {

GetComponent.<UnityEngine.Animation>().CrossFade("anim");

}