using UnityEngine;

public class Rifle : Weapon
{
    //Maximum ammo of the rifle
    private int m_maxAmmo;


    private void Update()
    {
        UpdateWeapon();
    }



    public override void Fire()
    {
        if (m_coolDownTimer < 0)
        {
            // TODO: Use object pooler for arrows :)
            //Instantiates a new arrow
            GameObject newBullet = Instantiate(m_projectile, shotPoint.position, shotPoint.rotation);
            //Gives the arrow a force
            newBullet.GetComponent<Rigidbody2D>().velocity = transform.right * m_launchForce;
            //Sets the cooldown on the weapon to the shot cooldown that it has currently
            m_coolDownTimer = m_shotCooldown;
        }
    }

    public void SetAmmo(int ammoCount)
    {
        m_maxAmmo = ammoCount;
    }
}