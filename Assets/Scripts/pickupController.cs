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
    public long ReloadTimeMSeconds;
    public float ShotCoolDown;

    public long lastShot;
    public long reloadStartedAt;
    public long DateNow;

    public bool isGun;

    public bool reloading;

    public Rigidbody rb;
    public BoxCollider boxCollider;
    public MeshCollider meshCollider;

    public Quaternion StartRotation;
    public Quaternion reloadRotation;

    // Start is called before the first frame update
    void Start()
    {
        Camera = FindObjectOfType<MouseLook>().gameObject;
        boxCollider = gameObject.GetComponent<BoxCollider>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
        StartRotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        


        if(Input.GetKeyDown(KeyCode.R) && !reloading && CurrentAmmoInMagazine != MaxAmmo)
        {
            reloadStartedAt = DateNow;
            reloading = true;
        }

        if(Camera == null)
        {
            Camera = FindObjectOfType<MouseLook>().gameObject;
        }
        if(picked)
        {
            rb.isKinematic = true;
            if (boxCollider) boxCollider.enabled = false;
            if (meshCollider) meshCollider.enabled = false;

            gameObject.layer = 2;
        }
        if(!picked)
        {
            rb.isKinematic = false;
            if (boxCollider) boxCollider.enabled = true;
            if (meshCollider) meshCollider.enabled = true;

            gameObject.layer = 0;
        }
        if(gameObject.name == "RESPAWN")
        {
            transform.position = FindObjectOfType<GameControl>().MakeVector3Positive(transform.position);
        }
        DateNow = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if(DateNow - reloadStartedAt > ReloadTimeMSeconds && reloading)
        {
            CurrentAmmoInMagazine = MaxAmmo;
            reloading = false;
        }
        
    }
    public void Action()
    {
        if (!reloading)
        {
            if (CurrentAmmoInMagazine == 0)
            {
                
                reloadStartedAt = DateNow;
                reloading = true;
                return;
            }
            if (DateNow - lastShot < ShotCoolDown) return;
            CurrentAmmoInMagazine--;
            lastShot = DateNow;
            if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out RaycastHit hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                Shootable shootableController = hitObject.GetComponent<Shootable>();
                GameObject Effect = Instantiate(shootEffectPrefab, hit.point, Quaternion.identity);
                Effect.GetComponent<Destroyer>().CreatedAt = DateNow;
                if (!shootableController) return;
                shootableController.RegisterShoot(Damage);

            };
        }
    }
}
