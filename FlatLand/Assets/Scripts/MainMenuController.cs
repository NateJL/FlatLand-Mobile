using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private GameManager manager;
    private TouchScreenKeyboard keyboard;

    public GameObject mainMenuPage;
    public GameObject newGamePage;
    public GameObject loadGamePage;
    public GameObject loadGameFailedPage;
    [Space(20)]
    public TextMeshProUGUI loadGameDataText;
    public TextMeshProUGUI newGameDataText;
    public TextMeshProUGUI newGamePlayerNameText;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        manager.mainMenuController = this;
        MainMenuButtonAction(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard != null)
        {
            if (keyboard.status == TouchScreenKeyboard.Status.Visible)
            {

            }
            else if (keyboard.status == TouchScreenKeyboard.Status.Done)
            {
                manager.playerData.playerName = keyboard.text;
                newGamePlayerNameText.SetText(manager.playerData.playerName);
                SaveSystem.SavePlayerData(manager.playerData);
                HideAllPages();
                newGamePage.SetActive(true);
            }
        }
    }

    /// <summary>
    /// <para>Method callback for buttons in main menu.</para>
    /// <para>0: calls self (back to main menu)</para>
    /// <para>1: exit the application</para>
    /// <para>2: open the new game page</para>
    /// <para>3: open the load game page</para>
    /// </summary>
    public void MainMenuButtonAction(int value)
    {
        HideAllPages();
        switch(value)
        {
            case 0:
                mainMenuPage.SetActive(true);
                break;
            case 1:
                Application.Quit();
                break;
            case 2:
                if(!manager.LoadSaveGameData())
                {
                    manager.playerData = new PlayerData();
                    newGamePlayerNameText.SetText(manager.playerData.playerName);
                    newGameDataText.SetText("<b>Edges:</b> " + manager.playerData.edgeCount +
                                       "\n<b>Health:</b> " + manager.playerData.health +
                                       "\n<b>Speed:</b> " + manager.playerData.speed +
                                       "\n<b>Points:</b> " + manager.playerData.points);
                    newGamePage.SetActive(true);
                }
                else
                {
                    manager.playerData = new PlayerData();
                    newGamePlayerNameText.SetText(manager.playerData.playerName);
                    newGameDataText.SetText("<b>Edges:</b> " + manager.playerData.edgeCount +
                                       "\n<b>Health:</b> " + manager.playerData.health +
                                       "\n<b>Speed:</b> " + manager.playerData.speed +
                                       "\n<b>Points:</b> " + manager.playerData.points);
                    newGamePage.SetActive(true);
                    //mainMenuPage.SetActive(true);
                    //Debug.Log("Load Game already exists!");
                }
                break;
            case 3:
                if (manager.LoadSaveGameData())
                {
                    loadGameDataText.SetText("<b>Name:</b> " + manager.playerData.playerName +
                                       "\n<b>Edges:</b> " + manager.playerData.edgeCount +
                                       "\n<b>Health:</b> " + manager.playerData.health +
                                       "\n<b>Speed:</b> " + manager.playerData.speed +
                                       "\n<b>Points:</b> " + manager.playerData.points);
                    loadGamePage.SetActive(true);
                }
                else
                    loadGameFailedPage.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// <para>Method callback for buttons in new game page.</para>
    /// <para>0: back to main menu</para>
    /// <para>1: Save new player data and start new game</para>
    /// <para>2: Open keyboard to edit player name</para>
    /// </summary>
    public void NewGameButtonAction(int value)
    {
        HideAllPages();
        switch (value)
        {
            case 0:
                mainMenuPage.SetActive(true);
                break;
            case 1:
                SaveSystem.SavePlayerData(manager.playerData);
                manager.LoadScene(ApplicationConstants.Scenes.FLATLAND);
                break;
            case 2:
                HandleTouchscreenkeyboard(manager.playerData.playerName);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// <para>Method callback for buttons in load game page.</para>
    /// <para>0: back to main menu</para>
    /// <para>1: </para>
    /// </summary>
    public void LoadGameButtonAction(int value)
    {
        HideAllPages();
        switch (value)
        {
            case 0:
                mainMenuPage.SetActive(true);
                break;
            case 1:
                manager.LoadScene(ApplicationConstants.Scenes.FLATLAND);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// <para>Method callback for buttons in failed load game page.</para>
    /// <para>0: back to main menu</para>
    /// <para>1: </para>
    /// </summary>
    public void LoadGameFailedButtonAction(int value)
    {
        HideAllPages();
        switch (value)
        {
            case 0:
                mainMenuPage.SetActive(true);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void HideAllPages()
    {
        mainMenuPage.SetActive(false);
        newGamePage.SetActive(false);
        loadGamePage.SetActive(false);
        loadGameFailedPage.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void HandleTouchscreenkeyboard(string text)
    {
        if (keyboard == null)
            keyboard = TouchScreenKeyboard.Open(text);

        if (!keyboard.active)
        {
            keyboard = TouchScreenKeyboard.Open(text);
        }
    }
}
