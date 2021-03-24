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
    public long ReloadTimeSeconds;
    public float ShotCoolDown;

    public float lastShot;
    public long reloadStartedAt;
    public long DateNow;

    public bool isGun;

    public bool reloading;

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
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.layer = 2;
        }
        if(!picked)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<BoxCollider>().enabled = true;
            gameObject.layer = 0;
        }
        if(gameObject.name == "RESPAWN")
        {
            transform.position = FindObjectOfType<GameControl>().MakeVector3Positive(transform.position);
        }
        DateNow = DateTimeOffset.Now.ToUnixTimeSeconds();
        if(DateNow - reloadStartedAt > ReloadTimeSeconds && reloading)
        {
            CurrentAmmoInMagazine = MaxAmmo;
            reloading = false;
        }
        
    }
    public void Action()
    {
        if (!reloading)
        {

            CurrentAmmoInMagazine--;
            if (CurrentAmmoInMagazine == 0)
            {
                reloadStartedAt = DateNow;
                reloading = true;
                return;
            }
            if (DateNow - lastShot < ShotCoolDown) return;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                Shootable shootableController = hitObject.GetComponent<Shootable>();
                GameObject Effect = Instantiate(shootEffectPrefab, hit.point, Quaternion.identity);
                Effect.GetComponent<Destroyer>().CreatedAt = DateNow;
                Debug.Log("DEBUG");
                if (!shootableController) return;
                shootableController.RegisterShoot(Damage);

            };
        }
    }
}
