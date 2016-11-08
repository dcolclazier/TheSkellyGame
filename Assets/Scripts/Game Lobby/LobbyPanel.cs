using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour {

    public Button CancelButton;
    public Button StartGameButton;

    public void OnClickCancelLobby() {
        
        LobbyPlayer.ColorsInUse.Clear();
        MultiplayerManager.Instance.OnLobbyClientDisconnect(null);
        PanelManager.Instance.SwitchPanel(PanelManager.Instance.MultiplayerPanel);
    }

    public void OnClickStart() {
        if(!NetworkServer.active) Debug.Log("It looks like the non-host tried to start the game?");
        MultiplayerManager.Instance.StartLobbyGame();
    }
}
