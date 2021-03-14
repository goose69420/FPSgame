using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class pickupController : MonoBehaviour
{
    public GameObject Camera;
    public GameObject shootEffectPrefab;

    public Vector3 offset;
    public bool needsRotation;

    public bool pickupable;
    public bool picked;
    public bool hasAction;

    public float CurrentAmmoInMagazine;
    public float Damage;
    public float MaxAmmo;
    public float ReloadTimeSeconds;
    public float ShotCoolDown;

    public float lastShot;
    public float reloadStartedAt;
    public long DateNow;

    // Start is called before the first frame update
    void Start()
    {
        Camera = FindObjectOfType<MouseLook>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera == null)
        {
            Camera = FindObjectOfType<MouseLook>().gameObject;
        }
        if(picked)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
        if(!picked)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
        if(gameObject.name == "RESPAWN")
        {
            transform.position = FindObjectOfType<GameControl>().MakeVector3Positive(transform.position);
        }
        DateNow = DateTimeOffset.Now.ToUnixTimeSeconds();
        if(DateNow - reloadStartedAt > ReloadTimeSeconds)
        {
            CurrentAmmoInMagazine = MaxAmmo;
        }
        
    }
    public void Action()
    {
        Debug.Log("Стрельнул");
        if (CurrentAmmoInMagazine == 0) return;
        if (DateNow - lastShot < ShotCoolDown) return;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit))
        {
            Debug.Log("Полетел луч))");
            GameObject hitObject = hit.collider.gameObject;
            Shootable shootableController = hitObject.GetComponent<Shootable>();
            Instantiate(shootEffectPrefab, hitObject.transform);
            Debug.Log("DEBUG");
            if (!shootableController) return;
            shootableController.RegisterShoot(Damage);
        };
    }
}
