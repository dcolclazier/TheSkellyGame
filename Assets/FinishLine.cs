using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {

    public GameManager _manager;
    void Awake()
    {
        
    }

	void OnTriggerEnter2D(Collider2D c)
    {
        var player = GameManager.Instance.GetPlayer(c.gameObject);
        if(player != null)
        {
            _manager.DisablePlayerControl();
            _manager.FinishGame();
        }
    }
}
