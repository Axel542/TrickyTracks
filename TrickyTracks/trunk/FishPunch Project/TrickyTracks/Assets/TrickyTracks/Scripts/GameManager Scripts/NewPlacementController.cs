﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Written by Angus Secomb
//Last edited 28/11/2017.
public class NewPlacementController : MonoBehaviour
{
    private GameObject[] items;

    public Color disabledColor;

    public AudioSource scrollItems1, scrollItems2, placeItem;

    public float exclusionDistance = 10.0f;

    [HideInInspector]
    public bool isPaused = false;

    //Manager variables to access other scripts on the manager.
    private GameObject manager;
    private GamePadManager gpManager;
    private PlayerSelectActor psActor;
    private AudioSource swoosh;

    private LapsManager lapManager;

    public float prefabRotationSpeed = 2.0f;

    //Player Gamepad Instances
    private xbox_gamepad gamepad1, gamepad2, gamepad3, gamepad4;

    //Images for item UI that displays current item, next item and previous item.
    private Image currentItem1, currentItemBack1;
    private Image currentItem2, currentItemBack2;
    private Image currentItem3, currentItemBack3;
    private Image currentItem4, currentItemBack4;
    private Image powerup1, powerup2, powerup3, powerup4, powerup1_2P, powerup2_2P;
    private Image powerupIcon1, powerupIcon2, powerupIcon3, powerupIcon4, powerupIcon1_2P, powerupIcon2_2P;

    private Image itemCount1Num1, itemCount1Num2, itemCount2Num1, itemCount2Num2, itemCount3Num1, itemCount3Num2,
                  itemCount4Num1, itemCount4Num2;

    public Sprite[] nums;

    private Image triggerR1, triggerL1, triggerR2, triggerL2, triggerR3, triggerL3, triggerR4, triggerL4;

    private Image button1, button2, button3, button4;

    //Traps gameobject prefabs
    [Header("Traps")]
    public GameObject buzzsaw;
    public GameObject ramp;
    public GameObject oilslick;
    public GameObject boostsaw;

    //Item prefabs
    [Header("Items")]
    public GameObject boost;
    public GameObject rpg;
    public GameObject mine;

    //Sprites for all the items.
    [Header("Item Icons")]
    public Sprite buzzsawIcon;
    public Sprite rampIcon;
    public Sprite oilslickIcon;
    public Sprite boostIcon;
    public Sprite mineIcon;
    public Sprite rpgIcon;
    public Sprite blankIcon;
    public Sprite blankPowerupIcon;

    [Header("Player Icons")]
    public Sprite playerSprite1;
    public Sprite playerSprite2;
    public Sprite playerSprite3;
    public Sprite playerSprite4;

    //Object that the raycast shoots down from.
    private GameObject raycastObject1, raycastObject2, raycastObject3, raycastObject4;

    //The object currently about to be placed
    private GameObject currentPlaceableObject1, currentPlaceableObject2,
                       currentPlaceableObject3, currentPlaceableObject4;

    //The object that is placed when the prefab is released with releaseprefab().
    private GameObject placeableObject1, placeableObject2,
                       placeableObject3, placeableObject4;

    //Index for each player item list.
    private int prefabIndex1 = 0, prefabIndex2 = 0, prefabIndex3 = 0, prefabIndex4 = 0;

    private bool cannotPlace1 = false, cannotPlace2 = false, cannotPlace3 = false, cannotPlace4 = false;

    //kart gameobjects.
    private GameObject kart1, kart2, kart3, kart4;

    //List with all trap and all item prefabs respectively.
    private List<GameObject> trapPrefabs = new List<GameObject>();
    private List<GameObject> itemPrefabs = new List<GameObject>();

    //list that stores random numbers for each player so they receive different items every lap.
    private List<int> randNumP1 = new List<int>(), randNumP2 = new List<int>(),
                      randNumP3 = new List<int>(), randNumP4 = new List<int>();

    //The list where player items are stored.
    [HideInInspector]
    public List<GameObject> itemsP1 = new List<GameObject>(), itemsP2 = new List<GameObject>(),
                             itemsP3 = new List<GameObject>(), itemsP4 = new List<GameObject>();

    //temporary number that random function assigns a number to before pushing to a random number list.
    private int randTempNum1;

    //Layer mask so the raycast object ignores the trap/item its placing.
    int layerMask;

    private Image greyBackDrop, pauseUI, pauseContinue, pauseMenu;

    private int buttonIndex = 1;
    private float raceCountDownTimer = 3.0f;
    float deadZone = 0.9f;

    float coolDown = 0.3f;
    float cdCopy = 0.3f;

    // Use this for initialization
    void Start()
    {
        //Grab manager instances from manager gameobject.
        manager = this.gameObject;
        gpManager = GetComponent<GamePadManager>();
        psActor = manager.GetComponent<PlayerSelectActor>();
        lapManager = manager.GetComponent<LapsManager>();


        button1 = GameObject.Find("Button1").GetComponent<Image>();
        button2 = GameObject.Find("Button2").GetComponent<Image>();
        button3 = GameObject.Find("Button3").GetComponent<Image>();
        button4 = GameObject.Find("Button4").GetComponent<Image>();

        triggerL1 = GameObject.Find("TriggerL1").GetComponent<Image>();
        triggerR1 = GameObject.Find("TriggerR1").GetComponent<Image>();
        triggerL2 = GameObject.Find("TriggerL2").GetComponent<Image>();
        triggerR2 = GameObject.Find("TriggerR2").GetComponent<Image>();
        triggerL3 = GameObject.Find("TriggerL3").GetComponent<Image>();
        triggerR3 = GameObject.Find("TriggerR3").GetComponent<Image>();
        triggerL4 = GameObject.Find("TriggerL4").GetComponent<Image>();
        triggerR4 = GameObject.Find("TriggerR4").GetComponent<Image>();

        //add traps to trap list.
        trapPrefabs.Add(buzzsaw);
        trapPrefabs.Add(oilslick);
        trapPrefabs.Add(ramp);
        cdCopy = coolDown;

        greyBackDrop = GameObject.Find("GreyBackDrop").GetComponent<Image>();
        pauseUI = GameObject.Find("PauseUIPaused").GetComponent<Image>();
        pauseContinue = GameObject.Find("PauseUIContinue").GetComponent<Image>();
        pauseMenu = GameObject.Find("PauseUIMainMenu").GetComponent<Image>();
        //add items to item list
        itemPrefabs.Add(boost);
        itemPrefabs.Add(rpg);
        itemPrefabs.Add(mine);

        //I know how bad gameobject.Find is dont worry just used it as i had to write 99%
        //of the game myself so had no time to plan out a proper system :)
        itemCount1Num1 = GameObject.Find("ItemCount1Num1").GetComponent<Image>();
        itemCount1Num2 = GameObject.Find("ItemCount1Num2").GetComponent<Image>();
        itemCount2Num1 = GameObject.Find("ItemCount2Num1").GetComponent<Image>();
        itemCount2Num2 = GameObject.Find("ItemCount2Num2").GetComponent<Image>();
        itemCount3Num1 = GameObject.Find("ItemCount3Num1").GetComponent<Image>();
        itemCount3Num2 = GameObject.Find("ItemCount3Num2").GetComponent<Image>();
        itemCount4Num1 = GameObject.Find("ItemCount4Num1").GetComponent<Image>();
        itemCount4Num2 = GameObject.Find("ItemCount4Num2").GetComponent<Image>();


        currentItemBack1 = GameObject.Find("CurrentItemBack1").GetComponent<Image>();
        currentItemBack2 = GameObject.Find("CurrentItemBack2").GetComponent<Image>();
        currentItemBack3 = GameObject.Find("CurrentItemBack3").GetComponent<Image>();
        currentItemBack4 = GameObject.Find("CurrentItemBack4").GetComponent<Image>();

        currentItem1 = GameObject.Find("CurrentItem1").GetComponent<Image>();
        currentItem2 = GameObject.Find("CurrentItem2").GetComponent<Image>();
        currentItem3 = GameObject.Find("CurrentItem3").GetComponent<Image>();
        currentItem4 = GameObject.Find("CurrentItem4").GetComponent<Image>();

        swoosh = GameObject.Find("AirSwooshSound").GetComponent<AudioSource>();

        //depending on playercount.
        switch (psActor.playerCount)
        {
            case 2:
                //Randomise items.
                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];

                //Assign gamepads.
                for (int i = 1; i <= GamePadManager.Instance.ConnectedTotal(); ++i)
                {
                    xbox_gamepad temppad = GamePadManager.Instance.GetGamePad(i);

                    if (temppad.newControllerIndex == 1)
                    {
                        gamepad1 = temppad;
                    }

                    if (temppad.newControllerIndex == 2)
                    {
                        gamepad2 = temppad;
                    }
                }
                //     gamepad1 = GamePadManager.Instance.GetGamePad(1);
                //   gamepad2 = GamePadManager.Instance.GetGamePad(2);

                //Assign raycast object and karts
                raycastObject1 = GameObject.Find("RayCast1");
                raycastObject2 = GameObject.Find("RayCast2");
                kart1 = GameObject.Find("PlayerCharacter_001");
                kart2 = GameObject.Find("PlayerCharacter_002");

                //disabled UI sprites.
                currentItem2.enabled = false;
                currentItem4.enabled = false;
                currentItemBack2.enabled = false;
                currentItemBack4.enabled = false;
                currentItemBack3.sprite = playerSprite2;

                GameObject.Find("ItemCount3").SetActive(false);
                itemCount3Num1.enabled = false;
                itemCount3Num2.enabled = false;

                GameObject.Find("ItemCount4").SetActive(false);
                itemCount4Num1.enabled = false;
                itemCount4Num2.enabled = false;

                button2.enabled = false;
                button4.enabled = false;

                triggerR2.enabled = false;
                triggerL2.enabled = false;
                triggerR4.enabled = false;
                triggerL4.enabled = false;

                break;
            case 3:
                //Randomise items.
                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                randomiseItems(randTempNum1, randNumP3);
                allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];
                placeableObject3 = itemsP3[0];

                button4.enabled = false;

                ////Assign gamepads.
                //gamepad1 = GamePadManager.Instance.GetGamePad(1);
                //gamepad2 = GamePadManager.Instance.GetGamePad(2);
                //gamepad3 = GamePadManager.Instance.GetGamePad(3);


                //Assign gamepads.
                for (int i = 1; i <= GamePadManager.Instance.ConnectedTotal(); ++i)
                {
                    xbox_gamepad temppad = GamePadManager.Instance.GetGamePad(i);

                    if (temppad.newControllerIndex == 1)
                    {
                        gamepad1 = temppad;
                    }

                    if (temppad.newControllerIndex == 2)
                    {
                        gamepad2 = temppad;
                    }

                    if(temppad.newControllerIndex == 3)
                    {
                        gamepad3 = temppad;
                    }
                }

                raycastObject1 = GameObject.Find("RayCast1");
                raycastObject2 = GameObject.Find("RayCast2");
                raycastObject3 = GameObject.Find("RayCast3");

                kart1 = GameObject.Find("PlayerCharacter_001");
                kart2 = GameObject.Find("PlayerCharacter_002");
                kart3 = GameObject.Find("PlayerCharacter_003");

                currentItemBack4.enabled = false;
                currentItem4.enabled = false;

                GameObject.Find("ItemCount4").SetActive(false);
                itemCount4Num1.enabled = false;
                itemCount4Num2.enabled = false;

                triggerR4.enabled = false;
                triggerL4.enabled = false;

                break;
            case 4:

                randomiseItems(randTempNum1, randNumP1);
                allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                randomiseItems(randTempNum1, randNumP2);
                allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                randomiseItems(randTempNum1, randNumP3);
                allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                randomiseItems(randTempNum1, randNumP4);
                allocateRandItems(randNumP4, itemPrefabs, trapPrefabs, itemsP4);

                placeableObject1 = itemsP1[0];
                placeableObject2 = itemsP2[0];
                placeableObject3 = itemsP3[0];
                placeableObject4 = itemsP4[0];


                for (int i = 1; i <= GamePadManager.Instance.ConnectedTotal(); ++i)
                {
                    xbox_gamepad temppad = GamePadManager.Instance.GetGamePad(i);

                    if (temppad.newControllerIndex == 1)
                    {
                        gamepad1 = temppad;
                    }

                    if (temppad.newControllerIndex == 2)
                    {
                        gamepad2 = temppad;
                    }

                    if (temppad.newControllerIndex == 3)
                    {
                        gamepad3 = temppad;
                    }
                    if (temppad.newControllerIndex == 4)
                    {
                        gamepad4 = temppad;
                    }
                }


                //Assign gamepads.
                for (int i = 1; i <= GamePadManager.Instance.ConnectedTotal(); ++i)
                {
                    xbox_gamepad temppad = GamePadManager.Instance.GetGamePad(i);

                    if (temppad.newControllerIndex == 1)
                    {
                        gamepad1 = temppad;
                    }

                    if (temppad.newControllerIndex == 2)
                    {
                        gamepad2 = temppad;
                    }

                    if (temppad.newControllerIndex == 3)
                    {
                        gamepad3 = temppad;
                    }

                    if(temppad.newControllerIndex == 4)
                    {
                        gamepad4 = temppad;
                    }
                }

                raycastObject1 = GameObject.Find("RayCast1");
                raycastObject2 = GameObject.Find("RayCast2");
                raycastObject3 = GameObject.Find("RayCast3");
                raycastObject4 = GameObject.Find("RayCast4");

                kart1 = GameObject.Find("PlayerCharacter_001");
                kart2 = GameObject.Find("PlayerCharacter_002");
                kart3 = GameObject.Find("PlayerCharacter_003");
                kart4 = GameObject.Find("PlayerCharacter_004");

                break;
            default:
                break;
        }


        layerMask = 1 << LayerMask.NameToLayer("Item");
        layerMask = ~layerMask;
    }

    // Update is called once per frame
    void Update()
    {
        raceCountDownTimer -= Time.deltaTime;

        if (raceCountDownTimer < 0)
        {

            if (gamepad1.GetButtonDown("Start") && Time.timeScale == 1 && !isPaused)
            {
              isPaused = true;
                Time.timeScale = 0;
            }
            else if (gamepad1.GetButtonDown("Start") && Time.timeScale == 0)
            {
                 isPaused = false;
                Time.timeScale = 1;

            }
        }
        if (Time.timeScale == 0)
        {

            coolDown -= Time.unscaledDeltaTime;
            if (buttonIndex == 1)
            {
                if (gamepad1.GetStick_L().Y < -deadZone && coolDown < 0)
                {
                    coolDown = cdCopy;
                    buttonIndex = 2;
                }
                if (gamepad1.GetButtonDown("A"))
                {
                    isPaused = false;
                    isPaused = false;
                    Time.timeScale = 1;
                    this.enabled = false;

                }
                if (gamepad1.GetButtonUp("A"))
                {
                    this.enabled = true;
                }
            }

            if (buttonIndex == 2)
            {
                if (gamepad1.GetStick_L().Y > deadZone && coolDown < 0)
                {
                    coolDown = cdCopy;
                    buttonIndex = 1;
                }
                if (gamepad1.GetButtonDown("A"))
                {
                    Time.timeScale = 1;
                    Destroy(GameObject.Find("Manager"));
                    Destroy(GameObject.Find("Music"));
                    SceneManager.LoadScene(0);
                }
            }
            greyBackDrop.enabled = true;
            pauseContinue.enabled = true;
            pauseMenu.enabled = true;
            pauseUI.enabled = true;



        }
        else if (Time.timeScale == 1)
        {
            greyBackDrop.enabled = false;
            pauseContinue.enabled = false;
            pauseMenu.enabled = false;
            pauseUI.enabled = false;
        }

        switch (buttonIndex)
        {
            case 1:
                pauseContinue.color = Color.yellow;
                pauseMenu.color = Color.grey;
                break;
            case 2:
                pauseContinue.color = Color.grey;
                pauseMenu.color = Color.yellow;
                break;
        }








        items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject obj in items)
        {
            if (currentPlaceableObject1 != null)
            {
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject1.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject1.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;


                    cannotPlace1 = true;
                }
                else
                {
                    cannotPlace1 = false;
                    Renderer rend;
                    rend = currentPlaceableObject1.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.white;
                    rend.material.color = new Color(0, 0, 0, 0);

                }
            }

            if (currentPlaceableObject2 != null)
            {
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject2.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject2.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;
                    cannotPlace2 = true;
                }
                else
                {
                    cannotPlace2 = false;
                    Renderer rend;
                    rend = currentPlaceableObject2.GetComponentInChildren<Renderer>();

                    rend.material.color = Color.white;
                    rend.material.color = new Color(0, 0, 0, 0);
                }
            }

            if (currentPlaceableObject3 != null)
            {
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject3.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject3.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;
                    cannotPlace3 = true;
                }
                else
                {
                    cannotPlace3 = false;
                    Renderer rend;
                    rend = currentPlaceableObject3.GetComponentInChildren<Renderer>();

                    rend.material.color = Color.white;
                    rend.material.color = new Color(0, 0, 0, 0);

                }
            }

            if (currentPlaceableObject4 != null)
            {
                if ((Vector3.Distance(obj.transform.position, currentPlaceableObject4.transform.position) < exclusionDistance))
                {
                    Renderer rend;
                    rend = currentPlaceableObject4.GetComponentInChildren<Renderer>();
                    rend.material.color = Color.red;
                    cannotPlace4 = true;
                }
                else
                {
                    cannotPlace4 = false;
                    Renderer rend;
                    rend = currentPlaceableObject4.GetComponentInChildren<Renderer>();

                    rend.material.color = Color.white;
                    rend.material.color = new Color(0, 0, 0, 0);

                }
            }
        }
        if (lapManager.raceCountdownTimer < 0)
        {
            objectGeneration();
            releasePrefab();
            changePrefab();
        }

        switch (psActor.playerCount)
        {
            case 2:

                switchItemIcons(prefabIndex1, currentItem1, itemsP1);
                switchItemIcons(prefabIndex2, currentItem3, itemsP2);

                if (lapManager.raceCountdownTimer < 0)
                {
                    buttonPress(gamepad1, triggerL1, triggerR1, button1);
                    buttonPress(gamepad2, triggerL3, triggerR3, button3);
                }

                if (kart1.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }
                if (kart2.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject2 = itemsP2[0];
                }

                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }

                playerItemCountUi(itemsP1, itemCount1Num1, itemCount1Num2);
                playerItemCountUi(itemsP2, itemCount2Num1, itemCount2Num2);


                break;
            case 3:

                if (lapManager.raceCountdownTimer < 0)
                {
                    buttonPress(gamepad1, triggerL1, triggerR1, button1);
                    buttonPress(gamepad2, triggerL2, triggerR2, button2);
                    buttonPress(gamepad3, triggerL3, triggerR3, button3);
                }

                switchItemIcons(prefabIndex1, currentItem1, itemsP1);
                switchItemIcons(prefabIndex2, currentItem2, itemsP2);
                switchItemIcons(prefabIndex3, currentItem3, itemsP3);

                if (kart1.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }
                if (kart2.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject2 = itemsP2[0];
                }
                if (kart3.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP3.Clear();
                    randomiseItems(randTempNum1, randNumP3);
                    allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                    kart3.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject3 = itemsP3[0];
                }
                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }
                if (currentPlaceableObject3 != null)
                {
                    fitPrefabToTrack(raycastObject3, currentPlaceableObject3, gamepad3);
                }

                playerItemCountUi(itemsP1, itemCount1Num1, itemCount1Num2);
                playerItemCountUi(itemsP2, itemCount3Num1, itemCount3Num2);
                playerItemCountUi(itemsP3, itemCount2Num1, itemCount2Num2);

                

                break;
            case 4:

                if (lapManager.raceCountdownTimer < 0)
                {
                    buttonPress(gamepad1, triggerL1, triggerR1, button1);
                    buttonPress(gamepad2, triggerL2, triggerR2, button2);
                    buttonPress(gamepad3, triggerL3, triggerR3, button3);
                    buttonPress(gamepad4, triggerL4, triggerR4, button4);
                }

                if (kart1.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP1.Clear();
                    randomiseItems(randTempNum1, randNumP1);
                    allocateRandItems(randNumP1, itemPrefabs, trapPrefabs, itemsP1);
                    kart1.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject1 = itemsP1[0];
                }
                if (kart2.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP2.Clear();
                    randomiseItems(randTempNum1, randNumP2);
                    allocateRandItems(randNumP2, itemPrefabs, trapPrefabs, itemsP2);
                    kart2.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject2 = itemsP2[0];
                }
                if (kart3.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP3.Clear();
                    randomiseItems(randTempNum1, randNumP3);
                    allocateRandItems(randNumP3, itemPrefabs, trapPrefabs, itemsP3);
                    kart3.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject3 = itemsP3[0];
                }
                if (kart4.GetComponent<PlayerActor>().assignNewTraps)
                {
                    randNumP4.Clear();
                    randomiseItems(randTempNum1, randNumP4);
                    allocateRandItems(randNumP4, itemPrefabs, trapPrefabs, itemsP4);
                    kart4.GetComponent<PlayerActor>().assignNewTraps = false;
                    placeableObject4 = itemsP4[0];
                }
                if (currentPlaceableObject1 != null)
                {
                    fitPrefabToTrack(raycastObject1, currentPlaceableObject1, gamepad1);
                }
                if (currentPlaceableObject2 != null)
                {
                    fitPrefabToTrack(raycastObject2, currentPlaceableObject2, gamepad2);
                }
                if (currentPlaceableObject3 != null)
                {
                    fitPrefabToTrack(raycastObject3, currentPlaceableObject3, gamepad3);
                }
                if (currentPlaceableObject4 != null)
                {
                    fitPrefabToTrack(raycastObject4, currentPlaceableObject4, gamepad4);
                }
 
             



                switchItemIcons(prefabIndex1, currentItem1, itemsP1);
                switchItemIcons(prefabIndex2, currentItem2, itemsP2);
                switchItemIcons(prefabIndex3, currentItem3, itemsP3);
                switchItemIcons(prefabIndex4, currentItem4, itemsP4);

                playerItemCountUi(itemsP1, itemCount1Num1, itemCount1Num2);
                playerItemCountUi(itemsP2, itemCount3Num1, itemCount3Num2);
                playerItemCountUi(itemsP3, itemCount2Num1, itemCount2Num2);
                playerItemCountUi(itemsP4, itemCount4Num1, itemCount4Num2);

            
                break;
            default:
                break;
        }

      

    }

    //Generates new object when players A button is down and item count is above 0.
    void objectGeneration()
    {
        switch (psActor.playerCount)
        {
            case 2:
                if (gamepad1.GetButtonDown("A"))
                {
                    if (currentPlaceableObject1 == null)
                    {
                        if (itemsP1.Count > 0)
                        {
                            if (placeableObject1 == rpg)
                            {
                                kart1.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject1 == mine)
                            {
                                kart1.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject1 == boost)
                            {
                                kart1.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject1 = Instantiate(placeableObject1);
                                currentPlaceableObject1.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                if (gamepad2.GetButtonDown("A"))
                {
                    if (currentPlaceableObject2 == null)
                    {
                        if (itemsP2.Count > 0)
                        {
                            if (placeableObject2 == rpg)
                            {
                                kart2.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject2 == mine)
                            {
                                kart2.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject2 == boost)
                            {
                                kart2.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject2 = Instantiate(placeableObject2);
                                currentPlaceableObject2.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                break;
            case 3:
                if (gamepad1.GetButtonDown("A"))
                {
                    if (currentPlaceableObject1 == null)
                    {
                        if (itemsP1.Count > 0)
                        {
                            if (placeableObject1 == rpg)
                            {
                                kart1.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject1 == mine)
                            {
                                kart1.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject1 == boost)
                            {
                                kart1.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject1 = Instantiate(placeableObject1);
                                currentPlaceableObject1.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                if (gamepad2.GetButtonDown("A"))
                {
                    if (currentPlaceableObject2 == null)
                    {
                        if (itemsP2.Count > 0)
                        {
                            if (placeableObject2 == rpg)
                            {
                                kart2.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject2 == mine)
                            {
                                kart2.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject2 == boost)
                            {
                                kart2.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject2 = Instantiate(placeableObject2);
                                currentPlaceableObject2.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                if (gamepad3.GetButtonDown("A"))
                {
                    if (currentPlaceableObject3 == null)
                    {
                        if (itemsP3.Count > 0)
                        {
                            if (placeableObject3 == rpg)
                            {
                                kart3.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject3 == mine)
                            {
                                kart3.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject3 == boost)
                            {
                                kart3.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject3 = Instantiate(placeableObject3);
                                currentPlaceableObject3.tag = "PlaceableObject";
                            }
                        }
                    }
                }



                break;
            case 4:
                if (gamepad1.GetButtonDown("A"))
                {
                    if (currentPlaceableObject1 == null)
                    {
                        if (itemsP1.Count > 0)
                        {
                            if (placeableObject1 == rpg)
                            {
                                kart1.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject1 == mine)
                            {
                                kart1.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject1 == boost)
                            {
                                kart1.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject1 = Instantiate(placeableObject1);
                                currentPlaceableObject1.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                if (gamepad2.GetButtonDown("A"))
                {
                    if (currentPlaceableObject2 == null)
                    {
                        if (itemsP2.Count > 0)
                        {
                            if (placeableObject2 == rpg)
                            {
                                kart2.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject2 == mine)
                            {
                                kart2.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject2 == boost)
                            {
                                kart2.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject2 = Instantiate(placeableObject2);
                                currentPlaceableObject2.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                if (gamepad3.GetButtonDown("A"))
                {
                    if (currentPlaceableObject3 == null)
                    {
                        if (itemsP3.Count > 0)
                        {
                            if (placeableObject3 == rpg)
                            {
                                kart3.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject3 == mine)
                            {
                                kart3.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject3 == boost)
                            {
                                kart3.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject3 = Instantiate(placeableObject3);
                                currentPlaceableObject3.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                if (gamepad4.GetButtonDown("A"))
                {
                    if (currentPlaceableObject4 == null)
                    {
                        if (itemsP4.Count > 0)
                        {
                            if (placeableObject4 == rpg)
                            {
                                kart4.GetComponent<PlayerActor>().itemRPG = true;
                            }
                            else if (placeableObject4 == mine)
                            {
                                kart4.GetComponent<PlayerActor>().itemMine = true;
                            }
                            else if (placeableObject4 == boost)
                            {
                                kart4.GetComponent<PlayerActor>().itemBoost = true;
                            }
                            else
                            {
                                currentPlaceableObject4 = Instantiate(placeableObject4);
                                currentPlaceableObject4.tag = "PlaceableObject";
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }

    }

    //Raycast shoots down from raycast object and rotates item to fit track based on the hitinfo from raycast.
    void fitPrefabToTrack(GameObject raycastObject, GameObject currentPlaceableObject, xbox_gamepad gamepad)
    {

        RaycastHit hitInfo;
        if (Physics.Raycast(raycastObject.transform.position, -raycastObject.transform.up, out hitInfo, 1000, layerMask))
        {
            currentPlaceableObject.transform.position = hitInfo.point;

            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, new Vector3(hitInfo.normal.x, hitInfo.normal.y, hitInfo.normal.z));
            currentPlaceableObject.transform.forward = raycastObject.transform.forward;
            currentPlaceableObject.transform.Rotate(0, gamepad.triggerRotation, 0);

        }
    }

    //When A is not pressed down and there count is above 0
    //rb or lb switches between item prefabs
    void changePrefab()
    {
        switch (psActor.playerCount)
        {
            case 2:
                if (itemsP1.Count > 0)
                {
                    if (!gamepad1.GetButton("A"))
                    {
                        if (gamepad1.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 < itemsP1.Count - 1)
                            {
                                prefabIndex1++;
                            }
                            else
                            {
                                prefabIndex1 = 0;
                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        if (gamepad1.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 > 0)
                            {
                                prefabIndex1--;
                            }
                            else
                            {
                                if (itemsP1.Count == 0)
                                {
                                    prefabIndex1 = itemsP1.Count;
                                }
                                if (itemsP1.Count > 0)
                                {
                                    prefabIndex1 = itemsP1.Count - 1;
                                }

                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }

                    }
                }
                if (itemsP2.Count > 0)
                {
                    if (!gamepad2.GetButton("A"))
                    {
                        if (gamepad2.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 < itemsP2.Count - 1)
                            {
                                prefabIndex2++;
                            }
                            else
                            {
                                prefabIndex2 = 0;
                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                        if (gamepad2.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 > 0)
                            {
                                prefabIndex2--;
                            }
                            else
                            {
                                if (itemsP2.Count == 0)
                                {
                                    prefabIndex2 = itemsP2.Count;
                                }
                                if (itemsP2.Count > 0)
                                {
                                    prefabIndex2 = itemsP2.Count - 1;
                                }

                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }

                    }
                }
                break;
            case 3:
                if (itemsP1.Count > 0)
                {
                    if (!gamepad1.GetButton("A"))
                    {
                        if (gamepad1.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 < itemsP1.Count - 1)
                            {
                                prefabIndex1++;
                            }
                            else
                            {
                                prefabIndex1 = 0;
                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        if (gamepad1.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 > 0)
                            {
                                prefabIndex1--;
                            }
                            else
                            {
                                if (itemsP1.Count == 0)
                                {
                                    prefabIndex1 = itemsP1.Count;
                                }
                                if (itemsP1.Count > 0)
                                {
                                    prefabIndex1 = itemsP1.Count - 1;
                                }

                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }

                    }
                }
                if (itemsP2.Count > 0)
                {
                    if (!gamepad2.GetButton("A"))
                    {
                        if (gamepad2.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 < itemsP2.Count - 1)
                            {
                                prefabIndex2++;
                            }
                            else
                            {
                                prefabIndex2 = 0;
                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                        if (gamepad2.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 > 0)
                            {
                                prefabIndex2--;
                            }
                            else
                            {
                                if (itemsP2.Count == 0)
                                {
                                    prefabIndex2 = itemsP2.Count;
                                }
                                if (itemsP2.Count > 0)
                                {
                                    prefabIndex2 = itemsP2.Count - 1;
                                }

                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }

                    }
                }
                if (itemsP3.Count > 0)
                {
                    if (!gamepad3.GetButton("A"))
                    {
                        if (gamepad3.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 < itemsP3.Count - 1)
                            {
                                prefabIndex3++;
                            }
                            else
                            {
                                prefabIndex3 = 0;
                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }
                        if (gamepad3.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 > 0)
                            {
                                prefabIndex3--;
                            }
                            else
                            {
                                if (itemsP3.Count == 0)
                                {
                                    prefabIndex3 = itemsP3.Count;
                                }
                                if (itemsP3.Count > 0)
                                {
                                    prefabIndex3 = itemsP3.Count - 1;
                                }

                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }

                    }
                }
                break;
            case 4:
                if (itemsP1.Count > 0)
                {
                    if (!gamepad1.GetButton("A"))
                    {
                        if (gamepad1.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 < itemsP1.Count - 1)
                            {
                                prefabIndex1++;
                            }
                            else
                            {
                                prefabIndex1 = 0;
                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }
                        if (gamepad1.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject1);
                            if (prefabIndex1 > 0)
                            {
                                prefabIndex1--;
                            }
                            else
                            {
                                if (itemsP1.Count == 0)
                                {
                                    prefabIndex1 = itemsP1.Count;
                                }
                                if (itemsP1.Count > 0)
                                {
                                    prefabIndex1 = itemsP1.Count - 1;
                                }

                            }
                            placeableObject1 = itemsP1[prefabIndex1];
                        }

                    }
                }
                if (itemsP2.Count > 0)
                {
                    if (!gamepad2.GetButton("A"))
                    {
                        if (gamepad2.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 < itemsP2.Count - 1)
                            {
                                prefabIndex2++;
                            }
                            else
                            {
                                prefabIndex2 = 0;
                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }
                        if (gamepad2.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject2);
                            if (prefabIndex2 > 0)
                            {
                                prefabIndex2--;
                            }
                            else
                            {
                                if (itemsP2.Count == 0)
                                {
                                    prefabIndex2 = itemsP2.Count;
                                }
                                if (itemsP2.Count > 0)
                                {
                                    prefabIndex2 = itemsP2.Count - 1;
                                }

                            }
                            placeableObject2 = itemsP2[prefabIndex2];
                        }

                    }
                }
                if (itemsP3.Count > 0)
                {
                    if (!gamepad3.GetButton("A"))
                    {
                        if (gamepad3.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 < itemsP3.Count - 1)
                            {
                                prefabIndex3++;
                            }
                            else
                            {
                                prefabIndex3 = 0;
                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }
                        if (gamepad3.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject3);
                            if (prefabIndex3 > 0)
                            {
                                prefabIndex3--;
                            }
                            else
                            {
                                if (itemsP3.Count == 0)
                                {
                                    prefabIndex3 = itemsP3.Count;
                                }
                                if (itemsP3.Count > 0)
                                {
                                    prefabIndex3 = itemsP3.Count - 1;
                                }

                            }
                            placeableObject3 = itemsP3[prefabIndex3];
                        }

                    }
                }
                if (itemsP4.Count > 0)
                {
                    if (!gamepad4.GetButton("A"))
                    {
                        if (gamepad4.GetButtonDown("RB"))
                        {
                            scrollItems1.Play();
                            Destroy(currentPlaceableObject4);
                            if (prefabIndex4 < itemsP4.Count - 1)
                            {
                                prefabIndex4++;
                            }
                            else
                            {
                                prefabIndex4 = 0;
                            }
                            placeableObject4 = itemsP4[prefabIndex4];
                        }
                        if (gamepad4.GetButtonDown("LB"))
                        {
                            scrollItems2.Play();
                            Destroy(currentPlaceableObject4);
                            if (prefabIndex4 > 0)
                            {
                                prefabIndex4--;
                            }
                            else
                            {
                                if (itemsP4.Count == 0)
                                {
                                    prefabIndex4 = itemsP4.Count;
                                }
                                if (itemsP4.Count > 0)
                                {
                                    prefabIndex4 = itemsP4.Count - 1;
                                }

                            }
                            placeableObject4 = itemsP4[prefabIndex4];
                        }

                    }
                }
                break;
            default:
                break;
        }
    }

    //Release prefab when a button comes up from being pressed.
    void releasePrefab()
    {
        switch (psActor.playerCount)
        {
            case 2:
                if (gamepad1.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP1.Count > 0)
                    {
                        if ((placeableObject1 == rpg) || (placeableObject1 == mine) || (placeableObject1 == boost))
                        {

                            itemsP1.RemoveAt(prefabIndex1);
                            if (prefabIndex1 <= (itemsP1.Count))
                            {
                                if (prefabIndex1 != 0)
                                {
                                    prefabIndex1--;
                                }
                            }
                            if (itemsP1.Count >= 1)
                            {

                                placeableObject1 = itemsP1[prefabIndex1];

                            }

                            if (itemsP1.Count == 0)
                            {
                                placeableObject1 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject1.tag = "Item";
                            //make current object copy null
                            //remove item at the index
                            currentPlaceableObject1 = null;
                            itemsP1.RemoveAt(prefabIndex1);

                            if(!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            if (prefabIndex1 <= (itemsP1.Count))
                            {
                                if (prefabIndex1 != 0)
                                {
                                    prefabIndex1--;
                                }
                            }
                            if (itemsP1.Count >= 1)
                            {

                                placeableObject1 = itemsP1[prefabIndex1];

                            }
                        }
                        if (itemsP1.Count == 0)
                        {
                            placeableObject1 = null;

                        }
                    }
                }
                else if (gamepad1.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject1);
                }

                if (gamepad2.GetButtonUp("A") && !cannotPlace2)
                {
                    if (itemsP2.Count > 0)
                    {
                        if ((placeableObject2 == rpg) || (placeableObject2 == mine) || (placeableObject2 == boost))
                        {

                            itemsP2.RemoveAt(prefabIndex2);
                            if (prefabIndex2 <= (itemsP2.Count))
                            {
                                if (prefabIndex2 != 0)
                                {
                                    prefabIndex2--;
                                }
                            }
                            if (itemsP2.Count >= 1)
                            {

                                placeableObject2 = itemsP2[prefabIndex2];

                            }

                            if (itemsP2.Count == 0)
                            {
                                placeableObject2 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject2.tag = "Item";
                            //make current object copy null
                            //remove item at the index
                            currentPlaceableObject2 = null;
                            itemsP2.RemoveAt(prefabIndex2);

                            if (!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            if (prefabIndex2 == (itemsP2.Count))
                            {
                                if (prefabIndex2 != 0)
                                {
                                    prefabIndex2--;
                                }
                            }
                            if (itemsP2.Count >= 1)
                            {

                                placeableObject2 = itemsP2[prefabIndex2];
                            }
                        }
                        if (itemsP2.Count == 0)
                        {
                            placeableObject2 = null;
                        }
                    }
                }
                else if (gamepad2.GetButtonUp("A") && cannotPlace2)
                {
                    Destroy(currentPlaceableObject2);
                }
                break;
            case 3:
            //CHECK THIS TODAY
            //
            //
                if (gamepad1.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP1.Count > 0)
                    {
                        if ((placeableObject1 == rpg) || (placeableObject1 == mine) || (placeableObject1 == boost))
                        {

                            itemsP1.RemoveAt(prefabIndex1);
                            if (prefabIndex1 <= (itemsP1.Count))
                            {
                                if (prefabIndex1 != 0)
                                {
                                    prefabIndex1--;
                                }
                            }
                            if (itemsP1.Count >= 1)
                            {

                                placeableObject1 = itemsP1[prefabIndex1];

                            }

                            if (itemsP1.Count == 0)
                            {
                                placeableObject1 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject1.tag = "Item";
                            //make current object copy null
                            //remove item at the index
                            currentPlaceableObject1 = null;
                            itemsP1.RemoveAt(prefabIndex1);
                            if (prefabIndex1 <= (itemsP1.Count))
                            {
                                if (prefabIndex1 != 0)
                                {
                                    prefabIndex1--;
                                }
                            }
                            if (itemsP1.Count >= 1)
                            {

                                placeableObject1 = itemsP1[prefabIndex1];

                            }
                        }
                        if (itemsP1.Count == 0)
                        {
                            placeableObject1 = null;

                        }
                    }
                }
                else if (gamepad1.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject1);
                }

                if (gamepad2.GetButtonUp("A") && !cannotPlace2)
                {
                    if (itemsP2.Count > 0)
                    {
                        if ((placeableObject2 == rpg) || (placeableObject2 == mine) || (placeableObject2 == boost))
                        {

                            itemsP2.RemoveAt(prefabIndex2);
                            if (prefabIndex2 <= (itemsP2.Count))
                            {
                                if (prefabIndex2 != 0)
                                {
                                    prefabIndex2--;
                                }
                            }
                            if (itemsP2.Count >= 1)
                            {

                                placeableObject2 = itemsP2[prefabIndex2];

                            }

                            if (itemsP2.Count == 0)
                            {
                                placeableObject2 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject2.tag = "Item";
                            //make current object copy null
                            //remove item at the index
                            currentPlaceableObject2 = null;
                            itemsP2.RemoveAt(prefabIndex2);

                            if (!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            if (prefabIndex2 == (itemsP2.Count))
                            {
                                if (prefabIndex2 != 0)
                                {
                                    prefabIndex2--;
                                }
                            }
                            if (itemsP2.Count >= 1)
                            {

                                placeableObject2 = itemsP2[prefabIndex2];
                            }
                        }
                        if (itemsP2.Count == 0)
                        {
                            placeableObject2 = null;
                        }
                    }
                }
                else if (gamepad2.GetButtonUp("A") && cannotPlace2)
                {
                    Destroy(currentPlaceableObject2);
                }
                if (gamepad3.GetButtonUp("A") && !cannotPlace3)
                {
                    if (itemsP3.Count > 0)
                    {
                        if ((placeableObject3 == rpg) || (placeableObject3 == mine) || (placeableObject3 == boost))
                        {

                            itemsP3.RemoveAt(prefabIndex3);
                            if (prefabIndex3 <= (itemsP3.Count))
                            {
                                if (prefabIndex3 != 0)
                                {
                                    prefabIndex3--;
                                }
                            }
                            if (itemsP3.Count >= 1)
                            {

                                placeableObject3 = itemsP3[prefabIndex3];

                            }

                            if (itemsP3.Count == 0)
                            {
                                placeableObject3 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject3.tag = "Item";
                            //make current object copy null
                            //remove item at the index

                            if (!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            currentPlaceableObject3 = null;
                            itemsP3.RemoveAt(prefabIndex3);
                            if (prefabIndex3 == (itemsP3.Count))
                            {
                                if (prefabIndex3 != 0)
                                {
                                    prefabIndex3--;
                                }
                            }
                            if (itemsP3.Count >= 1)
                            {

                                placeableObject3 = itemsP3[prefabIndex3];
                            }
                        }
                        if (itemsP3.Count == 0)
                        {
                            placeableObject3 = null;
                        }
                    }
                }
                else if (gamepad3.GetButtonUp("A") && cannotPlace3)
                {
                    Destroy(currentPlaceableObject3);
                }
                break;
            case 4:
                //CHECK THIS TODAY
                //
                //
                if (gamepad1.GetButtonUp("A") && !cannotPlace1)
                {
                    if (itemsP1.Count > 0)
                    {
                        if ((placeableObject1 == rpg) || (placeableObject1 == mine) || (placeableObject1 == boost))
                        {

                            itemsP1.RemoveAt(prefabIndex1);
                            if (prefabIndex1 <= (itemsP1.Count))
                            {
                                if (prefabIndex1 != 0)
                                {
                                    prefabIndex1--;
                                }
                            }
                            if (itemsP1.Count >= 1)
                            {

                                placeableObject1 = itemsP1[prefabIndex1];

                            }

                            if (itemsP1.Count == 0)
                            {
                                placeableObject1 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject1.tag = "Item";
                            //make current object copy null
                            //remove item at the index

                            if (!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            currentPlaceableObject1 = null;
                            itemsP1.RemoveAt(prefabIndex1);
                            if (prefabIndex1 <= (itemsP1.Count))
                            {
                                if (prefabIndex1 != 0)
                                {
                                    prefabIndex1--;
                                }
                            }
                            if (itemsP1.Count >= 1)
                            {

                                placeableObject1 = itemsP1[prefabIndex1];

                            }
                        }
                        if (itemsP1.Count == 0)
                        {
                            placeableObject1 = null;

                        }
                    }
                }
                else if (gamepad1.GetButtonUp("A") && cannotPlace1)
                {
                    Destroy(currentPlaceableObject1);
                }

                if (gamepad2.GetButtonUp("A") && !cannotPlace2)
                {
                    if (itemsP2.Count > 0)
                    {
                        if ((placeableObject2 == rpg) || (placeableObject2 == mine) || (placeableObject2 == boost))
                        {

                            itemsP2.RemoveAt(prefabIndex2);
                            if (prefabIndex2 <= (itemsP2.Count))
                            {
                                if (prefabIndex2 != 0)
                                {
                                    prefabIndex2--;
                                }
                            }
                            if (itemsP2.Count >= 1)
                            {

                                placeableObject2 = itemsP2[prefabIndex2];

                            }

                            if (itemsP2.Count == 0)
                            {
                                placeableObject2 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject2.tag = "Item";
                            //make current object copy null
                            //remove item at the index

                            if (!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            currentPlaceableObject2 = null;
                            itemsP2.RemoveAt(prefabIndex2);
                            if (prefabIndex2 == (itemsP2.Count))
                            {
                                if (prefabIndex2 != 0)
                                {
                                    prefabIndex2--;
                                }
                            }
                            if (itemsP2.Count >= 1)
                            {

                                placeableObject2 = itemsP2[prefabIndex2];
                            }
                        }
                        if (itemsP2.Count == 0)
                        {
                            placeableObject2 = null;
                        }
                    }
                }
                else if (gamepad2.GetButtonUp("A") && cannotPlace2)
                {
                    Destroy(currentPlaceableObject2);
                }
                if (gamepad3.GetButtonUp("A") && !cannotPlace3)
                {
                    if (itemsP3.Count > 0)
                    {
                        if ((placeableObject3 == rpg) || (placeableObject3 == mine) || (placeableObject3 == boost))
                        {

                            itemsP3.RemoveAt(prefabIndex3);
                            if (prefabIndex3 <= (itemsP3.Count))
                            {
                                if (prefabIndex3 != 0)
                                {
                                    prefabIndex3--;
                                }
                            }
                            if (itemsP3.Count >= 1)
                            {

                                placeableObject3 = itemsP3[prefabIndex3];

                            }

                            if (itemsP3.Count == 0)
                            {
                                placeableObject3 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject3.tag = "Item";
                            //make current object copy null
                            //remove item at the index

                            if (!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            currentPlaceableObject3 = null;
                            itemsP3.RemoveAt(prefabIndex3);
                            if (prefabIndex3 == (itemsP3.Count))
                            {
                                if (prefabIndex3 != 0)
                                {
                                    prefabIndex3--;
                                }
                            }
                            if (itemsP3.Count >= 1)
                            {

                                placeableObject3 = itemsP3[prefabIndex3];
                            }
                        }
                        if (itemsP3.Count == 0)
                        {
                            placeableObject3 = null;
                        }
                    }
                }
                else if (gamepad3.GetButtonUp("A") && cannotPlace3)
                {
                    Destroy(currentPlaceableObject3);
                }
                if (gamepad4.GetButtonUp("A") && !cannotPlace4)
                {
                    if (itemsP4.Count > 0)
                    {
                        if ((placeableObject4 == rpg) || (placeableObject4 == mine) || (placeableObject4 == boost))
                        {

                            itemsP4.RemoveAt(prefabIndex4);
                            if (prefabIndex4 <= (itemsP4.Count))
                            {
                                if (prefabIndex4 != 0)
                                {
                                    prefabIndex4--;
                                }
                            }
                            if (itemsP4.Count >= 1)
                            {

                                placeableObject4 = itemsP4[prefabIndex4];

                            }

                            if (itemsP4.Count == 0)
                            {
                                placeableObject4 = null;

                            }
                        }
                        else
                        {
                            currentPlaceableObject4.tag = "Item";
                            //make current object copy null
                            //remove item at the index

                            if (!swoosh.isPlaying)
                            {
                                swoosh.Play();
                            }
                            currentPlaceableObject4 = null;
                            itemsP4.RemoveAt(prefabIndex4);
                            if (prefabIndex4 == (itemsP4.Count))
                            {
                                if (prefabIndex4 != 0)
                                {
                                    prefabIndex4--;
                                }
                            }
                            if (itemsP4.Count >= 1)
                            {

                                placeableObject4 = itemsP4[prefabIndex4];
                            }
                        }
                        if (itemsP4.Count == 0)
                        {
                            placeableObject4 = null;
                        }
                    }
                }
                else if (gamepad4.GetButtonUp("A") && cannotPlace4)
                {
                    Destroy(currentPlaceableObject4);
                }
                break;
            default:
                break;
        }

    }

    //Randomises items
    //Creates a list of random numbers between 0,3 (trap list size)
    void randomiseItems(int intToRand, List<int> playerList)
    {
        //First 5 are traps, second 5 are items.
        for (int i = 0; i < 6; ++i)
        {
            intToRand = Random.Range(0, 3);
            playerList.Add(intToRand);
        }

    }

    //Allocates random items to player list based on the random numbers generated.
    void allocateRandItems(List<int> numberList, List<GameObject> itemList, List<GameObject> trapList, List<GameObject> playerItemList)
    {
        //Trap Allocation.
        for (int i = 0; i < 3; ++i)
        {
            if (playerItemList.Count <= 12)
            {
                playerItemList.Add(trapList[numberList[i]]);

            }
        }

        //Item allocation.
        for (int i = 3; i < 6; ++i)
        {
            if (playerItemList.Count <= 12)
            {
                playerItemList.Add(itemList[numberList[i]]);
            }
        }
    }

    //Switches between item icons displaying current selected item, previous item and next item.
    //if itemlist index = certain trap or item assign icon.
    void switchItemIcons(int prefabIndex, Image currentItem, List<GameObject> playerItems)
    {
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == buzzsaw)
            {
                currentItem.sprite = buzzsawIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == ramp)
            {
                currentItem.sprite = rampIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == oilslick)
            {
                currentItem.sprite = oilslickIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == rpg)
            {
                currentItem.sprite = rpgIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == mine)
            {
                currentItem.sprite = mineIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count > 0)
            if (playerItems[prefabIndex] == boost)
            {
                currentItem.sprite = boostIcon;
                currentItem.color = new Color(1, 1, 1, 1);
            }
        if (playerItems.Count == 0)
        {
            currentItem.sprite = blankIcon;
            currentItem.color = new Color(0, 0, 0, 0);
        }

    }

    void playerItemCountUi(List<GameObject> playerItems, Image playerSprites, Image playerSprites2)
    {
        switch (playerItems.Count)
        {
            case 0:
                playerSprites.sprite = nums[0];
                playerSprites2.enabled = false;
                break;
            case 1:
                playerSprites.sprite = nums[1];
                playerSprites2.enabled = false;
                break;
            case 2:
                playerSprites.sprite = nums[2];
                playerSprites2.enabled = false;
                break;
            case 3:
                playerSprites.sprite = nums[3];
                playerSprites2.enabled = false;
                break;
            case 4:
                playerSprites.sprite = nums[4];
                playerSprites2.enabled = false;
                break;
            case 5:
                playerSprites.sprite = nums[5];
                playerSprites2.enabled = false;
                break;
            case 6:
                playerSprites.sprite = nums[6];
                playerSprites2.enabled = false;
                break;
            case 7:
                playerSprites.sprite = nums[7];
                playerSprites2.enabled = false;
                break;
            case 8:
                playerSprites.sprite = nums[8];
                playerSprites2.enabled = false;
                break;
            case 9:
                playerSprites.sprite = nums[9];
                playerSprites2.enabled = false;
                break;
            case 10:
                playerSprites2.enabled = true;
                playerSprites.sprite = nums[1];
                playerSprites2.sprite = nums[0];
                break;
            case 11:
                playerSprites2.enabled = true;
                playerSprites.sprite = nums[1];
                playerSprites2.sprite = nums[1];
                break;
            case 12:
                playerSprites2.enabled = true;
                playerSprites.sprite = nums[1];
                playerSprites2.sprite = nums[2];
                break;
            default:
                break;

        }
    }


    void buttonPress(xbox_gamepad gamepad, Image sprite, Image sprite2, Image abutton)
    {
        if(gamepad.GetButton("RB"))
        {
            sprite2.color = Color.grey;
        }
        else
        {
            sprite2.color = Color.white;
        }
        if(gamepad.GetButton("LB"))
        {
            sprite.color = Color.grey;
            
        }
        else
        {
            sprite.color = Color.white;
        }
        if(gamepad.GetButton("A"))
        {
            abutton.color = Color.grey;
        }
        else
        {
            abutton.color = Color.white;
        }

    }

}