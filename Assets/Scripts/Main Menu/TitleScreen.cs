using UnityEngine;

public class TitleScreen : MonoBehaviour {

    void Awake() {
        //var test = FindObjectOfType<TitleScreen>();
        //if (test != this)
        //{
        //    Destroy(test.gameObject);
        //}
    }
	// Use this for initialization
	void Start () {
	    DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
