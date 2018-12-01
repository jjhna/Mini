using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    private GameObject Foodie, Spawnie;
    public bool[] foodcheckarray = new bool[100];
    Mini1 min1;
    GameObject[] AIArray = new GameObject[100];
    AImovment1[] AImoveArray = new AImovment1[100];

    //A constructor to intialize and get components from the clone AI's
    void FoodConstructor()
    {
        //Finds the spawn gameobject
        Spawnie = GameObject.Find("Spawn");
        min1 = Spawnie.GetComponent<Mini1>();

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
                AImoveArray[i] = AIArray[i].GetComponent<AImovment1>();
            }
        }
    }

    // Use this for initialization
    void Start ()
    {
        FoodConstructor();
    }

    // Update is called once per frame
    void Update ()
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
