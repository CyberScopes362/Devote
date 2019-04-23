using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class EditTaskAssignment : MonoBehaviour
{
    TaskManager taskManager;
    public List<TaskManager.Assignment> assignmentTasks = new List<TaskManager.Assignment>();
    public List<TaskManager.Subject> subjects = new List<TaskManager.Subject>();
    public GameObject assignmentHolder;
    public GameObject noATObj;

    Color setColor;
    Color colorOrange;
    Color colorOrangeDark;
    Color defDateTextCol;

    bool autoClear;
    bool firstSet = false;


    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;

        colorOrange = new Color(1f, 167f / 255f, 0f);
        colorOrangeDark = new Color(233f / 255f, 105f / 225f, 44f / 225f);
        defDateTextCol = new Color(101f / 255f, 101f / 255f, 101f / 255f);

        //No need to call on start because OnEnable will be called anyway
        Refresh();

        firstSet = true;
    }

    void OnEnable()
    {
        if(firstSet)
            Refresh();
    }

    public void Refresh()
    {
        assignmentTasks = taskManager.assignmentTasks;
        subjects = taskManager.subjects;

        autoClear = taskManager.stgAutoClear;
        int completedCount = 0;

        //Idk how lambda expressions work but theyre damn awesome.
        assignmentTasks = assignmentTasks.OrderBy(x => x.subject).ToList();
        assignmentTasks = assignmentTasks.OrderBy(x => x.dateSet).ToList();
        assignmentTasks = assignmentTasks.OrderByDescending(x => x.isPrioritised).ToList();

        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        if (assignmentTasks.Count == 0)
        {
            var noAssignmentMessage = Instantiate(noATObj);
            noAssignmentMessage.transform.SetParent(gameObject.transform, false);
        }

        foreach (TaskManager.Assignment eachAssignment in assignmentTasks)
        {
            AssignmentTask assignmentComponent;
            assignmentComponent = assignmentHolder.GetComponent<AssignmentTask>();

            //Find subject colour
            foreach (TaskManager.Subject eachSubject in subjects)
            {
                if (eachSubject.subjectName == eachAssignment.subject)
                    setColor = eachSubject.colorCode;
            }

            assignmentComponent.headingText.text = eachAssignment.heading;
            assignmentComponent.descText.text = eachAssignment.description;
            assignmentComponent.subjectText.text = eachAssignment.subject;
            assignmentComponent.subjectText.color = setColor;
            assignmentComponent.sideIndicator.color = setColor;
            assignmentComponent.completionFill.color = setColor;
            assignmentComponent.taskID = eachAssignment.ID;

            //Dates
            DateTime dateSetFixed = new DateTime(eachAssignment.dateSet.Year, eachAssignment.dateSet.Month, eachAssignment.dateSet.Day);
            DateTime todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime tomorrowsDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            DateTime yesterdaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1);

            if (dateSetFixed == todaysDate)
            {
                assignmentComponent.dateText.text = "Due Today";
                assignmentComponent.dateText.color = colorOrangeDark;
            }
            else
            {
                if (dateSetFixed == tomorrowsDate)
                {
                    assignmentComponent.dateText.text = "Due Tomorrow";
                    assignmentComponent.dateText.color = colorOrange;
                }
                else
                {
                    assignmentComponent.dateText.text = "Due on " + eachAssignment.dateSet.ToString("dddd") + ", " + eachAssignment.dateSet.Day + " " + eachAssignment.dateSet.ToString("MMMM");
                    assignmentComponent.dateText.color = defDateTextCol;
                }
            }

            if (dateSetFixed < todaysDate)
            {
                if (dateSetFixed == yesterdaysDate)
                    assignmentComponent.dateText.text = "Overdue - Due Yesterday";
                else
                    assignmentComponent.dateText.text = "Overdue - Due on " + eachAssignment.dateSet.ToString("dddd") + ", " + eachAssignment.dateSet.Day + " " + eachAssignment.dateSet.ToString("MMMM");

                assignmentComponent.dateText.color = Color.red;
            }

            if (!autoClear || eachAssignment.completion != assignmentHolder.GetComponent<AssignmentTask>().completion.maxValue)
            {
                var newAssignment = Instantiate(assignmentHolder);
                newAssignment.transform.SetParent(gameObject.transform, false);

                //Realign heading if no desc
                if (assignmentComponent.descText.text == "")
                {
                    newAssignment.GetComponent<AssignmentTask>().headingText.gameObject.transform.localPosition = new Vector2(newAssignment.GetComponent<AssignmentTask>().headingText.gameObject.transform.localPosition.x, newAssignment.GetComponent<AssignmentTask>().headingText.gameObject.transform.localPosition.y - 50f);
                    newAssignment.GetComponent<AssignmentTask>().headingText.fontSize += 2;
                    Destroy(newAssignment.GetComponent<AssignmentTask>().descText.gameObject);
                }
            }
            else
            {
                completedCount += 1;
            }
        }

        if (completedCount == assignmentTasks.Count && assignmentTasks.Count != 0)
        {
            var noAssignmentMessage = Instantiate(noATObj);
            noAssignmentMessage.transform.SetParent(gameObject.transform, false);
        }
    }
}