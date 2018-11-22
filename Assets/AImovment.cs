using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

//Script to move the multiple AI's
public class AImovment : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    private GameObject Boto; 
    private Rigidbody2D rd;
    public float movespeed, timecheck, raycastlength, raymovenum = 0.0f;
    public bool exit = false;
    public int unfitness;
    private float nextCheck = 0.0f;
    public GameObject Checks;
    public GameObject[] checkArray = new GameObject[10];
    [SerializeField]
    private int checkNum = 0;

    //A constructor to intialize and get components from the clone AI's
    public void BotConstructor()
    {
        Boto = GameObject.Find("AI(Clone)");
        Boto.name = "AI";
        rd = Boto.GetComponent<Rigidbody2D>();
        unfitness = 0;
    }

    // Use this for initialization
    void Start ()
    {
        BotConstructor();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Allraychecks();
        int ran1 = RandomInt(0, 4);
        Movement(ran1);
    }

    //If the AI enters a checkpoint the fitness goes down
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Checkpoint")
        {
            unfitness--;
        }
    }

    //If the AI is in a checkpoint then the checkpoint gets added to the array
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Checkpoint")
        {
            //Adds a checkpoint every x seconds
            if (Time.time > nextCheck)
            {
                if (checkNum == checkArray.Length)
                {
                    checkNum = 0;
                    checkArray[checkNum] = col.gameObject;
                }
                else
                {
                    checkArray[checkNum] = col.gameObject;
                    checkNum++;
                }
                nextCheck += timecheck;
            }
        }
    }

    //If the AI exits a checkkpoint
    void OnTriggerExit2D(Collider2D col)
    {

    }

    //If collision is detected with a wall the fitness goes down
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Wall")
        {
            unfitness++;
        }
        else if (col.gameObject.tag == "Final")
        {
            exit = true;
            unfitness = unfitness - 100;
        }
    }

    //Function to randomly choose between min and max, return int
    static int RandomInt(int min, int max)
    {
        return rand.Next(min, max);
    }

    //Checks to see if an object is around the AI, if so move in opposite direction
    void RaycheckUp()
    {
        RaycastHit2D fwdhit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), raycastlength, 1 << 10);
        if (fwdhit.collider != null)
        {
            Vector2 movement = new Vector2(0, -raymovenum);
            rd.AddForce(movement);
        }
    }

    void RaycheckDown()
    {
        RaycastHit2D bckhit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), raycastlength, 1 << 10);
        if (bckhit.collider)
        {
            Vector2 movement = new Vector2(0, raymovenum);
            rd.AddForce(movement);
        }
    }

    void RaycheckRight()
    {
        RaycastHit2D righit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), raycastlength, 1 << 10);
        if (righit.collider)
        {
            Vector2 movement = new Vector2(-raymovenum, 0);
            rd.AddForce(movement);
        }
    }

    void RaycheckLeft()
    {
        RaycastHit2D lefhit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), raycastlength, 1 << 10);
        if (lefhit.collider)
        {
            Vector2 movement = new Vector2(raymovenum, 0);
            rd.AddForce(movement);
        }
    }

    void Allraychecks()
    {
        RaycheckUp();
        RaycheckDown();
        RaycheckLeft();
        RaycheckRight();
    }

    //Controls to move the AI around N,S,W,E
    void Movement(int num)
    {
        if (0 == num)
        {
            Vector2 movement = new Vector2(movespeed, 0);
            rd.AddForce(movement);
        }
        else if (1 == num)
        {
            Vector2 movement = new Vector2(-movespeed, 0);
            rd.AddForce(movement);
        }
        else if (2 == num)
        {
            Vector2 movement = new Vector2(0, movespeed);
            rd.AddForce(movement);
        }
        else if (3 == num)
        {
            Vector2 movement = new Vector2(0, -movespeed);
            rd.AddForce(movement);
        }
    }

}
