using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveLoadManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDataSaveLoad> dataSaveLoadObjects;
    private FileDataHandler dataHandler;
   public static SaveLoadManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogError("Found more than one Save Load Manager in the scene");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.dataSaveLoadObjects = FindAllDataSaveLoadObjects();
        LoadGame();
    }
    public void NewGame() 
    {
        this.gameData = new GameData();
    
    }
    public void LoadGame() 
    {
        //load any saved datafrom a file using the data handler
        this.gameData = dataHandler.Load();

        //if no data can be loaded, initialize to a new game
        if (this.gameData == null) 
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        //Push the loaded data to all other scripts that need it 
        foreach (IDataSaveLoad dataSaveLoadObj in dataSaveLoadObjects)
        {
            dataSaveLoadObj.LoadData(gameData);
        }
    }

    public void SaveGame() 
    {
        //pass the data to other scriptsso they can update it
        foreach (IDataSaveLoad dataSaveLoadObj in dataSaveLoadObjects) 
        {
            dataSaveLoadObj.SaveData(gameData);
        }


        // save that data to a file using the data handler
        dataHandler.Save(gameData);
        
       
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataSaveLoad> FindAllDataSaveLoadObjects() 
    {
        IEnumerable<IDataSaveLoad> dataSaveLoadObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataSaveLoad>();
        return new List<IDataSaveLoad>(dataSaveLoadObjects);
    }
}
