using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {


    [Header("Network Related")] [Space] [SyncVar] public Color Color;
    [SyncVar] public string PlayerName;
    [SyncVar] public int PlayerNumber;
    [SyncVar] public int LocalId;
    [SyncVar] public bool IsReady;


    public override void OnStartClient() {
        base.OnStartClient();
        if (!isServer)
            GameManager.AddPlayer(gameObject, PlayerNumber, Color, PlayerName, LocalId);

        //var spriteRenderer = gameObject.Ge
    }

    public override void OnNetworkDestroy() {
        GameManager.Instance.RemovePlayer(gameObject);
    }
}