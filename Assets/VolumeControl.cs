using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour {

    private Slider _volumeSlider;
	// Use this for initialization
	void Start () {
	    _volumeSlider = GetComponent<Slider>();
	}

    public void OnValueChanged() {
        AudioListener.volume = _volumeSlider.value;
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
