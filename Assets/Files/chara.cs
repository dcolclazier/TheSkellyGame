using UnityEngine;
using System.Collections;

public class chara : MonoBehaviour {

	private
	Vector3 vec;
	float speed = 10;

	// Use this for initialization
	void Start () 
	{
		vec = this.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
		vec.x += (Input.GetAxis("Horizontal") * speed * Time.deltaTime);
		//vec = vec.x;
		this.transform.position = vec;
	}
}
