using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class EditTaskRevision : MonoBehaviour
{
    TaskManager taskManager;
    public List<TaskManager.Revision> revisionTasks = new List<TaskManager.Revision>();
    public List<TaskManager.Subject> subjects = new List<TaskManager.Subject>();
    public GameObject revisionHolder;
    public GameObject noRVObj;

    public Sprite[] subIconsForStudy;
    public Color colorRevise;
    public Color colorNotes;
    public Color colorOngoing;

    Color setColor;

    bool firstSet = false;

    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;

        //No need to call on start because OnEnable will be called anyway
        Refresh();

        firstSet = true;
    }

    void OnEnable()
    {
        if (firstSet)
            Refresh();
    }

    public void Refresh()
    {
        revisionTasks = taskManager.revisionTasks;
        subjects = taskManager.subjects;

        //Idk how lambda expressions work but theyre damn awesome.
        //Sort based on subject alphabetical order
        revisionTasks = revisionTasks.OrderBy(x => x.subject).ToList();
        revisionTasks = revisionTasks.OrderByDescending(x => x.isPrioritised).ToList();

        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        if (revisionTasks.Count == 0)
        {
            var noRevisionMessage = Instantiate(noRVObj);
            noRevisionMessage.transform.SetParent(gameObject.transform, false);
        }

        foreach (TaskManager.Revision eachRevision in revisionTasks)
        {
            RevisionTask revisionComponent;
            revisionComponent = revisionHolder.GetComponent<RevisionTask>();

            //Find subject colour
            foreach (TaskManager.Subject eachSubject in subjects)
            {
                if (eachSubject.subjectName == eachRevision.subject)
                    setColor = eachSubject.colorCode;
            }

            revisionComponent.mainText.text = eachRevision.mainText;
            revisionComponent.subjectText.text = eachRevision.subject;
            revisionComponent.subjectText.color = setColor;
            revisionComponent.sideIndicator.color = setColor;
            revisionComponent.taskID = eachRevision.ID;

            //Set revision icon thingy
            if(revisionComponent.mainText.text.Contains("<i>Revise"))
            {
                if (revisionComponent.mainText.text.Contains("<color=black><b><i>Revise: </i></b></color>"))
                    revisionComponent.mainText.text = revisionComponent.mainText.text.Replace("<color=black><b><i>Revise: </i></b></color>", "");

                if (revisionComponent.mainText.text.Contains("<color=black><b><i>Revise</i></b></color>"))
                    revisionComponent.mainText.text = revisionComponent.mainText.text.Replace("<color=black><b><i>Revise</i></b></color>", "");

                revisionComponent.subIcon.sprite = subIconsForStudy[0];
                revisionComponent.subIcon.color = colorRevise;
            }

            if (revisionComponent.mainText.text.Contains("<i>Update Notes"))
            {
                if(revisionComponent.mainText.text.Contains("<color=navy><b><i>Update Notes: </i></b></color>"))
                    revisionComponent.mainText.text = revisionComponent.mainText.text.Replace("<color=navy><b><i>Update Notes: </i></b></color>", "");

                if (revisionComponent.mainText.text.Contains("<color=navy><b><i>Update Notes</i></b></color>"))
                    revisionComponent.mainText.text = revisionComponent.mainText.text.Replace("<color=navy><b><i>Update Notes</i></b></color>", "");

                revisionComponent.subIcon.sprite = subIconsForStudy[1];
                revisionComponent.subIcon.color = colorNotes;
            }

            if (revisionComponent.mainText.text.Contains("<i>Ongoing Task"))
            {
                if (revisionComponent.mainText.text.Contains("<color=grey><b><i>Ongoing Task: </i></b></color>"))
                    revisionComponent.mainText.text = revisionComponent.mainText.text.Replace("<color=grey><b><i>Ongoing Task: </i></b></color>", "");

                if (revisionComponent.mainText.text.Contains("<color=grey><b><i>Ongoing Task</i></b></color>"))
                    revisionComponent.mainText.text = revisionComponent.mainText.text.Replace("<color=grey><b><i>Ongoing Task</i></b></color>", "");

                revisionComponent.subIcon.sprite = subIconsForStudy[2];
                revisionComponent.subIcon.color = colorOngoing;
            }

            var newRevision = Instantiate(revisionHolder);
            newRevision.transform.SetParent(gameObject.transform, false);
        }
    }
}