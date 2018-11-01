using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//Written By Angus Secomb
//Last edited 28/11/17.
public class WinActor : MonoBehaviour
{

    private Renderer kartOne, kartTwo, kartThree, kartFour;

    public Material red, blue, green, yellow;

    private GameObject kart_go, kart_go2, kart_go3, kart_go4;

    private Camera cameraOne;

    private GameObject RaceUI;

    private LapsManager lapManager;
    private PlayerSelectActor psActor;

    private Image podFirst, podSecond, podThird, podFourth;

    private Image menuButton;
    private Image menuText;

    public GameObject newManager;

    private ParticleSystem firework1, firework2;

    private AudioSource firework;

    [HideInInspector]
    public bool raceOver = false;

    // Use this for initialization
    void Start()
    {
        cameraOne = GameObject.Find("FinishCamera").GetComponent<Camera>();
        kartOne = GameObject.Find("KartFirst").GetComponentInChildren<Renderer>();
        kartTwo = GameObject.Find("KartSecond").GetComponentInChildren<Renderer>();
        kartThree = GameObject.Find("KartThird").GetComponentInChildren<Renderer>();
        kartFour = GameObject.Find("KartFourth").GetComponentInChildren<Renderer>();
        podFirst = GameObject.Find("PodiumFirst").GetComponent<Image>();
        podSecond = GameObject.Find("PodiumSecond").GetComponent<Image>();
        podThird = GameObject.Find("PodiumThird").GetComponent<Image>();
        podFourth = GameObject.Find("PodiumFourth").GetComponent<Image>();
        psActor = GetComponent<PlayerSelectActor>();
        RaceUI = GameObject.FindGameObjectWithTag("RaceUI");
        menuButton = GameObject.Find("MenuButton").GetComponent<Image>();
        menuText = GameObject.Find("MenuText").GetComponent<Image>();

        firework1 = GameObject.Find("Fireworks").GetComponentInChildren<ParticleSystem>();
        firework2 = GameObject.Find("Fireworks1").GetComponentInChildren<ParticleSystem>();
        firework = GameObject.Find("FireworkSound").GetComponent<AudioSource>();

        menuButton.enabled = false;
        menuText.enabled = false;
        podFirst.enabled = false;
        podSecond.enabled = false;
        podThird.enabled = false;
        podFourth.enabled = false;
        lapManager = GetComponent<LapsManager>();

        kart_go = GameObject.Find("Camera1");
        kart_go2 = GameObject.Find("Camera2");
        kart_go3 = GameObject.Find("Camera3");
        kart_go4 = GameObject.Find("Camera4");
    }

    // Update is called once per frame
    void Update()
    {


        kartTwo.material = blue;
        if (raceOver)
        {
            cameraOne.enabled = true;
            RaceUI.SetActive(false);
            switch (psActor.playerCount)
            {
                case 2:
                    kartThree.transform.parent.gameObject.SetActive(false);
                    kartFour.transform.parent.gameObject.SetActive(false);
                    kart_go.SetActive(false);
                    kart_go2.SetActive(false);
                    menuButton.enabled = true;
                    menuText.enabled = true;

                    if(!firework.isPlaying)
                    {
                        firework.Play();
                    }

                    if (!firework1.isPlaying)
                    {
                        firework1.Play();
                    }
                    if (!firework2.isPlaying)
                    {
                        firework2.Play();
                    }

                    if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 2))
                    {
                        kartOne.material = red;
                        kartTwo.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 1))
                    {
                        kartOne.material = blue;
                        kartTwo.material = red;
                    }

                    if (lapManager.kart1.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }
                    if (lapManager.kart2.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }

                    break;
                case 3:
    //                kartThree.transform.parent.gameObject.SetActive(false);
                    kartFour.transform.parent.gameObject.SetActive(false);
                    kart_go.SetActive(false);
                    kart_go2.SetActive(false);
                    kart_go3.SetActive(false);
                    menuButton.enabled = true;
                    menuText.enabled = true;

                    if (!firework.isPlaying)
                    {
                        firework.Play();
                    }


                    if (!firework1.isPlaying)
                    {
                        firework1.Play();
                    }
                    if (!firework2.isPlaying)
                    {
                        firework2.Play();
                    }

                    if ((lapManager.kart1.kartPosition ==1) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 3))
                    {
                        kartOne.material = red;
                        kartTwo.material = blue;
                        kartThree.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 2))
                    {
                        kartOne.material = red;
                        kartTwo.material = green;
                        kartThree.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 3))
                    {
                        kartOne.material = blue;
                        kartTwo.material = red;
                        kartThree.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 1))
                    {
                        kartOne.material = green;
                        kartTwo.material = red;
                        kartThree.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 2))
                    {
                        kartOne.material = blue;
                        kartTwo.material = green;
                        kartThree.material = red;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 1))
                    {
                        kartOne.material = green;
                        kartTwo.material = blue;
                        kartThree.material = red;
                    }

                    if (lapManager.kart1.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }
                    if (lapManager.kart2.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }
                    if (lapManager.kart3.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }

                    break;
                case 4:
                    kart_go.SetActive(false);
                    kart_go2.SetActive(false);
                    kart_go3.SetActive(false);
                    kart_go4.SetActive(false);
                    menuButton.enabled = true;
                    menuText.enabled = true;

                    if (!firework.isPlaying)
                    {
                        firework.Play();
                    }


                    if (!firework1.isPlaying)
                    {
                        firework1.Play();
                    }
                    if(!firework2.isPlaying)
                    {
                        firework2.Play();
                    }
                    if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 3) &&
            (lapManager.kart4.finalPosition == 4))
                    {
                        kartOne.material = red;
                        kartTwo.material = blue;
                        kartThree.material = green;
                        kartFour.material = yellow;
                    }
                    else if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 4) &&
                       (lapManager.kart4.kartPosition == 3))
                    {
                        kartOne.material = red;
                        kartTwo.material = blue;
                        kartThree.material = yellow;
                        kartFour.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 2) &&
                       (lapManager.kart4.kartPosition == 4))
                    {
                        kartOne.material = red;
                        kartTwo.material = green;
                        kartThree.material = blue;
                        kartFour.material = yellow;
                    }
                    else if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 4) &&
                       (lapManager.kart4.kartPosition == 2))
                    {
                        kartOne.material = red;
                        kartTwo.material = yellow;
                        kartThree.material = blue;
                        kartFour.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 4) && (lapManager.kart3.kartPosition == 3) &&
                       (lapManager.kart4.kartPosition == 2))
                    {
                        kartOne.material = red;
                        kartTwo.material = yellow;
                        kartThree.material = green;
                        kartFour.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 1) && (lapManager.kart2.kartPosition == 4) && (lapManager.kart3.kartPosition == 2) &&
                       (lapManager.kart4.kartPosition == 3))
                    {
                        kartOne.material = red;
                        kartTwo.material = green;
                        kartThree.material = yellow;
                        kartFour.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 3) &&
                       (lapManager.kart4.kartPosition == 4))
                    {
                        kartOne.material = blue;
                        kartTwo.material = red;
                        kartThree.material = green;
                        kartFour.material = yellow;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 4) &&
                      (lapManager.kart4.kartPosition == 3))
                    {
                        kartOne.material = blue;
                        kartTwo.material = red;
                        kartThree.material = yellow;
                        kartFour.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 1) &&
                      (lapManager.kart4.kartPosition == 4))
                    {
                        kartOne.material = green;
                        kartTwo.material = red;
                        kartThree.material = blue;
                        kartFour.material = yellow;

                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 4) &&
                   (lapManager.kart4.kartPosition == 1))
                    {
                        kartOne.material = yellow;
                        kartTwo.material = red;
                        kartThree.material = blue;
                        kartFour.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 4) && (lapManager.kart3.kartPosition == 3) &&
                  (lapManager.kart4.kartPosition == 1))
                    {
                        kartOne.material = yellow;
                        kartTwo.material = red;
                        kartThree.material = green;
                        kartFour.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 2) && (lapManager.kart2.kartPosition == 4) && (lapManager.kart3.kartPosition == 1) &&
                    (lapManager.kart4.kartPosition == 3))
                    {
                        kartOne.material = green;
                        kartTwo.material = red;
                        kartThree.material = yellow;
                        kartFour.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 2) &&
                    (lapManager.kart4.kartPosition == 4))
                    {
                        kartOne.material = blue;
                        kartTwo.material = green;
                        kartThree.material = red;
                        kartFour.material = yellow;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 4) &&
                  (lapManager.kart4.kartPosition == 2))
                    {
                        kartOne.material = blue;
                        kartTwo.material = yellow;
                        kartThree.material = red;
                        kartFour.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 1) &&
                  (lapManager.kart4.kartPosition == 4))
                    {
                        kartOne.material = green;
                        kartTwo.material = blue;
                        kartThree.material = red;
                        kartFour.material = yellow;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 4) &&
                (lapManager.kart4.kartPosition == 1))
                    {
                        kartOne.material = yellow;
                        kartTwo.material = blue;
                        kartThree.material = red;
                        kartFour.material = green;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 4) && (lapManager.kart3.kartPosition == 1) &&
                      (lapManager.kart4.kartPosition == 2))
                    {
                        kartOne.material = green;
                        kartTwo.material = yellow;
                        kartThree.material = red;
                        kartFour.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 3) && (lapManager.kart2.kartPosition == 4) && (lapManager.kart3.kartPosition == 2) &&
                     (lapManager.kart4.kartPosition == 1))
                    {
                        kartOne.material = yellow;
                        kartTwo.material = green;
                        kartThree.material = red;
                        kartFour.material = blue;
                    }
                    else if ((lapManager.kart1.kartPosition == 4) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 2) &&
                    (lapManager.kart4.kartPosition == 3))
                    {
                        kartOne.material = blue;
                        kartTwo.material = green;
                        kartThree.material = yellow;
                        kartFour.material = red;
                    }
                    else if ((lapManager.kart1.kartPosition == 4) && (lapManager.kart2.kartPosition == 1) && (lapManager.kart3.kartPosition == 3) &&
                    (lapManager.kart4.kartPosition == 2))
                    {
                        kartOne.material = blue;
                        kartTwo.material = yellow;
                        kartThree.material = green;
                        kartFour.material = red;
                    }
                    else if ((lapManager.kart1.kartPosition == 4) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 3) &&
                    (lapManager.kart4.kartPosition == 1))
                    {
                        kartOne.material = yellow;
                        kartTwo.material = blue;
                        kartThree.material = green;
                        kartFour.material = red;
                    }
                    else if ((lapManager.kart1.kartPosition  == 4) && (lapManager.kart2.kartPosition == 2) && (lapManager.kart3.kartPosition == 1) &&
                   (lapManager.kart4.kartPosition == 3))
                    {
                        kartOne.material = green;
                        kartTwo.material = blue;
                        kartThree.material = yellow;
                        kartFour.material = red;
                    }
                    else if ((lapManager.kart1.kartPosition == 4) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 2) &&
                   (lapManager.kart4.kartPosition == 1))
                    {
                        kartOne.material = yellow;
                        kartTwo.material = green;
                        kartThree.material = blue;
                        kartFour.material = red;
                    }
                    else if ((lapManager.kart1.kartPosition == 4) && (lapManager.kart2.kartPosition == 3) && (lapManager.kart3.kartPosition == 1) &&
                  (lapManager.kart4.kartPosition == 2))
                    {
                        kartOne.material = green;
                        kartTwo.material = yellow;
                        kartThree.material = blue;
                        kartOne.material = red;
                    }

                    if (lapManager.kart1.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }
                    if (lapManager.kart2.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }
                    if (lapManager.kart3.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }
                    if (lapManager.kart4.gamepad.GetButtonDown("B"))
                    {
                        SceneManager.LoadScene(0);
                        Instantiate(newManager);
                        GameObject.Destroy(this.gameObject);
                    }


                    break;
            }
        }
    }
}