using System;
using UnityEngine;

[Serializable]
public class PlayerManager {
    // This class manages various player settings...
    // It works with GameManager class to control how players behave
    // and  whether or not players have control of their player abilities
    // during different phases of the game
    public Color PlayerColor;
    public Transform SpawnPoint;
    [HideInInspector]
    public int PlayerNumber;
    [HideInInspector]
    public GameObject Instance;
    //[HideInInspector]
    //public GameObject PlayerRenderers;
    [HideInInspector]
    public string PlayerName;
    [HideInInspector]
    public int LocalPlayerId;



    

    private PlayerSetup _playerSetup;
    private PlayerMovement _playerMovement;
   
    public void Init() {
        //get references to all player components...
        _playerSetup = Instance.GetComponent<PlayerSetup>();
        _playerMovement = Instance.GetComponent<PlayerMovement>();

        //setup is used for Network related sync.
        _playerSetup.PlayerColor = PlayerColor;
        _playerSetup.PlayerName = PlayerName;
        _playerSetup.PlayerNumber = PlayerNumber;
        _playerSetup.LocalId = LocalPlayerId;

    }

    public void Reset() {
        _playerMovement.SetDefaults();
        if (_playerMovement.hasAuthority) {
            _playerMovement.Rigidbody.position = SpawnPoint.position;
        }
    }

    public void EnableControl() {
        _playerMovement.enabled = true;
        

    }

    public void DisableControl() {
        _playerMovement.enabled = false;

    }
}