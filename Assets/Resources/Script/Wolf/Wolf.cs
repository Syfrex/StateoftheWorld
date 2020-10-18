using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sheepie;
public class Wolf : MonoBehaviour
{

    private int Wolfstate = 3;
    private Vector3 Destination = Vector3.zero;
    private GameObject target;
    private bool chasing = false;
    private float speed = 0.4f;
    public float hungrymeater = 20;
    private List<GameObject> nearbySheep = new List<GameObject> { };
    private List<GameObject> nearbyGrass = new List<GameObject> { };
    private bool noSheepfound = true;
    private bool walking = false;

    void Start()
    {
        InvokeRepeating("CheckLamb", 0, 0.5f);
    }

    void Update()
    {
        switch (Wolfstate)
        {

            case 0:
                {
                    WolfHungry();
                    break;
                }
            case 1:
                {
                    WolfFull();

                    break;
                }
            case 2:
                {
                    WolfEat();
                    break;
                }
            case 3:
                {
                    Wolfwander();
                    break;
                }

        }
        hungrymeater -= 0.8f * Time.deltaTime;

        if (hungrymeater <= 0f)
        {
            transform.localRotation = Quaternion.Euler(180, 0, 0);
            speed = 0;
            this.transform.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            GameObject.Find("GameManager").GetComponent<GoScript>().Wolflist.Remove(this.gameObject);

            Destroy(this.gameObject, 5f);
        }
        //if(target != null)
        //{
        //    Destination = target.transform.position;

        //}

    }
    void CheckLamb()
    {
        if (chasing == false)
        {
            for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GoScript>().Sheeplist.Count; i++)
            {
                if (Vector3.Distance(this.transform.position, GameObject.Find("GameManager").GetComponent<GoScript>().Sheeplist[i].transform.position) <= 1.0f) //Om något lamm är nära vargen
                {
                    target = GameObject.Find("GameManager").GetComponent<GoScript>().Sheeplist[i];
                    target.GetComponent<Sheep>().beingchased = true;
                    target.GetComponent<Sheep>().monster = this.gameObject;

                    //target.GetComponent<Sheep>().beingchased = true;
                    //Destination = target.transform.position;
                    chasing = true;
                    Wolfstate = 0;
                    break; //Hittar ett lamm, stoppar loopen
                }
            }

        }
    }
    void WolfHungry() // State 0
    {

        if (Vector3.Distance(this.transform.position, target.transform.position) >= 0.05f) //går mot målet tills den är nära nog
        {
   
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else //Den har nått sin destination och börjar äta i WolfEat()
        {
            Wolfstate = 2;
        }
        if (target == null || target.GetComponent<Sheep>().sheepstate == 5)
        {
            walking = false;
            chasing = false;

            Wolfstate = 3;
        }
    }
    void WolfFull()// State 1
    {
        transform.localRotation = Quaternion.Euler(180, 0, 0);
     
        target = null;
        if (hungrymeater < 20) //Väntar tills vargen blir hungrig igen
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            chasing = false;
            walking = false;
            Wolfstate = 3;
        }
        if (hungrymeater > 45) //Om den blir "supermätt" ska den spawna ett nytt lamm
        {
            GameObject.Find("GameManager").GetComponent<GoScript>().AddWolf(this.transform.position + new Vector3(0.3f, 0, 0));
            hungrymeater -= 30;
        }
    }

    void WolfEat()// State 2
    {

        hungrymeater += 0.2f;
        target.GetComponent<Sheep>().sheepstate = 5;
        target.GetComponent<Sheep>().speed = 0;
        target.GetComponent<Sheep>().hungrymeater -= 0.1f;
        if (target.GetComponent<Sheep>().hungrymeater <= -1f)
        {
            target = null;
            Wolfstate = 1;
        }
        if(target == null)
        {
            Wolfstate = 1;
        }
    }

    void Wolfwander()// State 3
    {
        if (walking == false)
        {
            for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist.Count; i++)
            {
                if (Vector3.Distance(this.transform.position, GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].transform.position) <= 1.0f)
                {
                    nearbyGrass.Add(GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].gameObject);
                }
            }
            Destination = nearbyGrass[Random.Range(0, nearbyGrass.Count)].transform.position;
            walking = true;
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, Destination, speed * Time.deltaTime);
        if (Vector3.Distance(this.transform.position, Destination) < 0.2f)
        {
            nearbyGrass.Clear();
            walking = false;

        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1);
        Gizmos.color = Color.red;
        if(target != null)
        Gizmos.DrawLine(transform.position, target.transform.position);

    }
}

