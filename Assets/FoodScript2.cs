using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript2 : MonoBehaviour
{
    /* Name: Na, Jonathan For: ICS 674 Professor: Lee Altenberg Project: Final Maze Project Class: FoodScript2.cs
    The purpose of this class is to gather data from the AI's of each cycle and determine if they have triggered the 
    food gameobject at any point during the AI's lifespan.
    This current class is built for both the small food size tiles. */
    private GameObject Foodie, Spawnie;
    public bool[] foodcheckarray = new bool[100];
    private Mini4 min1;
    private GameObject[] AIArray = new GameObject[100];
    private AImovment4[] AImoveArray = new AImovment4[100];

    //A constructor to intialize and get components to create the Food gameobject
    void FoodConstructor()
    {
        //Finds the spawn gameobject
        Spawnie = GameObject.Find("Spawn");
        min1 = Spawnie.GetComponent<Mini4>();

        //Finds the current Food gameobject and sets the arrays to false
        Foodie = GameObject.Find("Food");
        for (int i = 0; i < foodcheckarray.Length; i++)
        {
            foodcheckarray[i] = false;
        }
    }

    //Checks the Mini script to see if the AI's have been reset
    void AIcheck()
    {
        if (min1.doney == true)
        {
            AIArray = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < AIArray.Length; i++)
            {
                AImoveArray[i] = AIArray[i].GetComponent<AImovment4>();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        FoodConstructor();
    }

    // Update is called once per frame
    void Update()
    {
        Checkdead();
        AIcheck();
    }

    //Checks if all the AI's are dead if so then all the foodcheckarrays become false again
    void Checkdead()
    {
        if (min1.allDead == true)
        {
            for (int i = 0; i < foodcheckarray.Length; i++)
            {
                foodcheckarray[i] = false;
            }
        }
    }

}
