using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Angus Secomb
//Last edited 5/11/17
public class BuzzsawActor : MonoBehaviour
{

    private GameObject sawBlade;
    private MeshRenderer bladeRender;
    public MeshCollider bladeCollider;
    private PrefabDisabledActor disableActor;

    private AudioSource buzzsaw;

    private GameObject[] players;

    private float coolDown = 5.0f;
    private int counter = 3;
    public float sawSpeed = 3.0f;
    public float bladeSpinSpeed = 500.0f;
    private bool goLeft = true;
    private bool goRight = false;

    // Use this for initialization
    void Start()
    {
        sawBlade = this.gameObject;
        bladeRender = sawBlade.GetComponentInChildren<MeshRenderer>();
      //  bladeCollider = sawBlade.GetComponentInChildren<MeshCollider>();
        disableActor = sawBlade.GetComponentInParent<PrefabDisabledActor>();
        buzzsaw = GameObject.Find("BuzzsawSound").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        coolDown -= Time.deltaTime;
        players = GameObject.FindGameObjectsWithTag("Player");



        if (coolDown < 0)
        {
            foreach (GameObject obj in players)
            {
                if (Vector3.Distance(obj.transform.position, this.gameObject.transform.position) < 10)
                {
                    if (!buzzsaw.isPlaying)
                    {
                        buzzsaw.Play();
                    }
                }
            }
        }
       
        //  sawBlade.transform.Rotate(5 * Time.deltaTime, 0, 0);
        if (counter < 1)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
        if (disableActor.timer < 0)
        {
            if (goLeft)
            {
                sawBlade.transform.Translate(0, 0, -sawSpeed * Time.deltaTime);
                bladeRender.transform.Rotate(-bladeSpinSpeed * Time.deltaTime, 0, 0);
            //  bladeCollider.transform.Rotate(-bladeSpinSpeed * Time.deltaTime, 0, 0);

            }
            if (goRight)
            {
                sawBlade.transform.Translate(0, 0, sawSpeed * Time.deltaTime);
                bladeRender.transform.Rotate(bladeSpinSpeed * Time.deltaTime, 0, 0);
              //   bladeCollider.transform.Rotate(bladeSpinSpeed * Time.deltaTime, 0, 0);

            }
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {

            PlayerActor kart;
            kart = coll.gameObject.GetComponentInParent<PlayerActor>();

            if (!kart.immuneToDamage)
            {
                counter--;
                kart.playerDisabled = true;
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "Trigger2")
        {
            goLeft = false;
            goRight = true;
        }

        if (coll.gameObject.name == "Trigger1")
        {
            goRight = false;
            goLeft = true;
        }

        //if (coll.gameObject.tag == "Player")
        //{

        //        PlayerActor kart;
        //        kart = coll.gameObject.GetComponentInParent<PlayerActor>();

        //        if (!kart.immuneToDamage)
        //        {
        //            kart.playerDisabled = true;
        //        }
        //}

    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.tag == "Trigger1")
        {
            goRight = true;
        }
    }

}
