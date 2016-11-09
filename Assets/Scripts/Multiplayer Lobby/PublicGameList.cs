using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking.Match;
using UnityEngine.UI;



public class PublicGameList : MonoBehaviour
{
    private MultiplayerManager _netManager = MultiplayerManager.Instance;

    private VerticalLayoutGroup _layout;
    public RectTransform GameListContentTransform;

    public GameObject PublicGameEntry;
    public static PublicGameList Instance = null;

    private readonly List<PublicGameEntry> _games = new List<PublicGameEntry>();
    public int CurrentPage { get; protected set; }
    public int PreviousPage { get; protected set; }
    public GameObject NoGamesFound;

    void OnEnable()
    {
        CurrentPage = 0;
        PreviousPage = 0;
    }
    // Use this for initialization
    void Start()
    {
        Instance = this;
        _layout = GameListContentTransform.GetComponent<VerticalLayoutGroup>();

    }

    public void AddGame(PublicGameEntry gameEntry, MatchInfoSnapshot gameInfo)
    {

        if (_games.Contains(gameEntry)) return;
        _games.Add(gameEntry);

        gameEntry.Populate(gameInfo);
        gameEntry.transform.SetParent(GameListContentTransform, false);

    }

    public void ClearGames() {
        Debug.Log("Clearing games");
       
        foreach (var game in _games.ToList())
        {
            Debug.Log("Destroying game : " + game.GameNameText);
            _games.Remove(game);
            Destroy(game.gameObject);
        }
    }
    public void RequestPage(int page)
    {
        PreviousPage = CurrentPage;
        CurrentPage = page;
        if (_netManager == null) _netManager = FindObjectOfType<MultiplayerManager>();
        if(_netManager.matchMaker == null) _netManager.StartMatchMaker();
        _netManager.matchMaker.ListMatches(page, 6, "", true, 0, 0, OnGuiMatchList);
    }
    public void ChangePage(int dir)
    {
        int newPage = Mathf.Max(0, CurrentPage + dir);

        //if we have no server currently displayed, need we need to refresh page0 first instead of trying to fetch any other page
        if (NoGamesFound.activeSelf)
            newPage = 0;

        RequestPage(newPage);
    }
    private void OnGuiMatchList(bool success, string extendedinfo, List<MatchInfoSnapshot> publicGameList)
    {

        if (publicGameList.Count == 0)
        {
            if (CurrentPage == 0)
                NoGamesFound.SetActive(true);
            CurrentPage = PreviousPage;
            return;
        }

        NoGamesFound.SetActive(false);
        foreach (var game in _games)
        {
            Destroy(game);
        }
        foreach (var gameInfo in publicGameList)
        {
            var game = Instantiate(PublicGameEntry);
            AddGame(game.GetComponent<PublicGameEntry>(), gameInfo);
        }
    }

}
