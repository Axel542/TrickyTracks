using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Written by Angus Secomb
//Last edited on 28/11/17
public class MenuActor : MonoBehaviour {

    private GamePadManager gpManager;
    private xbox_gamepad gamepad1, gamepad2;

    private AudioSource gearShiftOne, gearShiftTwo, beepBeep;

    float deadZone = 0.9f;

    float coolDown = 0.3f;
    float cdCopy = 0.3f;

    public GameObject playButton, optionButton, exitButton;

    Image playi, optioni, exiti;

    Image creditI, controlI;

    public GameObject creditUI, title;
    public GameObject controlBTN, creditBTN;

    public Camera menu, credits;

    private bool creditsON = false;
    private bool controlsOn = false;

    private Image bbutton;
    public Image backToMenu;
    public Image greyDrop;

    public Image Controls;


    Color y = new Color(1, 0.92f, 0.016f, 1);

    private int buttonIndex = 1;


    public void LoadLevel(string loadlevel)
    {
        SceneManager.LoadScene(loadlevel);
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

	// Use this for initialization
	void Start () {
        gpManager = this.gameObject.GetComponent<GamePadManager>();
        gamepad1 = GamePadManager.Instance.GetGamePad(1);
        gamepad2 = GamePadManager.Instance.GetGamePad(2);
        gearShiftOne = GameObject.Find("GearShiftOne").GetComponent<AudioSource>();
        gearShiftTwo = GameObject.Find("GearShiftTwo").GetComponent<AudioSource>();
        beepBeep = GameObject.Find("BeepBeep").GetComponent<AudioSource>();
        bbutton = GameObject.Find("BButton").GetComponent<Image>();
        //playButton = GameObject.Find("PlayBTN");
        //optionButton = GameObject.Find("OptionsBTN");
        //exitButton = GameObject.Find("ExitBTN");
        playi = playButton.GetComponent<Image>();
 //       optioni = optionButton.GetComponent<Image>();
        exiti = exitButton.GetComponent<Image>();
        creditI = creditBTN.GetComponent<Image>();
        controlI = controlBTN.GetComponent<Image>();
        bbutton.enabled = false;
        backToMenu.enabled = false;
        Controls.enabled = false;
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(buttonIndex);

 

        switch (buttonIndex)
        {
            case 1:
                playi.color = y;
                controlI.color = Color.grey ;
                exiti.color = Color.grey;
                creditI.color = Color.grey;

                coolDown -= Time.deltaTime;
                if (!creditsON)
                {
                    if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                    {
                        gearShiftOne.Play();
                        buttonIndex = 3;
                        coolDown = cdCopy;
                    }
                    if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                    {
                        gearShiftTwo.Play();
                        buttonIndex = 2;
                        coolDown = cdCopy;
                    }

                    if (gamepad1.GetButtonDown("Start"))
                    {
                        beepBeep.Play();
                        LoadLevel(1);
                        GameObject.Destroy(this.gameObject);
                    }

                    if (gamepad1.GetButtonDown("A"))
                    {
                        beepBeep.Play();
                        LoadLevel(1);
                        GameObject.Destroy(this.gameObject);
                    }
                }


                break;

            case 2:
                coolDown -= Time.deltaTime;
               // optioni.color = y;
                playi.color = Color.grey;
                exiti.color = Color.grey;
                creditI.color = Color.grey;
                controlI.color = y;

                if (!creditsON && !controlsOn)
                {
                    if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                    {
                        gearShiftOne.Play();
                        buttonIndex = 1;
                        coolDown = cdCopy;
                    }
                    if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                    {
                        gearShiftTwo.Play();
                        buttonIndex = 3;
                        coolDown = cdCopy;
                    }

                    if (!controlsOn)
                    {
                        if (gamepad1.GetButtonDown("A") || gamepad1.GetButtonDown("Start"))
                        {
                            playButton.SetActive(false);
                            exitButton.SetActive(false);
                            controlBTN.SetActive(false);
                            creditBTN.SetActive(false);
                            title.SetActive(false);
                            Controls.enabled = true;
                            bbutton.enabled = true;
                            backToMenu.enabled = true;
                            controlsOn = true;
                            greyDrop.enabled = true;
                        }
                    }
                }

                if(controlsOn)
                {
                    if (gamepad1.GetButtonDown("B"))
                    {
                        Controls.enabled = false;
                        controlsOn = false;
                        backToMenu.enabled = false;
                        bbutton.enabled = false;
                        playButton.SetActive(true);
                        exitButton.SetActive(true);
                        controlBTN.SetActive(true);
                        creditBTN.SetActive(true);
                        title.SetActive(true);
                        greyDrop.enabled = false;
                    }
                }
                break;
            case 3:
                coolDown -= Time.deltaTime;
                exiti.color = Color.grey;
                playi.color = Color.grey;
                creditI.color = y;
                controlI.color = Color.grey;

                if (!creditsON && !controlsOn)
                {
                    if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                    {
                        gearShiftOne.Play();
                        buttonIndex = 2;
                        coolDown = cdCopy;
                    }
                    if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                    {
                        gearShiftTwo.Play();
                        buttonIndex = 4;
                        coolDown = cdCopy;
                    }
            
                if ((gamepad1.GetButtonDown("Start")) || (gamepad1.GetButtonDown("A")))
                    {
                        creditUI.SetActive(true);
                        playButton.SetActive(false);
                        exitButton.SetActive(false);
                        controlBTN.SetActive(false);
                        creditBTN.SetActive(false);
                        title.SetActive(false);
                        bbutton.enabled = true;
                        backToMenu.enabled = true;
                        creditsON = true;
                    }
                }


                if (creditsON)
                    {
                        if (gamepad1.GetButtonDown("B"))
                        {
                            creditsON = false;
                            creditUI.SetActive(false);
                            playButton.SetActive(true);
                            exitButton.SetActive(true);
                            controlBTN.SetActive(true);
                            creditBTN.SetActive(true);
                            title.SetActive(true);
                               backToMenu.enabled = false;
                              bbutton.enabled = false;
                        }
                    }
            

             

                break;
            case 4:
                exiti.color = y;
                playi.color = Color.grey;
                creditI.color = Color.grey;
                controlI.color = Color.grey;
                coolDown -= Time.deltaTime;

                if (!creditsON)
                {
                    if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                    {
                        gearShiftOne.Play();
                        coolDown = cdCopy;
                        buttonIndex = 3;
                    }
                    if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                    {
                        gearShiftTwo.Play();
                        buttonIndex = 1;
                        coolDown = cdCopy;
                    }

                    if (gamepad1.GetButtonDown("A"))
                    {
                        beepBeep.Play();
                        CloseApplication();
                    }
                }
                break;
            default:
                break;
        }

       


        //if (creditsON)
        //{
        //    if (gamepad1.GetButtonDown("A"))
        //    {
        //        creditsON = false;
        //        creditUI.SetActive(false);
        //        playButton.SetActive(true);
        //        exitButton.SetActive(true);
        //        controlBTN.SetActive(true);
        //        creditBTN.SetActive(true);
        //        title.SetActive(true);
        //    }
        //}
    }
}
