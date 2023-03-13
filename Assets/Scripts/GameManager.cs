using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Reference to the player
    [SerializeField] private Player m_player;
    //Variable to reference the high score text
    [SerializeField] private TextMeshProUGUI m_highScoreText;

    private CrossBow m_crossbow;
    private Rifle m_rifle;

    //Reference to the game over screen
    [SerializeField] private GameObject m_gameOverHUD;
    [SerializeField] private GameObject m_gameHUD;

    //Boolean variable as a flag for if new high score acheived
    private bool m_wasNewHighScore = false;

    //List in inspector for the different upgrades of crossbow - look in stats for the upgrade features
    [SerializeField] List<Stats> m_crossBowStats;
    //Initial level of crossbow set to element 0 in the list 
    private int m_crossbowUpgradeLevel = -1;

    //List in inspector for the different upgrades of rifle - look in stats for the upgrade features
    [SerializeField] List<Stats> m_rifleStats;
    //Initial level of rifle set to element 0 in the list
    private int m_rifleUpgradeLevel = -1;
    //Boolean variable to check is the rifle has been unlocked yet
    private bool m_isRifleUnlocked = false;

    //Instance for the singleton pattern
    private static GameManager m_instance;


    [Header("Wave Manager and Wave UI")]
    //Reference to wave manager
    [SerializeField] private WaveManager m_waveManager;
    //Reference to all UI - buttons on screen and all text
    [SerializeField] private TextMeshProUGUI m_waveCounter;
    [SerializeField] private TextMeshProUGUI m_totalMonsters;
    [SerializeField] private Button m_nextWaveButton;

    //Singleton pattern
    public static GameManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new GameManager();
        }

        return m_instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initalise singleton pattern
        m_instance = this;

        //Sets the high score on screen - initally as 00000
        if (PlayerPrefs.HasKey(Constants.PLAYER_HIGH_SCORE))
        {
            m_highScoreText.text = $"HiScore: {PlayerPrefs.GetInt(Constants.PLAYER_HIGH_SCORE):00000}";
        }

        //Sets the game HUD (the above UI) as visible
        m_gameHUD.SetActive(true);
        //Makes sure game over screen is invisible
        m_gameOverHUD.SetActive(false);

        Time.timeScale = 1f;

        // TODO: REMOVE 
        m_player.Gold = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_waveManager.HasEverythingSpawned() && m_waveManager.GetMonsterCount() == 0)
        {
            // The wave has ended, show the UI
            m_nextWaveButton.gameObject.SetActive(true);
            m_totalMonsters.gameObject.SetActive(false);
        }
        else
        {
            m_totalMonsters.text = $"Monsters Remaining: {m_waveManager.GetMonsterCount()}";
        }
    }

    //Method for when the game ends
    public void GameOver()
    {
        Time.timeScale = 0f;

        //Calls SaveHighScore method, passing in the current points of the user
        SaveHighScore(m_player.Points);

        Debug.Log("Game Over");

        // TODO: Tell the object pooler to tell the enemies to hide their hearts

        //Sets the current game HUD as invisible
        m_gameHUD.SetActive(false);
        //Makes game over screen visible
        m_gameOverHUD.SetActive(true);
    }

    //Method for checking if the new current score is higher than the current high score
    private void SaveHighScore(int playerScore)
    {
        // Save the player's highscore
        if (PlayerPrefs.HasKey(Constants.PLAYER_HIGH_SCORE))
        {
            //Variable to hold the current high score
            int previousHiScore = PlayerPrefs.GetInt(Constants.PLAYER_HIGH_SCORE);

            //Checks if the current score is higher than the current high score
            if (previousHiScore < playerScore)
            {
                //Replaces the high score on screen with the current score acheived by player
                PlayerPrefs.SetInt(Constants.PLAYER_HIGH_SCORE, playerScore);
                //Flag for changed high score
                m_wasNewHighScore = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt(Constants.PLAYER_HIGH_SCORE, playerScore);
        }
    }

    //Method for restarting the game
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Method for upgrading the crossbow
    public void OnUpgradeCrossbowPressed()
    {
        //Checks if the upgrades have been maxed out for the crossbow
        if (m_crossbowUpgradeLevel - 1 > m_crossBowStats.Count)
        {
            // Set text to say "MAXIMUM UPGRADE"
            return;
        }

        // If gold is greater than next upgrade
        if (m_crossBowStats[m_crossbowUpgradeLevel + 1].m_goldCost <= m_player.Gold)
        {
            Stats stats = m_crossBowStats[++m_crossbowUpgradeLevel];

            // ++ increments the variable by one 
            m_player.UpgradeWeapon(stats);
            //Takes away cost from the player gold variable
            m_player.Gold -= stats.m_goldCost;
            Debug.Log("Upgraded weapon!");
        }
    }

    //Method attached to button so that the next wave is triggered when the user wants
    public void OnNextWavePressed()
    {
        m_waveManager.OnStartWave();
        m_nextWaveButton.gameObject.SetActive(false);
        m_totalMonsters.gameObject.SetActive(true);
        m_waveCounter.text = $"Wave: {m_waveManager.GetWaveCount()}/10";
    }
}
