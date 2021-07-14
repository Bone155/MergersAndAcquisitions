using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public TextMeshProUGUI rt;
    public TextMeshProUGUI mb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrencyManager.GetInstance() != null)
        {
            rt.text = $"RT: {CurrencyManager.GetInstance().getRT()}";
            mb.text = $"MB: {CurrencyManager.GetInstance().getMB()}";
        }
    }
}
