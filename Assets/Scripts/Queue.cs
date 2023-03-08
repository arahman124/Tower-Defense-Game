using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queue<T> : MonoBehaviour
{


    //Class for an individual node in the linked list - <T> is called generics - allows for any data type to be used when stated in other scripts
    public class Node<T>
    {
        //Variable to hold the data (game object) and the position of the next item
        public T data;
        //Initially set to null - look below
        public Node<T> next;


        //Constructor - creates new node
        public Node(T data)
        {
            this.data = data;
            this.next = null;
        }
    }

    //Front and rear pointers for the queue
    public Node<T> front;
    public Node<T> rear;

    //Initalise queue - set both front and rear pointer to null
    public Queue()
    {
        this.front = null;
        this.rear = null;
    }

    //Dequeue function that removes an item from the end of the list
    public Node<T> dequeue()
    {

        if (this.front == null)
        {
            Debug.Log("Queue is empty");
            return null;
        }


        if (this.front == null)
        {
            this.rear = null;
        }

        this.front = this.front.next;
        return this.front;
    }


    //Enqueue function adds an element to the queue - passed in as parameter
    public void enqueue(T data)
    {
        //Creates a node
        Node<T> newData = new Node<T>(data);

        //Checks if the 
        if (this.rear == null)
        {
            this.front = newData;
            this.rear = newData;
        }
        else
        {
            this.rear.next = newData;
            this.rear = newData;
        }

    }

}
