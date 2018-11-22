using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Uses the car movement, rotation + front raycast
public class Mini2 : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    public int generation = 1;
    public GameObject Bot, Off, Parent;
    public GameObject[] BotArray = new GameObject[100];
    public GameObject[] ParentArray = new GameObject[50];
    public GameObject[] OffArray = new GameObject[50];

    AImovment1[] AImove = new AImovment1[100];
    ParentScripo[] parents = new ParentScripo[50];
    OffspringScript[] offspring = new OffspringScript[50];

    public bool allDead = false;
    [SerializeField]
    private int[] FitArray = new int[100];
    [SerializeField]
    private int[] OffFitArray = new int[50];
    [SerializeField]
    private int[] ParentFitArray = new int[50];
    private bool[] endArray = new bool[100];

    void Start()
    {
        Spawn();
        SpawnCopy();
        FillArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Exitcheck() == true)
        {
            Debug.Log("Exit finished");
        }
        //if all AI's are dead then new generation
        else if (allDead == true)
        {
            FitnessFill();
            EraseCopies();
            SpawnCopy();
            generation++;
            Selection();
            Recombine();
            Mutation();
            EraseAI();
            Spawn();
            AddOffspring();
        }
        else
        {
            FitnessFill();
            CheckCollide();
        }
    }

    //Adds the new offsprings to the AI group
    void AddOffspring()
    {
        int j = 0;
        int i = 0;
        while (j < 100)
        {
            parents[i].parentarray.CopyTo(AImove[j].randarray, 0);
            j++;
            offspring[i].offspringarray.CopyTo(AImove[j].randarray, 0);
            i++;
            j++;
        }
    }

    //Tournament Selection, send to parents
    void Selection()
    {
        //find 50 possible parents
        int a = 0;

        //Find 50 parents 
        while (a != 50)
        {
            int holder1 = 0;
            int holder2 = 0;
            int i = 0;
            //put x random numbers into the parentoarray
            while (i != 1)
            {
                //its alright we ignore the fact about duplicates, think clones
                holder1 = RandomInt(0, 100);
                holder2 = RandomInt(0, 100);
                if (holder1 == holder2)
                {
                    i++;
                }
            }
            if (AImove[holder1].fitness < AImove[holder2].fitness)
            {
                AImove[holder2].randarray.CopyTo(parents[a].parentarray, 0);
                ParentFitArray[a] = AImove[holder2].fitness;
            }
            else
            {
                AImove[holder1].randarray.CopyTo(parents[a].parentarray, 0);
                ParentFitArray[a] = AImove[holder1].fitness;
            }
            a++;
        }
    }

    //Gets the 2 first geneos and randomly chooses to copulate with either the 
    //2 fittest genes or two random genes, however the gene is randomly choosen
    //Recombine parents into offsprings
    void Recombine()
    {
        //int[] temparray = new int[235];
        //Transfer parents to offspring, plus the fitness
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i].parentarray.CopyTo(offspring[i].offspringarray, 0);
            OffFitArray[i] = ParentFitArray[i];
        }

        int off1 = OffFitArray[0] + 1;
        int off2 = OffFitArray[1] + 1;
        int offer1 = offspring[0].offspringarray[off1];
        int offer2 = offspring[1].offspringarray[off2];

        for (int i = off1 + 1; i < 235; i++)
        {
            offspring[0].offspringarray[i] = offer2;
        }
        for (int i = off2 + 1; i < 235; i++)
        {
            offspring[1].offspringarray[i] = offer1;
        }

        //if (OffFitArray[0] < OffFitArray[1])
        //{
        //    System.Array.Copy(offspring[1].offspringarray, 0, temparray, 0, OffFitArray[1]);
        //    System.Array.Copy(offspring[0].offspringarray, 0, offspring[1].offspringarray, 0, OffFitArray[1]);
        //    System.Array.Copy(temparray, 0, offspring[0].offspringarray, 0, OffFitArray[1]);
        //}
        //else
        //{
        //    System.Array.Copy(offspring[0].offspringarray, 0, temparray, 0, OffFitArray[0]);
        //    System.Array.Copy(offspring[1].offspringarray, 0, offspring[0].offspringarray, 0, OffFitArray[0]);
        //    System.Array.Copy(temparray, 0, offspring[1].offspringarray, 0, OffFitArray[0]);
        //}
    }

    //Scramble mutation kinda: page 69
    //Randomly changes the value in prefilled path of the array
    //Changes only the offsprings
    void Mutation()
    {
        //Keep this if only doing mutation
        //for (int i = 0; i < parents.Length; i++)
        //{
        //    parents[i].parentarray.CopyTo(offspring[i].offspringarray, 0);
        //    OffFitArray[i] = ParentFitArray[i];
        //}

        for (int i = 0; i < offspring.Length; i++)
        {
            int ran1 = RandomInt(1, 5);
            int a = OffFitArray[i] + 1;
            for (; a < 235; a++)
            {
                offspring[i].offspringarray[a] = ran1;
            }
        }
    }

    //Instantiates or creates the 10 new AI clones
    void Spawn()
    {
        for (int i = 0; i < BotArray.Length; i++)
        {
            //GameObject go gets the output insantiate which is the type of gameobject and then assign it to AImove
            BotArray[i] = Instantiate(Bot, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
            AImove[i] = BotArray[i].GetComponent<AImovment1>();
            allDead = false;
        }
    }

    void SpawnCopy()
    {
        for (int i = 0; i < OffArray.Length; i++)
        {
            OffArray[i] = Instantiate(Off, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
            offspring[i] = OffArray[i].GetComponent<OffspringScript>();
        }
        for (int b = 0; b < ParentArray.Length; b++)
        {
            ParentArray[b] = Instantiate(Parent, new Vector2(-6 + (b * 0.001f), 4.3f), transform.rotation);
            parents[b] = ParentArray[b].GetComponent<ParentScripo>();
        }
    }

    //Fills an entire array with only 1 movement direction
    void FillArray()
    {
        for (int i = 0; i < AImove.Length; i++)
        {
            int ran1 = RandomInt(1, 5);
            for (int a = 0; a < 235; a++)
            {
                AImove[i].randarray[a] = ran1;
            }
        }
    }

    //Function to randomly choose between min and max, return int+
    static int RandomInt(int min, int max)
    {
        return rand.Next(min, max);
    }

    //Function to randomly choose between min and max, return double
    static double RandomDoublet(double min, double max)
    {
        return rand.NextDouble() * (max - min) + min;
    }

    //Erases the AI after every generation
    void EraseAI()
    {
        for (int i = 0; i < BotArray.Length; i++)
        {
            Destroy(BotArray[i]);
        }
    }

    void EraseCopies()
    {
        for (int i = 0; i < OffArray.Length; i++)
        {
            Destroy(OffArray[i]);
        }
        for (int i = 0; i < ParentArray.Length; i++)
        {
            Destroy(ParentArray[i]);
        }
    }

    //Checks if the AI touches the exit area, returns t or f
    bool Exitcheck()
    {
        for (int i = 0; i < AImove.Length; i++)
        {
            if (AImove[i].exit == true)
            {
                return true;
            }
        }
        return false;
    }

    //Fills in all the fitness and dead AI to checklist
    void FitnessFill()
    {
        for (int i = 0; i < AImove.Length; i++)
        {
            FitArray[i] = AImove[i].fitness;
            endArray[i] = AImove[i].collided;
        }
    }

    //Check to see if all AI's are dead
    void CheckCollide()
    {
        foreach (bool b in endArray)
        {
            if (b)
            {
                allDead = true;
            }
            else
            {
                allDead = false;
                break;
            }
        }
    }

}
