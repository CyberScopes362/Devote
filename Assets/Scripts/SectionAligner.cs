using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionAligner : MonoBehaviour
{
    public RectTransform mainCanvas;

    // Use this for initialization
    void Start()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, mainCanvas.rect.height);
    }
}
