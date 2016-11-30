using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerV2 : MonoBehaviour {

    static public bool continueCount;
    public Text timerText;
    private float milliseconds = 0, seconds = 0, minutes = 0;

    // Use this for initialization
    void Start ()
    {
        continueCount = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(continueCount)
        {
            milliseconds += Time.deltaTime * 100;
            timerText.text = "TIMER: " + minutes + ":" + seconds + ":" + (int)milliseconds;
            if(milliseconds >= 100)
            {
                seconds += 1;
                milliseconds = 0;
            }
            if(seconds >= 60)
            {
                minutes += 1;
                seconds = 0;
            }
            if(seconds <= 10)
            {
                timerText.text = "TIMER: " + minutes + ":" + "0" + seconds + ":" + (int)milliseconds;
            }
        }
	}
}
