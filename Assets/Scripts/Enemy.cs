using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float speed = 10f;
    private Transform m_target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_target.position, speed * Time.deltaTime);

        
    }


    public void SetTarget(Transform target)
    {
        m_target = target;

    }
}
