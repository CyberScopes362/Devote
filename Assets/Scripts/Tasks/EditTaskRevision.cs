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
            if(revisionComponent.mainText.text.Contains("<b><i>Revise"))
                revisionComponent.subIcon.sprite = subIconsForStudy[0];

            if (revisionComponent.mainText.text.Contains("<b><i>Update Notes"))
                revisionComponent.subIcon.sprite = subIconsForStudy[1];

            if (revisionComponent.mainText.text.Contains("<b><i>Ongoing Task"))
                revisionComponent.subIcon.sprite = subIconsForStudy[2];

            var newRevision = Instantiate(revisionHolder);
            newRevision.transform.SetParent(gameObject.transform, false);
        }
    }
}