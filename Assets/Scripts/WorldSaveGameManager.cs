using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public PlayerManager player;

    [Header("Save Data Writer")]
    SaveGameDataWriter saveGameDataWriter;

    [Header("Current Character Data")]
    //Character slot #
    public CharacterSaveData currentCharacterSaveData;
    [SerializeField] private string fileName;

    [Header("Save/Load")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Did not destroy");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Did destroy");
            return;
        }
    }

    private void Start()
    {
        //DontDestroyOnLoad(gameObject);

        saveGameDataWriter = new SaveGameDataWriter();
        saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveGameDataWriter.dataSaveFileName = fileName;
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();

        }
        else if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public bool SaveFileExists()
    {
        return saveGameDataWriter.CheckIfSaveFileExists();
    }

    // Save Game
    public void SaveGame()
    {
        saveGameDataWriter = new SaveGameDataWriter();
        saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveGameDataWriter.dataSaveFileName = fileName;

        player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);
        saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);
    }

    // Load Game
    public void LoadGame()
    {
        saveGameDataWriter = new SaveGameDataWriter();
        saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveGameDataWriter.dataSaveFileName = fileName;
        currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

        StartCoroutine(LoadWorldSceneAsynchronously());
    }

    private IEnumerator LoadWorldSceneAsynchronously()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerManager>();
        }

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Game City");

        while (!loadOperation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            //Enable a loading screen & pass the loading progress to a slider/loading effect
            yield return null;
        }

        player = FindObjectOfType<PlayerManager>();

        if (player != null)
        {
            player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
            player.playerUIManager.healthBar.SetCurrentHealth(player.playerStatsManager.playerCurrentHealth);
        }
        else
        {
            Debug.LogError("PlayerManager not found after scene load!");
        }
    }

    public void SaveLosesCount(int losesCount)
    {
        PlayerPrefs.SetInt("LosesCount", losesCount);
        PlayerPrefs.Save();
    }

    public int LoadLosesCount()
    {
        return PlayerPrefs.GetInt("LosesCount", 0); // Default to 0 if not found
    }
}
