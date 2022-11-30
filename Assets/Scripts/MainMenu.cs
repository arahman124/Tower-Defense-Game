using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Creates the game object for the controls panel
    [SerializeField] private GameObject m_controlsPanel;

    public void PlayPressed()
    {
        //Displays this comment in the console to confirm that the button has been pressed - testing
        Debug.Log("I Hit the Play button");
        //Changes the scene to scene 1 which is the playing game state
        SceneManager.LoadScene(1);
    }

    public void ControlsPressed()
    {
        //Displays this comment in the console to confirm that the button has been pressed - testing
        Debug.Log("I Hit the Controls button");
        //Sets the control panel to the opposite state (active or inactive) in order to make it appear (active means it appears)
        m_controlsPanel.SetActive(!m_controlsPanel.activeSelf);
    }

    public void QuitPressed()
    {
        //Displays this comment in the console to confirm that the button has been pressed -testing
        Debug.Log("I HIT THE QUIT BUTTON");
        //Closes the application
        Application.Quit();
    }
}
