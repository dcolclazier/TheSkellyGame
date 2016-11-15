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

        // Spawn player
        //GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0.5f, 5.0f, 0.5f), Quaternion.identity, 0);
        var camera = GameObject.FindWithTag("MainCamera");
        if (camera != null)
        {
            Debug.Log("Setting follow target.");
            var followScript = camera.GetComponent<Camera2DFollow>();
            if (followScript != null) {
                followScript.target = transform;
                Debug.Log("Follow target: " + transform.position.x + "," + transform.position.y);
            }

        }
    }
    public override void OnStartLocalPlayer() {
        base.OnStartLocalPlayer();
        CmdColorChanged(PlayerColor);

        OnJoinedRoom();

        //if (GetComponent<NetworkView>().isMine)
        //    Camera.main.GetComponent<Camera2DFollow>().target = transform;
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