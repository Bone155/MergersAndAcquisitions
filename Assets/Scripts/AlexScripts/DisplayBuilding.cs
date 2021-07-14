using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBuilding : MonoBehaviour
{
    public GameObject panel;
    public Image buildingImage;
    public TextMeshProUGUI display;
    public Camera camera;
    PositionTracker tracker;
    private static DisplayBuilding instance;
    public GameObject target;
    private void Start()
    {
        instance = this;
        panel.SetActive(false);
    }
    private void CloseIfOutSide()
    {
        if (Control.Instance() != null && Control.Instance().isTouchEnded())
        {
            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            if (camera != null && !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Control.Instance().TouchPos(), camera))
            {
                panel.SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Control.Instance() != null && Control.Instance().GetTarget() != null)
        {
            panel.SetActive(true);
            buildingImage.sprite = Control.Instance().GetTarget().GetComponent<SpriteRenderer>().sprite;
            Control.Instance().GetTarget().TryGetComponent<PositionTracker>(out tracker);
            display.text = $"Type: {tracker.buildingType} \n Tier: {tracker.tier + 1}";
            target = Control.Instance().GetTarget().gameObject;
        }
        else
        {
            CloseIfOutSide();
        }

    }
    public static DisplayBuilding GetInstance()
    {
        return instance != null ? instance : null;
    }
}