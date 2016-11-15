using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Use this for initialization
    public Transform playerTransform;
    
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (playerTransform != null) {
	        transform.position = playerTransform.position + new Vector3(0, 10, -20);
	    }
	}
}
