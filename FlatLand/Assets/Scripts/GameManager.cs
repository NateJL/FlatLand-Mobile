using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public MainMenuController mainMenuController;
    public GameOverlayController gameOverlayController;
    public AudioManager audioManager;
    public PoolManager poolManager;
    public GameObject player;
    public PlayerData playerData;
    public int currentScene;
    [Space(10)]
    public GameObject playerPrefab;

    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            Debug.Log("Initial GameManager initialized.");
        }
        else if (manager != this)
        {
            Debug.Log("Another GameManager found, destroying this...");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Debug.Log("GameManager initialized.");
    }

    // Start is called before the first frame update
    void Start()
    {
        mainMenuController = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<MainMenuController>();
        audioManager = GetComponent<AudioManager>();
        poolManager = GetComponent<PoolManager>();

        audioManager.Initialize();
        poolManager.Initialize(this);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Load the specified scene by enum value.
    /// </summary>
    public void LoadScene(ApplicationConstants.Scenes nextScene)
    {
        switch(nextScene)
        {
            case ApplicationConstants.Scenes.MAIN_MENU:
                SceneManager.LoadScene("MainMenu");
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    StartCoroutine("waitForSceneLoad", 0);
                }
                break;
            case ApplicationConstants.Scenes.FLATLAND:
                SceneManager.LoadScene("FlatLand");
                if (SceneManager.GetActiveScene().buildIndex != 1)
                {
                    StartCoroutine("waitForSceneLoad", 1);
                }
                break;
        }
    }

    /// <summary>
    /// Coroutine to wait for scene to load before starting scripts.
    /// </summary>
    IEnumerator waitForSceneLoad(int sceneNumber)
    {
        while (SceneManager.GetActiveScene().buildIndex != sceneNumber)
        {
            yield return null;
        }

        // Do anything after proper scene has been loaded
        if (SceneManager.GetActiveScene().buildIndex == sceneNumber && sceneNumber > 0)
        {
            Debug.Log("Saved spawn: " + playerData.playerPosition);
            player = Instantiate(playerPrefab, playerData.playerPosition, Quaternion.identity);
        }
        currentScene = sceneNumber;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool LoadSaveGameData()
    {
        if (SaveSystem.CheckForPlayerData())
        {
            playerData = SaveSystem.LoadPlayerData();
            return true;
        }
        else
            return false;
    }
}
