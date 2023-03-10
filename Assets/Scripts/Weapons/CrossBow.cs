using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Inherits from the weapon.cs
public class CrossBow : Weapon
{
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Calls the function from weapon.cs
        UpdateWeapon();
    }

    //Fires the arrow from the crossbow on screen - overide function taken from base
    public override void Fire()
    {
        if (m_coolDownTimer < 0)
        {
            // TODO: Use object pooler for arrows :)
            //Instantiates a new arrow
            GameObject newArrow = Instantiate(m_projectile, shotPoint.position, shotPoint.rotation);
            //Gives the arrow a force
            newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * m_launchForce;
            AudioManager.Instance.PlaySFX("Bow");
            //Sets the cooldown on the weapon to the shot cooldown that it has currently
            m_coolDownTimer = m_shotCooldown;
        }
    }
}
