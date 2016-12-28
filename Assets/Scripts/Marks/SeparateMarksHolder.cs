using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SeparateMarksHolder : MonoBehaviour
{
    ActionController actionController;

    public Text nameText;
    public Text markText;
    public Text valueText;
    public Text assessmentLetterText;
    [HideInInspector]
    public int markID;

    bool isDeleted;
    float timerDestroy;

    void Start()
    {
        actionController = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>().actionController;
    }

    void Update()
    {
        if (isDeleted)
        {
            timerDestroy += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, new Vector2(-Screen.width * 6f, transform.position.y), Time.deltaTime);

            foreach (Transform child in transform)
            {
               // child.transform.position = Vector2.Lerp(child.transform.position, new Vector2(-Screen.width * 6f, child.transform.position.y), Time.deltaTime);

                Text[] allChildTexts = child.GetComponentsInChildren<Text>();
                Image[] allChildImages = child.GetComponentsInChildren<Image>();

                foreach (Text tCom in allChildTexts)
                    tCom.color = Color.Lerp(tCom.color, Color.clear, 20f * Time.deltaTime);

                foreach (Image iCom in allChildImages)
                    iCom.color = Color.Lerp(iCom.color, Color.clear, 20f * Time.deltaTime);

            }

            Text thisText = GetComponent<Text>();
            thisText.color = Color.Lerp(thisText.color, Color.clear, 20f * Time.deltaTime);

            if (timerDestroy > 0.2f)
                actionController.MarksFunctions();
        }
    }

    public void OnClickDelete()
    {
        actionController.OnClick_DeleteMarks(markID);
        isDeleted = true;
    }
}