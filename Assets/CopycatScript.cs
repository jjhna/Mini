using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopycatScript : MonoBehaviour {

    public int fitness;
    private GameObject Boto;
    public int[] copycatarray = new int[235];

    // Use this for initialization
    void Start ()
    {
        BotConstructor();
    }

    public void BotConstructor()
    {
        Boto = GameObject.Find("Copycat(Clone)");
        Boto.name = "Copycat";
        fitness = -1;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
