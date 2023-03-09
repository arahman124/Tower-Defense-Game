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
}
