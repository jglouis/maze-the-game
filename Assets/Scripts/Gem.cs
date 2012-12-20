using UnityEngine;
using System.Collections;

public class Gem : MonoBehaviour {
	public float rotationSpeed = 100.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
	}
	
	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			Destroy(gameObject);
			col.gameObject.SendMessage("GemCollected");
			
			//create a new maze
			//GameObject.FindGameObjectWithTag("Maze").SendMessage("CreatePerfectMaze");
			Maze.CreatePerfectMaze();
			
			//create random messages
			//GameObject.FindGameObjectWithTag("Maze").SendMessage("AddRandomMessages", 1);
			Maze.AddRandomMessages(3,3);
			
		}
	}

}
