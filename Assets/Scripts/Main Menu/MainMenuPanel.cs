using UnityEngine;

public class MainMenuPanel : MonoBehaviour {

    public MultiplayerManager NetManager;
	public void OnClickSinglePlayer()
    {

    }

    public void OnClickMultiPlayer()
    {
        if(NetManager == null) Debug.Log("WTF???????");
        NetManager.SwitchPanel(NetManager.MultiplayerPanel);
    }
    public void OnClickSettings()
    {
        NetManager.SwitchPanel(NetManager.SettingsPanel);
    }
    public void OnClickExit()
    {

    }

}
