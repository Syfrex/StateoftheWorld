using UnityEngine;
using System.Collections;
namespace Grassie
{
    public class Grass : MonoBehaviour
    {

        public int grassstate = 0;
        public float timer = 0;
        public bool beingeaten = false;
        public bool edible = false;

        private Sprite[] sprites;

        void Start()
        {
            sprites = Resources.LoadAll<Sprite>("Sprite/minecraft");
            this.transform.GetComponent<SpriteRenderer>().sprite = sprites[76];
            timer = Random.Range(0, 60);
            edible = false;
        }

        void Update()
        {

            switch (grassstate) //Statemachine för Gräs
            {

                case 0:
                    {
                        Grassgrow0();
                        break;
                    }
                case 1:
                    {
                        Grassgrow1();

                        break;
                    }
                case 2:
                    {
                        Grassgrow2();
                        break;
                    }
                case 3:
                    {
                        Grassgrow3();
                        break;
                    }
                case 4:
                    {
                        Grassgrow4();
                        break;
                    }
                case 5:
                    {
                        Grassgrow5();
                        break;
                    }
                case 6:
                    {
                        Grassgrow6();
                        break;
                    }
                case 7:
                    {
                        Grassgrow7();
                        break;
                    }
                case 8:
                    {
                        Grassgrow8();
                        break;
                    }
                case 9:
                    {
                        Grassgrow9();
                        break;
                    }
                case 10:
                    {
                        Grassgrow10();
                        break;
                    }
                case 11:
                    {
                        Grassgrow11();
                        break;
                    }
                case 12:
                    {
                        Grassgrow12();
                        break;
                    }
            }
            if (beingeaten == false)
                timer += GameObject.Find("GameManager").GetComponent<GoScript>().grasstimer;
        }
        void Grassgrow0()
        {
            if (edible == true)
            {
                edible = false;
            }
            if (this.transform.GetComponent<SpriteRenderer>().sprite != sprites[76])
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[76];
            }

            if (timer >= 40)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[78];
                grassstate = 1;
                edible = true;
            }
        }
        void Grassgrow1()
        {
            if (timer >= 45)
            {
                edible = true;

                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[79];
                grassstate = 2;
            }
        }
        void Grassgrow2()
        {
            if (timer >= 50)
            {
                edible = true;

                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[80];
                grassstate = 3;
            }
        }
        void Grassgrow3()
        {
            if (timer >= 55)
            {
                edible = true;

                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[81];
                grassstate = 4;
            }
        }
        void Grassgrow4()
        {
            if (timer >= 60)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[82];
                grassstate = 5;
            }

        }
        void Grassgrow5()
        {
            if (timer >= 65)
            {
                edible = false;
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[83];
                grassstate = 6;
            }

        }
        void Grassgrow6()
        {
            if (timer >= 67)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[99];
                grassstate = 7;
            }

        }
        void Grassgrow7()
        {
            if (timer >= 68)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[98];

                grassstate = 8;
            }
        }
        void Grassgrow8()
        {
            if (timer >= 69)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[97];

                grassstate = 9;
            }

        }
        void Grassgrow9()
        {
            if (timer >= 70)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[96];

                grassstate = 10;
            }

        }
        void Grassgrow10()
        {
            if (timer >= 71)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[95];

                grassstate = 11;
            }

        }
        void Grassgrow11()
        {
            if (timer >= 72)
            {
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[94];

                grassstate = 12;
            }

        }
        void Grassgrow12()
        {
            if (timer >= 74)
            {
                for (int i = 0; i < GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist.Count; i++)
                {
                    if (Vector3.Distance(this.transform.position, GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].transform.position) <= 0.5f && GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i] != this.transform.gameObject)
                    {
                        if (GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].GetComponent<Grass>().grassstate == 0 && GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].GetComponent<Grass>().beingeaten == false)
                        {
                            if (Random.value < 0.4f) //40% chans att den plusar på timer på de som ännu inte börjat växa runt om!
                            {
                                GameObject.Find("GameManager").GetComponent<GoScript>().Grasslist[i].GetComponent<Grass>().timer += 20f;
                            }
                        }
                    }
                }
                this.transform.GetComponent<SpriteRenderer>().sprite = sprites[76];
                timer = Random.Range(0, 40);
                grassstate = 0;

            }

        }

    }
}
