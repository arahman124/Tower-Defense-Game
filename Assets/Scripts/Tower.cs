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

            float percentage = value / m_maxHealth * 100f;

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
            if(percentage < 0f)
            {
                m_healthBarImage.gameObject.SetActive(false);
                // TODO: GAME OVER inside some sort of GameManager script that has references to the UI and stuff
                Time.timeScale = 0f;
                Debug.Log("Game Over");
            }

            m_health = value;

            m_healthBarText.text = $"{percentage}%";
        }
    }

    [SerializeField] private float m_maxHealth;
    private float m_health;
    private Animator m_animator;

    [SerializeField] private TextMeshProUGUI m_healthBarText;
    [SerializeField] private Slider m_healthBarSlider;
    [SerializeField] private Image m_healthBarImage;
    [SerializeField] private List<Color> m_damageColours;


    // Start is called before the first frame update
    void Start()
    {
        Health = m_maxHealth;
        m_healthBarImage.color = m_damageColours[0];

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