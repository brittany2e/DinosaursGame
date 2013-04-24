using UnityEngine;
using System.Collections;

public class StartBonus : MonoBehaviour {
	
	public GameObject player;
	float speed = 6f;
	bool closeEnough;
	public float tolerance = 0.1f;
	Vector3 destinationPos;
	Vector3 destinationRot;
	
	// Use this for initialization
	void Start () {
		closeEnough = true;
		destinationPos = transform.position;
		destinationRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
			//Camera thirdPersonView = player.transform.position - player.transform.forward;
			//Camera.mainCamera.transform.Translate(thirdPersonView);//Vector3.forward);
			GameObject thirdPersonCamera = player.transform.Find("ThirdPersonCamera");
			destinationPos.position = thirdPersonCamera.transform.position;
			destinationRot = thirdPersonCamera.transform.rotation;
		}
    }
}
