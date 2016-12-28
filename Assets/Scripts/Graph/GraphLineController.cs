using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI.Extensions;

public class GraphLineController : MonoBehaviour
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


    void Start()
    {
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

            if(i != 0)
            {
                var insLine = Instantiate(faintLinePrefab);
                insLine.transform.SetParent(faintLineSpace.transform, false);
                insLine.transform.localPosition = new Vector2(transform.localPosition.x, insY.transform.localPosition.y);
                insLine.isStatic = true;
            }

            percentValue += 10;
        }

        for(int i = 0; i < marksAmount; i++)
        {
            xTextPrefab.GetComponent<Text>().text = letterValue.ToString();

            var insX = Instantiate(xTextPrefab);
            insX.transform.SetParent(xTextParent.transform, false);

            if(!float.IsNaN(xWidth / (marksAmount - 1) * i))
                insX.transform.localPosition = new Vector2((xWidth / (marksAmount - 1) * i) - (xWidth / 2), insX.transform.localPosition.y);

            insX.isStatic = true;
            letterValue = (char)(((int)letterValue) + 1);
        }

        //Set line points
        for(int i = 0; i < marksAmount; i++)
        {
            //If its the first point, push it up a bit just so the end part doesnt stick out.
            if(i == 0)
                lineGraph.Points[i] = new Vector2((xWidth / (marksAmount - 1) * i + 3.5f), yHeight / 100f * marks[i]);
            else
                lineGraph.Points[i] = new Vector2((xWidth / (marksAmount - 1) * i), yHeight / 100f * marks[i]);
        }

        //Create Average mark line
        foreach(int mark in marks)
            avMark += mark;

        if(marks.Length != 0)
            avMark /= marksAmount;

        var insAvLine = Instantiate(faintLinePrefab) as GameObject;
        insAvLine.GetComponent<Image>().color = avLineColor;
        insAvLine.transform.SetParent(faintLineSpace.transform, false);
        insAvLine.transform.localPosition = new Vector2(transform.localPosition.x, (yHeight / 100f * avMark) - (yHeight / 2));
    }
}