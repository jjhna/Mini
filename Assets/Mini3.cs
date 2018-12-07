using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Uses the car movement, rotation + front raycast
public class Mini3 : MonoBehaviour
{
    public static System.Random rand = new System.Random();
    public int generation = 1;
    public int botnum = 0;
    public int parentnum = 0;
    public int offspringnum = 0;
    private int ansnum = 235;
    public GameObject Bot, Off, Parent;
    public GameObject[] BotArray;
    public GameObject[] ParentArray;
    public GameObject[] OffArray;

    AImovment3[] AImove;
    ParentScripo2[] parents;
    OffspringScript2[] offspring;

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
            AImove[i] = BotArray[i].GetComponent<AImovment3>();
            allDead = false;
        }
    }

    //Creates new Offspring and Parent gameobjects
    void SpawnCopy()
    {
        for (int i = 0; i < offspringnum; i++)
        {
            OffArray[i] = Instantiate(Off, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
            offspring[i] = OffArray[i].GetComponent<OffspringScript2>();
        }
        for (int b = 0; b < parentnum; b++)
        {
            ParentArray[b] = Instantiate(Parent, new Vector2(-6 + (b * 0.001f), 4.3f), transform.rotation);
            parents[b] = ParentArray[b].GetComponent<ParentScripo2>();
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

        AImove = new AImovment3[botnum];
        parents = new ParentScripo2[parentnum];
        offspring = new OffspringScript2[offspringnum];

        FitArray = new int[botnum];
        ParentFitArray = new int[parentnum];
        OffFitArray = new int[offspringnum];

        endArray = new bool[botnum];
    }

    ////Stochastic universal sampling, wheel selection with 2 fixed points
    //int SelectionWheel()
    //{
    //    int sum = FitArray.Sum();
    //    int n = offspring.Length;
    //    int p = (sum / n);
    //    int start = RandomInt(0, p);
    //    int point = 0;
    //    for (int i = 0; i < n; i++)
    //    {
    //        point = (start + (i * p));
    //    }
    //    return point;
    //}

    //int[] ReturnSelectionWheel(int point)
    //{
    //    int[] keep = new int[50];
    //    for (int p = 0; p < point; p++)
    //    {
    //        int i = 0;
    //        //int sumy = FitArray.Skip(i).Sum();
    //        int sumy = 0;
    //        for (int d = 0; d < i; d++)
    //        {
    //            sumy += FitArray[d];
    //        }
    //        while (sumy < p)
    //        {
    //            i++;
    //        }
    //        keep[p] = FitArray[i];
    //    }
    //    return keep;
    //}

    //void FinalSelection()
    //{
    //    int final = SelectionWheel();
    //    int[] smat = ReturnSelectionWheel(final);
    //    for (int i = 0; i < smat.Length; i++)
    //    {

    //    }
    //}

    ////Randomly changes the value in prefilled path of the array for each parent and offspring
    //void Mutation()
    //{
    //    double ran6 = RandomDoublet(0.0, 1.0);
    //    double ran2 = RandomDoublet(0.0, 1.0);
    //    if (ran6 < 0.8)
    //    {
    //        //80% chance for offspring to mutate, using large flip bit mutation
    //        //Mutation using bit flip whole mutation of array from last death location
    //        //Changes rest of the array from before it hits a wall
    //        for (int i = 0; i < offspringnum; i++)
    //        {
    //            int ran8 = RandomInt(1, 5);
    //            int a = OffHitArray[i];
    //            if (a > 5)
    //            {
    //                a = a - ran8;
    //            }
    //            int h = 0;
    //            while (h != 1)
    //            {
    //                int ran1 = RandomInt(1, 5);
    //                int offtest = offspring[i].offspringarray[a];
    //                int checker = MoveCheck(offtest);
    //                if (ran1 != offtest && ran1 != checker)
    //                {
    //                    for (; a < ansnum; a++)
    //                    {
    //                        offspring[i].offspringarray[a] = ran1;
    //                    }
    //                    h++;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        //20% chance for offspring to use swap mutation with 2-10 whole arrays from a random array location
    //        for (int i = 0; i < offspringnum; i++)
    //        {
    //            int j = 0;
    //            int d = 0;
    //            int ran4 = RandomInt(0, (ansnum - 25));
    //            int ran5 = RandomInt(5, 25);
    //            int[] temp = new int[ran5];
    //            //Get all 5-10 elements and store in a temp array
    //            for (int b = ran4; b < (ran4 + ran5); b++)
    //            {
    //                temp[j] = offspring[i].offspringarray[b];
    //                j++;
    //            }
    //            //Use fisher-yates shuffle, shuffles the sequence
    //            int k = temp.Length;
    //            while (k > 1)
    //            {
    //                k--;
    //                int f = rand.Next(k + 1);
    //                int temptemp = temp[f];
    //                temp[f] = temp[k];
    //                temp[k] = temptemp;
    //            }
    //            //Now put back that shit into the array
    //            for (int b = ran4; b < (ran4 + ran5); b++)
    //            {
    //                offspring[i].offspringarray[b] = temp[d];
    //                d++;
    //            }
    //        }
    //    }

    //    if (ran2 < 0.8)
    //    {
    //        //80% chance for parent to mutate, using large flip bit mutation
    //        //Mutation using bit flip whole mutation of array from last death location
    //        //Changes rest of the array from before it hits a wall
    //        for (int i = 0; i < parentnum; i++)
    //        {
    //            int ran8 = RandomInt(1, 25);
    //            int a = ParentHitArray[i];
    //            if (a > 25)
    //            {
    //                a = a - ran8;
    //            }
    //            int h = 0;
    //            while (h != 1)
    //            {
    //                int ran1 = RandomInt(1, 5);
    //                int offtest = parents[i].parentarray[a];
    //                int checker = MoveCheck(offtest);
    //                if (ran1 != offtest && ran1 != checker)
    //                {
    //                    for (; a < ansnum; a++)
    //                    {
    //                        parents[i].parentarray[a] = ran1;
    //                    }
    //                    h++;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        //20% chance for parent to use swap mutation with 2-10 whole arrays from a random array location
    //        for (int i = 0; i < parentnum; i++)
    //        {
    //            int j = 0;
    //            int d = 0;
    //            int ran4 = RandomInt(0, (ansnum - 25));
    //            int ran5 = RandomInt(5, 25);
    //            int[] temp = new int[ran5];
    //            //Get all 5-10 elements and store in a temp array
    //            for (int b = ran4; b < (ran4 + ran5); b++)
    //            {
    //                temp[j] = parents[i].parentarray[b];
    //                j++;
    //            }
    //            //Use fisher-yates shuffle, shuffles the sequence
    //            int k = temp.Length;
    //            while (k > 1)
    //            {
    //                k--;
    //                int f = rand.Next(k + 1);
    //                int temptemp = temp[f];
    //                temp[f] = temp[k];
    //                temp[k] = temptemp;
    //            }
    //            //Now put back that shit into the array
    //            for (int b = ran4; b < (ran4 + ran5); b++)
    //            {
    //                parents[i].parentarray[b] = temp[d];
    //                d++;
    //            }
    //        }
    //    }
    //}
}
