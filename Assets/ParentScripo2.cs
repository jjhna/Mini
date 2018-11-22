using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentScripo2 : MonoBehaviour
{

    public int fitness;
    private GameObject Boto;
    public int[] parentarray = new int[235];

    // Use this for initialization
    void Start()
    {
        BotConstructor();
    }

    public void BotConstructor()
    {
        Boto = GameObject.Find("ParentBot(COPY)(Clone)");
        Boto.name = "ParentBot";
        fitness = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
