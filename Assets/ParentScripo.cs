using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScripo : MonoBehaviour {

    public int fitness;
    private GameObject Boto;
    public int[] parentarray = new int[500];

    // Use this for initialization
    void Start ()
    {
        BotConstructor();
    }

    public void BotConstructor()
    {
        Boto = GameObject.Find("ParentBot(Clone)");
        Boto.name = "ParentBot";
        fitness = -1;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
