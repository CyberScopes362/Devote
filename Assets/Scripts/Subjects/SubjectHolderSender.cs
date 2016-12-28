using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubjectHolderSender : MonoBehaviour
{
    EditSubjects editSubjects;
    string thisSubj;

    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        editSubjects = initializer.editSubjects;

        thisSubj = GetComponentInChildren<Text>().text;
    }

    public void OnEnableColorPicker()
    {
        editSubjects.OnEnableColorPickerMove(thisSubj);
    }
}