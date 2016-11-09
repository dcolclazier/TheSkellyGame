using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour {

    public Button CancelButton;
    public Button StartGameButton;
    public MultiplayerManager NetManager;
    //void Start() {
    //    DontDestroyOnLoad(gameObject);
    //}
    public void OnClickCancelLobby() {

        NetManager.LeaveLobby();
    }

    public void OnClickStart() {
        if(!NetworkServer.active) Debug.Log("It looks like the non-host tried to start the game?");
        NetManager.StartLobbyGame();
    }
}
