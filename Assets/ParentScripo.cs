using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScripo : MonoBehaviour {

    private GameObject Boto;
    public int fitness;
    public int[] parentarray = new int[300];

    // Use this for initialization
    void Start ()
    {
        BotConstructor();
    }

    void BotConstructor()
    {
        Boto = GameObject.Find("ParentBot(Clone)");
        Boto.name = "ParentBot";
        fitness = -1;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
