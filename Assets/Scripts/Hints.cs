using UnityEngine;
using System.Collections;

public class Hints : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ShowHint("Explore the labyrinth and find the forgotten gems");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator ShowHint(string message){
		guiText.text = message;
		guiText.enabled = true;
		yield return new WaitForSeconds(5);
		guiText.enabled = false;		
	}
}
