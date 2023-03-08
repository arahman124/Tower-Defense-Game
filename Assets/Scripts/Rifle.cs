using UnityEngine;

public class Rifle : Weapon
{
    private int m_maxAmmo;


    private void Update()
    {
        UpdateWeapon();
    }



    public override void Fire()
    {
        throw new System.NotImplementedException();
    }

    public void SetAmmo(int ammoCount)
    {
        m_maxAmmo = ammoCount;
    }
}