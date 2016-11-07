using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MultiplayerPanel : MonoBehaviour {

    public MenuManager MenuManager;
    public MultiplayerManager NetManager = MultiplayerManager.Instance;
    
    public RectTransform MainMenuPanel;
    public RectTransform GameLobbyPanel;
    //public RectTransform 

    public InputField MultiplayerGameName;
    public InputField MultiplayerHostAddress;

    

    public void OnEnable() {
        
        NetManager.StartMatchMaker();
    }
	
	public void OnClickBack() {
        
        // unload all multiplayer stuff...

        MenuManager.SwitchPanel(MainMenuPanel);
    }

    public void OnClickHost() {
        NetManager.StartHost();
    }

    public void OnClickConnect() {
        
        MenuManager.SwitchPanel(GameLobbyPanel);

        NetManager.networkAddress = MultiplayerHostAddress.text;
        NetManager.StartClient();

        MenuManager.DisplayInfoPanel("Connecting...", "Cancel", NetManager.CancelClientConnection);

    }
    public void OnClickCreateMpGame() {

        NetManager.SetMatchHost(NetManager.matchHost,NetManager.matchPort,true);
        //NetManager.StartMatchMaker();

        NetManager.matchMaker.CreateMatch(MultiplayerGameName.text, (uint) NetManager.maxPlayers, true, "", "", "", 0, 0,
            NetManager.OnMatchCreate);

        MenuManager.CurrentlyMatchmaking = true;
        MenuManager.DisplayInfoPanel("Creating match...", "Cancel", NetManager.CancelHostConnection);
     
    }

    public void OnClickStartServer() {
        
        MenuManager.SwitchPanel(null);

        NetManager.StartServer();

        MenuManager.DisplayInfoPanel("Server running...", "Cancel", NetManager.StopServer);
    }

    
}
