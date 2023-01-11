using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //variable to hold speed of monsters
    float speed = 2f;
    //Variable for the position that the enemy moves towards
    private Transform m_target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Transforms/changes position of the enemy such that it moves towards the given position
        transform.position = Vector2.MoveTowards(transform.position, m_target.position, speed * Time.deltaTime);
    }

    //Method for setting the target that the enemy moves towards
    public void SetTarget(Transform target)
    {
        //sets the target to move towards as the given target from the spawner script
        m_target = target;

    }
}
