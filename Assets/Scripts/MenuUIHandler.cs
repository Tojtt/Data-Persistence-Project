using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif


// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public MainManager mainManager;
    private int currentBestScore;
    private string bestScoreString;
    private string bestPlayer;
    public Text BestScore;

    public GameObject inputField; //variable to assign the name from input field
    
    private void Start()
    {
        mainManager.LoadHighScore();
        BestScore.text = "Best Score : " + bestPlayer + " : " + bestScoreString;
    }

    public void Storename()
    {
        
        StaticDataController.playerName = inputField.GetComponent<Text>().text;
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    // public void LoadHighScore()
    // {
    //     string path = Application.persistentDataPath + "/savefile.json";
    //     if (File.Exists(path))
    //     {
    //         string json = File.ReadAllText(path);
    //         SaveData data = JsonUtility.FromJson<SaveData>(json);

    //         currentBestScore = data.saveBestScore;
    //         bestPlayer = data.savePlayerName;
            

    //     }
    // }

}
