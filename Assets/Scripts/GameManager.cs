using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;

    [SerializeField] private Player m_player;
    [SerializeField] private TextMeshProUGUI m_highScoreText;

    private bool m_wasNewHighScore = false;

    public static GameManager GetInstance()
    {
        if(m_instance == null)
        {
            m_instance = new GameManager();
        }

        return m_instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_instance = this;

        if (PlayerPrefs.HasKey(Constants.PLAYER_HIGH_SCORE))
        {
            m_highScoreText.text = $"HiScore: {PlayerPrefs.GetInt(Constants.PLAYER_HIGH_SCORE):00000}";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        SaveHighScore(m_player.Points);

        Debug.Log("Game Over");
    }

    private void SaveHighScore(int playerScore)
    {
        // Save the player's highscore
        if (PlayerPrefs.HasKey(Constants.PLAYER_HIGH_SCORE))
        {
            int previousHiScore = PlayerPrefs.GetInt(Constants.PLAYER_HIGH_SCORE);

            if(previousHiScore < playerScore)
            {
                PlayerPrefs.SetInt(Constants.PLAYER_HIGH_SCORE, playerScore);
                m_wasNewHighScore = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt(Constants.PLAYER_HIGH_SCORE, playerScore);
        }
    }
}
