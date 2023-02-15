using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    //Arrow prefab
    [SerializeField] public GameObject arrow;
    //Amount of force applied to the arrow
    [SerializeField] public float launchForce;
    //Place where arrow is shot from
    [SerializeField] public Transform shotPoint;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //Position of crossbow and mouse
        Vector2 crossbowPosition = transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Direction of the arrow
        Vector2 direction = mousePosition - crossbowPosition;
        //Sets default direction to looking right
        transform.right = direction;
        //Fires the weapon when LMC
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    private void Fire()
    {
        //Instantiates a new arrow
        GameObject newArrow = Instantiate(arrow, shotPoint.position, shotPoint.rotation);
        //Gives the arrow a force
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
    }
}