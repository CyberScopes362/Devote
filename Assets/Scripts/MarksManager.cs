using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MarksManager : MonoBehaviour
{
    public List<Marks> marks = new List<Marks>();
    public int marksIDCount;

    void Awake()
    {
        LoadMarks();
        SaveMarks();
    }

    public void AddMark(string name, string subj, int mark, int value)
    {
        marks.Insert(marks.Count, new Marks());

        marks[marks.Count - 1].assessmentName = name;
        marks[marks.Count - 1].subjectName = subj;
        marks[marks.Count - 1].mark = mark;
        marks[marks.Count - 1].ID = marksIDCount;
        marks[marks.Count - 1].value = value;

        marksIDCount += 1;
    }

    public void SaveMarks()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/m.dat", FileMode.OpenOrCreate);

        AllMarksData allMarksData = new AllMarksData();

        allMarksData.sMarks = marks;
        allMarksData.sMarkIDCount = marksIDCount;
      
        bf.Serialize(file, allMarksData);
        file.Close();
    }

    public void LoadMarks()
    {
        if (File.Exists(Application.persistentDataPath + "/m.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/m.dat", FileMode.Open);
            AllMarksData allMarksData = (AllMarksData)bf.Deserialize(file);
          
            marks = allMarksData.sMarks;
            marksIDCount = allMarksData.sMarkIDCount;

            file.Close();
        }
    }

    [System.Serializable]
    public class AllMarksData
    {
        public List<Marks> sMarks = new List<Marks>();

        public int sMarkIDCount;
    }

    [System.Serializable]
    public class Marks
    {
        public string assessmentName;
        public string subjectName;
        public int mark;
        public int value;
        public int ID;
    }
}