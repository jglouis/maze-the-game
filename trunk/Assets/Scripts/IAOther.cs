using UnityEngine;
using System.Collections;
using Pathfinding;

public class IAOther : MonoBehaviour {
	public float roamingSpeed = 10.0f;
	public float rotateSpeed = 50.0f;
	public float followSpeed = 20.0f;
	public float fleeingSpeed = 20.0f;
	public float rayOffset = 10.0f;
	public float raycastDist = 10.0f;
	public float distanceToWall = 2.0f;
	string behaviour = "ROAMING"; // May also be FOLLOWING or FLEEING
	
	Vector3? waypoint;
	Vector3 start;
	
	public Vector3 targetPosition;
	
	// Use this for initialization
	void Start () {
	     //Get a reference to the Seeker component we added earlier
        Seeker seeker = GetComponent<Seeker>();
        
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
		AstarPath.active.Scan();
        seeker.StartPath (transform.position,targetPosition, OnPathComplete);
	}
	
	// Update is called once per frame
	void Update () {

		
		
		switch(behaviour){
		case "ROAMING":
			break;
		case "FOLLOWING":
			break;
		case "FLEEING":			
			break;			
		}
	}	
	
	public void OnPathComplete (Path p) {
        Debug.Log ("Yey, we got a path back. Did it have an error? "+p.error);
    }
	
	void Roam(){
		// If no roaming waypoint, then look for a far enough target 
		if(waypoint == null){
			Debug.DrawRay(transform.position, transform.forward * raycastDist);
			RaycastHit hit;
			bool isCollider = Physics.Raycast(transform.position, transform.forward, out hit, raycastDist);
			if(isCollider && hit.collider.renderer.enabled){
				transform.Rotate(Vector3.up * Time.smoothDeltaTime * rotateSpeed);
			}
			else{
				//transform.Translate(Vector3.forward * roamingSpeed * Time.smoothDeltaTime);
				start = transform.position;
				waypoint = hit.point;
			}		
		}else{// go to the waypoint
			Vector3 target = waypoint.Value;
			transform.position = Vector3.MoveTowards(start, target, roamingSpeed);
		}
	}
}