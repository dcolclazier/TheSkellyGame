using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GraphicsOptions : MonoBehaviour {

    private Dropdown _qualityDropdown; 
    private enum Options
    {
        Fastest, Fast, Simple, Good, Beautiful, Fantastic
    }
	// Use this for initialization
	void Start () {
        _qualityDropdown = GetComponent<Dropdown>();
	}
	public void OnValueChanged()
    {
        QualitySettings.SetQualityLevel(_qualityDropdown.value);
        
    }
	// Update is called once per frame
	void Update () {
	
	}
}
