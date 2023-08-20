using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreTextCurrent;
    public Text ScoreTextPersistent;
    public GameObject GameOverText;
    public GameManager GameManager;

    private bool m_Started = false;
    private int m_Points;
    private string m_playerName;

    private bool m_GameOver = false;

    void Start()
    {
        GameManager = FindObjectOfType<GameManager>();

        m_playerName = GameManager.CurrentPlayerName;
        ScoreTextCurrent.text = $"{m_playerName}'s score : {m_Points}";

        if (ScoreManager.Instance.TopScore != 0)
        {
            ScoreTextPersistent.text = $"Highscore: {ScoreManager.Instance.TopPlayer} : {ScoreManager.Instance.TopScore}";
        }
        else
        {
            ScoreTextPersistent.text = "";
        }

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                GameManager.ExitGame();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreTextCurrent.text = $"{m_playerName}'s score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (m_Points > ScoreManager.Instance.TopScore)
        {
            ScoreManager.Instance.UpdateLeaderBoard(m_playerName, m_Points);
            ScoreTextPersistent.text = $"Highscore: {ScoreManager.Instance.TopPlayer} : {ScoreManager.Instance.TopScore}";
        }
    }
}
