using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public float maxHP;
    public float currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void RegisterShoot(float damage)
    {
        currentHP -= damage;
        if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
