using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffspringScript : MonoBehaviour {

    private GameObject Boto;
    public int fitness;
    public int[] offspringarray = new int[400];
    public int[] offspringtemp = new int[400];

    // Use this for initialization
    void Start ()
    {
        BotConstructor();
    }

    void BotConstructor()
    {
        Boto = GameObject.Find("OffspringAI(Clone)");
        Boto.name = "OffspringAI";
        fitness = 0;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
