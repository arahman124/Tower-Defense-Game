using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D m_rb;


    [SerializeField] private float m_damage;



    // Start is called before the first frame update
    void Start()
    {
        m_rb.velocity = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Checks for collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Condition that if the bullet hits any object with the ground tag, it is deleted
        if (collision.gameObject.CompareTag(Constants.GROUND_TAG))
        {
            //The bullet is deleted
            Destroy(gameObject);
        }

        //Condition that if the bullet hits any object with the ground tag, it is deleted
        if (collision.gameObject.layer == LayerMask.NameToLayer(Constants.ENEMY_LAYER))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.Health -= m_damage;

            //The bullet is deleted
            Destroy(gameObject);
        }
    }
}
