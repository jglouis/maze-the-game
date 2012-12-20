using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GUIText GUIScore;
	int score = 0;
	public float raycastDist = 20.0f;
	public float rayOffset = 10.0f;
	public GUITexture blinkingTexture;
	public GameObject flashlight;

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		StartCoroutine("Blink");
	}
	
	// Update is called once per frame
	void Update () {
		
		// Backward raycasting
		RaycastToWalls(-transform.forward, rayOffset);
		if(Input.GetButtonDown("Fire2"))
			flashlight.light.enabled = !flashlight.light.enabled;		
		
	}
	
	
	void GemCollected(){
		score++;
		GUIScore.text = score.ToString();
	}
	
	void RaycastToWalls(Vector3 direction, float offset = 0.0f){
		RaycastHit[] hits;
		
		Vector3 rayStart = transform.position + direction * offset;
			
		hits = Physics.RaycastAll(rayStart, direction, raycastDist);
		
		
		foreach(RaycastHit hit in hits){
			if(hit.collider.gameObject.tag == "Wall")
				hit.collider.gameObject.SendMessage("SetWallState");
			//Debug.DrawLine (rayStart, hit.point);
		}
	}
	
	
	IEnumerator Blink(){
		
		while(true){
			if(Input.GetButton("Fire1")){
				blinkingTexture.SendMessage("CloseEye");
				//forward ray casting
				yield return new WaitForSeconds(0.05f);
				RaycastToWalls(transform.forward);
				yield return new WaitForSeconds(0.05f);
			
				while(Input.GetButton("Fire1"))
					yield return null;
				
				blinkingTexture.SendMessage("OpenEye");
				yield return new WaitForSeconds(0.10f);
			}
			yield return null;
		}
	
	}
	
}
