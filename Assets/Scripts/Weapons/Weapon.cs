using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    //Arrow prefab
    [SerializeField] protected GameObject m_projectile;
    //Amount of force applied to the arrow
    [SerializeField] protected float m_launchForce;
    //Place where arrow is shot from
    public Transform shotPoint;

    //Protected variable - private variable but accessible to the child classes
    //Cooldown variable between shots
    [SerializeField] protected float m_shotCooldown;
    //Variable to count down on the cooldown
    protected float m_coolDownTimer;

    //Variable for the damage of a single projectile
    protected float m_projectileDamage;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("UPDATING BASE");


        
    }

    //Single function to handle any updates to the Weapon at each frame
    protected void UpdateWeapon()
    {
        m_coolDownTimer -= Time.deltaTime;
        MoveWeapon();

        //The weapon is shot if LMC is true
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    //Weapon follows the mouse on screen
    private void MoveWeapon()
    {
        //Position of crossbow and mouse
        Vector2 weaponPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Direction of the arrow
        Vector2 direction = mousePosition - weaponPosition;
        //Sets default direction to looking right
        transform.right = direction;
    }

    //Abstract class for the sub-classes
    public abstract void Fire();

    //Sets the cooldown to the given parameter passed in
    public void SetCooldown(float cooldown)
    {
        m_shotCooldown = cooldown;
    }

    //Method for changing the launch force of the projectile
    public void SetLaunchForce(float launchForce)
    {
        m_launchForce = launchForce;
    }

    //Method for changing damage down by each projectile
    public void SetDamage(float damage)
    {
        m_projectileDamage = damage;
    }
}