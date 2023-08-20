using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int TopScore { get; private set; }
    public string TopPlayer { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateLeaderBoard(string playerName, int newScore)
    {
        TopPlayer = playerName;
        TopScore = newScore;
    }

    public static void OnLeaderBoardLoad(string playerName, int score)
    {
        ScoreManager.Instance.TopScore = score;
        ScoreManager.Instance.TopPlayer = playerName;
    }
}
