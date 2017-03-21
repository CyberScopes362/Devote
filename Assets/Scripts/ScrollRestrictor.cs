using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScrollRestrictor : MonoBehaviour
{
    TaskManager taskManager;
    public bool isMarksScroll;

    public GameObject allTasksHolder;
    public GameObject contentHolder;
    public int allTasksCount;
    public float endPoint;
    public float defaultDistanceMin;

    public float minTasksOpen;
    public float minTasksClosed;

    float verticalSpacing;
    public float additionalVerticalSpacing;

    public GameObject blind;
    BlindController blindController;
    bool blindOpen;
    ScrollRect scrollRect;

    public EditMarks editMarks;
    float totalYPos;
    int subjectsAmount;


    void Start()
    {
        taskManager = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>().taskManager;
        blindController = blind.GetComponent<BlindController>();
        scrollRect = GetComponent<ScrollRect>();

        contentHolder.transform.localPosition = new Vector2(contentHolder.transform.localPosition.x, defaultDistanceMin);

        if(!isMarksScroll)
            verticalSpacing = allTasksHolder.GetComponent<VerticalLayoutGroup>().spacing;
    }

    void Update()
    {
        if(isMarksScroll)
        {
            MarksScrollRestrictor();
            return;
        }

        allTasksCount = allTasksHolder.transform.childCount;
        endPoint = (allTasksCount * verticalSpacing) + additionalVerticalSpacing - 710 + defaultDistanceMin;

        blindOpen = blindController.blindOpen;

        if (blindOpen)
            endPoint = (allTasksCount * verticalSpacing) + additionalVerticalSpacing - 710 + defaultDistanceMin;
        else
            endPoint = (allTasksCount * verticalSpacing) + additionalVerticalSpacing - 1220 + defaultDistanceMin;

        if (allTasksCount >= minTasksOpen && blindOpen)
        {
            if(!Input.GetMouseButton(0))
            {
                if (contentHolder.transform.localPosition.y < defaultDistanceMin)
                {
                    scrollRect.inertia = false;
                    contentHolder.transform.localPosition = Vector2.Lerp(contentHolder.transform.localPosition, new Vector2(contentHolder.transform.localPosition.x, defaultDistanceMin), 20f * Time.deltaTime);
                }


                if (contentHolder.transform.localPosition.y > endPoint)
                {
                    scrollRect.inertia = false;
                    contentHolder.transform.localPosition = Vector2.Lerp(contentHolder.transform.localPosition, new Vector2(contentHolder.transform.localPosition.x, endPoint), 20f * Time.deltaTime);
                }
            }
            else
            {
                scrollRect.inertia = true;
            }
        }
        else
        {
            if(allTasksCount >= minTasksClosed && !blindOpen)
            {
                if (!Input.GetMouseButton(0))
                {
                    if (contentHolder.transform.localPosition.y < defaultDistanceMin)
                    {
                        scrollRect.inertia = false;
                        contentHolder.transform.localPosition = Vector2.Lerp(contentHolder.transform.localPosition, new Vector2(contentHolder.transform.localPosition.x, defaultDistanceMin), 20f * Time.deltaTime);
                    }


                    if (contentHolder.transform.localPosition.y > endPoint)
                    {
                        scrollRect.inertia = false;
                        contentHolder.transform.localPosition = Vector2.Lerp(contentHolder.transform.localPosition, new Vector2(contentHolder.transform.localPosition.x, endPoint), 20f * Time.deltaTime);
                    }
                }
                else
                {
                    scrollRect.inertia = true;
                }
            }
            else
            {
                //If less than minTasksOpen tasks, or less than minTasksClosed while expanded, no scroll allowed
                contentHolder.transform.localPosition = new Vector2(contentHolder.transform.localPosition.x, defaultDistanceMin);
            }
        }
    }

    void MarksScrollRestrictor()
    {
        endPoint = additionalVerticalSpacing - 710 + defaultDistanceMin;
        blindOpen = blindController.blindOpen;
        totalYPos = editMarks.totalYPos;
        subjectsAmount = taskManager.subjects.Count;

        if (blindOpen)
            endPoint = totalYPos + additionalVerticalSpacing - 710 + defaultDistanceMin;
        else
            endPoint = totalYPos + additionalVerticalSpacing - 1220 + defaultDistanceMin;

        if(subjectsAmount > 0)
        {
            if (totalYPos > 0)
            {
                if (!Input.GetMouseButton(0))
                {
                    if (contentHolder.transform.localPosition.y < defaultDistanceMin)
                    {
                        scrollRect.inertia = false;
                        contentHolder.transform.localPosition = Vector2.Lerp(contentHolder.transform.localPosition, new Vector2(contentHolder.transform.localPosition.x, defaultDistanceMin), 20f * Time.deltaTime);
                    }

                    if (contentHolder.transform.localPosition.y > endPoint)
                    {
                        scrollRect.inertia = false;
                        contentHolder.transform.localPosition = Vector2.Lerp(contentHolder.transform.localPosition, new Vector2(contentHolder.transform.localPosition.x, endPoint), 20f * Time.deltaTime);
                    }
                }
                else
                {
                    scrollRect.inertia = true;
                }
            }
        }
        else
        {
            contentHolder.transform.localPosition = new Vector2(contentHolder.transform.localPosition.x, defaultDistanceMin);
        }
       
    }
}
