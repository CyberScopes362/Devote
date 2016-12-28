using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class EditTaskHomework : MonoBehaviour
{
    TaskManager taskManager;
    public List<TaskManager.Homework> homeworkTasks = new List<TaskManager.Homework>();
    public List<TaskManager.Subject> subjects = new List<TaskManager.Subject>();
    public GameObject homeworkHolder;
    public GameObject noHWObj;

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
        homeworkTasks = taskManager.homeworkTasks;
        subjects = taskManager.subjects;

        autoClear = taskManager.stgAutoClear;
        int completedCount = 0;

        //Set Order of homeworkTasks based on date.
        //Idk how lambda expressions work but theyre damn awesome.

        homeworkTasks = homeworkTasks.OrderBy(x => x.subject).ToList();
        homeworkTasks = homeworkTasks.OrderBy(x => x.dateSet).ToList();
        homeworkTasks = homeworkTasks.OrderBy(x => x.isComplete).ToList();
        homeworkTasks = homeworkTasks.OrderByDescending(x => x.isPrioritised).ToList();

        //Old lambda system
        //homeworkTasks.Sort((x, y) => x.dateSet.CompareTo(y.dateSet));
        //homeworkTasks.Sort((x, y) => y.isPrioritised.CompareTo(x.isPrioritised));


        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        if (homeworkTasks.Count == 0)
        {
            var noHomeworkMessage = Instantiate(noHWObj);
            noHomeworkMessage.transform.SetParent(gameObject.transform, false);
        }

        foreach (TaskManager.Homework eachHomework in homeworkTasks)
        {
            HomeworkTask homeworkComponent;
            homeworkComponent = homeworkHolder.GetComponent<HomeworkTask>();

            //Find subject colour
            foreach(TaskManager.Subject eachSubject in subjects)
            {
                if (eachSubject.subjectName == eachHomework.subject)
                    setColor = eachSubject.colorCode;
            }

            homeworkComponent.headingText.text = eachHomework.heading;
            homeworkComponent.descText.text = eachHomework.description;
            homeworkComponent.subjectText.text = eachHomework.subject;
            homeworkComponent.subjectText.color = setColor;
            homeworkComponent.sideIndicator.color = setColor;
            homeworkComponent.taskID = eachHomework.ID;

            //Dates
            DateTime dateSetFixed = new DateTime(eachHomework.dateSet.Year, eachHomework.dateSet.Month, eachHomework.dateSet.Day);
            DateTime todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime tomorrowsDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            DateTime yesterdaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-1);

            if (dateSetFixed == todaysDate)
            {
                homeworkComponent.dateText.text = "Due Today";
                homeworkComponent.dateText.color = colorOrangeDark;
            }
            else
            {
                if (dateSetFixed == tomorrowsDate)
                {
                    homeworkComponent.dateText.text = "Due Tomorrow";
                    homeworkComponent.dateText.color = colorOrange;
                }
                else
                {
                    homeworkComponent.dateText.text = "Due on " + eachHomework.dateSet.ToString("dddd") + ", " + eachHomework.dateSet.Day + " " + eachHomework.dateSet.ToString("MMMM");
                    homeworkComponent.dateText.color = defDateTextCol;
                }
            }

            if(dateSetFixed < todaysDate)
            {
                if (dateSetFixed == yesterdaysDate)
                    homeworkComponent.dateText.text = "Overdue (Due Yesterday)";
                else
                    homeworkComponent.dateText.text = "Overdue (Due on " + eachHomework.dateSet.ToString("dddd") + ", " + eachHomework.dateSet.Day + " " + eachHomework.dateSet.ToString("MMMM") + ")";

                homeworkComponent.dateText.color = Color.red;
            }

            if(!autoClear || !eachHomework.isComplete)
            {
                var newHomework = Instantiate(homeworkHolder);
                newHomework.transform.SetParent(gameObject.transform, false);

                //Realign heading if no desc
                if (homeworkComponent.descText.text == "")
                {
                    newHomework.GetComponent<HomeworkTask>().headingText.gameObject.transform.localPosition = new Vector2(newHomework.GetComponent<HomeworkTask>().headingText.gameObject.transform.localPosition.x, newHomework.GetComponent<HomeworkTask>().headingText.gameObject.transform.localPosition.y - 50f);
                    newHomework.GetComponent<HomeworkTask>().headingText.fontSize += 2;
                    Destroy(newHomework.GetComponent<HomeworkTask>().descText.gameObject);
                }
            }
            else
            {
                completedCount += 1;
            }
        }



        if(completedCount == homeworkTasks.Count && homeworkTasks.Count != 0)
        {
            var noHomeworkMessage = Instantiate(noHWObj);
            noHomeworkMessage.transform.SetParent(gameObject.transform, false);
        }


    }
}