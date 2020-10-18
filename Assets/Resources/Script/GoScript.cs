using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoScript : MonoBehaviour
{
    [Header("Grass")]
    public float Grasscount = 100;
    private float Weedcount = 10;

    private GameObject Grass;// = Resources.Load("Grass") as GameObject;
    [HideInInspector]
    public float grasstimer = 0;
    private float Dirtcount = 100;
    private float dirtycount = 10;
    [HideInInspector]
    public List<GameObject> Grasslist;

    private GameObject Dirt;// = Resources.Load("Grass") as GameObject;


    [Header("Sheep")]
    public int Sheepcount = 10;
    private GameObject Sheeper;// = Resources.Load("Sheep") as GameObject;
    [HideInInspector]
    public List<GameObject> Sheeplist;


    [Header("Wolf")]


    public int Wolfcount = 1;
    public float WolfSpawnTime = 5f;
    private GameObject Wolf;//= Resources.Load("Wolf") as GameObject;
    [HideInInspector]
    public List<GameObject> Wolflist;

    void Start()
    {
        grasstimer = 0;
        Grass = Resources.Load("Prefabs/Grass") as GameObject;
        Dirt = Resources.Load("Prefabs/Dirt") as GameObject;
        Sheeper = Resources.Load("Prefabs/Sheep") as GameObject;
        Wolf = Resources.Load("Prefabs/Wolf") as GameObject;
        Weedcount = Mathf.Sqrt(Grasscount);
        dirtycount = Mathf.Sqrt(Grasscount);
        for (int i = 0; i < dirtycount; i++)
        {
            for (int y = 0; y < dirtycount; y++)
            {
                GameObject Dirtie = GameObject.Instantiate(Dirt);
                Dirtie.transform.position = new Vector3(0.32f * y, 0.32f * i, 0);

            }
        }
        for (int i = 0; i < Weedcount; i++)
        {
            for (int y = 0; y < Weedcount; y++)
            {
                //GameObject.Instantiate(Grass);
                Grasslist.Add(GameObject.Instantiate(Grass));
                Grass.transform.position = new Vector3(0.32f * y, 0.32f * i, 0);
            }

        }
        for (int i = 0; i < Sheepcount; i++)
        {

            Sheeplist.Add(GameObject.Instantiate(Sheeper));
            Sheeper.transform.position = new Vector3(Random.Range(0, Weedcount * 0.32f ), Random.Range(0, Weedcount * 0.32f), 0);
        }

        InvokeRepeating("SpawnWolf", WolfSpawnTime, 5f);


    }
    void SpawnWolf()
    {
        if(Wolflist.Count< Wolfcount)
        { 
        for (int i = 0; i < Wolfcount; i++)
        {
            Wolflist.Add(GameObject.Instantiate(Wolf));

            Wolf.transform.position = new Vector3(Random.Range(0, Weedcount * 0.32f), Random.Range(0, Weedcount * 0.32f), 0);
        }
            CancelInvoke();
        }
    }
    void Update()
    {
        grasstimer = Time.deltaTime;
    }
    public void AddSheep(Vector3 startPos)
    {
        Sheeplist.Add(GameObject.Instantiate(Sheeper));
        Sheeper.transform.position = startPos;
    }
    public void AddWolf(Vector3 startPos)
    {
        Wolflist.Add(GameObject.Instantiate(Wolf));
        Wolf.transform.position = startPos;
    }
}
