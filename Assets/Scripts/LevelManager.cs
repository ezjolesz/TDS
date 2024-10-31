using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform [] path;
    public Transform StartPoint;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 1000;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to puchase this item");
            return false;
        }
    }
}
