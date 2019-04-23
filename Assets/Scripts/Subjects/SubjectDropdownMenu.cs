using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using FantomLib;
using System.Globalization;


public class SubjectDropdownMenu : MonoBehaviour
{
    TaskManager taskManager;
    [HideInInspector]
    public Dropdown thisDropdown;
    public Text dateText;
    public Text dateTextExtended;

    public DateTime setDate;
    public string defaultDate = "";

    //int cYear;
    //int cMonth;
    //int cDay;
    string dateFormed;

    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;

        thisDropdown = GetComponent<Dropdown>();

        if(dateText != null)
        {
            dateText.text = DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year;
            dateTextExtended.text = DateTime.Today.DayOfWeek.ToString() + ", " + DateTime.Today.Day + " " + DateTime.Today.ToString("MMMM");
            setDate = DateTime.Now;
        }

        SetOptions();
    }

    void OnEnable()
    {
        SetOptions();
    }

    public void SetOptions()
    {
        //Because OnEnable is called before start, in order to prevent errors, we first have to check if the values in start have been set yet.

        if(taskManager != null)
        {
            thisDropdown.ClearOptions();

            foreach (TaskManager.Subject eachSubject in taskManager.subjects)
            {
                Dropdown.OptionData newData = new Dropdown.OptionData(eachSubject.subjectName);
                thisDropdown.options.Add(newData);
            }

            thisDropdown.RefreshShownValue();
        }
    }

    public void OpenDatePicker()
    {

        AndroidPlugin.ShowDatePickerDialog(DateTime.Today.Day + "/" + DateTime.Today.Month + "/" + DateTime.Today.Year, "dd/MM/yyyy", gameObject.name, "SetDate", "android:Theme.DeviceDefault.Light.Dialog.Alert");

        /*  AndroidNativePopups.OpenDatePickerDialog(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, 
          (int year, int month, int day) => 
          {
              cYear = year;
              cMonth = month + 1;
              cDay = day;

              dateFormed = cDay.ToString() + "/" + cMonth.ToString() + "/" + cYear.ToString();
              dateText.text = dateFormed;

              setDate = new DateTime(cYear, cMonth, cDay);
              dateTextExtended.text = setDate.ToString("dddd") + ", " + cDay + " " + setDate.ToString("MMMM");
          });
          */
    }

    public void OpenDatePicker(string defaultDate)
    {
        this.defaultDate = defaultDate;
        OpenDatePicker();
    }

    public void SetDate(string result)
    {
        dateFormed = result;
        dateText.text = dateFormed;

        setDate = DateTime.ParseExact(result, "dd/MM/yyyy", CultureInfo.CurrentCulture);
        dateTextExtended.text = setDate.ToString("dddd") + ", " + setDate.Day + " " + setDate.ToString("MMMM");
    }
}
