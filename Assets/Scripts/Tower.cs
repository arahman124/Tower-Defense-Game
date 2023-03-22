using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public float Health
    {
        get => m_health;
        set
        {
            // TODO: Update healthbar
            m_healthBarSlider.value = value / m_maxHealth;

            float percentage = Mathf.Max(0, value / m_maxHealth * 100f);

            if (percentage < 75f)
            {
                m_healthBarImage.color = m_damageColours[1];
            }
            if (percentage < 50f)
            {
                m_healthBarImage.color = m_damageColours[2];
            }
            if (percentage < 25f)
            {
                m_healthBarImage.color = m_damageColours[3];
            }
            if (percentage < 10f)
            {
                m_healthBarImage.color = m_damageColours[4];
            }
            if(percentage <= 0f)
            {
                m_healthBarImage.gameObject.SetActive(false);

                GameManager.GetInstance().GameOver();
            }

            m_health = value;

            m_healthBarText.text = $"{percentage}%";
        }
    }

    //Variable for the max health the tower can have
    [SerializeField] private float m_maxHealth;
    //Variable for the current health of the tower
    private float m_health;
    //Reference to the animator
    private Animator m_animator;

    //Variable for the text of the health bar - title
    [SerializeField] private TextMeshProUGUI m_healthBarText;
    //Variable for the slider of the health bar itself
    [SerializeField] private Slider m_healthBarSlider;
    //Variable for the image of the health bar attached to the slider
    [SerializeField] private Image m_healthBarImage;
    //List of colours that the bar can change to - changed in inspector
    [SerializeField] private List<Color> m_damageColours;


    // Start is called before the first frame update
    void Start()
    {
        Health = m_maxHealth;
        m_healthBarImage.color = m_damageColours[0];

        //Stores the animator of the current object the script is attached to for further manipulation
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void TakeDamage(float incomingDps)
    {
        Health -= incomingDps;
        m_animator.SetTrigger(Constants.TOWER_DAMAGE);
    }
}
