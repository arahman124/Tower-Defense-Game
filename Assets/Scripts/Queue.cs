using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue<T>
{
    //Class for an individual node in the linked list - <T> is called generics - allows for any data type to be used when stated in other scripts
    public class Node<T>
    {
        //Variable to hold the data (game object) and the position of the next item
        public T Data;
        //Initially set to null - look below
        public Node<T> Next;


        //Constructor - creates new node
        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    //Front and rear pointers for the queue
    public Node<T> Front;
    public Node<T> Rear;

    //Initalise queue - set both front and rear pointer to null
    public Queue()
    {
        Front = null;
        Rear = null;
    }

    //Dequeue function that removes an item from the end of the list
    public Node<T> dequeue()
    {

        if (Front == null)
        {
            Debug.Log("Queue is empty");
            return null;
        }


        if (Front == null)
        {
            Rear = null;
        }

        Front = Front.Next;
        return Front;
    }


    //Enqueue function adds an element to the queue - passed in as parameter
    public void enqueue(T data)
    {
        //Creates a node
        Node<T> newData = new Node<T>(data);

        //Checks if 
        if (Rear == null)
        {
            Front = newData;
            Rear = newData;
        }
        else
        {
            Rear.Next = newData;
            Rear = newData;
        }

    }

}
