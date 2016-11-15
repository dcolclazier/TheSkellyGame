using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {


    [Header("Network Related")] [Space]

    [SyncVar(hook = "OnMyColor")]
    public Color PlayerColor;

    [SyncVar] public string PlayerName;
    [SyncVar] public int PlayerNumber;
    [SyncVar] public int LocalId;
    [SyncVar] public bool IsReady;


    public override void OnStartClient() {
        base.OnStartClient();
        if (!isServer)
            GameManager.AddPlayer(gameObject, PlayerNumber, PlayerColor, PlayerName, LocalId);
        OnMyColor(PlayerColor);
    }

    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        CmdColorChanged(PlayerColor);
    }

    void OnMyColor(Color newColor) {
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }
    public override void OnNetworkDestroy() {
        GameManager.Instance.RemovePlayer(gameObject);
    }

    [Command]
    public void CmdColorChanged(Color color) {
        PlayerColor = color;
    }

    
}