using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;

public class PublicGameEntry : MonoBehaviour {

    public Text AvailableSlotsText;
    public Text GameNameText;
    public Button JoinButton;

    private MatchInfoSnapshot _gameInfo; 

  
    public void OnJoinClicked() {
        if (_gameInfo.currentSize < _gameInfo.maxSize) 
            JoinMatch(_gameInfo.networkId);
        else
            UpdateJoinButton(false);
        
    }

    private void JoinMatch(NetworkID networkId) {
        MultiplayerManager.Instance.matchMaker.JoinMatch(networkId, "", "", "", 0, 0, MultiplayerManager.Instance.OnMatchJoined);
        MultiplayerManager.Instance.DisplayInfoPanel("Connecting...", "Cancel", MultiplayerManager.Instance.CancelClientConnection);
    }

    public void Populate(MatchInfoSnapshot gameInfo) {
        _gameInfo = gameInfo;
        GameNameText.text = gameInfo.name;
        AvailableSlotsText.text = "(" + gameInfo.currentSize +
            "\\" + gameInfo.maxSize + ")";
        UpdateJoinButton(_gameInfo.maxSize > _gameInfo.currentSize);
    }

    private void UpdateJoinButton(bool available) {
        JoinButton.interactable = available;
        JoinButton.GetComponentInChildren<Text>().color = available ? Color.black : Color.grey;
    }
}
