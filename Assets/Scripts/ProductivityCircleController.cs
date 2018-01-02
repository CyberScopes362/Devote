using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

//Now also controls the priority indicator movement and what not

public class ProductivityCircleController : MonoBehaviour
{
    TaskManager taskManager;

    Image blind;
    public Image blindBG;
    public Image proCircle;
    public Image proCircleEx;
    public Text proText;
    public Text proUpdateText;

    public Color green;
    public Color grey;

    float productivityLevel;
    float productivityChanged;

    DateTime previousDate;

    float realUseValue;
    float changedAmount;

    float tempUseValue;

    public Color priorityRed;
    public Color priorityGrey;
    Color setColorPriority;

    public GameObject priorityIndicator;
    Image priorityBG;
    public Text priorityText;

    bool priorityIndicatorOut;
    public float xOutIndicator;
    public float xDefIndicator;
    float setXIndicator;

    int priorityCount;


    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;

        blind = GetComponent<Image>();

        productivityLevel = taskManager.totalProLevel;
        productivityChanged = taskManager.totalProChangeAmount;
        priorityBG = priorityIndicator.GetComponent<Image>();

        realUseValue = Mathf.Clamp(Mathf.CeilToInt(productivityLevel * 100f), 0f, 200f);
        changedAmount = Mathf.CeilToInt(productivityChanged * 100f);

        proText.text = realUseValue.ToString() + "%";

        if(Convert.ToInt32(changedAmount) < 0)
            proUpdateText.text = changedAmount.ToString() + "%";
        else
        {
            if(Convert.ToInt32(changedAmount) == 0)
                proUpdateText.text = "--%";
            else
                proUpdateText.text = "+" + changedAmount.ToString() + "%";
        }

        setXIndicator = xDefIndicator;

        priorityCount = taskManager.priorityCount;

        if (priorityCount == 0)
            priorityIndicatorOut = false;
        else
            priorityIndicatorOut = true;
    }

    public void ZeroSet()
    {
        proCircle.fillAmount = 0;
        proCircleEx.fillAmount = 0;
        blind.color = grey;
        blindBG.color = blind.color;
        tempUseValue = 0f;
    }

    void Update()
    {
        priorityCount = taskManager.priorityCount;

        proCircle.fillAmount = Mathf.Lerp(proCircle.fillAmount, realUseValue / 100f, 1.5f * Time.deltaTime);
        proCircleEx.fillAmount = Mathf.Lerp(proCircleEx.fillAmount, (realUseValue / 100f) - 1f, 1.5f * Time.deltaTime);

        blind.color = Color.Lerp(blind.color, Color.Lerp(grey, green, realUseValue / 100f), 1.5f * Time.deltaTime);
        blindBG.color = blind.color;

        tempUseValue = Mathf.Lerp(tempUseValue, realUseValue, 1.5f * Time.deltaTime);
        
        proText.text = Mathf.CeilToInt(tempUseValue).ToString() + "%";

        //Priority Indicator Stuff
        if (priorityIndicatorOut)
            setXIndicator = xOutIndicator;
        else
            setXIndicator = xDefIndicator;

        if (priorityCount == 0)
        {
            setColorPriority = priorityGrey;
            priorityText.text = "No Priorities Set!";
        }
        else
        {
            setColorPriority = priorityRed;

            if(priorityCount == 1)
                priorityText.text = "<b>One</b> Priority Remaining";
            else
                priorityText.text = "<b>" + priorityCount + "</b> Priorities Remaining";
        }

        priorityIndicator.transform.localPosition = Vector2.Lerp(priorityIndicator.transform.localPosition, new Vector2(setXIndicator, priorityIndicator.transform.localPosition.y), 8f * Time.deltaTime);
        priorityBG.color = Color.Lerp(priorityBG.color, setColorPriority, 8f * Time.deltaTime);
    }

    public void OnClickPriorityIndicator()
    {
        if (priorityIndicatorOut)
            priorityIndicatorOut = false;
        else
            priorityIndicatorOut = true;
    }
}