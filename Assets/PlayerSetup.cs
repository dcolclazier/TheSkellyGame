using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets._2D;

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
    void OnJoinedRoom()
    {
        Debug.Log("Connected to Room");

        var cam = GameObject.FindWithTag("MainCamera");
        if (cam == null) return;

        Debug.Log("Setting follow target.");

        var followScript = cam.GetComponent<Camera2DFollow>();
        if (followScript == null) return;

        followScript.target = transform;
        Debug.Log("Follow target: " + transform.position.x + "," + transform.position.y);
    }
    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        CmdColorChanged(PlayerColor);

        OnJoinedRoom();
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