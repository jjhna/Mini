﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/* Name: Na, Jonathan For: ICS 674 Professor: Lee Altenberg Project: Final Maze Project Class: AImovment4.cs
    The purpose of this class is to control the AI, from its movement array to its collision and fitness.
    This current class is built for the larger food size tiles. */
public class AImovment4 : MonoBehaviour
{
    private GameObject Boto, Spawno;
    public float movespeed = 0.0f;
    public bool exit, collided = false;
    public int fitness;
    [SerializeField]
    private int whatnumami = 0;
    private int randint;
    public int randhit;
    public int[] randarray = new int[400];
    private GameObject[] FoodItems = new GameObject[237];
    private FoodScript2[] foodies = new FoodScript2[237];
    private Mini4 min1;

    //A constructor to intialize and get components from the clone AI's
    void BotConstructor()
    {
        //Finds the Spawn gameobject
        Spawno = GameObject.Find("Spawn");
        min1 = Spawno.GetComponent<Mini4>();

        //Find the current AI gameobject
        Boto = GameObject.Find("AI2(Clone)");
        Boto.name = "AI";

        //Finds what index this AI is
        whatnumami = System.Array.IndexOf(min1.BotArray, Boto);

        fitness = 0;
        randint = 0;
    }

    // Use this for initialization
    void Start()
    {
        BotConstructor();
        TrackFood();
    }

    // Update is called once per frame
    void Update()
    {
        MoveArray();
    }

    //If AI hits a wall it dies, the exit programs end, a food then it checks if it hasn't be triggered before
    //If the food hasn't been triggered by that particular AI then the fitness rating goes up and that food becomes disabled
    //To the AI that has triggered it
    void OnTriggerEnter2D(Collider2D col)
    {
        //If AI hits a wall = dead
        if (col.gameObject.tag == "Wall")
        {
            randhit = randint;
            Boto.SetActive(false);
            collided = true;
        }
        //If AI reaches the exit = Win = Finish
        else if (col.gameObject.tag == "Final")
        {
            min1.winner = whatnumami;
            min1.winrandhit = randint;
            exit = true;
        }
        //If AI triggers a food tile then it gains a fitness point, 
        //If its a triggered food tile from before then it loses a fitness point. 
        else if (col.gameObject.tag == "FoodTag")
        {
            GameObject miep = col.gameObject;
            int idam = System.Array.IndexOf(FoodItems, miep);
            if (col.gameObject == FoodItems[idam])
            {
                if (foodies[idam].foodcheckarray[whatnumami] == false)
                {
                    foodies[idam].foodcheckarray[whatnumami] = true;
                    fitness++;
                }
                else
                {
                    //fitness -= 10;
                    fitness--;
                }
            }
        }
    }

    //Finds all the food gameobjects and put into an array and its foodscript
    void TrackFood()
    {
        FoodItems = GameObject.FindGameObjectsWithTag("FoodTag");
        for (int i = 0; i < foodies.Length; i++)
        {
            foodies[i] = FoodItems[i].GetComponent<FoodScript2>();
        }
    }

    //Moves the array and keeps track of what iteration in the arrya and kills the AI if it goes past the array length
    void MoveArray()
    {
        if (randint == randarray.Length)
        {
            Boto.SetActive(false);
            collided = true;
            randhit = randint;
        }
        else
        {
            int ran1 = randarray[randint];
            Movement(ran1);
            randint++;
        }
    }

    //Movement AI 1 - down, 2 - up, 3 - left, 4 - right
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
