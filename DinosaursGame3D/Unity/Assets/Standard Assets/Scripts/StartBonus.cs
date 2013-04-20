using UnityEngine;
using System.Collections;

public class StartBonus : MonoBehaviour {
	
	public GameObject player;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other) {
        if(other.gameObject == player) {
			//Destroy(other.gameObject);
			Camera.mainCamera.transform.Translate(Vector3.forward);
		}
    }
}
