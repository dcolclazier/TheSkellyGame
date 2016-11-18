using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class Timer : MonoBehaviour
{

    public GameObject[] players;
    int[] winners;
    public List<Vector2> _pos = new List<Vector2>();

    public float timer;
    public Text timerText;

    private
    float minutes = 0;
    float seconds = 0;
    float miliseconds = 0;
    bool endTimer = false;
    string Movement_Script_Name = "PlayerController";
    void Start()

    {
        
    }

    void Awake()
    {
        if (timer >= 60)
        {
            while (timer > 60)
            {
                timer -= 60;
                minutes += 1;
            }
        }
        seconds = timer;
    }

    void Update()
    {
        if (minutes >= 1 || seconds >= 1)
        {
            if (seconds > 0 && miliseconds <= 0)
            {
                seconds--;
                miliseconds += 100;
            }
            if (seconds <= 0 && minutes > 0)
            {
                minutes -= 1;
                seconds += 59;
            }
        }
        if(miliseconds < 10)
            timerText.text = "TIMER: " + minutes + ":" + seconds + ":" + "0" +(int)miliseconds;
        else
            timerText.text = "TIMER: " + minutes + ":" + seconds + ":" + (int)miliseconds;
        if(seconds < 10)
            timerText.text = "TIMER: " + minutes + ":" + "0" + seconds + ":" + (int)miliseconds;
        else
            timerText.text = "TIMER: " + minutes + ":" + seconds + ":" + (int)miliseconds;
        if (seconds > 0 || miliseconds > 0)
        {
            miliseconds -= Time.deltaTime * 100;
        }
        else
        {
            timerText.text = "TIMES UP!";
            if (!endTimer)
            {
                endTimer = true;
                DisablePlayers();
            }
         
        }
    }

    void DisablePlayers()
    {
        players = GameObject.FindGameObjectsWithTag("player_PREFAB");
        for (int x = 0; x < players.Length; x++)
        {
            players[x].GetComponent<PlayerMovement>().enabled = false;
            players[x].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            players[x].GetComponent<Animator>().SetBool("Death", false);
            StartCoroutine(disableAfterDelay());
        }
    }

    IEnumerator disableAfterDelay()
    {
        yield return new WaitForSeconds(.1f);

        for (int x = 0; x < players.Length; x++)
        {
            players[x].GetComponent<Animator>().SetBool("MovingLeft", false);
            players[x].GetComponent<Animator>().SetBool("MovingRight", false);
            print(players[x].gameObject.transform.position.x);
        }
        players = players.OrderBy(d => d.gameObject.transform.position.x).ToArray();
    }
}
