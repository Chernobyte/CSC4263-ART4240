
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharSelectOverlord : MonoBehaviour {

    public Text startText;

    private const int maxNumPlayers = 4;
    private CharSelectInfoPanel[] infoPanels;
    private List<PlayerSelection> playerSelections = new List<PlayerSelection>();
    private CharacterIconData[] charSelectOptions;
    private bool canStartGame = false;

    void Start()
    {
        InitCharSelectOptions();
        InitCharSelectInfoPanels();
        InitCharSelectCursors();

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        HandleInput();
    }

    void InitCharSelectOptions()
    {
        var charSelectOptionGOs = GameObject.FindGameObjectsWithTag("CharacterSelectOption");
        charSelectOptions = charSelectOptionGOs.Select(n => n.GetComponent<CharacterIconData>()).OrderByDescending(n => n.transform.position.y).ToArray();
    }

    void InitCharSelectInfoPanels()
    {
        var infoPanelGOs = GameObject.FindGameObjectsWithTag("CharacterSelectInfoPanel");
        infoPanels = infoPanelGOs.Select(n => n.GetComponent<CharSelectInfoPanel>()).ToArray();
    }

    void InitCharSelectCursors()
    {
        var playerCursorGOs = GameObject.FindGameObjectsWithTag("Cursor");
        var playerCursors = playerCursorGOs.Select(n => n.GetComponent<CharSelectCursor>()).ToArray();

        for (int i=0; i<playerCursors.Length; i++)
        {
            var cursor = playerCursors[i];
            var infoPanel = infoPanels.First(n => n.playerId == cursor.playerId);
            cursor.Init(this, charSelectOptions, infoPanel);
        }
    }

    public void ConfirmSelection(CharSelectCursor cursor, CharacterIconData characterIcons, GameObject characterPrefab)
    {
        playerSelections.Add(new PlayerSelection(cursor.playerId, characterIcons));

        if (playerSelections.Count >= 1)
        //if (playerSelections.Count >= 2)
        {
            canStartGame = true;
        }
    }

    public void CancelSelection(CharSelectCursor cursor)
    {
        var selectionToRemove = playerSelections.First(n => n.playerId == cursor.playerId);
        playerSelections.Remove(selectionToRemove);

        if (playerSelections.Count < 1)
        //if (playerSelections.Count < 2)
        {
            canStartGame = false;
        }
            
    }

    public List<PlayerSelection> ReqeustPlayerSelections()
    {
        return playerSelections.ToList();
    }

    private void HandleInput()
    {
        var startInputReceived = Input.GetButtonDown("Start");
        startText.gameObject.SetActive(canStartGame);

        

        if (canStartGame && startInputReceived)
        {
            SceneManager.LoadScene("Game");
        }
    }
}
