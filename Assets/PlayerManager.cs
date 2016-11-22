using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
public class PlayerManager : MonoBehaviour {
    // This class manages various player settings...
    // It works with GameManager class to control how players behave
    // and  whether or not players have control of their player abilities
    // during different phases of the game
    public Color PlayerColor;
    public Transform SpawnPoint;
    public int PlayerNumber;
    public GameObject Player;
    public string PlayerName;
    public int LocalPlayerId;

    private Rigidbody2D _rigidbody;
    private PlayerSetup _playerSetup;
    private PlayerMovement _playerMovement;
    private PlayerHealth _playerHealth;
    private PlayerAnimations _playerAnimations;
    private float _respawnTime = 3;
    private float _defaultResetTime = 3;
    public float CurrentHealth { get { return _playerHealth.CurrentHealth; } }

    public void Init() {
        //get references to all player components...
        _playerSetup = Player.GetComponent<PlayerSetup>();
        _playerMovement = Player.GetComponent<PlayerMovement>();
        _playerAnimations = Player.GetComponent<PlayerAnimations>();
        _rigidbody = Player.GetComponent<Rigidbody2D>();

        _playerHealth = Player.GetComponent<PlayerHealth>();
        _playerHealth._Manager = this;
        _playerHealth.SetDefaults();

        Debug.Log("Player Movement: " + _playerMovement.isActiveAndEnabled);
        //setup is used for Network related sync.
        _playerSetup.PlayerColor = PlayerColor;
        _playerSetup.PlayerName = PlayerName;
        _playerSetup.PlayerNumber = PlayerNumber;
        _playerSetup.LocalId = LocalPlayerId;

    }

    

    private IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
    }
    
    public void EnablePlayerControl() {
        
        _playerMovement.enabled = true;
    }

    public void DisablePlayerControl() {
        _playerMovement.Stop();
        _playerMovement.enabled = false;
    }

    
    public IEnumerator OnJustDied() {

        _playerMovement.enabled = false;
        _playerAnimations.OnJustDied();
        _rigidbody.velocity = Vector2.zero;
        //_rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        
        yield return StartCoroutine(Wait(1));
        if (_playerMovement.hasAuthority) {
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _playerMovement.Rigidbody.position = SpawnPoint.position;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

        yield return StartCoroutine(Wait(_respawnTime - 1));
        Respawn();
        
    }
    public void Respawn()
    {

        Debug.Log("Respawning player");

        _playerAnimations.OnRespawned();

        _respawnTime = _defaultResetTime;

        _playerHealth.SetDefaults();

        _playerMovement.enabled = true;
        _playerMovement.SetDefaults();
        _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void DoDamage(float currentHealth) {
        
        _playerHealth.DoDamage(currentHealth);
    }
    public void DoDamage(float currentHealth, float respawnTime) {
        
        _playerHealth.DoDamage(currentHealth);
        _respawnTime = respawnTime;
    }

    public void Kill(float respawnTime) {
        
        DoDamage(CurrentHealth);
        _respawnTime = respawnTime;
    }
}