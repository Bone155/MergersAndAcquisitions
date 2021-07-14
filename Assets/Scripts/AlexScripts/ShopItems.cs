using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItems : MonoBehaviour
{
    private static ShopItems instance;
    public TextMeshProUGUI sellPriceText;
    public TextMeshProUGUI sellPriceBooster;
    public TextMeshProUGUI sellPriceReduceTime;
    public string sellPriceString = "$";
    public int reduceTimeMoneyAmount = 20000;
    public int moneyBoostAmount = 20000;
    public int priceIncreaseMoney = 100000;
    public int priceIncreaseReduce = 100000;
    public GameObject storePanel;
    bool doOnce = false;
    private int price;
    private void Start()
    {
        instance = this;
    }
    public void Update()
    {
        if(DisplayBuilding.GetInstance() != null)
        {
            PositionTracker tracker;
            if (DisplayBuilding.GetInstance().panel.activeSelf && DisplayBuilding.GetInstance().target != null)
            {
                if (DisplayBuilding.GetInstance().target.TryGetComponent<PositionTracker>(out tracker))
                {
                    price = 100 * (tracker.tierTracker + 1);
                    if (sellPriceText != null)
                    {
                        sellPriceText.text = $"{sellPriceString}{price}";
                    }
                }
            }
        }
        if(storePanel != null && storePanel.activeSelf && !doOnce)
        {
            if (sellPriceReduceTime != null && CurrencyManager.GetInstance() != null)
            {
                sellPriceReduceTime.text = $"- {reduceTimeMoneyAmount + (priceIncreaseReduce * CurrencyManager.GetInstance().getRT())}";
            }
            if (sellPriceBooster != null && CurrencyManager.GetInstance() != null)
            {
                sellPriceBooster.text = $"- {moneyBoostAmount + (priceIncreaseMoney * CurrencyManager.GetInstance().getMB())}";
            }
            doOnce = true;
        }
        else
        {
            doOnce = false;
        }
    }
    public static ShopItems GetInstance()
    {
        return instance != null ? instance : null;
    }
    public void ReduceTime()
    {
        if(SpawnManager.GetInstance() != null && SpawnManager.GetInstance().decreasePerAction < SpawnManager.GetInstance().time && CurrencyManager.GetInstance() != null)
        {
            long value = reduceTimeMoneyAmount + (priceIncreaseReduce * CurrencyManager.GetInstance().getRT());
            if (CurrencyManager.GetInstance().Get() >= value)
            {
                CurrencyManager.GetInstance().addRT(1);
                if (SpawnManager.GetInstance() != null && CurrencyManager.GetInstance().getRT() < SpawnManager.GetInstance().time)
                {
                    SpawnManager.GetInstance().decreasePerAction = CurrencyManager.GetInstance().getRT();
                    CurrencyManager.GetInstance().Remove(value);
                }
                else if(SpawnManager.GetInstance() != null && CurrencyManager.GetInstance().getRT() >= SpawnManager.GetInstance().time)
                {
                    SpawnManager.GetInstance().decreasePerAction = SpawnManager.GetInstance().time - 1;
                    CurrencyManager.GetInstance().setRT((long)SpawnManager.GetInstance().decreasePerAction);
                }
            }
        }
    }

    public void MoneyBoost()
    {
        if(CurrencyManager.GetInstance() != null)
        {
            long value = moneyBoostAmount + (priceIncreaseMoney * CurrencyManager.GetInstance().getMB());
            if (CurrencyManager.GetInstance() != null && CurrencyManager.GetInstance().Get() >= value)
            {
                CurrencyManager.GetInstance().Remove(value);
                CurrencyManager.GetInstance().addMB(1);
            }
        }
    }

    public void SellBuilding()
    {
        if(DisplayBuilding.GetInstance() != null && DisplayBuilding.GetInstance().target != null)
        {
            CurrencyManager.GetInstance().Add(price);
            Destroy(DisplayBuilding.GetInstance().target);
            DisplayBuilding.GetInstance().panel.SetActive(false);

        }
    }
}
