using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using uCPf;
using System.Collections;

public class EditSubjects : MonoBehaviour
{
    TaskManager taskManager;
    ActionController actionController;
    public List<TaskManager.Subject> subjects = new List<TaskManager.Subject>();
    public GameObject allSubjectsParent;
    public GameObject subjectHolder;
    public GameObject colorPicker;
    public ColorPicker colorPickerScript;

    Vector2 setPosCol;

    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;
        actionController = initializer.actionController;
        setPosCol = new Vector2(-1000f, transform.localPosition.y);

        subjects = taskManager.subjects;
        Refresh();
    }

    void OnEnable()
    {
        Refresh();
        OnEnableColorPickerMoveBack();
    }

    void Update()
    {
        colorPicker.transform.localPosition = Vector2.Lerp(colorPicker.transform.localPosition, setPosCol, 20f * Time.deltaTime);
    }

    public void Refresh()
    {
        //First, delete all old objects, then recreate new ones
        foreach (Transform child in allSubjectsParent.transform)
            GameObject.Destroy(child.gameObject);

        foreach (TaskManager.Subject eachSubject in subjects)
        {
            subjectHolder.GetComponentInChildren<Text>().text = eachSubject.subjectName;

            foreach(Image img in subjectHolder.GetComponentsInChildren<Image>())
            {
                if (img.gameObject.name == "ColorCodeImage")
                    img.color = eachSubject.colorCode;
            }
           
            var newSubject = Instantiate(subjectHolder);
            newSubject.transform.SetParent(allSubjectsParent.transform, false);
        }
    }

    public void OnEnableColorPickerMove(string subj)
    {
        setPosCol = new Vector2(0f, transform.localPosition.y);
        actionController.tempEditSubjName = subj;
    }

    public void OnEnableColorPickerMoveBack()
    {
        setPosCol = new Vector2(-1000f, transform.localPosition.y);
    }

    public void OnEnableColorPickerChanged()
    {
        Color chosenCol = colorPickerScript.hsv;
        actionController.OnClick_EditSubject(chosenCol);
    }
}
