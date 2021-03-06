using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	float speed = 6f;
	bool closeEnough;
	public float tolerance = 0.1f;
	Vector3 destination;
	Vector3 prevPosition;
	
	// Use this for initialization
	void Start () {
		closeEnough = true;
		destination = transform.position;
		prevPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown ( 0 )){
			Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
			RaycastHit hit = new RaycastHit();
			
			if(Physics.Raycast(ray, out hit)) {
				closeEnough = false;
				destination = hit.point;
				destination.y = transform.position.y;
			}
		}
		
		if (!closeEnough) {
			if( Vector3.Distance(transform.position, destination) < tolerance) {
				closeEnough = true;
				transform.position = prevPosition;
				destination = transform.position;
			}
			else {
				prevPosition = transform.position;
				transform.LookAt(destination);
				transform.Translate(Vector3.forward * Time.deltaTime * speed);
			}
		}
	}
	int timesCollided = 0;
	void OnCollisionEnter(Collision theCollision) {
		Debug.Log("Collided with " + theCollision.gameObject.name + timesCollided++);
		closeEnough = true;
		transform.position = prevPosition;
		destination = transform.position;
	}	
}