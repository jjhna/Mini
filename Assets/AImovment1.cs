using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Uses the car movement, rotation + front raycast
//Script to move the multiple AI's
public class AImovment1 : MonoBehaviour
{
    private GameObject Boto;
    public float movespeed = 0.0f;
    public bool exit, collided = false;
    public int fitness;
    [SerializeField]
    private int randint;
    public int[] randarray = new int[500];

    //A constructor to intialize and get components from the clone AI's
    public void BotConstructor()
    {
        Boto = GameObject.Find("AI1(Clone)");
        Boto.name = "AI";
        fitness = -1;
        randint = 0;
    }

    // Use this for initialization
    void Start()
    {
        BotConstructor();
    }

    // Update is called once per frame
    void Update()
    {
        MoveArray();
    }

    //If collision is detected with a wall the fitness goes down
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            Boto.SetActive(false);
            collided = true;
        }
        else if (col.gameObject.tag == "Final")
        {
            exit = true;
        }
    }

    //Have to make AI die if over than 235
    void MoveArray()
    {
        if (randint == randarray.Length)
        {
            Boto.SetActive(false);
            collided = true;
        }
        else
        {
            int ran1 = randarray[randint];
            Movement(ran1);
            if (collided == false)
            {
                
            }
            randint++;
        }
    }

    void Movement(int ran1)
    {
        switch (ran1)
        {
            case 1:
                transform.position -= transform.up * movespeed; //1 - move down
                break;
            case 2:
                transform.position += transform.up * movespeed; //2 - move up
                break;
            case 3:
                transform.position -= transform.right * movespeed; //3 - move left
                break;
            case 4:
                transform.position += transform.right * movespeed; //4 - move right
                break;
        }
    }

}
