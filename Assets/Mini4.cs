using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


/* Name: Na, Jonathan For: ICS 674 Professor: Lee Altenberg Project: Final Maze Project Class: Mini1.cs
    The purpose of this class is to gather all the necessary data to perform the Evolutionary
    Compuation cycle: Selection, Recombine, Mutation, Repeat. This current class is built for
    the larger food size tiles. */
public class Mini4 : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    public int generation = 1;
    public int botnum, parentnum, offspringnum, ansnum, winner, winrandhit = 0;

    public GameObject Bot, Off, Parent;
    public GameObject[] BotArray, ParentArray, OffArray;

    AImovment4[] AImove;
    ParentScripo[] parents;
    OffspringScript[] offspring;

    public bool doney, allDead = false;
    [SerializeField]
    private int[] FitArray, OffFitArray, ParentFitArray, RandHitArray, ParentHitArray, OffHitArray;
    private bool[] endArray;
    private float timer;
    [SerializeField]
    private float finalTime = 0.0f;
    [SerializeField]
    List<int> FitList, CountList, ParList = new List<int>();
    private int countthis = 0;

    private List<int>[] holdmyhand = new List<int>[100];

    void Start()
    {
        Setup();
        Spawn();
        SpawnCopy();
        FillArray();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //If a AI reaches the exit or generation counter hits 1000 amount then program ends
        if (Exitcheck() == true || generation == 1000)
        {
            finalTime = timer;
            WriteText();
            PressPause();
        }
        //if all AI's are dead then new generation
        else if (allDead == true)
        {
            FitnessFill();
            AverageFitness();
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

    //Calculates the average of all the fitness every generation
    void AverageFitness()
    {
        countthis++;
        int fitsum = FitArray.Sum();
        int parsum = ParentFitArray.Sum();
        for (int i = 0; i < holdmyhand.Length; i++)
        {
            holdmyhand[i].Add(AImove[i].fitness);
        }

        int fitavg = fitsum / botnum;
        int paravg = parsum / parentnum;

        FitList.Add(fitavg);
        CountList.Add(countthis);
        ParList.Add(paravg);
    }

    //Data is written to text files in A:\Document\ICS 674\Data <==== CHANGE LOCATION IN THIS FUNCTION TO WHERE YOU WANT THE DATA TO BE SENT
    void WriteText()
    {
        List<int> Finalist = new List<int>();
        Finalist = holdmyhand[winner].ToList();
        string line = string.Join(",", FitList.Select(n => n.ToString()).ToArray());
        string line2 = string.Join(",", CountList.Select(n => n.ToString()).ToArray());
        string line3 = string.Join(",", ParList.Select(n => n.ToString()).ToArray());
        string line4 = string.Join(",", Finalist.Select(n => n.ToString()).ToArray());
        System.IO.StreamWriter file1 = new System.IO.StreamWriter("A:/Document/ICS 674/Data/datafit4.txt");
        file1.WriteLine("Mini4");
        file1.WriteLine("Generation: " + generation);
        file1.WriteLine("Final time: " + (finalTime / 60));
        file1.WriteLine("Winner final array: " + winrandhit + " final fitness: " + AImove[winner].fitness);
        file1.WriteLine("Average total fitness: " + FitList.Count);
        file1.WriteLine("Average fitness Array: " + line);
        file1.WriteLine("Count total: " + CountList.Count);
        file1.WriteLine("Count Array: " + line2);
        file1.WriteLine("Average total parents: " + ParList.Count);
        file1.WriteLine("Parent fitness Array: " + line3);
        file1.WriteLine("Finalist final fitness: " + AImove[winner].fitness);
        file1.WriteLine("Finallist: " + line4);
        for (int i = 0; i < holdmyhand.Length; i++)
        {
            List<int> tempalist = new List<int>();
            tempalist = holdmyhand[i].ToList();
            string liner = string.Join(",", tempalist.Select(n => n.ToString()).ToArray());
            file1.WriteLine("Array fitness lists " + i + ": " + liner);
        }
        file1.Close();
    }

    //Tournament Selection, pits 1 vs 1 random selected parents, send to parents, parents clone to offsprings
    void Selection()
    {
        //tempselect holds the winners to give the poor fuckers a chance
        int[] tempselect = new int[50];
        for (int a = 0; a < parentnum; a++)
        {
            int holder1 = 0;
            int holder2 = 0;
            int i = 0;
            //Select 2 random AI's from the list, while they are not the same or used previously
            while (i != 1)
            {
                holder1 = RandomInt(0, botnum);
                holder2 = RandomInt(0, botnum);
                //Note Array.IndexOf returns -1 if element can't be found in the array
                int nam1 = System.Array.IndexOf(tempselect, holder1);
                int nam2 = System.Array.IndexOf(tempselect, holder2);
                //If the AI is not the same and not been used then the two random AI's proceed
                if (holder1 != holder2 && nam1 == -1 && nam2 == -1)
                {
                    i++;
                }
            }

            //Compares the two random AI's
            if (AImove[holder1].fitness < AImove[holder2].fitness)
            {
                AImove[holder2].randarray.CopyTo(parents[a].parentarray, 0);
                ParentFitArray[a] = FitArray[holder2];
                ParentHitArray[a] = RandHitArray[holder2];
                tempselect[a] = holder2;
            }
            else
            {
                AImove[holder1].randarray.CopyTo(parents[a].parentarray, 0);
                ParentFitArray[a] = FitArray[holder1];
                ParentHitArray[a] = RandHitArray[holder1];
                tempselect[a] = holder1;
            }
        }

        //Transfer parents to offspring, plus the fitness
        //Technically cloning the parents atm for crossover
        for (int k = 0; k < parentnum; k++)
        {
            parents[k].parentarray.CopyTo(offspring[k].offspringarray, 0);
            OffFitArray[k] = ParentFitArray[k];
            OffHitArray[k] = ParentHitArray[k];
        }
    }

    //Uses single-point crossover based off a random crossover point, pits both AI's by sets of twos
    void Recombine()
    {
        int k = 0;
        int j = (offspringnum / 2);

        while (j != offspringnum)
        {
            int ran1 = RandomInt(0, ansnum);
            offspring[j].offspringarray.CopyTo(offspring[j].offspringtemp, 0);
            System.Array.Copy(offspring[k].offspringarray, ran1, offspring[j].offspringarray, ran1, (ansnum - ran1));
            System.Array.Copy(offspring[j].offspringtemp, ran1, offspring[k].offspringarray, ran1, (ansnum - ran1));
            k++;
            j++;
        }
    }

    //Randomly changes the value in prefilled path of the array for each parent and offspring
    void Mutation()
    {
        double ran6 = RandomDoublet(0.0, 1.0);
        double ran2 = RandomDoublet(0.0, 1.0);
        if (ran6 < 0.8)
        {
            //80% chance for offspring to mutate, using large flip bit mutation
            //Mutation using bit flip whole mutation of array from last death location
            //Changes rest of the array from before it hits a wall
            for (int i = 0; i < offspringnum; i++)
            {
                int ran8 = RandomInt(0, 5);
                int a = OffHitArray[i];
                if (a > 5)
                {
                    a -= ran8;
                }
                int h = 0;
                while (h != 1)
                {
                    int ran1 = RandomInt(1, 5);
                    int offtest = offspring[i].offspringarray[a];
                    int checker = MoveCheck(offtest);
                    if (ran1 != offtest && ran1 != checker)
                    {
                        for (; a < ansnum; a++)
                        {
                            offspring[i].offspringarray[a] = ran1;
                        }
                        h++;
                    }
                }
            }
        }
        else
        {
            //20% chance for offspring to use flip mutation but only with 1-10 indexes in the array
            for (int i = 0; i < offspringnum; i++)
            {
                int a = OffHitArray[i];
                int ran5 = RandomInt(1, 10);
                int b = 0;
                if (a > 0 && a < 390)
                {
                    b = a + ran5;
                }
                int h = 0;
                while (h != 1)
                {
                    int ran1 = RandomInt(1, 5);
                    int offtest = offspring[i].offspringarray[a];
                    int checker = MoveCheck(offtest);
                    if (ran1 != offtest && ran1 != checker)
                    {
                        for (; a < b; a++)
                        {
                            offspring[i].offspringarray[a] = ran1;
                        }
                        h++;
                    }
                }
            }
        }

        if (ran2 < 0.8)
        {
            //80% chance for parent to mutate, using large flip bit mutation
            //Mutation using bit flip whole mutation of array from last death location
            //Changes rest of the array from before it hits a wall
            for (int i = 0; i < parentnum; i++)
            {
                int a = ParentHitArray[i];
                int ran8 = RandomInt(0, 5);
                if (a > 5)
                {
                    a -= ran8;
                }
                int h = 0;
                while (h != 1)
                {
                    int ran1 = RandomInt(1, 5);
                    int offtest = parents[i].parentarray[a];
                    int checker = MoveCheck(offtest);
                    if (ran1 != offtest && ran1 != checker)
                    {
                        for (; a < ansnum; a++)
                        {
                            parents[i].parentarray[a] = ran1;
                        }
                        h++;
                    }
                }
            }
        }
        else
        {
            //20% chance for offspring to use flip mutation but only with 1-10 indexes in the array
            for (int i = 0; i < parentnum; i++)
            {
                int a = ParentHitArray[i];
                int ran5 = RandomInt(1, 10);
                int b = 0;
                if (a > 0 && a < 390)
                {
                    b = a + ran5;
                }
                int h = 0;
                while (h != 1)
                {
                    int ran1 = RandomInt(1, 5);
                    int offtest = parents[i].parentarray[a];
                    int checker = MoveCheck(offtest);
                    if (ran1 != offtest && ran1 != checker)
                    {
                        for (; a < b; a++)
                        {
                            parents[i].parentarray[a] = ran1;
                        }
                        h++;
                    }
                }
            }
        }
    }

    //Adds the new offsprings and alpha parents to the AI group, Also tells the Food gameobjects that the AI's have been reset
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

        doney = true; //Tells the Food gameobjects that the AI's has been reset
    }

    //Instantiates or creates new AI gameobjects
    void Spawn()
    {
        for (int i = 0; i < botnum; i++)
        {
            //GameObject go gets the output insantiate which is the type of gameobject and then assign it to AImove
            BotArray[i] = Instantiate(Bot, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
            AImove[i] = BotArray[i].GetComponent<AImovment4>();
            allDead = false;
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

    //Instantiates how many AI's, Parents and Offsprings there are from the user input
    void Setup()
    {
        BotArray = new GameObject[botnum];
        ParentArray = new GameObject[parentnum];
        OffArray = new GameObject[offspringnum];

        AImove = new AImovment4[botnum];
        parents = new ParentScripo[parentnum];
        offspring = new OffspringScript[offspringnum];

        FitArray = new int[botnum];
        RandHitArray = new int[botnum];
        ParentFitArray = new int[parentnum];
        ParentHitArray = new int[parentnum];
        OffFitArray = new int[offspringnum];
        OffHitArray = new int[offspringnum];

        endArray = new bool[botnum];

        for (int i = 0; i < holdmyhand.Length; i++)
        {
            List<int> holdy = new List<int>();
            holdmyhand[i] = holdy;
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

    //Checks if the AI touches the exit area, returns t or f, if true then the whole program ends
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

    //Fills in all the fitness, last known array movement and dead AI to checklist
    void FitnessFill()
    {
        doney = false; //Lets the Food know that the AI is still alive
        for (int i = 0; i < botnum; i++)
        {
            FitArray[i] = AImove[i].fitness;
            endArray[i] = AImove[i].collided;
            RandHitArray[i] = AImove[i].randhit;
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

    //Prevents the array from moving in the opposite direciton
    int MoveCheck(int checky)
    {
        if (checky == 1)
        {
            return 2;
        }
        else if (checky == 2)
        {
            return 1;
        }
        else if (checky == 3)
        {
            return 4;
        }
        else //(checky == 4)
        {
            return 3;
        }
    }

    //Temporary pauses unity to collect data, freezes the AI's
    void PressPause()
    {
        for (int i = 0; i < botnum; i++)
        {
            BotArray[i].SetActive(false);
        }
        Debug.Break();
    }

}
