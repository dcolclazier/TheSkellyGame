using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnClientEnterLobby() {
        base.OnClientEnterLobby();

        MultiplayerManager.Instance.OnPlayerCountChange(1);
    }
}
