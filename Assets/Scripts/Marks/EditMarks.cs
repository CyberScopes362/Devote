using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

public class EditMarks : MonoBehaviour
{
    MarksManager marksManager;
    TaskManager taskManager;
    MarkScript markScript;
    SeparateMarksHolder subPrefabHolder;

    public Text yearDateText;

    public GameObject mainPrefab;
    public GameObject subPrefab;
    public GameObject avPrefab;
    public GameObject noMarksPrefab;

    public List<MarksManager.Marks> marks = new List<MarksManager.Marks>();
    public List<TaskManager.Subject> subjects = new List<TaskManager.Subject>();

    Color setColor;
    public float defaultLength;
    public float defaultMarkLength;
    public float totalYPos;


    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        marksManager = initializer.marksManager;
        taskManager = initializer.taskManager;

        marks = marksManager.marks;
        subjects = taskManager.subjects;

        yearDateText.text = DateTime.Now.Year.ToString();

        markScript = mainPrefab.GetComponent<MarkScript>();
        subPrefabHolder = subPrefab.GetComponent<SeparateMarksHolder>();

        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in transform)
            GameObject.Destroy(child.gameObject);

        totalYPos = 0;

        if (subjects.Count == 0)
        {
            var insNo = Instantiate(noMarksPrefab);
            insNo.transform.SetParent(transform, false);
        }
        else
        {
            var insAv = Instantiate(avPrefab);
            insAv.transform.SetParent(this.transform, false);
            insAv.transform.SetAsFirstSibling();
            totalYPos += 70f;

            float allAvMarks = 0;
            int allSubjCountForAv = 0;

            foreach (TaskManager.Subject eachSubject in subjects)
            {
                setColor = eachSubject.colorCode;

                markScript.mainHeading.text = eachSubject.subjectName;
                markScript.mainHeading.color = setColor;
                markScript.lineGraph.color = setColor;

                List<int> thisMarks = new List<int>();
                List<int> thisValue = new List<int>();

                foreach (MarksManager.Marks eachMark in marksManager.marks)
                {
                    if (eachMark.subjectName == eachSubject.subjectName)
                    {
                        thisMarks.Add(eachMark.mark);
                        thisValue.Add(eachMark.value);
                    }
                }

                float avMarks = 0;

                foreach (int mark in thisMarks)
                {
                    avMarks += mark;
                    allAvMarks += mark;
                    allSubjCountForAv += 1;
                }

                if (thisMarks.Count != 0)
                    avMarks /= thisMarks.Count;
                else
                    avMarks = 0;


                markScript.avMarkTextNo.text = "~ " + Mathf.RoundToInt(avMarks).ToString() + "%";
                markScript.avMarkText.color = markScript.graphLineController.avLineColor;
                markScript.avMarkTextNo.color = markScript.graphLineController.avLineColor;
                markScript.graphLineController.marks = thisMarks.ToArray();

                var insMain = Instantiate(mainPrefab);
                insMain.transform.SetParent(this.transform, false);
                insMain.transform.localPosition = new Vector2(transform.localPosition.x, -totalYPos);


                foreach (Transform child in insMain.GetComponent<MarkScript>().subjectMarksParent.transform)
                    GameObject.Destroy(child.gameObject);

                char letterList = 'A';

                foreach (MarksManager.Marks eachMark in marksManager.marks)
                {
                    if (eachMark.subjectName == eachSubject.subjectName)
                    {
                        subPrefabHolder.assessmentLetterText.text = "Assessment " + letterList.ToString() + ":";
                        letterList = (char)(((int)letterList) + 1);

                        subPrefabHolder.nameText.text = eachMark.assessmentName;
                        subPrefabHolder.markText.text = "Mark: " + Convert.ToInt32(eachMark.mark) + "%";
                        subPrefabHolder.valueText.text = "Value: " + Convert.ToInt32(eachMark.value) + "%";

                        subPrefabHolder.markID = eachMark.ID;

                        var insSub = Instantiate(subPrefab);
                        insSub.transform.SetParent(insMain.GetComponent<MarkScript>().subjectMarksParent.transform, false);

                        totalYPos += defaultMarkLength;
                    }
                }

                totalYPos += defaultLength;
            }

            allAvMarks /= allSubjCountForAv;

            if (allSubjCountForAv > 0)
                insAv.GetComponent<AvMarkScript>().avMarkText.text = allAvMarks.ToString("F2") + "%";
            else
                insAv.GetComponent<AvMarkScript>().avMarkText.text = "--%";
        }
    }
}