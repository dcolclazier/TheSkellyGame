using UnityEngine;
using System.Collections;

public class HoleTrap : MonoBehaviour {

	// Use this for initialization
    public Transform _respawnPoint;
    private const float RespawnTime = 5;

    void OnTriggerEnter2D(Collider2D c) {

        var player = GameManager.Instance.GetPlayer(c.gameObject);
        player.Kill(RespawnTime);
        player.SpawnPoint = _respawnPoint;

    }

    IEnumerator wait(float time) {
        yield return new WaitForSeconds(time);
    }
}
