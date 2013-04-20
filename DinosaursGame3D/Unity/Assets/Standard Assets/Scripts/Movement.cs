using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	float speed = 6f;
	bool closeEnough;
	public float tolerance = 0.1f;
	Vector3 destination;
	
	// Use this for initialization
	void Start () {
		closeEnough = true;
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
				//transform.position = destination;
				closeEnough = true;
			}
			else {
				transform.LookAt(destination);
				transform.Translate(Vector3.forward * Time.deltaTime * speed);
			}
		}
	}
}