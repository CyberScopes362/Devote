using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class Initializer : MonoBehaviour
{
    public GameObject mainCanvas;
    public TaskManager taskManager;
    public MarksManager marksManager;
    public ViewMarksManager viewMarksManager;
    public ActionController actionController;
    public ToastIndicator toastIndicator;
    public HorizontalMovement horizontalMovement;
    public EditTaskHomework editTaskHW;
    public EditTaskAssignment editTaskAT;
    public EditTaskRevision editTaskRV;
    public EditMarks editMarks;
    public ProductivityCircleController proCircleControl;
    public EditSubjects editSubjects;

    public GameObject startUpScreen;
    public GameObject guidePrefab;

    void Start()
    {
        Screen.fullScreen = false;

        if(PlayerPrefs.GetInt("guideDone") != 1)
        {
            Instantiate(guidePrefab, mainCanvas.transform, false);
            startUpScreen.transform.SetAsLastSibling();
        }
    }
}
