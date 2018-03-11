using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;


public class ActionController : MonoBehaviour
{
    ToastIndicator toastIndicator;
    TaskManager taskManager;
    MarksManager marksManager;

    [HideInInspector]
    public EditTaskHomework editTaskHW;
    [HideInInspector]
    public EditTaskAssignment editTaskAT;
    [HideInInspector]
    public EditTaskRevision editTaskRV;
    EditMarks editMarks;
    EditSubjects editSubjects;

    public bool error;

    public SubjectDropdownMenu homeworkSubDropMenu;
    public SubjectDropdownMenu editHomeworkSubDropMenu;
    public SubjectDropdownMenu assignmentSubDropMenu;
    public SubjectDropdownMenu editAssignmentSubDropMenu;
    public SubjectDropdownMenu revisionSubDropMenu;
    public SubjectDropdownMenu marksSubDropMenu;


    //Toast Colors
    public Color grey;
    public Color red;

    //Entities

    //Add Subject
    public Image addSubjectImage;
    public InputField addSubjectInput;

    //Add Homework
    public InputField addHomeworkName;
    public InputField addHomeworkDesc;
    public Text addHomeworkChosenSubj;
    public DateTime addHomeworkDate;

    //Edit Homework
    public InputField editHomeworkName;
    public InputField editHomeworkDesc;
    public Text editHomeworkChosenSubj;
    public DateTime editHomeworkDate;

    int taskIDEditHW;

    //Add Assignment
    public InputField addAssignmentName;
    public InputField addAssignmentDesc;
    public InputField addAssignmentParts;
    public Text addAssignmentChosenSubj;
    public DateTime addAssignmentDate;

    //Edit Assignment
    public InputField editAssignmentName;
    public InputField editAssignmentDesc;
    public Text editAssignmentChosenSubj;
    public DateTime editAssignmentDate;

    int taskIDEditAT;

    //Add Revision
    public InputField addRevisionText;
    public Dropdown addRevisionTypeDD;
    public Text addRevisionTypeText;
    public Text addRevisionChosenSubj;

    //Add Marks
    public InputField addMarksTitle;
    public Text addMarksChosenSubj;
    public InputField addMarksMark;
    public InputField addMarksValue;



    public string tempEditSubjName;

    /*
    //
    Android toasts are now disabled; inbuilt toasts are used instead.
    //
    */

    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();

        taskManager = initializer.taskManager.GetComponent<TaskManager>();
        marksManager = initializer.marksManager;
        toastIndicator = initializer.toastIndicator;
        editSubjects = initializer.editSubjects;

        editTaskHW = initializer.editTaskHW;
        editTaskAT = initializer.editTaskAT;
        editTaskRV = initializer.editTaskRV;
        editMarks = initializer.editMarks;
    }

    public void OnClick_AddSubject()
    {
        error = false;

        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if(eachSubject.subjectName == addSubjectInput.text)
            {
                Error("Subject already exists;\nPlease choose a new name for this subject.");
                break;
            }
        }

        if(addSubjectInput.text == "")
            Error("Please give this subject a title.");

        if (!error)
        {
            taskManager.AddSubject(addSubjectInput.text, addSubjectImage.color);
            taskManager.HideAllPages();
            toastIndicator.ActivateToast("Subject " + addSubjectInput.text + " Added", grey);
            //AndroidNativePopups.OpenToast("Subject " + addSubjectInput.text + " Added", AndroidNativePopups.ToastDuration.Long);
        }

        if (!error)
        {
            GeneralFunctions();
            MarksFunctions();
        }
    }

    public void OnClick_DeleteSubject(GameObject subjectObj, string subjectName)
    {
        error = false;

        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
          
            if (eachSubject.subjectName == subjectName)
            {
                //Check if any tasks use this subject, and if so, stop it from getting deleted
                foreach (TaskManager.Homework eachTask in taskManager.homeworkTasks)
                {
                    if (eachTask.subject == eachSubject.subjectName)
                    {
                        Error("Cannot delete a subject while tasks \nuse them.");
                        break;
                    }
                }

                foreach (TaskManager.Assignment eachTask in taskManager.assignmentTasks)
                {
                    if (eachTask.subject == eachSubject.subjectName)
                    {
                        Error("Cannot delete a subject while tasks \nuse them.");
                        break;
                    }
                }

                foreach (TaskManager.Revision eachTask in taskManager.revisionTasks)
                {
                    if (eachTask.subject == eachSubject.subjectName)
                    {
                        Error("Cannot delete a subject while tasks \nuse them.");
                        break;
                    }
                }

                if (!error)
                {
                    Destroy(subjectObj);
                    taskManager.subjects.Remove(eachSubject);
                    toastIndicator.ActivateToast("Subject " + subjectName + " Deleted", grey);
                    //AndroidNativePopups.OpenToast("Subject " + subjectName + " Deleted", AndroidNativePopups.ToastDuration.Long);
                    break;
                }
            }
        }

        if (!error)
        {
            GeneralFunctions();
            MarksFunctions();
        }
    }

    public void OnClick_EditSubject(Color colorCode)
    {
        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if (eachSubject.subjectName == tempEditSubjName)
            {
                taskManager.EditSubject(tempEditSubjName, colorCode);
                SubjectsRefresh();
                GeneralFunctions();
                break;
            }
        }
    }


    public void OnClick_AddHomework()
    {
        error = false;

        addHomeworkDate = homeworkSubDropMenu.setDate;

        TaskManager.Subject chosenSubj = new TaskManager.Subject();

        foreach(TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if(eachSubject.subjectName == addHomeworkChosenSubj.text)
            {
                chosenSubj = eachSubject;
                break;
            }
        }

        if(chosenSubj.subjectName == "" || chosenSubj.subjectName == null)
            Error("Add subjects first before creating tasks.");

        if (addHomeworkName.text == "")
            Error("Please give this task a heading.");


        if(!error)
        {
            taskManager.AddHomework(addHomeworkName.text, addHomeworkDesc.text, chosenSubj, addHomeworkDate);
            taskManager.HideAllPages();
            toastIndicator.ActivateToast("Added Homework Task:\n" + addHomeworkName.text, grey);
            //AndroidNativePopups.OpenToast("Added Homework Task: " + addHomeworkName.text, AndroidNativePopups.ToastDuration.Long);
        }

        if (!error)
            GeneralFunctions();
    }

    public void OnClick_DeleteHomework(int taskID)
    {
        error = false;

        foreach (TaskManager.Homework eachHomework in taskManager.homeworkTasks)
        {
            if (eachHomework.ID == taskID)
            {
                taskManager.homeworkTasks.Remove(eachHomework);
                toastIndicator.ActivateToast("Deleted homework task:\n" + eachHomework.heading, grey);
                //AndroidNativePopups.OpenToast("Deleted Homework Task: " + eachHomework.heading, AndroidNativePopups.ToastDuration.Long);
                break;
            }
        }

        //General Functions will be called by actual object
    }

    public void OnClick_EditTaskHWOpen(int taskID)
    {
        taskIDEditHW = taskID;

        foreach (TaskManager.Homework eachHomework in taskManager.homeworkTasks)
        {
            if (eachHomework.ID == taskID)
            {
                //Set General Input
                editHomeworkName.text = eachHomework.heading;
                editHomeworkDesc.text = eachHomework.description;
                editHomeworkChosenSubj.text = eachHomework.subject;

                //Set Subject in Dropdown
                int i = 0;

                foreach(Dropdown.OptionData eachOption in editHomeworkSubDropMenu.thisDropdown.options)
                {
                    if(eachOption.text == eachHomework.subject)
                    {
                        editHomeworkSubDropMenu.thisDropdown.value = i;
                        break;
                    }

                    i += 1;
                }

                //Set Date Components
                editHomeworkSubDropMenu.setDate = eachHomework.dateSet;
                editHomeworkSubDropMenu.dateText.text = eachHomework.dateSet.Day.ToString() + "/" + eachHomework.dateSet.Month.ToString() + "/" + eachHomework.dateSet.Year.ToString();
                editHomeworkSubDropMenu.dateTextExtended.text = eachHomework.dateSet.ToString("dddd") + ", " + eachHomework.dateSet.Day.ToString() + " " + eachHomework.dateSet.ToString("MMMM");
                break;
            }
        }
    }

    public void OnClick_SaveEditedTaskHW()
    {
        error = false;

        TaskManager.Subject chosenSubj = new TaskManager.Subject();

        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if (eachSubject.subjectName == editHomeworkChosenSubj.text)
            {
                chosenSubj = eachSubject;
                break;
            }
        }

        if (chosenSubj.subjectName == "" || chosenSubj.subjectName == null)
            Error("Add subjects first before creating tasks.");

        if (editHomeworkName.text == "")
            Error("Please give this task a heading.");

        if(!error)
        {
            for (int i = 0; i <= taskManager.homeworkTasks.Count; i++)
            {
                if (taskManager.homeworkTasks[i].ID == taskIDEditHW)
                {
                    //Do some shit to find real index in homework list of item
                    taskManager.EditHomework(editHomeworkName.text, editHomeworkDesc.text, chosenSubj, editHomeworkSubDropMenu.setDate, i);
                    taskManager.HideAllPages();
                    toastIndicator.ActivateToast("Edited homework task:\n" + editHomeworkName.text, grey);
                    break;
                }
            }
        }

        if(!error)
            GeneralFunctions();
    }

    public void OnClick_AddAssignment()
    {
        error = false;

        addAssignmentDate = assignmentSubDropMenu.setDate;

        TaskManager.Subject chosenSubj = new TaskManager.Subject();

        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if (eachSubject.subjectName == addAssignmentChosenSubj.text)
            {
                chosenSubj = eachSubject;
                break;
            }
        }

        if (chosenSubj.subjectName == "" || chosenSubj.subjectName == null)
            Error("Add subjects first before creating tasks.");

        if (addAssignmentName.text == "")
            Error("Please give this task a heading.");


        if (!error)
        {
            int partsConvert = 0;
            if (addAssignmentParts.text == "" || Convert.ToInt32(addAssignmentParts.text) == 0)
                partsConvert = 100;
            else
                partsConvert = Convert.ToInt32(addAssignmentParts.text);

            taskManager.AddAssignment(addAssignmentName.text, addAssignmentDesc.text, chosenSubj, addAssignmentDate, partsConvert);
            taskManager.HideAllPages();
            toastIndicator.ActivateToast("Added assignment task:\n" + addAssignmentName.text, grey);
            //AndroidNativePopups.OpenToast("Added Assignment Task: " + addHomeworkName.text, AndroidNativePopups.ToastDuration.Long);
        }

        if (!error)
            GeneralFunctions();
    }

    public void OnClick_DeleteAssignment(int taskID)
    {
        error = false;

        foreach (TaskManager.Assignment eachAssignment in taskManager.assignmentTasks)
        {
            if (eachAssignment.ID == taskID)
            {
                taskManager.assignmentTasks.Remove(eachAssignment);
                toastIndicator.ActivateToast("Deleted assignment task:\n" + eachAssignment.heading, grey);
                //AndroidNativePopups.OpenToast("Deleted Assignment Task: " + eachAssignment.heading, AndroidNativePopups.ToastDuration.Long);
                break;
            }
        }

        //General Functions will be called by actual object
    }

    public void OnClick_EditTaskATOpen(int taskID)
    {
        taskIDEditAT = taskID;

        foreach (TaskManager.Assignment eachAssignment in taskManager.assignmentTasks)
        {
            if (eachAssignment.ID == taskID)
            {
                //Set General Input
                editAssignmentName.text = eachAssignment.heading;
                editAssignmentDesc.text = eachAssignment.description;
                editAssignmentChosenSubj.text = eachAssignment.subject;

                //Set Subject in Dropdown
                int i = 0;

                foreach (Dropdown.OptionData eachOption in editAssignmentSubDropMenu.thisDropdown.options)
                {
                    if (eachOption.text == eachAssignment.subject)
                    {
                        editAssignmentSubDropMenu.thisDropdown.value = i;
                        break;
                    }

                    i += 1;
                }

                //Set Date Components
                editAssignmentSubDropMenu.setDate = eachAssignment.dateSet;
                editAssignmentSubDropMenu.dateText.text = eachAssignment.dateSet.Day.ToString() + "/" + eachAssignment.dateSet.Month.ToString() + "/" + eachAssignment.dateSet.Year.ToString();
                editAssignmentSubDropMenu.dateTextExtended.text = eachAssignment.dateSet.ToString("dddd") + ", " + eachAssignment.dateSet.Day.ToString() + " " + eachAssignment.dateSet.ToString("MMMM");
                break;
            }
        }
    }

    public void OnClick_SaveEditedTaskAT()
    {
        error = false;

        TaskManager.Subject chosenSubj = new TaskManager.Subject();

        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if (eachSubject.subjectName == editAssignmentChosenSubj.text)
            {
                chosenSubj = eachSubject;
                break;
            }
        }

        if (chosenSubj.subjectName == "" || chosenSubj.subjectName == null)
            Error("Add subjects first before creating tasks.");

        if (editAssignmentName.text == "")
            Error("Please give this assignment a heading.");

        if (!error)
        {
            for (int i = 0; i <= taskManager.assignmentTasks.Count; i++)
            {
                if (taskManager.assignmentTasks[i].ID == taskIDEditAT)
                {
                    //Do some shit to find real index in homework list of item
                    taskManager.EditAssignment(editAssignmentName.text, editAssignmentDesc.text, chosenSubj, editAssignmentSubDropMenu.setDate, i);
                    taskManager.HideAllPages();
                    toastIndicator.ActivateToast("Edited assignment task:\n" + editAssignmentName.text, grey);
                    break;
                }
            }
        }

        if (!error)
            GeneralFunctions();
    }

    public void OnClick_AddRevision()
    {
        error = false;

        TaskManager.Subject chosenSubj = new TaskManager.Subject();

        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if (eachSubject.subjectName == addRevisionChosenSubj.text)
            {
                chosenSubj = eachSubject;
                break;
            }
        }

        if (chosenSubj.subjectName == "" || chosenSubj.subjectName == null)
            Error("Add subjects first before assigning tasks.");

        // > Description should be optional
     //   if (addRevisionText.text == "")
     //       Error("No Text Provided");

        if (!error)
        {
            string mainTextString = "<b><i>" + addRevisionTypeText.text + ": </i></b>" + addRevisionText.text;
            string extraString;

            //If no text, no colon
            if (addRevisionText.text == "")
                extraString = "</i></b></color>";
            else
                extraString = ": </i></b></color>";

            switch (addRevisionTypeText.text)
            {
                case "Revise":
                    mainTextString = "<color=black><b><i>" + addRevisionTypeText.text + extraString + addRevisionText.text;
                    break;

                case "Update Notes":
                    mainTextString = "<color=navy><b><i>" + addRevisionTypeText.text + extraString + addRevisionText.text;
                    break;

                case "Ongoing Task":
                    mainTextString = "<color=grey><b><i>" + addRevisionTypeText.text + extraString + addRevisionText.text;
                    break;
            }

            taskManager.AddRevision(mainTextString, chosenSubj);
            taskManager.HideAllPages();
            toastIndicator.ActivateToast("Added study task for " + chosenSubj.subjectName, grey);
            //AndroidNativePopups.OpenToast("Added Study Task for" + chosenSubj.subjectName, AndroidNativePopups.ToastDuration.Long);
        }

        if (!error)
            GeneralFunctions();
    }

    public void OnClick_DeleteRevision(int taskID, int type)
    {
        foreach (TaskManager.Revision eachRevision in taskManager.revisionTasks)
        {
            if (eachRevision.ID == taskID)
            {
                string toastText;

                if (type == 1)
                    toastText = "Study task completed";
                else
                    toastText = "Study task dismissed";

                taskManager.revisionTasks.Remove(eachRevision);
                toastIndicator.ActivateToast(toastText, grey);
                //AndroidNativePopups.OpenToast("Deleted Study Task", AndroidNativePopups.ToastDuration.Long);
                break;
            }
        }

        //General Functions will be called by actual object
    }

    public void OnClick_AddMarks()
    {
        error = false;

        TaskManager.Subject chosenSubj = new TaskManager.Subject();

        foreach (TaskManager.Subject eachSubject in taskManager.subjects)
        {
            if (eachSubject.subjectName == addMarksChosenSubj.text)
            {
                chosenSubj = eachSubject;
                break;
            }
        }

        if (chosenSubj.subjectName == "" || chosenSubj.subjectName == null)
            Error("Add subjects first before adding marks.");

        if (addMarksTitle.text == "")
            Error("Please give this mark a title.");

        if(addMarksMark.text == "")
            Error("Please add a mark for this assessment.");

        if (addMarksValue.text == "")
            Error("Please add a value for this assessment.");

        if (!error)
        {
            marksManager.AddMark(addMarksTitle.text, addMarksChosenSubj.text, Convert.ToInt32(addMarksMark.text), Convert.ToInt32(addMarksValue.text));
            taskManager.HideAllPages();
            toastIndicator.ActivateToast("Added mark for " + addMarksChosenSubj.text, grey);
        }

        if (!error)
            MarksFunctions();
    }

    public void OnClick_DeleteMarks(int markID)
    {
        foreach(MarksManager.Marks eachMark in marksManager.marks)
        {
            if(eachMark.ID == markID)
            {
                marksManager.marks.Remove(eachMark);
                toastIndicator.ActivateToast("Deleted mark for " + eachMark.subjectName, grey);
                break;
            }
        }
    }




    public void SubjectsRefresh()
    {
        editSubjects.Refresh();
    }

    public void GeneralFunctions()
    {
        taskManager.SaveAllContent();
        editTaskHW.Refresh();
        editTaskAT.Refresh();
        editTaskRV.Refresh();
        taskManager.UpdatePriorityCount();
    }

    public void MarksFunctions()
    {
        marksManager.SaveMarks();
        editMarks.Refresh();
    }



    public void ClearEntries()
    {
        //Reset Subject Text Input Field
        addSubjectInput.text = "";

        //Reset Task Input Fields
        addHomeworkName.text = "";
        addHomeworkDesc.text = "";
        addAssignmentName.text = "";
        addAssignmentDesc.text = "";
        addAssignmentParts.text = "";
        addRevisionText.text = "";

        // Reset Mark Input Fields
        addMarksTitle.text = "";
        addMarksMark.text = "";
        addMarksValue.text = "";

        //Set Subject Dropdowns
        if (homeworkSubDropMenu.thisDropdown != null)
            homeworkSubDropMenu.thisDropdown.value = 0;
        if (assignmentSubDropMenu.thisDropdown != null)
            assignmentSubDropMenu.thisDropdown.value = 0;
        if (revisionSubDropMenu.thisDropdown != null)
            revisionSubDropMenu.thisDropdown.value = 0;
        if (marksSubDropMenu.thisDropdown != null)
            marksSubDropMenu.thisDropdown.value = 0;

        //Set Revision Dropdown
        addRevisionTypeDD.value = 0;

        //Set Dates
        homeworkSubDropMenu.dateText.text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        homeworkSubDropMenu.dateTextExtended.text = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMMM");
        homeworkSubDropMenu.setDate = DateTime.Now;

        assignmentSubDropMenu.dateText.text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
        assignmentSubDropMenu.dateTextExtended.text = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMMM");
        assignmentSubDropMenu.setDate = DateTime.Now;
    }

    public void Error(string errorText)
    {
        error = true;
        toastIndicator.ActivateToast(errorText, red);
        //AndroidNativePopups.OpenToast(errorText, AndroidNativePopups.ToastDuration.Short);
        print(errorText);
    }

    public void Toast(string text)
    {
        toastIndicator.ActivateToast(text, grey);
        //AndroidNativePopups.OpenToast(errorText, AndroidNativePopups.ToastDuration.Short);
    }
}
