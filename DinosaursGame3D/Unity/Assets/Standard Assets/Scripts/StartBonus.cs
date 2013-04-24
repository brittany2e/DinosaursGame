using UnityEngine;
using System.Collections;

public class StartBonus : MonoBehaviour {
	
	public GameObject player;
	float speed = 6f;
	bool closeEnough;
	public float tolerance = 0.1f;
	Vector3 destinationPos;
	Quaternion destinationRot;
	Transform defaultTransform;
	
	// Use this for initialization
	void Start () {
		closeEnough = true;
		defaultTransform = transform;
		destinationPos = defaultTransform.position;
		destinationRot = defaultTransform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		closeEnough = Vector3.Distance(Camera.mainCamera.transform.position, destinationPos) < tolerance;
		if (!closeEnough) {
			//Camera.mainCamera.transform.Translate(Vector3.forward * Time.deltaTime * speed);
			//Camera.mainCamera.transform.
		}
		
	}
	
	void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
			Vector3 thirdPersonView = player.transform.Find("ThirdPersonCamera").transform.position; //player.transform.position - player.transform.forward;
			//DebugStreamer.message = ("thirdPersonView :" + thirdPersonView);
			Debug.Log("thirdPersonView :" + thirdPersonView);
			if(thirdPersonView == null){
				Camera.mainCamera.transform.Translate(thirdPersonView);//Vector3.forward * 40);
			}
			else {
				Camera.mainCamera.transform.TransformDirection(thirdPersonView);//Vector3.forward);
			}
			
			//Camera.mainCamera.transform.Translate(Vector3.forward * 40);
			/*closeEnough = false;
			
			// Set the destination.
			Transform thirdPersonCamera = player.transform.Find("ThirdPersonCamera");
			destinationPos = thirdPersonCamera.transform.position;
			destinationRot = thirdPersonCamera.transform.rotation;
			*/
		}
    }
}
