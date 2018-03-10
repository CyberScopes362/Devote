using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI.Extensions;
using System.Linq;

public class ViewGraphLineController : MonoBehaviour
{
    public GameObject yTextPrefab;
    public GameObject xTextPrefab;
    public GameObject yTextParent;
    public GameObject xTextParent;
    public GameObject faintLineSpace;

    public GameObject faintLinePrefab;
    public UILineRenderer lineGraph;
    public Color avLineColor;

    float yHeight;
    float xWidth;

    int marksAmount;
    int avMark;
    public int[] marks;
    public string[] subjects;
    public List<MarksManager.Marks> marksCore = new List<MarksManager.Marks>();
    public GameObject subjButton;
    float totalAvMark;

    public Text avMarkTotalText;
    public Text titleText;


    void Start()
    {
        UpdateGraph();
        AddSubjButtons();
    }
    
    void AddSubjButtons()
    {
        int i = 0;

        foreach(MarksManager.Marks markClass in marksCore)
            i++;

        subjects = new string[i];
        int v = 0;
        foreach (MarksManager.Marks markClass in marksCore)
        {
            subjects[v] = markClass.subjectName;
            v++;
        }

        subjects = subjects.Distinct().ToArray();

        int yShift = 0;
        for (int x = 0; x < subjects.Length; x++)
        {
            var insButton = Instantiate(subjButton, this.transform) as GameObject;
            insButton.transform.localPosition = new Vector2(transform.localPosition.x, -590f - yShift);
            yShift += 82;
            SubjectItemHolder lnController = insButton.GetComponent<SubjectItemHolder>();
            lnController.subjName.text = subjects[x];

            List<MarksManager.Marks> thisSubjMarks = new List<MarksManager.Marks>();

            foreach(MarksManager.Marks mark in marksCore)
                if(mark.subjectName == subjects[x])
                    thisSubjMarks.Add(mark);

            lnController.thisSubjMarks = thisSubjMarks;
            totalAvMark += lnController.SetAv();
        }

        totalAvMark /= subjects.Length;
        avMarkTotalText.text = "Overall Moderated Average: " + totalAvMark.ToString("F2") + "%";
    }

    public void UpdateGraph()
    {
        //Clear from last creation
        foreach (Transform child in yTextParent.transform)
            Destroy(child.gameObject);

        foreach (Transform child in xTextParent.transform)
            Destroy(child.gameObject);

        foreach (Transform child in faintLineSpace.transform)
            Destroy(child.gameObject);

        //End clear
        
        marksAmount = marks.Length;
        lineGraph.Points = new Vector2[marksAmount];
        avMark = 0;

        //Start Graph Structuring
        int percentValue = 0;
        char letterValue = 'A';

        yHeight = yTextParent.GetComponent<RectTransform>().sizeDelta.y;
        xWidth = xTextParent.GetComponent<RectTransform>().sizeDelta.x;

        for (int i = 0; i <= 10; i++)
        {
            yTextPrefab.GetComponent<Text>().text = percentValue.ToString();

            var insY = Instantiate(yTextPrefab);
            insY.transform.SetParent(yTextParent.transform, false);
            insY.transform.localPosition = new Vector2(transform.localPosition.x, (yHeight / 10 * i) - (yHeight / 2));
            insY.isStatic = true;

            if (i != 0)
            {
                var insLine = Instantiate(faintLinePrefab);
                insLine.transform.SetParent(faintLineSpace.transform, false);
                insLine.transform.localPosition = new Vector2(transform.localPosition.x, insY.transform.localPosition.y);
                insLine.isStatic = true;
            }

            percentValue += 10;
        }

        for (int i = 0; i < marksAmount; i++)
        {
            xTextPrefab.GetComponent<Text>().text = letterValue.ToString();

            var insX = Instantiate(xTextPrefab);
            insX.transform.SetParent(xTextParent.transform, false);

            if (!float.IsNaN(xWidth / (marksAmount - 1) * i))
                insX.transform.localPosition = new Vector2((xWidth / (marksAmount - 1) * i) - (xWidth / 2), insX.transform.localPosition.y);

            insX.isStatic = true;
            letterValue = (char)(((int)letterValue) + 1);
        }

        //Set line points
        for (int i = 0; i < marksAmount; i++)
        {
            //If its the first point, push it up a bit just so the end part doesnt stick out.
            if (i == 0)
                lineGraph.Points[i] = new Vector2((xWidth / (marksAmount - 1) * i + 3.5f), yHeight / 100f * marks[i]);
            else
                lineGraph.Points[i] = new Vector2((xWidth / (marksAmount - 1) * i), yHeight / 100f * marks[i]);
        }

        //Create Average mark line
        foreach (int mark in marks)
            avMark += mark;

        if (marks.Length != 0)
            avMark /= marksAmount;

        var insAvLine = Instantiate(faintLinePrefab) as GameObject;
        insAvLine.GetComponent<Image>().color = avLineColor;
        insAvLine.transform.SetParent(faintLineSpace.transform, false);
        insAvLine.transform.localPosition = new Vector2(transform.localPosition.x, (yHeight / 100f * avMark) - (yHeight / 2));
    }
}