using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Name: Na, Jonathan For: ICS 674 Professor: Lee Altenberg Project: Final Maze Project Class: ParentScripo.cs
    The purpose of this class is to be a place holder for the Parents.
    This current class is built for both the small and large food size tiles. */
public class ParentScripo : MonoBehaviour {

    private GameObject Boto;
    public int fitness;
    public int[] parentarray = new int[400];

    // Use this for initialization
    void Start ()
    {
        BotConstructor();
    }

    //A constructor to intialize and get components from the Parents
    void BotConstructor()
    {
        Boto = GameObject.Find("ParentBot(Clone)");
        Boto.name = "ParentBot";
        fitness = 0;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
