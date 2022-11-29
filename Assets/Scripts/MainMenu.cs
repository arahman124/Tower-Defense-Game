using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_controlsPanel;

    public void PlayPressed()
    {
        Debug.Log("I HIT THE PLAY BUTTON");
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
