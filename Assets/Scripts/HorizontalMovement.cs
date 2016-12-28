using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HorizontalMovement : MonoBehaviour
{
    [HideInInspector]
    public float setPosX = 0f;
    [HideInInspector]
    public int pageNumber = 1;
    public bool changingColours;

    public Image[] pageDots;
    public Color[] pageDotColours;

    public Button rightButton;
    public Button leftButton;

    public float minPages;
    public float maxPages;
    public float spaceBetweenPages;

    float timer;


    void Start()
    {
        pageDotColours = new Color[pageDots.Length];
        SetPageDotsColour();
    }

    void Update()
    {
        transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(setPosX, transform.localPosition.y), 10f * Time.deltaTime);

        if(changingColours)
        {
            timer += Time.deltaTime;

            for (int i = 0; i < pageDots.Length; i++)
                pageDots[i].color = Color.Lerp(pageDots[i].color, pageDotColours[i], 12f * Time.deltaTime);

            if (timer > 1f)
            {
                changingColours = false;
                timer = 0;
            }
        }
    }

    public void OnClickLeft()
    {
        if(pageNumber > minPages)
        {
            pageNumber -= 1;
            rightButton.interactable = true;
            setPosX += spaceBetweenPages;
        }

        if (pageNumber == minPages)
            leftButton.interactable = false;

        SetPageDotsColour();
    }

    public void OnClickRight()
    {
        if (pageNumber < maxPages)
        {
            pageNumber += 1;
            leftButton.interactable = true;
            setPosX -= spaceBetweenPages;
        }

        if (pageNumber == maxPages)
            rightButton.interactable = false;

        SetPageDotsColour();
    }

    public void SetToCentrePage()
    {
        pageNumber = 1;
        setPosX = 0;
        rightButton.interactable = true;
        leftButton.interactable = true;
        SetPageDotsColour();
    }

    void SetPageDotsColour()
    {
        for (int i = 0; i < pageDotColours.Length; i++)
        {
            if (i == pageNumber)
                pageDotColours[i] = Color.white;
            else
                pageDotColours[i] = new Color(1f,1f,1f, 0.25f);
        }

        changingColours = true;
    }
}