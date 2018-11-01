using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written By Angus Secomb
//Last edited 28/11/17
public class MineActor : MonoBehaviour {

    private PlayerActor kart;
    private AudioSource mineBeep;
    private AudioSource crash;

	// Use this for initialization
	void Start () {
        mineBeep = GameObject.Find("MineSound").GetComponent<AudioSource>();
        crash = GameObject.Find("ExplosionSound").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
     
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            kart = coll.gameObject.GetComponentInParent<PlayerActor>();
            if(!mineBeep.isPlaying)
            {
                mineBeep.Play();
            }
            if(!crash.isPlaying)
            {
                crash.Play();
            }
            if (!kart.immuneToDamage)
            {
                kart.playerDisabled = true;
            }
                GameObject.Destroy(this.gameObject.transform.parent.gameObject);
        }
        if(coll.gameObject.tag == "RPG")
        { 
            GameObject.Destroy(coll.gameObject.transform.parent.gameObject);
            GameObject.Destroy(this.gameObject.transform.parent.gameObject);
        }

        if (coll.gameObject.layer == 9)
        {
            Debug.Log("it's doing that thing i want");
        }

    }
}
