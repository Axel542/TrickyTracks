using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 28/11/17.
public class BoostBuzzsaw : MonoBehaviour {

    public float boostValue = 80.0f;
    public float boostTime = 2.0f;
    public float boostRespawnTime = 15.0f;

    public ParticleSystem particle;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        boostRespawnTime -= Time.deltaTime;
      


	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            if (boostRespawnTime < 0)
            {
                boostRespawnTime = 15.0f;
                coll.gameObject.GetComponentInParent<PlayerActor>().kart.SpeedBoost(boostValue, 2, boostTime, 1);
                if (!coll.gameObject.GetComponentInParent<PlayerActor>().boost1.isPlaying)
                {
                    coll.gameObject.GetComponentInParent<PlayerActor>().boost1.Play();
                }
                
                if(!coll.gameObject.GetComponentInParent<PlayerActor>().boost2.isPlaying)
                {
                    coll.gameObject.GetComponentInParent<PlayerActor>().boost2.Play();
                }
            }
        }
    }
}
