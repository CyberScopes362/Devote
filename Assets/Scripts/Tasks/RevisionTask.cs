using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class RevisionTask : MonoBehaviour
{
    TaskManager taskManager;
    ActionController actionController;

    public Text mainText;
    public Text subjectText;
    public Image sideIndicator;
    public Image subIcon;
    public int taskID;

  //  public Button confirmButton;

    Color subjectColor;

    public Button prioritiseButton;
    public Image priorityBorder;
    public bool isPrioritised;

    bool isDeleted;
    float timerDestroy;


    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;
        actionController = initializer.actionController;

        priorityBorder.color = Color.clear;

        foreach (TaskManager.Revision eachRevision in taskManager.revisionTasks)
        {
            if (eachRevision.ID == taskID)
            {
                isPrioritised = eachRevision.isPrioritised;
                break;
            }
        }

        //Now based off subjects and priorities instead.
        /*   if (mainText.text.Contains("Update Notes"))
                transform.SetAsFirstSibling();

            if (mainText.text.Contains("Ongoing Task"))
                transform.SetAsLastSibling();
        */
    }

    void Update()
    {
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


    public void OnClickPrioritise()
    {
        if (!isPrioritised)
            isPrioritised = true;
        else
            isPrioritised = false;

        foreach (TaskManager.Revision eachRevision in taskManager.revisionTasks)
        {
            if (eachRevision.ID == taskID)
            {
                eachRevision.isPrioritised = isPrioritised;

                if (isPrioritised)
                    actionController.Toast("Prioritised Study Task for " + eachRevision.subject);
                else
                    actionController.Toast("Unprioritised Study Task for " + eachRevision.subject);
                break;
            }
        }

        taskManager.UpdatePriorityCount();
        actionController.editTaskRV.Refresh();
        taskManager.SaveAllContent();
        //Maybe add toast (?) + Needs to initiate update for the priority counter
    }

    public void Unprioritise()
    {
        isPrioritised = false;

        foreach (TaskManager.Revision eachRevision in taskManager.revisionTasks)
        {
            if (eachRevision.ID == taskID)
            {
                eachRevision.isPrioritised = false;
                taskManager.UpdatePriorityCount();
                taskManager.SaveAllContent();
                break;
            }
        }
    }


    public void OnClickCompleteTask()
    {
        foreach (TaskManager.Revision eachRevision in taskManager.revisionTasks)
        {
            if (eachRevision.ID == taskID)
            {
                eachRevision.isComplete = true;
                break;
            }
        }

        taskManager.UpdateProductivity();
        actionController.OnClick_DeleteRevision(taskID, 1);
        isDeleted = true;
    }
  
    public void OnClickDeleteTask()
    {
        actionController.OnClick_DeleteRevision(taskID, 0);
        isDeleted = true;
    }
}