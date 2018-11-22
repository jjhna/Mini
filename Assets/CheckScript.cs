using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckScript : MonoBehaviour
{
    GameObject Checker, AIgo;
    AImovment AImove;
    //private Collider2D box;
    [SerializeField]
    private bool grounded;
    BoxCollider2D AIcol, Checkcol;
    
    
    //A constructor to intialize and get components from the clone AI's
    public void CheckConstructor()
    {
        //Checker = GameObject.Find("Check");
        //Collider2D[] inter = Physics2D.OverlapBox(transform.position, transform.localScale, );
        //Collider2D circle = Physics2D.OverlapCircle(transform.position, 1, 1 << 8);
        //int r = LayerMask.NameToLayer("Player");
        //Checks if a collider falls within the box area, returns a Collider2D
        //box = Physics2D.OverlapBox(transform.position, transform.localScale, 90.0f, LayerMask.GetMask("mynewlayer"));
        //Checks if a collider is touching any collider on a specific layer, returns bool
        //grounded = Physics2D.IsTouchingLayers(box, LayerMask.GetMask("mynewlayer"));
    }

    // Use this for initialization
    void Start ()
    {
        //CheckConstructor();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Checkingbox();
        //ColCheck();
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.gameObject.tag == "Player")
    //    {
    //        AIgo = col.gameObject;
    //        AImove = AIgo.GetComponent<AImovment>();
    //        AImove.unfitness--;
    //    }
    //}

    //void OnTriggerStay2D(Collider2D col)
    //{

    //}

    //void OnTriggerExit2D(Collider2D col)
    //{

    //}

    //void Checkingbox()
    //{
    //    Checkcol = Checker.GetComponent<BoxCollider2D>();
    //    if (AIgo == null)
    //    {
    //        AIgo = GameObject.FindWithTag("Player");
    //        AIcol = AIgo.GetComponent<BoxCollider2D>();
    //    }
    //    if (grounded == true)
    //    {
    //        AImove.unfitness = AImove.unfitness - 5;
    //        Debug.Log("Its true box is touching");
    //    }
        //if (AIcol.bounds.Intersects(Checkcol.bounds))
        //{
        //    Debug.Log("Got bounds");
        //}
    //}

    //void ColCheck()
    //{
    //    Checkcol = Checker.GetComponent<BoxCollider2D>();
    //    //box = Physics2D.OverlapBox(transform.position, transform.localScale, 90.0f, LayerMask.GetMask("mynewlayer"));
    //    //Checks collision by specific layer
    //    AIcol = AIgo.GetComponent<BoxCollider2D>();
    //    if (AIcol.bounds.Intersects(Checkcol.bounds))
    //    {
    //        Debug.Log("Got bounds");
    //    }
    //}

}
