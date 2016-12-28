using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using TheNextFlow.UnityPlugins;


public class SubjectDropdownMenu : MonoBehaviour
{
    TaskManager taskManager;
    [HideInInspector]
    public Dropdown thisDropdown;
    public Text dateText;
    public Text dateTextExtended;

    public DateTime setDate;

    int cYear;
    int cMonth;
    int cDay;
    string dateFormed;

    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;

        thisDropdown = GetComponent<Dropdown>();

        if(dateText != null)
        {
            dateText.text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            dateTextExtended.text = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMMM");
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
        //
        //Not sure why I need to -1 from the picker month, and +1 on the chosen option.... weird DateTime interactions
        //

        AndroidNativePopups.OpenDatePickerDialog(DateTime.Now.Year, DateTime.Now.Month - 1, DateTime.Now.Day, 
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
    }
}
