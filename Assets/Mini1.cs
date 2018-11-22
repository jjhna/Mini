using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Uses the car movement, rotation + front raycast
public class Mini1 : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    public int generation = 1;
    public int botnum = 0;
    public int parentnum = 0;
    public int offspringnum = 0;
    public int foodnum = 0;

    public int ansnum = 500;

    public GameObject Bot, Off, Parent, Food;
    public GameObject[] BotArray;
    public GameObject[] ParentArray;
    public GameObject[] OffArray;
    public GameObject[] FoodArray;

    AImovment1[] AImove;
    ParentScripo[] parents;
    OffspringScript[] offspring;
    FoodScript[] foods;

    public bool allDead = false;
    [SerializeField]
    private int[] FitArray;
    [SerializeField]
    private int[] OffFitArray;
    [SerializeField]
    private int[] ParentFitArray;
    private bool[] endArray;

    void Start()
    {
        Setup();
        Spawn();
        SpawnCopy();
        SpawnFood();
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

    //Tournament Selection, due to negative fitness numbers, send to parents, parents clone to offsprings
    void Selection()
    {

        for (int a = 0; a < parentnum; a++)
        {
            int holder1 = 0;
            int holder2 = 0;
            int i = 0;

            //Select 2 random AI's from the list
            while (i != 1)
            {
                holder1 = RandomInt(0, botnum);
                holder2 = RandomInt(0, botnum);
                if (holder1 != holder2)
                {
                    i++;
                }
            }

            //Compares the two random AI's
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
        }

        //Transfer parents to offspring, plus the fitness
        //Technically cloning the parents atm for crossover
        for (int k = 0; k < parentnum; k++)
        {
            parents[k].parentarray.CopyTo(offspring[k].offspringarray, 0);
            OffFitArray[k] = ParentFitArray[k];
        }
    }

    //Uses single-point crossover based off a random crossover point
    //Pits both AI's by sets of twos, actually better than non-random crossover points
    void Recombine()
    {
        int k = 0;
        int j = (offspringnum / 2);

        while (j != offspringnum)
        {
            int[] temparray = new int[ansnum];
            int ran1 = RandomInt(0, ansnum);
            offspring[j].offspringarray.CopyTo(temparray, 0);
            System.Array.Copy(offspring[k].offspringarray, ran1, offspring[j].offspringarray, ran1, (ansnum - ran1));
            System.Array.Copy(temparray, ran1, offspring[k].offspringarray, ran1, (ansnum - ran1));
            k++;
            j++;
        }
    }

    //Randomly changes the value in prefilled path of the array after the fitness gets off track
    void Mutation()
    {
        //100% chance for offspring to mutate
        for (int i = 0; i < offspringnum; i++)
        {
            int a = OffFitArray[i] + 1;
            int ran1 = RandomInt(1, 5);
            for (; a < ansnum; a++)
            {
                offspring[i].offspringarray[a] = ran1;
            }
        }

        //80% chance for parents to mutate, using flip bit
        double ran2 = RandomDoublet(0.0, 1.0);
        if (ran2 < 0.8)
        {
            for (int i = 0; i < parentnum; i++)
            {
                int b = OffFitArray[i] + 1;
                int ran3 = 0;
                int j = 0;
                while (j != 1)
                {
                    ran3 = RandomInt(1, 5);
                    if (b != ran3)
                    {
                        j++;
                    }
                }
                for (; b < ansnum; b++)
                {
                    parents[i].parentarray[b] = ran3;
                }
            }
        }
        else
        {
            //Use the swap mutation
            for (int i = 0; i < parentnum; i++)
            {
                int j = 0;
                int d = 0;
                int ran4 = RandomInt(0, (ansnum - 10));
                int ran5 = RandomInt(5, 10);
                int[] temp = new int[ran5];
                //Get all 5-10 elements and store in a temp array
                for (int b = ran4; b < (ran4 + ran5); b++)
                {
                    temp[j] = parents[i].parentarray[b];
                    j++;
                }
                //Use fisher-yates shuffle
                int k = temp.Length;
                while (k > 1)
                {
                    k--;
                    int f = rand.Next(k + 1);
                    int temptemp = temp[f];
                    temp[f] = temp[k];
                    temp[k] = temptemp;
                }
                //Now put back that shit into the array
                for (int b = ran4; b < (ran4 + ran5); b++)
                {
                    parents[i].parentarray[b] = temp[d];
                    d++;
                }
            }
        }
    }

    //Adds the new offsprings to the AI group
    void AddOffspring()
    {
        int j = 0;
        int i = 0;
        while (j < botnum)
        {
            parents[i].parentarray.CopyTo(AImove[j].randarray, 0);
            j++;
            offspring[i].offspringarray.CopyTo(AImove[j].randarray, 0);
            i++;
            j++;
        }
    }

    //Instantiates or creates new AI gameobjects
    void Spawn()
    {
        for (int i = 0; i < botnum; i++)
        {
            //GameObject go gets the output insantiate which is the type of gameobject and then assign it to AImove
            BotArray[i] = Instantiate(Bot, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
            AImove[i] = BotArray[i].GetComponent<AImovment1>();
            allDead = false;
        }
    }

    void SpawnFood()
    {
        for (int i = 0; i < foodnum; i++)
        {
            FoodArray[i] = Instantiate(Food, new Vector2(-6 + i, 4.3f), transform.rotation);
            foods[i] = FoodArray[i].GetComponent<FoodScript>();
        }
    }

    //Creates new Offspring and Parent gameobjects
    void SpawnCopy()
    {
        for (int i = 0; i < offspringnum; i++)
        {
            OffArray[i] = Instantiate(Off, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
            offspring[i] = OffArray[i].GetComponent<OffspringScript>();
        }
        for (int b = 0; b < parentnum; b++)
        {
            ParentArray[b] = Instantiate(Parent, new Vector2(-6 + (b * 0.001f), 4.3f), transform.rotation);
            parents[b] = ParentArray[b].GetComponent<ParentScripo>();
        }
    }

    //Fills an entire array with only 1 movement direction, used only once in the beginning
    void FillArray()
    {
        for (int i = 0; i < botnum; i++)
        {
            int ran1 = RandomInt(1, 5);
            for (int a = 0; a < ansnum; a++)
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

    //Erases the AI's after every generation
    void EraseAI()
    {
        for (int i = 0; i < botnum; i++)
        {
            Destroy(BotArray[i]);
        }
    }

    //Erases all the parents and offspring before starting a new generation
    void EraseCopies()
    {
        for (int i = 0; i < offspringnum; i++)
        {
            Destroy(OffArray[i]);
        }
        for (int i = 0; i < parentnum; i++)
        {
            Destroy(ParentArray[i]);
        }
    }

    //Checks if the AI touches the exit area, returns t or f
    bool Exitcheck()
    {
        for (int i = 0; i < botnum; i++)
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
        for (int i = 0; i < botnum; i++)
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

    //Instantiates how many AI's, Parents and Offsprings there are
    void Setup()
    {
        BotArray = new GameObject[botnum];
        ParentArray = new GameObject[parentnum];
        OffArray = new GameObject[offspringnum];
        FoodArray = new GameObject[foodnum];

        AImove = new AImovment1[botnum];
        parents = new ParentScripo[parentnum];
        offspring = new OffspringScript[offspringnum];
        foods = new FoodScript[foodnum];

        FitArray = new int[botnum];
        ParentFitArray = new int[parentnum];
        OffFitArray = new int[offspringnum];

        endArray = new bool[botnum];
    }

}
