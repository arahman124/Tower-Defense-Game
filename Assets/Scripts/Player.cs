using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int m_points;
    private int m_gold;

    [SerializeField] private TextMeshProUGUI m_pointsText;
    [SerializeField] private TextMeshProUGUI m_goldText;

    public int Points 
    { 
        get => m_points; 

        set
        {
            m_points = value;

            // Add leading zeroes https://stackoverflow.com/questions/4325267/c-sharp-convert-int-to-string-with-padding-zeros 
            m_pointsText.text = $"Score: {m_points:00000}";
        }
    }

    public int Gold 
    { 
        get => m_gold; 

        set
        {
            m_gold = value;

            m_goldText.text = $"x {m_gold}";
        } 
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    public void AddGold(int goldToAward)
    {
        Gold += goldToAward;
    }
}
