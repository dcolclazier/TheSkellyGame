using UnityEngine;
using System.Collections;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class PublicGameEntry : MonoBehaviour {

    public Text AvailableSlotsText;
    public Text GameNameText;
    public Button JoinButton;
    public MatchInfoSnapshot GameInfo { get; set; }
    public MultiplayerManager MultiplayerManager { get; set; }
  
    public void OnJoinClicked() {
        if (GameInfo.currentSize < GameInfo.maxSize) 
            JoinMatch(GameInfo.networkId, MultiplayerManager);
        else
            UpdateJoinButton(false);
        
    }

    private void JoinMatch(NetworkID networkId, MultiplayerManager multiplayerManager) {
        multiplayerManager.matchMaker.JoinMatch(networkId, "", "", "", 0, 0, multiplayerManager.OnMatchJoined);
        multiplayerManager.MenuManager.DisplayInfoPanel("Connecting...", "Cancel", multiplayerManager.CancelClientConnection);
    }

    public void Populate(MatchInfoSnapshot gameInfo, MultiplayerManager multiplayerManager) {
        GameInfo = gameInfo;
        GameNameText.text = gameInfo.name;
        AvailableSlotsText.text = "(" + gameInfo.currentSize +
            "\\" + gameInfo.maxSize + ")";
        UpdateJoinButton(GameInfo.maxSize > GameInfo.currentSize);

        MultiplayerManager = multiplayerManager;
    }

    private void UpdateJoinButton(bool available) {
        JoinButton.interactable = available;
        JoinButton.GetComponentInChildren<Text>().color = available ? Color.black : Color.grey;
    }
}
