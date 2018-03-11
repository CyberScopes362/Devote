using UnityEngine;
using System.Collections;
using AndNotify;

public class Test : MonoBehaviour
{

    public GameObject notification;
    private string title = "Hello",
            content = "Hello World!";
    private int notificationID = 0, days, hours, minutes, seconds = 15, count = 20;

    // Use this for initialization
    void Awake()
    {
        notification.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Title:");
        title = GUILayout.TextField(title);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Content:");
        content = GUILayout.TextField(content);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Notification ID:");
        notificationID = int.Parse(GUILayout.TextField(notificationID.ToString()));
        notificationID = (notificationID < 0) ? 0 : notificationID;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Days:");
        days = int.Parse(GUILayout.TextField(days.ToString()));
        days = (days < 0) ? 0 : days;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Hours:");
        hours = int.Parse(GUILayout.TextField(hours.ToString()));
        hours = (hours < 0) ? 0 : hours;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Minutes:");
        minutes = int.Parse(GUILayout.TextField(minutes.ToString()));
        minutes = (minutes < 0) ? 0 : minutes;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Seconds:");
        seconds = int.Parse(GUILayout.TextField(seconds.ToString()));
        seconds = (seconds < 0) ? 0 : seconds;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Count:");
        count = int.Parse(GUILayout.TextField(count.ToString()));
        count = (count < 0) ? 0 : count;
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Start"))
        {
            NotificationSetup scrpt = notification.GetComponent<NotificationSetup>();
            scrpt.Title = title;
            scrpt.Content = content;
            scrpt.notificationID = notificationID;
            scrpt.Days = days;
            scrpt.Hours = hours;
            scrpt.Minutes = minutes;
            scrpt.Seconds = seconds;
            scrpt.Count = count;
            notification.SetActive(true);
            notification.SetActive(false);
        }
        GUILayout.EndVertical();
    }
}
