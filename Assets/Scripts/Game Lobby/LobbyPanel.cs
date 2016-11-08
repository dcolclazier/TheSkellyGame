using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPanel : MonoBehaviour {

    public MultiplayerManager MultiplayerManager;
    public MenuManager MenuManager;
    public Button CancelButton;
    public Button StartGameButton;
    public RectTransform MultiPlayerPanel;

    public void OnClickCancelLobby() {
        
        LobbyPlayer.ColorsInUse.Clear();
        MultiplayerManager.CancelClientConnection();
        MenuManager.SwitchPanel(MultiPlayerPanel);
    }

    public void OnClickReady() {
        MultiplayerManager.OnLobbyServerPlayersReady();
    }
}
