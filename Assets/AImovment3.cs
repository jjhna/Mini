using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Uses the car movement, rotation + front raycast
//Script to move the multiple AI's
public class AImovment3 : MonoBehaviour
{
    private GameObject Boto;
    public float movespeed = 0.0f;
    public bool exit, collided = false;
    public int fitness;
    [SerializeField]
    private int randint;
    public int[] randarray = new int[235];
    //set at movement speed 0.2, contains 235
    private int[] ansarray = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,   //0-14
        4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 2, 2,   //15-40, 26
        1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 2, 2, 2,   //41-67
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 2, 2, 2, 2, 2, 2, 2, 2, 2,   //68-93
        2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1,   //94-119
        1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 2, 2, 2, 2, 2, 2, 2, 2, 4, 4, 4,   //120-145
        4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1,   //146-171
        3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,   //172-197
        1, 1, 1, 1, 1, 1, 1, 3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4,   //198-223
        4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1};                                            //224-235

    //A constructor to intialize and get components from the clone AI's
    public void BotConstructor()
    {
        Boto = GameObject.Find("AI1(COPY)(Clone)");
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
                if (randarray[randint] == ansarray[randint])
                {
                    fitness++;
                }
                else
                {
                    Boto.SetActive(false);
                    collided = true;
                }
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
