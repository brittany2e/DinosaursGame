using UnityEngine;
using System.Collections;

public class ZoomIn : MonoBehaviour {
	
	public GameObject player;
	float speed = 6f;
	GameObject camera;
	Vector3 closePos;
	Quaternion closeRot;
	Vector3 farPos;
	Quaternion farRot;
	
	// Use this for initialization
	void Start () {
		camera = Camera.main.gameObject;
		
		closePos = camera.transform.position;
		closeRot = camera.transform.rotation;
		
		farPos = camera.transform.position;
		farRot = camera.transform.rotation;
	}
	
	/*
	float result;
	float t = 0.0F;
	float speed = 30.0F;
	
	public void Update(){
	    t += Time.deltaTime;
	    result = Mathf.Lerp( 0, 100, t / speed );
	}
	*/
	
	// Update is called once per frame
	void Update () {
		//Camera.mainCamera.transform.TranslateDirection(target * Time.deltaTime * speed);
		if (Input.GetKeyDown (KeyCode.W))
        {
			Debug.Log("W" + closePos);
            //iTween.MoveBy(camera, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
			iTween.RotateTo(camera, new Vector3(closeRot.eulerAngles.x, closeRot.eulerAngles.y, closeRot.eulerAngles.z), 3.0f);
        	iTween.MoveTo(camera, iTween.Hash("x", closePos.x, "y", closePos.y, "z", closePos.z, "time", 3.0f)); //targetPos);
		}
    	else if (Input.GetKeyDown (KeyCode.S))
        {
            Debug.Log("S" + farPos);
            //iTween.MoveTo(Camera.mainCamera, iTween.Hash("from", targetPos, "to", zoomedOut));
			iTween.MoveTo(camera, iTween.Hash("x", farPos.x, "y", farPos.y, "z", farPos.z, "time", 3.0f));
			iTween.RotateTo(camera, new Vector3(farRot.eulerAngles.x, farRot.eulerAngles.y, farRot.eulerAngles.z), 3.0f);
        }
	}
	
	void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
			Transform thirdPersonView = player.transform.Find("ThirdPersonCamera").transform; //player.transform.position - player.transform.forward;
			if(thirdPersonView != null){
				closePos = thirdPersonView.position;
				closeRot = thirdPersonView.rotation;
				
				// Glide the camera
				iTween.RotateTo(camera, new Vector3(closeRot.eulerAngles.x, closeRot.eulerAngles.y, closeRot.eulerAngles.z), 3.0f);
        		iTween.MoveTo(camera, iTween.Hash("x", closePos.x, "y", closePos.y, "z", closePos.z, "time", 3.0f));
			}
		}
    }
}
