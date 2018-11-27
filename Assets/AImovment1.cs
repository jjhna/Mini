using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Script to move the multiple AI's
public class AImovment1 : MonoBehaviour
{
    private GameObject Boto, Spawno;
    public float movespeed = 0.0f;
    public bool exit, collided = false;
    public int fitness;
    private int whatnumami = 0;
    [SerializeField]
    private int randint;
    BoxCollider2D boxcol;
    public int randhit;
    public int[] randarray = new int[300];
    [SerializeField]
    GameObject[] FoodItems = new GameObject[100];
    [SerializeField]
    FoodScript[] foodies = new FoodScript[100];
    Mini1 min1;

    //A constructor to intialize and get components from the clone AI's
    void BotConstructor()
    {
        Spawno = GameObject.Find("Spawn");
        min1 = Spawno.GetComponent<Mini1>();
        
        Boto = GameObject.Find("AI1(Clone)");
        Boto.name = "AI";

        whatnumami = System.Array.IndexOf(min1.BotArray, Boto);

        fitness = 0;
        randint = 0;
        boxcol = Boto.AddComponent<BoxCollider2D>();
        boxcol.isTrigger = true;
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

    //void OnCollisionEnter2D(Collision2D col)
    //{
    //    if (col.gameObject.tag == "Wall")
    //    {
    //        Boto.SetActive(false);
    //        collided = true;
    //    }
    //    else if (col.gameObject.tag == "Final")
    //    {
    //        exit = true;
    //    }
    //}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            randhit = randint;
            Boto.SetActive(false);
            collided = true;
        }
        else if (col.gameObject.tag == "Final")
        {
            exit = true;
        }
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
            }
        }
    }

    //void OnTriggerExit2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "FoodTag")
    //    {
    //        GameObject miep = col.gameObject;
    //        int idam = System.Array.IndexOf(FoodItems, miep);
    //        if (col.gameObject == FoodItems[idam])
    //        {
    //            if (foodies[idam].foodcheckarray[whatnumami] == false)
    //            {
    //                foodies[idam].foodcheckarray[whatnumami] = true;
    //                fitness++;
    //            }
    //        }
    //    }
    //}

    void TrackFood()
    {
        FoodItems = GameObject.FindGameObjectsWithTag("FoodTag");
        for (int i = 0; i < foodies.Length; i++)
        {
            foodies[i] = FoodItems[i].GetComponent<FoodScript>();
        }
    }

    //Have to make AI die if over than 235
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
