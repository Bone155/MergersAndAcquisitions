using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public TextMeshProUGUI display;
    long currency;
    long reduceTime = 1;
    long moneyBooster = 1;
    private static CurrencyManager instance;
    // Update is called once per frame
    void Start()
    {
        instance = this;
        currency = 0;
        reduceTime = 1;
        moneyBooster = 1;
    }
    void Update()
    {
        if(currency < 0)
        {
            currency = 0;
        }
        if(reduceTime < 1)
        {
            reduceTime = 1;
        }
        if(moneyBooster < 1)
        {
            moneyBooster = 1;
        }
        display.text = $"${currency}";
    }
    public static CurrencyManager GetInstance()
    {
        return instance != null ? instance : null;
    }
    public static bool InstanceActive()
    {
        if (GetInstance() != null) return true;
        else return false;
    }
    // adds it to the currency with a multiplier
    public bool Add(long amount)
    {
        currency += amount * moneyBooster;
        return true;
    }
    // sets the currency
    public bool Set(long amount)
    {
        currency = amount;
        return true;
    }
    // gets the current currency
    public long Get()
    {
        return currency;
    }
    // removes the amount from the currency
    public bool Remove(long amount)
    {
        if (currency - amount >= 0)
        {
            currency -= amount;
            return true;
        }
        return false;
    }
    //returns number of reduce time boosters
    public long getRT()
    {
        return reduceTime;
    }
    //adds total reduce time boosters
    public bool addRT(long num)
    {
        reduceTime += num;
        return true;
    }
    public bool setRT(long num)
    {
        reduceTime = num;
        return true;
    }
    //removes one total reduce time boosters
    public void removeRT()
    {
        reduceTime--;
    }
    //returns number of money boosters
    public long getMB()
    {
        return moneyBooster;
    }
    //adds total money boosters
    public bool addMB(long num)
    {
        moneyBooster += num;
        return true;
    }
    public bool setMB(long num)
    {
        moneyBooster = num;
        return true;
    }
    //removes one total money boosters
    public void removeMB()
    {
        moneyBooster--;
    }
}
