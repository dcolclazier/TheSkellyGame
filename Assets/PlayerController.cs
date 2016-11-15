using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (!isLocalPlayer) return;
	    var x = Input.GetAxis("Horizontal")*Time.deltaTime*10.0f;
        transform.Translate(x,0,0);
	}
}
