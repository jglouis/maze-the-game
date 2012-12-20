using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	public string orientation;
	public int row;
	public int col;
	public GameObject pillar1;
	public GameObject pillar2;
	public GameObject[] wallTexts;


	// Use this for initialization
	void Start () {

		SetWallState();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	// Set the wall to be visible/invisible depending on the Maze script
	void SetWallState(){		
		//get the wall state (true : visible, false invisible)
		bool state = Maze.GetState(orientation, row, col);
		renderer.enabled = state;
		collider.isTrigger = !state;
		
		pillar1.renderer.enabled = state;
		pillar1.collider.enabled = state;
		pillar2.renderer.enabled = state;
		pillar2.collider.enabled = state;
		
		//change the layer
		if(state){
			gameObject.layer = 8; // "Walls" layer
		}
		else{
			gameObject.layer = 0; // default layer
		}
		
		for(int i = 0; i <  2; i++){
			GameObject wallText = wallTexts[i];
			wallText.renderer.enabled = state;
			wallText.GetComponent<TextMesh>().text = Maze.GetMessage(orientation, row, col, i);			
		}
		

		
	}
}
