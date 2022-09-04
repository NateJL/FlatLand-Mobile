using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverlayController : MonoBehaviour
{
    public GameObject gameOverlayPage;
    public GameObject gameMenuPage;
    public GameObject gameSettingsPage;

    [Space(20)]
    public GameObject gameDialoguePage;
    public TextMeshProUGUI dialogueTitleText;

    public ApplicationConstants.GameOverlay overlayMode;

    public Transform entryButtonParent;
    public GameObject entryButtonPrefab;

    public TextMeshProUGUI playerDialogueText;
    public NpcDialogueData dialogueData;

    [HideInInspector]
    public Dictionary<int, ElementUI> entryButtonCollection;
    public int entryButtonCount;

    public GameObject attackButton;

    public delegate void DialogueDelegateMethod(string value);

    // Start is called before the first frame update
    void Start()
    {
        GameManager.manager.gameOverlayController = this;
        entryButtonCollection = new Dictionary<int, ElementUI>();
        GameOverlayButtonAction(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// <para>Method callback for buttons in game overlay.</para>
    /// <para>0: calls self (back to overlay)</para>
    /// <para>1: opens the game menu</para>
    /// <para>2: attack button</para>
    /// <para>3: </para>
    /// </summary>
    public void GameOverlayButtonAction(int value)
    {
        HideAllPages();
        switch (value)
        {
            case 0:
                gameOverlayPage.SetActive(true);
                overlayMode = ApplicationConstants.GameOverlay.CLOSED;
                break;
            case 1:
                gameMenuPage.SetActive(true);
                overlayMode = ApplicationConstants.GameOverlay.MENU;
                break;
            case 2:
                gameOverlayPage.SetActive(true);
                GameManager.manager.player.GetComponent<PlayerController>().BeginAttack();
                break;
            case 3:
                
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// <para>Method callback for buttons in game menu.</para>
    /// <para>0: returns to overlay</para>
    /// <para>1: save and return to title screen</para>
    /// <para>2: open the settings page</para>
    /// <para>3: </para>
    /// </summary>
    public void GameMenuButtonAction(int value)
    {
        HideAllPages();
        switch (value)
        {
            case 0:
                gameOverlayPage.SetActive(true);
                overlayMode = ApplicationConstants.GameOverlay.CLOSED;
                break;
            case 1:
                SaveSystem.SavePlayerData(GameManager.manager.playerData);
                GameManager.manager.LoadScene(ApplicationConstants.Scenes.MAIN_MENU);
                break;
            case 2:
                gameSettingsPage.SetActive(true);
                overlayMode = ApplicationConstants.GameOverlay.SETTINGS;
                break;
            case 3:

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// <para>Method callback for buttons in game menu.</para>
    /// <para>0: returns to game menu</para>
    /// <para>1: </para>
    /// <para>2: </para>
    /// <para>3: </para>
    /// </summary>
    public void GameSettingsButtonAction(int value)
    {
        HideAllPages();
        switch (value)
        {
            case 0:
                gameMenuPage.SetActive(true);
                overlayMode = ApplicationConstants.GameOverlay.MENU;
                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// <para>Method callback for buttons in dialogue overlay.</para>
    /// <para>0: returns to game (close dialogue menu)</para>
    /// <para>1: Entry button #1</para>
    /// </summary>
    public void GameDialogueButtonAction(int value)
    {
        Debug.Log("Value sent from click: " + value.ToString());
        HideAllPages();
        switch (value)
        {
            case 0:
                gameOverlayPage.SetActive(true);
                overlayMode = ApplicationConstants.GameOverlay.CLOSED;
                break;
            case 1:
            case 2:
                gameDialoguePage.SetActive(true);
                SetPlayerDialogueText(dialogueData.entries[value - 1].entry);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Set the text data for the players dialogue box.
    /// </summary>
    /// <param name="text"></param>
    public void SetPlayerDialogueText(string text)
    {
        Debug.Log("Setting player dialogue...");
        if(dialogueData == null)
        {
            playerDialogueText.SetText("No Dialogue Found.");
            return;
        }

        playerDialogueText.SetText(text);
    }

    public void WriteToDialogue(DialogueDelegateMethod method, int value)
    {
        method(dialogueData.entries[value].entry);
    }

    /// <summary>
    /// Opens the dialogue page and fills with the given dialogue data parameter.
    /// </summary>
    public void OpenDialogueOverlay(NpcDialogueData dialogueData)
    {
        this.dialogueData = dialogueData;

        if (overlayMode != ApplicationConstants.GameOverlay.CLOSED && overlayMode != ApplicationConstants.GameOverlay.DIALOGUE)
            return;

        HideAllPages();
        overlayMode = ApplicationConstants.GameOverlay.DIALOGUE;

        dialogueTitleText.SetText(dialogueData.npcName);

        entryButtonCount = 0;
        foreach (KeyValuePair<int, ElementUI> kvp in entryButtonCollection)    // Return all buttons to object pool
        {
            GameManager.manager.poolManager.ReturnObjectUI(kvp.Value);
        }

        float buttonOffset = 0.0f;
        for (int i = 0; i < dialogueData.entries.Count; i++)
        {
            Vector3 offset = new Vector3(0, buttonOffset, 0);
            ElementUI newButton = GameManager.manager.poolManager.SpawnObjectUI(entryButtonPrefab.name, transform.position, transform.rotation, entryButtonParent);        // spawn button from pool

            if (newButton.element.CompareTag("Button"))
            {
                newButton.element.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(dialogueData.entries[i].title);
            }

            if(newButton.element.GetComponent<Button>() != null)
            {
                DialogueDelegateMethod cm = SetPlayerDialogueText;
                Button button = newButton.element.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate { WriteToDialogue(cm, i); });
            }

            newButton.element.transform.localPosition = offset;
            newButton.element.transform.localEulerAngles = new Vector3(0, 0, 0);
            newButton.element.transform.localScale = new Vector3(1, 1, 1);
            buttonOffset -= 60.0f;
            entryButtonCount++;
        }

        gameDialoguePage.SetActive(true);
    }

    /// <summary>
    /// 
    /// </summary>
    private void HideAllPages()
    {
        gameOverlayPage.SetActive(false);
        gameMenuPage.SetActive(false);
        gameSettingsPage.SetActive(false);
        gameDialoguePage.SetActive(false);
    }
}
