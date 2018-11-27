using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    private GameObject Foodie, Spawnie;
    public bool[] foodcheckarray = new bool[100];
    private bool donkie = false;
    Mini1 min1;
    [SerializeField]
    GameObject[] AIArray = new GameObject[100];
    [SerializeField]
    AImovment1[] AImoveArray = new AImovment1[100];


    //A constructor to intialize and get components from the clone AI's
    void FoodConstructor()
    {
        Spawnie = GameObject.Find("Spawn");
        min1 = Spawnie.GetComponent<Mini1>();
        
        Foodie = GameObject.Find("Food");
        for (int i = 0; i < foodcheckarray.Length; i++)
        {
            foodcheckarray[i] = false;
        }
    }

    void AIcheck()
    {
        if (min1.doney == true)
        {
            for (int i = 0; i < AIArray.Length; i++)
            {
                AIArray[i] = min1.BotArray[i];
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
