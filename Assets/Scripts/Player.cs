using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variables for the current points and gold that the user possesses
    private int m_points;
    private int m_gold;

    //Text variable of the gold and points on screen
    [SerializeField] private TextMeshProUGUI m_pointsText;
    [SerializeField] private TextMeshProUGUI m_goldText;

    //Reference to the weapon being used by the player
    [SerializeField] private Weapon m_weapon;

    //Automatic property - Used instead of a new classes and new attributes with get and set methods
    // Holds the points of the player 
    public int Points 
    { 
        get => m_points; 

        set
        {
            //Updates points on screen
            m_points = value;

            // Add leading zeroes https://stackoverflow.com/questions/4325267/c-sharp-convert-int-to-string-with-padding-zeros 
            m_pointsText.text = $"Score: {m_points:00000}";
        }
    }

    //Automatic property for gold
    public int Gold 
    { 
        get => m_gold; 

        set
        {
            //Updates gold count on screen
            m_gold = value;

            m_goldText.text = $"x {m_gold}";
        } 
    }



    // Start is called before the first frame update
    void Start()
    {
        if (m_weapon == null)
        {
            Debug.Log("AAAAAAAAAAAAH!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Fires the weapon when LMC
        if (Input.GetMouseButtonDown(0))
        {
            m_weapon.Fire();
        }
    }

    //Method for adding points - amount passed in as parameter
    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    //Method for adding gold - amount passed in as parameter
    public void AddGold(int goldToAward)
    {
        Gold += goldToAward;
    }

    //Method for upgrading the weapon
    public void UpgradeWeapon(Stats stats)
    {
        //stats script is shown in inspector for the next level of the weapon
        m_weapon.SetDamage(stats.m_damage);
        m_weapon.SetLaunchForce(stats.m_projectileSpeed);
        m_weapon.SetCooldown(stats.m_rateOfFire);

        // A better wya to do it would be to have a readonly enum WeaponType, and then cast based on the WeaponType variable
        try
        {
            ((Rifle)m_weapon).SetAmmo(stats.m_ammoAmount);
        }catch(Exception e)
        {
            Debug.Log("This obviosuly isn't a rifle...");
        }
    }
}
