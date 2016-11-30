using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsPanel : MonoBehaviour {

    public Button BackButton;
    public MultiplayerManager NetManager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnBackButtonClick() {
        NetManager.SwitchPanel(NetManager.MainMenuPanel);
    }
}
