using UnityEngine;

public class MainMenuPanel : MonoBehaviour {

    public MultiplayerManager NetManager;
	public void OnClickSinglePlayer() {
	    NetManager.StartHost();
	}

    public void OnClickMultiPlayer()
    {
        NetManager.SwitchPanel(NetManager.MultiplayerPanel);
    }
    public void OnClickSettings()
    {
        NetManager.SwitchPanel(NetManager.SettingsPanel);
    }
    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         //Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }

}
