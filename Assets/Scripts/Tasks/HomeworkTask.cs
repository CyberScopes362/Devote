using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class HomeworkTask : MonoBehaviour
{
    TaskManager taskManager;
    ActionController actionController;

    public Text headingText;
    public Text subjectText;
    public Text descText;
    public Text dateText;
    public Image sideIndicator;
    public Image checkboxTick;
    public Image buttonMaskImage;
    public int taskID;

    public Button deleteButton;
    public Button editButton;
    public Button prioritiseButton;

    public Image priorityBorder;

    Color subjectColor;
    [HideInInspector]
    public bool isComplete;
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

        priorityBorder.color = Color.clear;

        foreach (TaskManager.Homework eachHomework in taskManager.homeworkTasks)
        {
            if (eachHomework.ID == taskID)
            {
                isComplete = eachHomework.isComplete;
                isPrioritised = eachHomework.isPrioritised;

                if (isComplete)
                    checkboxTick.fillAmount = 1;               
                break;
            }
        }
    }

    void Update()
    {
        if (isComplete)
            checkboxTick.fillAmount = Mathf.Lerp(checkboxTick.fillAmount, 1, 7f * Time.deltaTime);
        else
            checkboxTick.fillAmount = Mathf.Lerp(checkboxTick.fillAmount, 0, 7f * Time.deltaTime);

        if (isPrioritised)
            priorityBorder.color = Color.Lerp(priorityBorder.color, Color.white, 5f * Time.deltaTime);
        else
            priorityBorder.color = Color.Lerp(priorityBorder.color, Color.clear, 5f * Time.deltaTime);

        if (extOptions)
            buttonMaskImage.fillAmount = Mathf.Lerp(buttonMaskImage.fillAmount, 1, 7f * Time.deltaTime);
        else
            buttonMaskImage.fillAmount = Mathf.Lerp(buttonMaskImage.fillAmount, 0, 7f * Time.deltaTime);

        if(isDeleted)
        {
            timerDestroy += Time.deltaTime;

            foreach (Transform child in transform)
            {
                child.transform.position = Vector2.Lerp(child.transform.position, new Vector2(-Screen.width * 6f, child.transform.position.y), Time.deltaTime);

                Text[] allChildTexts = child.GetComponentsInChildren<Text>();
                Image[] allChildImages = child.GetComponentsInChildren<Image>();

                foreach(Text tCom in allChildTexts)
                    tCom.color = Color.Lerp(tCom.color, Color.clear, 20f * Time.deltaTime);

                foreach(Image iCom in allChildImages)
                    iCom.color = Color.Lerp(iCom.color, Color.clear, 20f * Time.deltaTime);
            }
                

            if(timerDestroy > 0.2f)
                actionController.GeneralFunctions();
        }
    }

    public void OnClickCheckbox()
    {
        if (!isComplete)
        {
            isComplete = true;

            if(isPrioritised)
                Unprioritise();
        }
        else
            isComplete = false;

        foreach (TaskManager.Homework eachHomework in taskManager.homeworkTasks)
        {
            if (eachHomework.ID == taskID)
            {
                eachHomework.isComplete = isComplete;
                break;
            }
        }

        taskManager.SaveAllContent();
        taskManager.UpdateProductivity();
    }

    public void OnClickPrioritise()
    {
        if(!isComplete)
        {
            if (!isPrioritised)
                isPrioritised = true;
            else
                isPrioritised = false;

            foreach (TaskManager.Homework eachHomework in taskManager.homeworkTasks)
            {
                if (eachHomework.ID == taskID)
                {
                    eachHomework.isPrioritised = isPrioritised;

                    if(isPrioritised)
                        actionController.Toast("Prioritised Homework Task:\n" + eachHomework.heading);
                    else
                        actionController.Toast("Unprioritised Homework Task:\n" + eachHomework.heading);
                    break;
                }
            }

            taskManager.UpdatePriorityCount();
            actionController.editTaskHW.Refresh();
            taskManager.SaveAllContent();
            //Maybe add toast (?) + Needs to initiate update for the priority counter
        }
        else
            actionController.Error("Cannot Prioritise Completed Tasks.");

    }

    public void Unprioritise()
    {
        isPrioritised = false;

        foreach (TaskManager.Homework eachHomework in taskManager.homeworkTasks)
        {
            if (eachHomework.ID == taskID)
            {
                eachHomework.isPrioritised = false;
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
        actionController.OnClick_EditTaskHWOpen(taskID);
        taskManager.ShowCurrentPage(3);
    }

    public void OnClickDeleteTask()
    {
        actionController.OnClick_DeleteHomework(taskID);
        isDeleted = true;
    }
}