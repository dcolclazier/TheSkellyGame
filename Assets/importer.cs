using UnityEngine;
using System.Collections;

public class importer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (var components in GameObject.FindGameObjectsWithTag("FIXER")) {
            var raw = components.gameObject.name;
            int test = 0;
            if (raw[raw.Length - 1] == ')' && raw[raw.Length - 3] == '(' &&
                int.TryParse(raw[raw.Length - 2].ToString(), out test)) {
                raw = raw.Substring(0, raw.Length - 3);
            }
            Debug.Log(raw);
        }
        Debug.Log("Done.");
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
