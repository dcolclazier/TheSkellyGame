using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

    public float _StartingHealth = 100f;
    public PlayerManager _Manager;

    [SyncVar(hook = "OnHealthChanged")] private float _currentHealth;
    public float CurrentHealth { get { return _currentHealth;} }
    [SyncVar] private bool _justDied;

    public void DoDamage(float amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0f && !_justDied) {
            OnJustDied();
        }
    }
    void OnHealthChanged(float value)
    {
        _currentHealth = value;

        //anything else?
    }
    [ServerCallback]
    void OnJustDied() {
        _justDied = true;
        RpcOnJustDied();
    }
    [ClientRpc]
    private void RpcOnJustDied() {

        InternalOnJustDied();
    }

    private void InternalOnJustDied() {
        Debug.Log(_Manager.PlayerName + " just died.");
        StartCoroutine(_Manager.OnJustDied());
        //DisablePlayerMovement();
        //SetPlayerActive(false);
    }
    
    public void SetDefaults() {
        _currentHealth = _StartingHealth;
        _justDied = false;
        //SetPlayerActive(true);
    }

}
