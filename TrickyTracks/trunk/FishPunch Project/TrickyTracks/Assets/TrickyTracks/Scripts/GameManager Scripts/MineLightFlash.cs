using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Angus Secomb
//Last edited 28/11/17
public class MineLightFlash : MonoBehaviour {

    private Light redLight;
    public float coolDown = 0.5f;
    private float cdCopy;
    private bool turnOn = false, turnOff = false;
    private AudioSource beep;

    private GameObject[] players;

	// Use this for initialization
	void Start () {
        redLight = GetComponent<Light>();
        cdCopy = coolDown;
        turnOn = true;
        beep = GameObject.Find("MineBeepSound").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {


        players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject obj in players)
        {
            if(Vector3.Distance(obj.transform.position, this.gameObject.transform.position) < 15)
            {

                if (turnOn)
                {
                    if (!beep.isPlaying)
                    {
                        beep.Play();
                    }
                }
                else if (turnOff)
                {
                    if(beep.isPlaying)
                    {
                        beep.Stop();
                    }
                }
            }
        }



        coolDown -= Time.deltaTime;
        if (coolDown < 0)
        {
            coolDown = cdCopy;


            if (turnOn)
            {
                turnOn = false;
                turnOff = true;
            
                redLight.enabled = true;
            }
            else if (turnOff)
            {
                turnOn = true;
                turnOff = false;

                redLight.enabled = false;
            }
        }
	}
}
