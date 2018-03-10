using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SubjectItemHolder : MonoBehaviour
{
    public Text subjName;
    public Text subjAvMark;
    public List<MarksManager.Marks> thisSubjMarks;

    float valueModifier;
    float finalValue;

    List<int> marksList = new List<int>();
    public int[] marksArray;
    Color subjColor;

    ViewGraphLineController vGLC;

    private void Start()
    {
        vGLC = GetComponentInParent<ViewGraphLineController>();
    }

    public float SetAv()
    {
        foreach (MarksManager.Marks thisMark in thisSubjMarks)
        {
            finalValue += (thisMark.mark * thisMark.value / 100f);
            valueModifier += (thisMark.value / 100f);

            marksList.Add(thisMark.mark);
        }

        marksArray = marksList.ToArray();

        finalValue /= valueModifier;
        subjAvMark.text = Mathf.RoundToInt(finalValue).ToString() + "%";
        return finalValue;
    }

    public void SendMarks()
    {
        vGLC.marks = marksArray;
        vGLC.UpdateGraph();
    }
}
