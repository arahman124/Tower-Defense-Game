using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Arrows : MonoBehaviour
{
    //Rigidbody variable declared for arrow
    Rigidbody2D rb;
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
}