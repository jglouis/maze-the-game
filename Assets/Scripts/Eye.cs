using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}
	
	void CloseEye(){
		animation.Play("close_eye");
	}
	
	void OpenEye(){
		animation.Play("open_eye");	
	}
	
}
