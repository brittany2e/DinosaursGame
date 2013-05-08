using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {
	
	float speed = 6f;
	public float tolerance = 0.1f;
	Vector3 destination;
	Vector3 prevPosition;
	
	// Use this for initialization
	void Start () {
		destination = transform.position;
		prevPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown ( 0 )){
			Ray ray = Camera.main.ScreenPointToRay ( Input.mousePosition );
			RaycastHit hit = new RaycastHit();
			
			if(Physics.Raycast(ray, out hit)) {
				destination = hit.point;
				destination.y = transform.position.y;
			}
		}
		
		//Debug.Log("Position " + transform.position);
		
		if( Vector3.Distance(transform.position, destination) < tolerance) {
			transform.position = prevPosition;
			destination = transform.position;
		}
		else {
			prevPosition = transform.position;
			transform.LookAt(destination);
			transform.Translate(Vector3.forward * Time.deltaTime * speed);
		}
	}
	
	int count=0;
	void OnCollisionEnter(Collision theCollision) {
		Debug.Log ("Collision " + count++);
		transform.position = prevPosition;
		destination = transform.position;
	}	
	
	void OnTriggerEnter(Collider other) {
		Debug.Log ("New Trigger " + count++);
		transform.position = prevPosition;
		destination = transform.position;
	}
}