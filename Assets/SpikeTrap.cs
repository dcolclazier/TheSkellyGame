using UnityEngine;
using System.Collections;

public class SpikeTrap : MonoBehaviour
{

    public Transform _respawnPoint;
    private const float RespawnTime = 3;


    void OnTriggerEnter2D(Collider2D c)
    {
        var player = GameManager.Instance.GetPlayer(c.gameObject);
        if (player == null) return;

        Debug.Log("Spike trap is about to kill a player.");
        player.Kill(RespawnTime);
        player.SpawnPoint = _respawnPoint;
    }

}
