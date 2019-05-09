using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxHunger;
    public float maxThirst;

    public float currentHunger;
    public float currentThirst;

    public float hungerIncreaseRate;
    public float thirstIncreaseRate;

    public bool isDead;


    void Start()
    {
    
    }

    void Update()
    {

        checkDeath();

        if (!isDead)
        {
            increaseHunger(hungerIncreaseRate);
            increaseThirst(thirstIncreaseRate);
        }

    }

    public void increaseHunger(float hungerIncreaseRate)
    {

        currentHunger += hungerIncreaseRate * Time.deltaTime;

    }

    public void increaseThirst(float thirstIncreaseRate)
    {
        currentThirst += thirstIncreaseRate * Time.deltaTime;
    }


    public void checkDeath()
    {
        if ((currentThirst >= maxThirst) || (currentHunger >= maxHunger))
        {
            isDead = true;
        }
    }
}
