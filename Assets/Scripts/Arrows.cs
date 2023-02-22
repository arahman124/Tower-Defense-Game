using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Arrows : MonoBehaviour
{
    //Rigidbody variable declared for arrow
    Rigidbody2D rb;

    //Variable to hold tag for the ground
    private string GROUND_TAG = "Ground";

    // Start is called before the first frame update
    void Start()
    {
        //Accesses the rigidbody for the attached object (arrow)
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        //Calculates angle for arrow
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        //Rotates the arrow such that it has a nice arrow arc
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //Checks for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Condition that if the arrow hits any object with the ground tag, it is deleted
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            //The arrow is deleted
            Destroy(gameObject);
        }
    }
}