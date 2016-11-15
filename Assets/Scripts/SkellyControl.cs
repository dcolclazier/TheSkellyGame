/*using UnityEngine;
using System.Collections;

public class SkellyControl : MonoBehaviour {

	private Rigidbody2D ridgidBody;
	public float jumpForce = 6f;

	void Awake(){
		ridgidBody = GetComponent<Rigidbody2D> ();
	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("space")) {
			Jump ();
	
		}
	}
	void Jump(){
		rigidbody.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
	}
}
*/