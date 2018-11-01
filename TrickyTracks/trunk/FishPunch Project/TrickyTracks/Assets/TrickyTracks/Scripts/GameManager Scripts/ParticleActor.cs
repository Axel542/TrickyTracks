using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Angus Secomb
//Last edited 20/11/2017
public class ParticleActor : MonoBehaviour {

    public ParticleSystem sparksOne;
    public ParticleSystem sparksTwo;

    private ParticleSystem p3;

    private PlayerActor kart;

    public float playTime = 2.0f;

	// Use this for initialization
	void Start () {

        if (this.gameObject.tag == "Player")
        {
            kart = gameObject.GetComponent<PlayerActor>();
        }


	}

    // Update is called once per frame
    void Update()
    {


        if (kart != null)
        {
            if (kart.gamepad.GetButtonDown("B") && kart.kart.IsGrounded)
            {               

                sparksOne.time = playTime;

                sparksOne.Play();



                sparksTwo.time = playTime;

                sparksTwo.Play();


            }
            else if (kart.gamepad.GetButtonUp("B"))
            {
                if(sparksOne.isPlaying)
                {
                    sparksOne.Stop();
                }
                if(sparksTwo.isPlaying)
                {
                    sparksTwo.Stop();
                }

            }
        }
    }
}
