using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    float hitPoints = 100f;

    void Hit(float rawDamage)
    {
        
        hitPoints -= rawDamage;

        Debug.Log("OUCH: " + hitPoints.ToString());

        if (hitPoints <= 0)
        {
            Debug.Log("TODO: GAME OVER - YOU DIED");
            
        }
    }
}