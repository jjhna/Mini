using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mini : MonoBehaviour
{

    public static System.Random rand = new System.Random();
    public int generation = 1;
    private float nextAction = 0.0f;
    public float timeperiod = 0.1f;
    public GameObject Bot;
    public GameObject[] BotArray = new GameObject[10];
    AImovment[] AImove = new AImovment[10];
    [SerializeField]
    private int[] unfitArray = new int[10];
    [SerializeField]
    private int size;
    //public int[,] allarray = new int[10, 235];

    // Use this for initialization
    //Create 10 new AI clones
    void Start ()
    {
        Spawn();
        //Testfreeze();
    }

    // Update is called once per frame
    void Update ()
    {
        //Do not erase this, however inaccurate
        for (int i = 0; i < AImove.Length; i++)
        {
            unfitArray[i] = AImove[i].unfitness;
        }
        //Execute the code every x seconds
        if (Time.time > nextAction)
        {
            nextAction += timeperiod;
            if (Exitcheck(AImove) == false)
            {
                generation++;
                //Selection();
                //Recombine();
                //Mutation();
                //Addoffspring();
                //EraseAI();
                //Spawn();
            }
            else
            {
                Quit();
            }
        }
        EscapeKey();
    }

    //Erases the AI after every generation
    void EraseAI()
    {
        for (int i = 0; i < BotArray.Length; i++)
        {
            Destroy(BotArray[i]);
        }
    }

    //Gets the size of the entire AImovement array, returns int
    int Getsize()
    {
        for (int i = 0; i < AImove.Length; i++)
        {
            //only change this variable
            size = AImove[i].checkArray.Length;
        }
        return size;
    }

    //Quits the application in a build mode
    public static void Quit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBPLAYER
                 Application.OpenURL(webplayerQuitURL);
        #else
                 Application.Quit();
        #endif
    }

    //Checks if the AI touches the exit area, returns t or f
    bool Exitcheck(AImovment[] AImov)
    {
        for (int i = 0; i < AImove.Length; i++)
        {
            if (AImov[i].exit == false)
            {
                return false;
            }
        }
        return true;
    }

    //If user presses the Escacpe key then the program quits in build mode
    void EscapeKey()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    //Instantiates or creates the 10 new AI clones
    void Spawn()
    {
        for (int i = 0; i < BotArray.Length; i++)
        {
            //GameObject go gets the output insantiate which is the type of gameobject and then assign it to AImove
            BotArray[i] = Instantiate(Bot, new Vector2(-6 + (i * 0.2f), 4.3f), transform.rotation);
            AImove[i] = BotArray[i].GetComponent<AImovment>();
        }
    }

    //Test to move the position of the AI to the center from this script.
    void Testfreeze()
    {
        AImove[0].transform.position = new Vector2(5.2f, -4.5f);
        AImove[1].transform.position = new Vector2(5.2f, -4.5f);
        AImove[2].transform.position = new Vector2(5.2f, -4.5f);
    }

    //Function to randomly choose between min and max, return int
    static int RandomInt(int min, int max)
    {
        return rand.Next(min, max);
    }

    //Function to randomly choose between min and max, return double
    static double RandomDoublet(double min, double max)
    {
        return rand.NextDouble() * (max - min) + min;
    }

    //Randomly chooses if and what AI and genes to replace with.
    //Uses the Pareto principle
    void Mutation()
    {
        GameObject temp;
        double ran1 = RandomDoublet(0.0, 1.0);
        int ran2 = RandomInt(0, Getsize());
        int ran3 = RandomInt(0, 10);
        int ran4 = RandomInt(0, Getsize());
        int ran5 = RandomInt(0, 10);
        int ran6 = RandomInt(0, Getsize());
        int fitsauce = MaxFit();
        if (ran1 < 0.2)
        {
            GameObject parent1 = AImove[fitsauce].checkArray[ran2];
            GameObject parent2 = AImove[fitsauce].checkArray[ran4];
            temp = parent1;
            parent1 = parent2;
            parent2 = temp;
        }
        else
        {
            GameObject parent1 = AImove[ran3].checkArray[ran2];
            GameObject parent2 = AImove[ran5].checkArray[ran6];
            temp = parent1;
            parent1 = parent2;
            parent2 = temp;
        }
    }

    //Gets the 2 first geneos and randomly chooses to copulate with either the 
    //2 fittest genes or two random genes, however the gene is randomly choosen
    //Also uses the Pareto principle
    void Recombine()
    {
        GameObject temp;
        int fitsauce = MaxFit();
        int fittoo = FitToo();
        for (int i = 0; i < BotArray.Length; i++)
        {
            //If the recombination rate is less than 30% then the top 2 fittest genes will swap
            double ran1 = RandomDoublet(0.0, 1.0);
            if (ran1 < 0.3)
            {
                int ran3 = RandomInt(0, Getsize());
                int ran4 = RandomInt(0, Getsize());
                //Select4 parents with the fittest and 2nd fittest genes
                GameObject parent1 = AImove[fittoo].checkArray[ran4];
                GameObject parent2 = AImove[fittoo].checkArray[ran3];
                GameObject parent3 = AImove[fitsauce].checkArray[ran4];
                GameObject parent4 = AImove[fitsauce].checkArray[ran3];
                //Swap geneos with the parents
                temp = parent1;
                parent1 = parent2;
                parent2 = temp;
                temp = parent3;
                parent3 = parent4;
                parent4 = temp;
            }
            else
            {
                int ran2 = RandomInt(0, 10);
                int ran5 = RandomInt(0, Getsize());
                int ran6 = RandomInt(0, Getsize());
                //Select 4 parents with the first gene and random gene
                GameObject parent1 = AImove[i].checkArray[ran5];
                GameObject parent2 = AImove[i].checkArray[ran6];
                GameObject parent3 = AImove[ran2].checkArray[ran5];
                GameObject parent4 = AImove[ran2].checkArray[ran6];
                //Swap geneos with the parents
                temp = parent1;
                parent1 = parent2;
                parent2 = temp;
                temp = parent3;
                parent3 = parent4;
                parent4 = temp;
            }
        }
    }

    //Replaces the weakest AI with the stronger AI
    void Addoffspring()
    {
        int weaksauce = LeastFit();
        int fitsauce = MaxFit();
        AImove[weaksauce] = AImove[fitsauce];
    }

    //Tournament selection algorithm, select 2 random AI's and compared them
    //by their fitness, the best one will be placed into the population pool
    void Selection()
    {
        int mem = 0;
        while (mem < 10)
        {
            int ran1 = RandomInt(0, 5);
            int ran2 = RandomInt(5, 10);
            int first = AImove[ran1].unfitness;
            int second = AImove[ran2].unfitness;
            if (first < second)
            {
                AImove[mem] = AImove[ran1];
            }
            else
            {
                AImove[mem] = AImove[ran2];
            }
            mem = mem + 1;
        }
    }

    //finds the weak shit AI, return int
    int LeastFit()
    {
        int temp, temptemp, index = 0;
        for (int i = 0; i < AImove.Length; i++)
        {
            temp = AImove[i].unfitness;
            temptemp = AImove[index].unfitness;
            if (temp > temptemp)
            {
                index = i;
            }
        }
        return index;
    }

    //finds number 2nd sexiest AI, return int
    int FitToo()
    {
        int temp, temptemp, temper, two = 0;
        int index = 0;
        //int fittest = MaxFit();
        for (int i = 0; i < AImove.Length; i++)
        {
            temp = AImove[i].unfitness;
            temptemp = AImove[index].unfitness;
            temper = AImove[two].unfitness;
            if (temp < temptemp)
            {
                two = index;
                index = i;
            }
            else if (temp < temper)
            {
                two = i;
            }
        }
        return two;
    }

    //finds the best fittest AI, return int
    int MaxFit()
    {
        int temp, temptemp, index = 0;
        for (int i = 0; i < AImove.Length; i++)
        {
            temp = AImove[i].unfitness;
            temptemp = AImove[index].unfitness;
            if (temp < temptemp)
            {
                index = i;
            }
        }
        return index;
    }

    //Checks if AI matches answers to give fitness level
    //void AImovecheck()
    //{
    //    for (int a = 0; a < 10; a++)
    //    {
    //        if (AImove[a].collided == false)
    //        {
    //            allarray[a, fitsize] = AImove[a].randarray[fitsize];
    //        }
    //    }
    //    //234 or 117
    //    if (fitsize == 234)
    //    {
    //        return;
    //    }
    //    else
    //    {
    //        fitsize++;
    //    }
    //}


//    using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

////Uses the car movement, rotation + front raycast
//public class Mini1 : MonoBehaviour
//{
//    public static System.Random rand = new System.Random();
//    public int generation = 1;
//    public GameObject Bot, Off, Copy, Parent;
//    public GameObject[] BotArray = new GameObject[10];
//    public GameObject[] ParentArray = new GameObject[4];
//    public GameObject[] OffArray = new GameObject[8];
//    public GameObject[] CopyArray = new GameObject[10];
//    ParentScripo[] parents = new ParentScripo[4];
//    OffspringScript[] offspring = new OffspringScript[8];
//    AImovment1[] AImove = new AImovment1[10];
//    CopycatScript[] copycat = new CopycatScript[10];
//    public bool allDead = false;
//    [SerializeField]
//    private int[] FitArray = new int[10];
//    private bool[] endArray = new bool[10];
//    [SerializeField]
//    private int strong1, strong2, strong3, strong4;

//    //Create 10 new AI clones
//    void Start()
//    {
//        Spawn();
//        SpawnCopy();
//        FillArray();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Exitcheck() == true)
//        {
//            Debug.Log("Exit finished");
//        }
//        //if all AI's are dead then new generation
//        else if (allDead == true)
//        {
//            FitnessFill();
//            EraseCopies(); //Good
//            SpawnCopy(); //Good
//            Evaluate(); //Good
//            generation++;
//            Selection(); //Eh works?
//            Recombine(); //Needs fix
//            Mutation(); //Works
//            EraseAI(); //Good
//            Spawn(); //Good
//            AddOffspring(); //Needs work
//        }
//        else
//        {
//            FitnessFill();
//            CheckCollide();
//        }
//    }

//    //Adds the new offsprings to the AI group
//    void AddOffspring()
//    {
//        for (int i = 0; i < copycat.Length; i++)
//        {
//            copycat[i].copycatarray.CopyTo(AImove[i].randarray, 0);
//        }
//    }

//    void Evaluate()
//    {
//        //copy array to copy array holder
//        for (int i = 0; i < copycat.Length; i++)
//        {
//            AImove[i].randarray.CopyTo(copycat[i].copycatarray, 0);
//        }
//    }

//    //Tournament Selection, send to parents
//    void Selection()
//    {
//        int mem = 0;
//        while (mem < 4)
//        {
//            int ran1 = RandomInt(0, 5);
//            int ran2 = RandomInt(5, 10);
//            int first = AImove[ran1].fitness;
//            int second = AImove[ran2].fitness;
//            if (first < second)
//            {
//                AImove[ran2].randarray.CopyTo(parents[mem].parentarray, 0);
//            }
//            else
//            {
//                AImove[ran1].randarray.CopyTo(parents[mem].parentarray, 0);
//            }
//            mem++;
//        }
//    }

//    //Gets the 2 first geneos and randomly chooses to copulate with either the 
//    //2 fittest genes or two random genes, however the gene is randomly choosen
//    void Recombine()
//    {
//        int[] arraytemp = new int[235];
//        int[] arraytemp2 = new int[235];
//        //Take only the strong bits before the AI goes off course
//        if (AImove[strong1].fitness > -1)
//        {
//            int s1 = AImove[strong1].fitness;
//            System.Array.Copy(AImove[strong1].randarray, offspring[0].offspringarray, s1);
//            //for (int i = 0; i < s1; i++)
//            //{
//            //    //offspring[0].offspringarray[i] = AImove[strong1].randarray[i];
//            //    arraytemp[i] = AImove[strong1].randarray[i];
//            //    offspring[0].offspringarray[i] = arraytemp[i];
//            //}
//        }
//        else
//        {
//            int ran4 = RandomInt(1, 5);
//            for (int i = 0; i < 235; i++)
//            {
//                offspring[0].offspringarray[i] = ran4;
//            }
//        }

//        if (AImove[strong2].fitness > -1)
//        {
//            int s2 = AImove[strong2].fitness;
//            System.Array.Copy(AImove[strong2].randarray, offspring[1].offspringarray, s2);
//            //for (int j = 0; j < s2; j++)
//            //{
//            //    //offspring[1].offspringarray[j] = AImove[strong2].randarray[j];
//            //    arraytemp2[j] = AImove[strong2].randarray[j];
//            //    offspring[1].offspringarray[j] = arraytemp2[j];
//            //}
//        }
//        else
//        {
//            int ran5 = RandomInt(1, 5);
//            for (int i = 0; i < 235; i++)
//            {
//                offspring[1].offspringarray[i] = ran5;
//            }
//        }
//    }

//    //Randomly changes the value in prefilled path of the array
//    //Changes only the offsprings
//    void Mutation()
//    {
//        for (int i = 0; i < copycat.Length; i++)
//        {
//            int ran1 = RandomInt(1, 5);
//            if (FitArray[i] > -1)
//            {
//                int a = FitArray[i] + 1;
//                for (; a < 235; a++)
//                {
//                    copycat[i].copycatarray[a] = ran1;
//                }
//            }
//            else
//            {
//                for (int a = 0; a < 235; a++)
//                {
//                    copycat[i].copycatarray[a] = ran1;
//                }
//            }
//        }
//    }

//    //Instantiates or creates the 10 new AI clones
//    void Spawn()
//    {
//        for (int i = 0; i < BotArray.Length; i++)
//        {
//            //GameObject go gets the output insantiate which is the type of gameobject and then assign it to AImove
//            BotArray[i] = Instantiate(Bot, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
//            AImove[i] = BotArray[i].GetComponent<AImovment1>();
//            allDead = false;
//        }
//    }

//    void SpawnCopy()
//    {
//        for (int i = 0; i < OffArray.Length; i++)
//        {
//            OffArray[i] = Instantiate(Off, new Vector2(-6 + (i * 0.001f), 4.3f), transform.rotation);
//            offspring[i] = OffArray[i].GetComponent<OffspringScript>();
//        }
//        for (int b = 0; b < CopyArray.Length; b++)
//        {
//            CopyArray[b] = Instantiate(Copy, new Vector2(-6 + (b * 0.001f), 4.3f), transform.rotation);
//            copycat[b] = CopyArray[b].GetComponent<CopycatScript>();
//        }
//        for (int b = 0; b < ParentArray.Length; b++)
//        {
//            ParentArray[b] = Instantiate(Parent, new Vector2(-6 + (b * 0.001f), 4.3f), transform.rotation);
//            parents[b] = ParentArray[b].GetComponent<ParentScripo>();
//        }
//    }

//    //Fills an entire array with only 1 movement direction
//    void FillArray()
//    {
//        for (int i = 0; i < AImove.Length; i++)
//        {
//            int ran1 = RandomInt(1, 5);
//            for (int a = 0; a < 235; a++)
//            {
//                AImove[i].randarray[a] = ran1;
//            }
//        }
//    }

//    //Function to randomly choose between min and max, return int+
//    static int RandomInt(int min, int max)
//    {
//        return rand.Next(min, max);
//    }

//    //Function to randomly choose between min and max, return double
//    static double RandomDoublet(double min, double max)
//    {
//        return rand.NextDouble() * (max - min) + min;
//    }

//    //Erases the AI after every generation
//    void EraseAI()
//    {
//        for (int i = 0; i < BotArray.Length; i++)
//        {
//            Destroy(BotArray[i]);
//        }
//    }

//    void EraseCopies()
//    {
//        for (int i = 0; i < OffArray.Length; i++)
//        {
//            Destroy(OffArray[i]);
//        }
//        for (int i = 0; i < CopyArray.Length; i++)
//        {
//            Destroy(CopyArray[i]);
//        }
//        for (int i = 0; i < ParentArray.Length; i++)
//        {
//            Destroy(ParentArray[i]);
//        }
//    }

//    //Checks if the AI touches the exit area, returns t or f
//    bool Exitcheck()
//    {
//        for (int i = 0; i < AImove.Length; i++)
//        {
//            if (AImove[i].exit == true)
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    //Fills in all the fitness and dead AI to checklist
//    void FitnessFill()
//    {
//        for (int i = 0; i < AImove.Length; i++)
//        {
//            FitArray[i] = AImove[i].fitness;
//            endArray[i] = AImove[i].collided;
//        }
//    }

//    //Check to see if all AI's are dead
//    void CheckCollide()
//    {
//        foreach (bool b in endArray)
//        {
//            if (b)
//            {
//                allDead = true;
//            }
//            else
//            {
//                allDead = false;
//                break;
//            }
//        }
//    }

//}


}
