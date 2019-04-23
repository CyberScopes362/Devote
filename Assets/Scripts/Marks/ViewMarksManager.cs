using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class ViewMarksManager : MonoBehaviour
{
    MarksManager marksManager;
    public List<MarksManager.Marks> marks = new List<MarksManager.Marks>();
    public List<List<MarksManager.Marks>> publicListMarks = new List<List<MarksManager.Marks>>();
    public List<string> titleList = new List<string>();
    public GameObject gradeSetPrefab;
    public GameObject noCollection;


    private void Start()
    {
        marksManager = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>().marksManager;
        LoadCollection();
    }

    public void LoadSep(string title)
    {
        string thisTitle = title;
        BinaryFormatter bf = new BinaryFormatter();
        MarksSaveWrapper mainListsMarks = new MarksSaveWrapper();
        FileStream file;

        if (File.Exists(Application.persistentDataPath + "/z.dat"))
        {
            file = File.Open(Application.persistentDataPath + "/z.dat", FileMode.Open);
            mainListsMarks = (MarksSaveWrapper)bf.Deserialize(file);
        }
        else
        {
            file = File.Open(Application.persistentDataPath + "/z.dat", FileMode.Create);
            mainListsMarks = new MarksSaveWrapper();
        }

        publicListMarks = mainListsMarks.coreListMarks;
        titleList = mainListsMarks.titles;

        file.Close();
        AddAndSaveMarks(thisTitle);
    }

    void AddAndSaveMarks(string thisTitle)
    {
        marks = marksManager.marks;
        publicListMarks.Add(marks);
        titleList.Add(thisTitle);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/z.dat", FileMode.OpenOrCreate);
        MarksSaveWrapper mainListsMarks = new MarksSaveWrapper();

        mainListsMarks.coreListMarks = publicListMarks;
        mainListsMarks.titles = titleList;

        bf.Serialize(file, mainListsMarks);
        file.Close();

        print(mainListsMarks.coreListMarks.Count);
    }

    public void LoadCollection()
    {
        print("Started Collection Load");
        if (File.Exists(Application.persistentDataPath + "/z.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/z.dat", FileMode.Open);
            MarksSaveWrapper mainListsMarks = (MarksSaveWrapper)bf.Deserialize(file);

            publicListMarks = mainListsMarks.coreListMarks;
            titleList = mainListsMarks.titles;
            file.Close();

            //Create grade sets
            int x = 0;
            foreach (List<MarksManager.Marks> markSet in publicListMarks)
            {
                var insSet = Instantiate(gradeSetPrefab, transform) as GameObject;
                insSet.GetComponentInChildren<ViewGraphLineController>().marksCore = markSet;
                insSet.GetComponentInChildren<ViewGraphLineController>().titleText.text = titleList[x];
                x++;
            }
        }
        else
        {
            print("No grades");
            Instantiate(noCollection, transform.parent);
        }
    }

    [System.Serializable]
    public class MarksSaveWrapper
    {
        public List<List<MarksManager.Marks>> coreListMarks = new List<List<MarksManager.Marks>>();
        public List<string> titles = new List<string>();
    }
}