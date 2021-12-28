using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScore;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    public int currentBestScore = 0;
    private string bestScoreString;
    
    private bool m_GameOver = false;
    
    public static MainManager Instance;
    public string playerName;
    public string bestPlayer;

    // private void Awake() {

    //     //start of new code
    //     if (Instance != null)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //     //end of new code
    //     Instance = this;
    //     DontDestroyOnLoad(gameObject);
        
    //     LoadHighScore();
    // }
    //Start is called before the first frame update
    void Start()
    {
        playerName = StaticDataController.playerName;
        LoadHighScore();
        BestScore.text = "Best Score : " + bestPlayer + " : " + bestScoreString;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        
    } 

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        BestScoreText();
    }

    void BestScoreText()
    {
        if (m_Points > currentBestScore)
        {
            bestPlayer = playerName;
            currentBestScore = m_Points;
            bestScoreString = currentBestScore.ToString();
            BestScore.text = "Best Score : " + bestPlayer + " : " + bestScoreString;
        }
        else 
        {
            bestScoreString = currentBestScore.ToString();
            BestScore.text = "Best Score : " + bestPlayer + " : " + bestScoreString;
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighScore();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    [System.Serializable]
    class SaveData
    {
        public int saveBestScore;
        public string savePlayerName;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.saveBestScore = currentBestScore;
        data.savePlayerName = bestPlayer;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentBestScore = data.saveBestScore;
            bestPlayer = data.savePlayerName;
            bestScoreString = currentBestScore.ToString();
        }
    }

}
