using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffspringScript2 : MonoBehaviour
{

    public int fitness;
    private GameObject Boto;
    public int[] offspringarray = new int[235];

    // Use this for initialization
    void Start()
    {
        BotConstructor();
    }

    public void BotConstructor()
    {
        Boto = GameObject.Find("OffspringAI(COPY)(Clone)");
        Boto.name = "OffspringAI";
        fitness = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
