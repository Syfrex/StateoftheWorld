using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Grassie;
namespace Sheepie
{
    public class Sheep : MonoBehaviour
    {

        private float Stamina = 0;
        public float WoolAmount = 1;
        private float StandardSize = 1;
        private GameObject target;
        public GameObject monster = null;
        List<GameObject> FearSheep = new List<GameObject> { };
        List<GameObject> SheepList = new List<GameObject> { };

        private bool moving = false;
        public float speed = 0.2f;
        private bool sheepiseating = false;
        private bool nograssfound = true;
        public bool RunAwayLittleGirl = false;

        public int sheepstate = 0;
        public Vector3 Destination = Vector3.zero;
        public float hungrymeater = 10;
        private bool rotated = false;
        List<GameObject> nearbygrass = new List<GameObject> { };
        public bool beingchased = false;

        void Start()
        {
            WoolAmount = 1;
            SheepList = GameObject.Find("GameManager").GetComponent<GoScript>().Sheeplist;
            InvokeRepeating("UpdateSheepList", 5f, 1f);
            InvokeRepeating("CheckforMonster", 1f, 0.2f); //Invoke repeating är en metod som man själv får bestämma hur ofta den ska uppdateras. I detta fall ska den kolla efter vargar 5 gånger per sekund.
            StandardSize = 0.6f; //hårdkodade scalen till prefaben
        }
        void UpdateSheepList()
        {
            SheepList = GameObject.Find("GameManager").GetComponent<GoScript>().Sheeplist;
        }

        void Update()
        {
            switch (sheepstate) //Statemachine för lamm
            {

                case 0:
                    {
                        SheepHungry();
                        break;
                    }
                case 1:
                    {
                        SheepFull();

                        break;
                    }
                case 2:
                    {
                        SheepEat();
                        break;
                    }
                case 3:
                    {
                        SheepScared();
                        break;
                    }
                case 4:
                    {
                        Sheepwander();
                        break;
                    }
                case 5:
                    {
                        Sheepdead();
                        break;
                    }
                case 6:
                    {
                        Run();
                        break;
                    }

            }

            gameObject.transform.localScale = new Vector3(StandardSize * WoolAmount, StandardSize * WoolAmount, 0);
            if(WoolAmount < 1.5f)
            {
            WoolAmount += (0.1f * Time.deltaTime); //Wool grow over time that affect the runspeed
            }
            hungrymeater -= 0.8f * Time.deltaTime; //mat-timer 
            if (hungrymeater < 0f) //Lammet dör om den svälter
            {
                sheepstate = 5;
            }


            if (monster != null)
            {
                if (beingchased == true && hungrymeater > 0)
                {
                    sheepstate = 3;
                }
                if (Vector3.Distance(monster.transform.position, gameObject.transform.position) > 0.5f && beingchased == false)
                {
                    monster = null;
                    moving = false;
                    sheepstate = 0;
                }
            }

            if (FearSheep.Count > 0)
            {
                for (int i = 0; i < FearSheep.Count; i++)
                {
                    if (FearSheep[i] != null)
                    {
                        if (Vector3.Distance(FearSheep[i].gameObject.transform.position, gameObject.transform.position) > 0.5) //Om dom är en bit ifrån ska dom inte springa längre
                        {
                            if (FearSheep[i].GetComponent<Sheep>().beingchased == false)
                            {
                                FearSheep[i].GetComponent<Sheep>().RunAwayLittleGirl = false;
                                FearSheep[i].GetComponent<Sheep>().NoLongerFeared();
                                FearSheep.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }

        void CheckforMonster()
        {
            for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GoScript>().Wolflist.Count; i++)
            {
                if (monster == null)
                {
                    if (Vector3.Distance(this.transform.position, GameObject.Find("GameManager").GetComponent<GoScript>().Wolflist[i].transform.position) <= 0.7f)
                    {
                        monster = GameObject.Find("GameManager").GetComponent<GoScript>().Wolflist[i].gameObject;
                        speed = 0.2f / WoolAmount;
                    }
                }
            }
        }
        void FearNearbySheep()
        {
            if (sheepstate != 5)
            {
                for (int i = 0; i < SheepList.Count; i++)
                {
                    if (Vector3.Distance(SheepList[i].gameObject.transform.position, gameObject.transform.position) < 0.7 && SheepList[i].GetComponent<Sheep>().beingchased == false) //Om lammet är inom kort och inte redan är jagad
                    {
                        FearSheep.Add(SheepList[i]);
                        SheepList[i].GetComponent<Sheep>().RunAwayInFear(this.gameObject);
                        SheepList[i].GetComponent<Sheep>().RunAwayLittleGirl = true;
                        SheepList[i].GetComponent<Sheep>().moving = true;
                        SheepList[i].GetComponent<Sheep>().sheepstate = 6;
                    }

                }
            }
        }
        public void NoLongerFeared()
        {
            moving = false;
            if (monster != null)
            {
                monster = null;
            }
            sheepstate = 0; //set back to wander
        }
        public void RunAwayInFear(GameObject SheepInFear)//Perp vector = (y,-x)
        {
            Vector3 tempDirr = SheepInFear.GetComponent<Sheep>().monster.transform.position.normalized + SheepInFear.transform.position.normalized; //vector från lamm till varg
            if (SheepInFear.GetComponent<Sheep>().monster.transform.position.y < this.transform.position.y)
            {
                Destination = new Vector3(tempDirr.y, -tempDirr.x, 0) * 10; //Ta den rätvinkliga riktningen så den springer ifrån varg och de jagade lammet           
            }
            else if (SheepInFear.GetComponent<Sheep>().monster.transform.position.y > this.transform.position.y)
            {
                Destination = new Vector3(-tempDirr.y, -tempDirr.x, 0) * 10; //Ta den rätvinkliga riktningen så den springer ifrån varg och de jagade lammet               
            }

        }
        void SheepHungry() //Case 0
        {
            if (rotated == true)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                rotated = false;
            }
            if (beingchased == true)
            {
                sheepstate = 3;
            }
            if (hungrymeater > 12 && beingchased != true && RunAwayLittleGirl != true) //Ifall lammet kommer in här fastän den inte är hungrig ska den gå vidare till att vandra istället
            {
                sheepstate = 4;
            }
            if (moving == false)
            {

                for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist.Count; i++)
                {
                    if (Vector3.Distance(this.transform.position, GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].transform.position) <= 0.5f)
                    {
                        if (GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].GetComponent<Grass>().edible == true && GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].GetComponent<Grass>().beingeaten == false)
                        {

                            Destination = GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].transform.position;
                            GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].GetComponent<Grass>().beingeaten = true;
                            target = GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i];
                            //target.gameObject.GetComponent<Grass>().beingeaten = true;
                            moving = true;
                            break;

                        }

                    }
                }
            }
            else if (Destination == Vector3.zero)
            {
                sheepstate = 4;
            }

            if (Vector3.Distance(this.transform.position, Destination) > 0.05f) //går mot målet tills den är nära nog
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, Destination, (speed / WoolAmount) * Time.deltaTime);

            }
            else //Den har nått sin destination och börjar äta i SheepEat()
            {
                sheepstate = 2;
                sheepiseating = true;
            }

        }
        void SheepFull() //Case 1
        {
            if (hungrymeater > 20) //Om den blir "supermätt" ska den spawna ett nytt lamm
            {
                GameObject.Find("GameManager").GetComponent<GoScript>().AddSheep(this.transform.position + new Vector3(0.1f, 0, 0));
                hungrymeater -= 10;
            }
            else if (hungrymeater > 15 && hungrymeater < 20 && rotated == false) //om den blir mätt men inte tillräckligt mätt lägger sig lammet på rygg och vilar
            {
                transform.localRotation = Quaternion.Euler(180, 0, 0);
                rotated = true;
            }


            if (hungrymeater < 15)
            {
                if (transform.localRotation == Quaternion.Euler(180, 0, 0))
                {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                sheepstate = 0;
                rotated = false;
            }
        }
        void SheepEat()  //Case 2
        {
            if (RunAwayLittleGirl == true)
            {
                RunAwayLittleGirl = false;
            }
            if (target == null)
            {
                sheepstate = 4;

            }
            else
            {
                if (Stamina < 5) // Skaffa stamina för sprint ifall den äter
                {
                    Stamina += 0.05f;
                }

                target.GetComponent<Grass>().timer -= 0.1f;
                hungrymeater += 0.04f;
                if (target.GetComponent<Grass>().timer < 20) //Om lammet äter ner grästet till första stadiet, gå vidare
                {
                    target.GetComponent<Grass>().grassstate = 0;
                    target.GetComponent<Grass>().timer = 0;
                    target.GetComponent<Grass>().beingeaten = false;
                    sheepiseating = false;
                    target = null;
                    moving = false;
                    sheepstate = 1;
                }
            }
        }
        void SheepScared() //Case 3
        {
            if (monster != null)
            {
                FearNearbySheep();
                if (monster.GetComponent<Wolf>().hungrymeater < 0)
                {
                    sheepstate = 0;
                }
            }
            if (rotated == true)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                rotated = false;
            }
            if (monster.transform.position.x > this.transform.position.x && monster.transform.position.y > this.transform.position.y)
            {
                Destination = new Vector3((this.transform.position.x - monster.transform.position.x), (this.transform.position.y - monster.transform.position.y));
            }
            else if (monster.transform.position.x < this.transform.position.x && monster.transform.position.y < this.transform.position.y)
            {
                Destination = new Vector3((monster.transform.position.x + this.transform.position.x), (monster.transform.position.y + this.transform.position.y));
            }
            else if (monster.transform.position.x > this.transform.position.x && monster.transform.position.y < this.transform.position.y)
            {
                Destination = new Vector3((this.transform.position.x - monster.transform.position.x), (monster.transform.position.y + this.transform.position.y));
            }
            else if (monster.transform.position.x < this.transform.position.x && monster.transform.position.y > this.transform.position.y)
            {
                Destination = new Vector3((monster.transform.position.x + this.transform.position.x), (this.transform.position.y - monster.transform.position.y));
            }
            if (Stamina > 0 && beingchased) //Ifall den samlat stamina så kan den sprinta en snabbis
            {
                speed = 0.7f;
                Stamina -= 0.03f;
            }
            else
            {
                speed = 0.2f;
            }
            this.transform.position = Vector2.MoveTowards(this.transform.position, Destination, (speed / WoolAmount) * Time.deltaTime);
        }
        void Sheepwander() //Case 4
        {
            if (RunAwayLittleGirl == true)
            {
                RunAwayLittleGirl = false;
            }
            if (moving == false)
            {

                for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist.Count; i++)
                {
                    if (Vector3.Distance(this.transform.position, GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].transform.position) <= 0.5f)
                    {
                        nearbygrass.Add(GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].gameObject);
                    }
                }
                Destination = nearbygrass[Random.Range(0, nearbygrass.Count)].transform.position;
                moving = true;
            }
            this.transform.position = Vector3.MoveTowards(this.transform.position, Destination, (speed / WoolAmount) * Time.deltaTime);
            if (Vector3.Distance(this.transform.position, Destination) < 0.2f || hungrymeater < 10)
            {
                moving = false;
                nearbygrass.Clear();
                sheepstate = 0;
            }
        }
        void Sheepdead()//Case 5
        {
            transform.localRotation = Quaternion.Euler(180, 0, 0);
            rotated = true;
            speed = 0;
            this.transform.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            GameObject.Find("GameManager").GetComponent<GoScript>().Sheeplist.Remove(this.gameObject);
            Destroy(this.gameObject, 5f);

            for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist.Count; i++)
            {
                if (target != null)
                {
                    target.GetComponent<Grass>().beingeaten = false;
                    target = null;
                }
                else if (Vector3.Distance(this.transform.position, GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].transform.position) <= 0.1f)
                {
                    GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].GetComponent<Grass>().timer += 0.01f; // om lammet dör vid gräs så växer gräset snabbare
                    if (target != null)
                    {
                        target.GetComponent<Grass>().beingeaten = false;
                        target = null;
                    }
                }
            }
            if (FearSheep.Count > 0)
            {
                for (int i = 0; i < FearSheep.Count; i++)
                {
                    if (FearSheep[i] != null)
                    {
                        FearSheep[i].GetComponent<Sheep>().RunAwayLittleGirl = false;
                        FearSheep[i].GetComponent<Sheep>().NoLongerFeared();
                        FearSheep.RemoveAt(i);
                    }
                }
            }
            sheepstate = 10; //Sätta staten till en tom så den inte gör annan logik när den ska vara död
        }
        void Run() //case 6
        {
            if (rotated == true)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                rotated = false;
            }
            this.transform.position = Vector3.MoveTowards(this.transform.position, Destination, (speed / WoolAmount) * Time.deltaTime);
            if (RunAwayLittleGirl != true)
            {
                sheepstate = 0;
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.position, 0.7f);
            Gizmos.color = Color.magenta;
            if (monster != null)
                Gizmos.DrawLine(transform.position, Destination);

            Gizmos.color = Color.red;
            if (RunAwayLittleGirl == true)
            {
                Gizmos.DrawLine(transform.position, Destination);
            }


        }
    }
}
