using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffspringScript : MonoBehaviour {

    public int fitness;
    private GameObject Boto;
    public int[] offspringarray = new int[500];

    // Use this for initialization
    void Start ()
    {
        BotConstructor();
    }

    public void BotConstructor()
    {
        Boto = GameObject.Find("OffspringAI(Clone)");
        Boto.name = "OffspringAI";
        fitness = -1;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
