using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubjectParent : MonoBehaviour
{
    public GameObject subjectParent;
    ActionController actionController;

    void Start()
    {
        actionController = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>().actionController;
    }

    public void OnClick()
    {
        actionController.OnClick_DeleteSubject(subjectParent, subjectParent.GetComponentInChildren<Text>().text);
    }
}
