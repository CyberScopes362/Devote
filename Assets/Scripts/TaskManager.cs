using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Collections;

public class TaskManager : MonoBehaviour
{
    ActionController actionController;

    public GameObject blind;
    public float generalMovementSpeed;

    public GameObject popupParent;
    public GameObject pageParent;

    //Pages + Popups
    public GameObject[] allPages;
    public GameObject[] allPopups;

    //Date or some shit
    public Text[] dateText;

    //Data Holders
    public List<Subject> subjects = new List<Subject>();
    public List<Homework> homeworkTasks = new List<Homework>();
    public List<Assignment> assignmentTasks = new List<Assignment>();
    public List<Revision> revisionTasks = new List<Revision>();

    //Conversion Temporary Lists
    public List<SubjectSaveable> subjectsSaveable = new List<SubjectSaveable>();

    //Positions
    Vector2 setPosPopup;
    Vector2 setPosPage;

    Vector2 posPopupHidden;
    Vector2 posPageHidden;
    Vector2 posCentre;

    //Checks
    bool popupOut;
    bool pageShifting;
    bool toSaveDate;

    //ID Holder; ID system is used to identify all tasks, and differentiate between tasks which may be identical.
    public int IDCount;

    HorizontalMovement horizontalMovement;

    //For productivity
    public float totalProLevel;
    public float totalProLevelNew;
    public float totalProChangeAmount;
    float finishedHWLevel = 0f;
    float finishedATLevel = 0f;
    float finishedRVLevelDcml = 0f; //This one is already decimaled so does not need to be divided
    float proReqHW;
    float proReqAT;

    //For date resetter
    DateTime tomorrowsDateConstant;

    //For quotes
    public Text quoteText;
    public List<string> allQuotes = new List<string>();
    public List<int> quotesAlreadyUsed = new List<int>();
    string todaysQuote;

    //For settings - use "stg" as begginings
    public bool stgAutoClear;

    //Priority count
    public int priorityCount;


    void Awake()
    {
        LoadAllContent();

        DateTime todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        if (todaysDate >= tomorrowsDateConstant)
        {
            print("New day!");
            //Delete tasks which are overdue and completed
            UpdateProductivityMain();

            SetAllQuotes();
        }
        else
        {
            quoteText.text = todaysQuote;
        }

        SaveAllContent();
    }


    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        actionController = initializer.actionController;
        horizontalMovement = initializer.horizontalMovement;

        foreach (Text dateT in dateText)
            dateT.text = DateTime.Now.ToString("dddd") + ", " + DateTime.Now.Day + " " + DateTime.Now.ToString("MMMM");

        float centreOfScreenX = Screen.width / 2;
        float centreOfScreenY = Screen.height / 2;
        float defaultSidePosX = Screen.width * 2f;
        float defaultBottomPosY = -Screen.height;

        posPopupHidden = new Vector2(defaultSidePosX, centreOfScreenY);
        posPageHidden = new Vector2(centreOfScreenX, defaultBottomPosY);
        posCentre = new Vector2(centreOfScreenX, centreOfScreenY);

        setPosPopup = posPopupHidden;
        setPosPage = posPageHidden;

        foreach(GameObject page in allPages)
        {
            //Enable all gameobjects at the start
            if (!page.activeSelf)
                page.SetActive(true);

            page.transform.localPosition = new Vector2(page.transform.localPosition.x, 0f);
        }

        StartCoroutine(HidePagesFirstFrame());
        UpdateProductivity();
        UpdatePriorityCount();
        SaveAllContent();
    }

    void Update()
    {
        //Set position under blind
        transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, blind.transform.position.y), 24f * Time.deltaTime);

        //Side Panel Popup Auto Movement
        popupParent.transform.position = Vector2.Lerp(popupParent.transform.position, setPosPopup, generalMovementSpeed * 1.5f * Time.deltaTime);

        //Page Auto Movement
        pageParent.transform.position = Vector2.Lerp(pageParent.transform.position, setPosPage, generalMovementSpeed * Time.deltaTime);

        //Use android back button as same as quit button
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (setPosPopup == posCentre)
                HideAllPopups();
            else
            {
                if (setPosPage == posCentre)
                {
                    HideAllPages();
                    actionController.ClearEntries();
                }
                    
                else
                {
                    if(horizontalMovement.pageNumber != 1)
                        horizontalMovement.SetToCentrePage();
                    else
                        Application.Quit();
                }
            }
        }
    }



    //
    //
    // Start Custom Functions
    //
    //

    public void UpdatePriorityCount()
    {
        priorityCount = 0;

        foreach(Homework hw in homeworkTasks)
        {
            if (hw.isPrioritised)
                priorityCount += 1;
        }
        foreach (Assignment at in assignmentTasks)
        {
            if (at.isPrioritised)
                priorityCount += 1;
        }

        foreach (Revision rv in revisionTasks)
        {
            if (rv.isPrioritised)
                priorityCount += 1;
        }
    }


    void UpdateObsoleteTasks()
    {
        DateTime todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        //Using a reverse for loop so you can delete while iterating without interfering the process if items are removed.
        for (int i = homeworkTasks.Count - 1; i >= 0; i--)
        {
            //If task is overdue and complete

            if (homeworkTasks[i].isComplete)
            {
                DateTime dateSetFixed = new DateTime(homeworkTasks[i].dateSet.Year, homeworkTasks[i].dateSet.Month, homeworkTasks[i].dateSet.Day);

                if (dateSetFixed < todaysDate)
                    homeworkTasks.Remove(homeworkTasks[i]);
            }
        }

        for (int i = assignmentTasks.Count - 1; i >= 0; i--)
        {
            //If task is overdue and at 100%
            //Calculated relative to parts
            if (Mathf.CeilToInt((float)assignmentTasks[i].completion * (100f / assignmentTasks[i].parts)) == 100)
            {
                DateTime dateSetFixed = new DateTime(assignmentTasks[i].dateSet.Year, assignmentTasks[i].dateSet.Month, assignmentTasks[i].dateSet.Day);

                if (dateSetFixed < todaysDate)
                    assignmentTasks.Remove(assignmentTasks[i]);
            }
        }
    }

    public void UpdateProductivityMain()
    {
        UpdateObsoleteTasks();
        UpdateProductivity();

        totalProChangeAmount = totalProLevelNew - totalProLevel;
        totalProLevel = totalProLevelNew;

        finishedRVLevelDcml = 0;
        SaveAllContent();
    }

    void SetAllQuotes()
    {
        TextAsset quotesFile = Resources.Load("quotes") as TextAsset;
        string[] allLines = quotesFile.ToString().Split("\n"[0]);

        foreach(string line in allLines)
        {
            if (line != null)
                allQuotes.Add(line);
        }

        //Remove quotes which have already been used

        if(quotesAlreadyUsed != null)
        {
            if(quotesAlreadyUsed.Count >= allQuotes.Count)
                quotesAlreadyUsed.Clear();
            else
                foreach (int quoteIndex in quotesAlreadyUsed)
                    allQuotes.RemoveAt(quoteIndex);
        }

        UpdateQuote();
    }

    void UpdateQuote()
    {
        int randomNo = UnityEngine.Random.Range(0, allQuotes.Count - 1);
        //Add to list of used quotes index
        quotesAlreadyUsed.Add(randomNo);

        quoteText.text = allQuotes[randomNo];
        todaysQuote = quoteText.text;

        //Clear to save memory
        allQuotes.Clear();
    }

    public void UpdateProductivity()
    {
        if (totalProLevel == -1)
        {
            totalProLevel = 1;
            totalProLevelNew = 1;
            SaveAllContent();
        }

        float tempNewProLevel = 0;

        DateTime todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        proReqHW = 0;
        proReqAT = 0;
        finishedHWLevel = 0;
        finishedATLevel = 0;

        foreach (Homework HW in homeworkTasks)
        {
            DateTime dateSetFixed = new DateTime(HW.dateSet.Year, HW.dateSet.Month, HW.dateSet.Day);

            if (dateSetFixed == todaysDate)
                proReqHW += 1f;
            else
            {
                if (dateSetFixed < todaysDate)
                    proReqHW += 1.5f;
                else
                    proReqHW += (1f / ((dateSetFixed - todaysDate).Days + 1)) - 0.01f; 
            }

            if (HW.isComplete)
                finishedHWLevel += 1;
        }

        foreach (Assignment AT in assignmentTasks)
        {
            DateTime dateSetFixed = new DateTime(AT.dateSet.Year, AT.dateSet.Month, AT.dateSet.Day);

            if (dateSetFixed < todaysDate)
            {
                proReqAT += 1.5f;
                finishedATLevel += ((AT.completion * (100 / AT.parts)) / 100f) - 0.01f;
            }
            else
            {
                proReqAT += (1f / AT.dateStartedGap) * ((dateSetFixed - todaysDate).Days + 1);
                finishedATLevel += ((AT.completion * (100 / AT.parts)) / 100f) * ((dateSetFixed - todaysDate).Days + 1) - 0.01f;
            }
        }

        foreach (Revision RV in revisionTasks)
        {
            if (RV.isComplete)
            {
                if(finishedRVLevelDcml >= 0 && finishedRVLevelDcml <= 1)
                {
                    finishedRVLevelDcml += 0.07f;
                }
                else
                {
                    if (finishedRVLevelDcml > 1 && finishedRVLevelDcml <= 3)
                        finishedRVLevelDcml += 0.0475f;
                    else
                    {
                        if(finishedRVLevelDcml > 3)
                            finishedRVLevelDcml += 0.035f;
                    }
                }
            }
               
        }

        float finalHW = (finishedHWLevel / proReqHW);
        float finalAT = (finishedATLevel / proReqAT);

        tempNewProLevel = (finalHW + finalAT) / 2;

        if (float.IsNaN(finalHW) && float.IsNaN(finalAT))
            tempNewProLevel = 1;
        else
        {
            if (float.IsNaN(finalHW))
                tempNewProLevel = finalAT;

            if (float.IsNaN(finalAT))
                tempNewProLevel = finalHW;
        }

        tempNewProLevel += finishedRVLevelDcml;
        tempNewProLevel = Mathf.Clamp(tempNewProLevel, -2f, 2f);

        //This new system should be better and more incremental
        totalProLevelNew = (totalProLevel / 10f * 9f) + (tempNewProLevel / 10f);
        //    ((totalProLevel * 4) + tempNewProLevel) / 5f;
        print("Current Productivity Level: " + (totalProLevelNew * 100));
    }



    public void SaveAllContent()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/q.dat", FileMode.OpenOrCreate);

        AllData allData = new AllData();

        //Convert Subject colors to RGB

        List <SubjectSaveable> subjectsSaveable = new List<SubjectSaveable>();

        subjectsSaveable.Clear();
        for(int i = 1; i <= subjects.Count; i++)
        {
            var name = subjects[i - 1].subjectName; 
            var r = subjects[i - 1].colorCode.r;
            var g = subjects[i - 1].colorCode.g;
            var b = subjects[i - 1].colorCode.b;

            float[] rgb;
            rgb = new float[3];
            rgb[0] = r;
            rgb[1] = g;
            rgb[2] = b;

            SubjectSaveable tempSave = new SubjectSaveable();
            tempSave.subjectName = name;
            tempSave.colorCodeRGB = rgb;

            subjectsSaveable.Add(tempSave);
        }

        //End RGB conversion

        allData.sSubjectsSaveable = subjectsSaveable;
        allData.sHomeworkTasks = homeworkTasks;
        allData.sAssignmentTasks = assignmentTasks;
        allData.sRevisionTasks = revisionTasks;
        allData.sIDCount = IDCount;
        allData.sTotalProLevel = totalProLevel;
        allData.sTotalProLevelNew = totalProLevelNew;
        allData.sTotalProChangeAmount = totalProChangeAmount;
        allData.sFinishedRVLevelDcml = finishedRVLevelDcml;
        allData.sTodaysQuote = todaysQuote;
        allData.sQuotesAlreadyUsed = quotesAlreadyUsed;

        //Save Settings
        allData.sStgAutoClear = stgAutoClear;

        DateTime todaysDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        allData.sTomorrowsDateConstant = todaysDate.AddDays(1);

        bf.Serialize(file, allData);
        file.Close();

       // print("Saved");
    }

    public void LoadAllContent()
    {
        if(File.Exists(Application.persistentDataPath + "/q.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/q.dat", FileMode.Open);
            AllData allData = (AllData)bf.Deserialize(file);

            subjectsSaveable = allData.sSubjectsSaveable;
            homeworkTasks = allData.sHomeworkTasks;
            assignmentTasks = allData.sAssignmentTasks;
            revisionTasks = allData.sRevisionTasks;
            IDCount = allData.sIDCount;
            totalProLevelNew = allData.sTotalProLevelNew;
            tomorrowsDateConstant = allData.sTomorrowsDateConstant;
            todaysQuote = allData.sTodaysQuote;
            quotesAlreadyUsed = allData.sQuotesAlreadyUsed;

            //Load Settings
            stgAutoClear = allData.sStgAutoClear;

            totalProLevel = allData.sTotalProLevel;
            totalProChangeAmount = allData.sTotalProChangeAmount;
            finishedRVLevelDcml = allData.sFinishedRVLevelDcml;

            for (int i = 1; i <= subjectsSaveable.Count; i++)
            {
                var name = subjectsSaveable[i - 1].subjectName;
                var colorCode = new Color(subjectsSaveable[i - 1].colorCodeRGB[0], subjectsSaveable[i - 1].colorCodeRGB[1], subjectsSaveable[i - 1].colorCodeRGB[2]);

                Subject tempLoad = new Subject();
                tempLoad.subjectName = name;
                tempLoad.colorCode = colorCode;

                subjects.Add(tempLoad);
            }

            file.Close();
        }
        else
        {
            tomorrowsDateConstant = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);
            totalProLevel = 1;
            totalProChangeAmount = 0;

            //On first run, update quote.
            SetAllQuotes();

            //Set Settings by default
            stgAutoClear = false;
        }
    }


    IEnumerator HidePagesFirstFrame()
    {
        yield return new WaitForEndOfFrame();

        foreach (GameObject page in allPages)
        {
            //Disable all gameobjects to save cpu load
            if (page.activeSelf)
                page.SetActive(false);
        }
    }


    //Hide All Popups
    public void HideAllPopups()
    {
        setPosPopup = posPopupHidden;
        popupOut = false;
        //SaveAllContent();
    }

    //Hide All Pages
    public void HideAllPages()
    {
        setPosPage = posPageHidden;
        //SaveAllContent();
    }

    //Only Show Chosen Page
    public void ShowCurrentPage(int pageIndex)
    {
        HideAllPopups();

        if (setPosPage == posCentre)
        {
            setPosPage = posPageHidden;
            StartCoroutine(PageShift(pageIndex));
        }

        if (!pageShifting)
        {
            setPosPage = posCentre;

            for (int i = 1; i <= allPages.Length; i++)
            {
                //Filter depending on chosen option

                //First make sure all pages are off so they are appropriately reset
                allPages[i - 1].SetActive(false);

                if (i - 1 != pageIndex)
                    allPages[i - 1].SetActive(false);
                else
                    allPages[i - 1].SetActive(true);
            }
        }

        SaveAllContent();
    }

    IEnumerator PageShift(int pageIndex)
    {
        pageShifting = true;
        yield return new WaitForSeconds(0.1f);
        pageShifting = false;
        ShowCurrentPage(pageIndex);
    }

    //Only Show Chosen Popup
    public void ShowCurrentPopup(int popupIndex)
    {
        if (popupOut && allPopups[popupIndex].activeSelf)
            HideAllPopups();
        else
        {
            setPosPopup = posCentre;

            for (int i = 1; i <= allPopups.Length; i++)
            {
                //allPopups[i - 1].SetActive(true);

                if (i - 1 != popupIndex)
                    allPopups[i - 1].SetActive(false);
                else
                    allPopups[i - 1].SetActive(true);
            }

            popupOut = true;
        }
    }

    //Custom Functions to Add and Edit Items

    public void AddSubject(string subjectName, Color colorCode)
    {
        subjects.Insert(subjects.Count, new Subject());
        subjects[subjects.Count - 1].subjectName = subjectName;
        subjects[subjects.Count - 1].colorCode = colorCode;

        SaveAllContent();
    }

    public void EditSubject(string subjectName, Color colorCode)
    {
        foreach(Subject subj in subjects)
        {
            if(subj.subjectName == subjectName)
            {
                int ix = subjects.IndexOf(subj);
                subjects[ix].colorCode = colorCode;
                break;
            }
        }
    }

    public void AddHomework(string head, string desc, Subject subj, DateTime date)
    {
        homeworkTasks.Insert(homeworkTasks.Count, new Homework());

        homeworkTasks[homeworkTasks.Count - 1].heading = head;
        homeworkTasks[homeworkTasks.Count - 1].description = desc;
        homeworkTasks[homeworkTasks.Count - 1].subject = subj.subjectName;
        homeworkTasks[homeworkTasks.Count - 1].dateSet = date;
        homeworkTasks[homeworkTasks.Count - 1].isComplete = false;
        homeworkTasks[homeworkTasks.Count - 1].ID = IDCount;

        IDCount += 1;
    }

    public void EditHomework(string head, string desc, Subject subj, DateTime date, int IDCountedTaskHW)
    {
        homeworkTasks[IDCountedTaskHW].heading = head;
        homeworkTasks[IDCountedTaskHW].description = desc;
        homeworkTasks[IDCountedTaskHW].subject = subj.subjectName;
        homeworkTasks[IDCountedTaskHW].dateSet = date;
    }

    public void AddAssignment(string head, string desc, Subject subj, DateTime date, int parts)
    {
        assignmentTasks.Insert(assignmentTasks.Count, new Assignment());

        assignmentTasks[assignmentTasks.Count - 1].heading = head;
        assignmentTasks[assignmentTasks.Count - 1].description = desc;
        assignmentTasks[assignmentTasks.Count - 1].subject = subj.subjectName;
        assignmentTasks[assignmentTasks.Count - 1].dateSet = date;
        assignmentTasks[assignmentTasks.Count - 1].dateStartedGap = (date - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)).Days + 1;
        assignmentTasks[assignmentTasks.Count - 1].completion = 0;
        assignmentTasks[assignmentTasks.Count - 1].parts = parts;
        assignmentTasks[assignmentTasks.Count - 1].ID = IDCount;

        IDCount += 1;
    }

    public void EditAssignment(string head, string desc, Subject subj, DateTime date, int IDCountedTaskAT)
    {
        assignmentTasks[IDCountedTaskAT].heading = head;
        assignmentTasks[IDCountedTaskAT].description = desc;
        assignmentTasks[IDCountedTaskAT].subject = subj.subjectName;
        assignmentTasks[IDCountedTaskAT].dateSet = date;
    }

    public void AddRevision(string mainText, Subject subj)
    {
        revisionTasks.Insert(revisionTasks.Count, new Revision());

        revisionTasks[revisionTasks.Count - 1].mainText = mainText;
        revisionTasks[revisionTasks.Count - 1].subject = subj.subjectName;
        revisionTasks[revisionTasks.Count - 1].isComplete = false;
        revisionTasks[revisionTasks.Count - 1].ID = IDCount;

        IDCount += 1;
    }


    [System.Serializable]
    public class AllData
    {
        public List<SubjectSaveable> sSubjectsSaveable = new List<SubjectSaveable>();
        public List<Homework> sHomeworkTasks = new List<Homework>();
        public List<Assignment> sAssignmentTasks = new List<Assignment>();
        public List<Revision> sRevisionTasks = new List<Revision>();

        public int sIDCount;
        public DateTime sTomorrowsDateConstant;
        public float sTotalProLevel;
        public float sTotalProLevelNew;
        public float sTotalProChangeAmount;
        public float sFinishedRVLevelDcml;
        public string sTodaysQuote;
        public List<int> sQuotesAlreadyUsed = new List<int>();

        public bool sStgAutoClear;
    }

    //Because colors cannot be serialized and saved, the color must be split into an array of ints instead, so this temporary list is used instead.
    [System.Serializable]
    public class SubjectSaveable
    {
        public string subjectName;
        public float[] colorCodeRGB;
    }


    [System.Serializable]
    public class Subject
    {
        public string subjectName;
        public Color colorCode;

        public string[] chapters;
    }

    [System.Serializable]
    public class Homework
    {
        public string heading;
        public string description;
        public string subject;

        public DateTime dateSet;

        public bool isComplete;

        public bool isPrioritised;
        public int ID;
    }

    [System.Serializable]
    public class Assignment
    {
        public string heading;
        public string description;
        public string subject;

        public DateTime dateSet;
        //Day of creation gap; used for productivity calculations
        public int dateStartedGap;

        public int completion;
        public int parts;

        public bool isPrioritised;
        public int ID;
    }

    [System.Serializable]
    public class Revision
    {
        public string mainText;

        public string subject;
        public bool isComplete;

        public bool isPrioritised;
        public int ID;
    }
}
