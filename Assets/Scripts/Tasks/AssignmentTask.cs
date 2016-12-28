using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class AssignmentTask : MonoBehaviour
{
    TaskManager taskManager;
    ActionController actionController;

    public Text headingText;
    public Text subjectText;
    public Text descText;
    public Text dateText;
    public Image sideIndicator;
    public Slider completion;
    public Text completionText;
    public Image completionBG;
    public Image completionFill;
    public Image buttonMaskImage;
    public RectTransform sectionsTextParent;
    public GameObject sectionTextPrefab;
    public int taskID;

    public Button deleteButton;
    public Button editButton;

    Color darkGreen;
    Color lightGrey;
    Color subjectColor;
    int parts;

    public Text[] letters;

    public Button prioritiseButton;
    public Image priorityBorder;

    [HideInInspector]
    public bool isPrioritised;

    bool extOptions;

    bool isDeleted;
    float timerDestroy;


    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;
        actionController = initializer.actionController;

        lightGrey = new Color(0.75f, 0.75f, 0.75f);
        darkGreen = new Color(0, 176f / 255f, 9f / 255f);

        priorityBorder.color = Color.clear;

        foreach (TaskManager.Assignment eachAssignment in taskManager.assignmentTasks)
        {
            if (eachAssignment.ID == taskID)
            {
                isPrioritised = eachAssignment.isPrioritised;

                completion.value = eachAssignment.completion;
                completion.maxValue = eachAssignment.parts;
                parts = eachAssignment.parts;

                if (completion.value == completion.maxValue)
                    transform.SetAsLastSibling();
                break;
            }
        }

        if(parts != 100 && parts <= 10)
        {
            letters = new Text[parts]; 

            float widthSeparate = sectionsTextParent.sizeDelta.x / parts;

            char letter = 'A';

            for(int i = 1; i <= parts; i++)
            {
                sectionTextPrefab.GetComponent<Text>().rectTransform.sizeDelta = new Vector2(widthSeparate, sectionTextPrefab.GetComponent<Text>().rectTransform.sizeDelta.y);
                sectionTextPrefab.GetComponent<Text>().text = letter.ToString();
                var newLetter = Instantiate(sectionTextPrefab);
                newLetter.transform.SetParent(sectionsTextParent, false);
                newLetter.transform.localPosition = new Vector2(widthSeparate * i - (widthSeparate/2f), newLetter.transform.localPosition.y);

                letters[i - 1] = newLetter.GetComponent<Text>();

                if (i <= completion.value)
                    letters[i - 1].color = darkGreen;
                else
                    letters[i - 1].color = Color.grey;

                letter = (char)(((int)letter) + 1);
            }
        }

        //These must both also be executed on start otherwise there will be render flash with weird values.
        completionText.text = Mathf.CeilToInt((float)completion.value * (100f / parts)).ToString() + "%";
        completionBG.color = Color.Lerp(lightGrey, Color.green, completion.value * (100f / parts) / 150f); //150 because we dont want a full green
    }

    void Update()
    {
        //Ceil as a float value
        completionText.text = Mathf.CeilToInt((float)completion.value * (100f / parts)).ToString() + "%";
        completionBG.color = Color.Lerp(lightGrey, Color.green, completion.value * (100f / parts) / 150f); //150 because we dont want a full green

        if (extOptions)
            buttonMaskImage.fillAmount = Mathf.Lerp(buttonMaskImage.fillAmount, 1, 7f * Time.deltaTime);
        else
            buttonMaskImage.fillAmount = Mathf.Lerp(buttonMaskImage.fillAmount, 0, 7f * Time.deltaTime);

        if (isPrioritised)
            priorityBorder.color = Color.Lerp(priorityBorder.color, Color.white, 5f * Time.deltaTime);
        else
            priorityBorder.color = Color.Lerp(priorityBorder.color, Color.clear, 5f * Time.deltaTime);

        if (isDeleted)
        {
            timerDestroy += Time.deltaTime;

            foreach (Transform child in transform)
            {
                child.transform.position = Vector2.Lerp(child.transform.position, new Vector2(-Screen.width * 6f, child.transform.position.y), Time.deltaTime);

                Text[] allChildTexts = child.GetComponentsInChildren<Text>();
                Image[] allChildImages = child.GetComponentsInChildren<Image>();

                foreach (Text tCom in allChildTexts)
                    tCom.color = Color.Lerp(tCom.color, Color.clear, 20f * Time.deltaTime);

                foreach (Image iCom in allChildImages)
                    iCom.color = Color.Lerp(iCom.color, Color.clear, 20f * Time.deltaTime);
            }

            if (timerDestroy > 0.2f)
                actionController.GeneralFunctions();
        }
    }

    public void OnSliderValueChanged()
    {
        foreach (TaskManager.Assignment eachAssignment in taskManager.assignmentTasks)
        {
            if (eachAssignment.ID == taskID)
            {
                eachAssignment.completion = Mathf.CeilToInt((float)completion.value);

                if (eachAssignment.completion == completion.maxValue)
                    Unprioritise();
                break;
            }
        }

        //Change letter colours
        if (parts != 100 && parts <= 10)
        {
            for (int i = 1; i <= parts; i++)
            {
                if (i <= completion.value)
                    letters[i - 1].color = darkGreen;
                else
                    letters[i - 1].color = Color.grey;
            }
        }

        taskManager.SaveAllContent();
        taskManager.UpdateProductivity();
    }


    public void OnClickPrioritise()
    {
        if (completion.value != completion.maxValue)
        {
            if (!isPrioritised)
                isPrioritised = true;
            else
                isPrioritised = false;

            foreach (TaskManager.Assignment eachAssignment in taskManager.assignmentTasks)
            {
                if (eachAssignment.ID == taskID)
                {
                    eachAssignment.isPrioritised = isPrioritised;

                    if (isPrioritised)
                        actionController.Toast("Prioritised Assignment Task:\n" + eachAssignment.heading);
                    else
                        actionController.Toast("Unprioritised Assigment Task:\n" + eachAssignment.heading);
                    break;
                }
            }

            taskManager.UpdatePriorityCount();
            actionController.editTaskAT.Refresh();
            taskManager.SaveAllContent();
            //Maybe add toast (?) + Needs to initiate update for the priority counter
        }
        else
            actionController.Error("Cannot Prioritise Completed Tasks.");
    }

    public void Unprioritise()
    {
        isPrioritised = false;

        foreach (TaskManager.Assignment eachAssignment in taskManager.assignmentTasks)
        {
            if (eachAssignment.ID == taskID)
            {
                eachAssignment.isPrioritised = false;
                taskManager.UpdatePriorityCount();
                taskManager.SaveAllContent();
                break;
            }
        }
    }



    public void OnClickEditMain()
    {
        if (!extOptions)
        {
            deleteButton.interactable = true;
            editButton.interactable = true;
            prioritiseButton.interactable = true;
            extOptions = true;
        }
        else
        {
            deleteButton.interactable = false;
            editButton.interactable = false;
            prioritiseButton.interactable = false;
            extOptions = false;
        }
    }

    public void OnClickEditTask()
    {
        OnClickEditMain();
        actionController.OnClick_EditTaskATOpen(taskID);
        taskManager.ShowCurrentPage(5);
    }

    public void OnClickDeleteTask()
    {
        actionController.OnClick_DeleteAssignment(taskID);
        isDeleted = true;
    }
}