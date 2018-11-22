using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    private GameObject Foodie;

    //A constructor to intialize and get components from the clone AI's
    public void FoodConstructor()
    {
        Foodie = GameObject.Find("Food(Clone)");
        Foodie.name = "Food";
    }

    // Use this for initialization
    void Start ()
    {
        FoodConstructor();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
