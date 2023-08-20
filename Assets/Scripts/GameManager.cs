using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public GameManager Instance;

    public string CurrentPlayerName { get; private set; }

    [SerializeField] private GameObject highScoreDisplay;
    [SerializeField] private TMP_Text highestScoringPlayer;
    [SerializeField] private TMP_Text highestScore;
    [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadLeaderBoard();

        if (ScoreManager.Instance.TopScore != 0)
        {
            highestScoringPlayer.text = ScoreManager.Instance.TopPlayer;
            highestScore.text = ScoreManager.Instance.TopScore.ToString();
            highScoreDisplay.SetActive(true);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("main");
    }

    public void ExitGame()
    {
        SaveLeaderboard();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void SetPlayerName()
    {
        CurrentPlayerName = inputField.text;
    }

    [System.Serializable]
    public class SaveData
    {
        public string topPlayerName;
        public int topScoreValue;
    }

    public void SaveLeaderboard()
    {
        SaveData data = new SaveData
        {
            topPlayerName = ScoreManager.Instance.TopPlayer,
            topScoreValue = ScoreManager.Instance.TopScore
        };

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    public void LoadLeaderBoard()
    {
        string path = Application.persistentDataPath + "/savedata.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            string playerName = data.topPlayerName;
            int score = data.topScoreValue;

            ScoreManager.OnLeaderBoardLoad(playerName, score);
        }
    }
}
