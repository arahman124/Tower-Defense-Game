using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_controlsPanel;

    public void PlayPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void ControlsPressed()
    {
        m_controlsPanel.SetActive(!m_controlsPanel.activeSelf);
    }

    public void QuitPressed()
    {
        Debug.Log("I HIT THE QUIT BUTTON");

    }
}
