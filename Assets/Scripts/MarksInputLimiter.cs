using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class MarksInputLimiter : MonoBehaviour
{
    InputField thisInputField;

    void Start()
    {
        thisInputField = GetComponent<InputField>();
    }

    public void OnValueChanged()
    {
        if (thisInputField.text != "")
        {
            if (Convert.ToInt32(thisInputField.text) > 100)
                thisInputField.text = "100";
        }
    }
}
