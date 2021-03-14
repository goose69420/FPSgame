using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Destroyer : MonoBehaviour
{
    public long CreatedAt;
    public float LifeTime;
    public long DateNow;

    // Update is called once per frame
    void Update()
    {
        DateNow = DateTimeOffset.Now.ToUnixTimeSeconds();
        if (CreatedAt + LifeTime > DateNow)
        {
            Destroy(gameObject);
        }
    }
}
