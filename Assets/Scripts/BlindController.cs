using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class BlindController : MonoBehaviour
{
    //Vector2 touchPos;
    public float maxLocalY;
    public float minLocalY;

    //Touch speed but also direction with + and -
    public float touchSpeed;
    public float touchSpeedReq;

    Vector2 setPos;
    Vector2 refVector;
    RectTransform thisRect;
    public float smoothAmount;

    public bool dragging;
    public bool blindOpen;

    public float yPosTop;
    public float yPosBot;

    private void Start()
    {
        //yPosTop = Screen.height * 2f;
        thisRect = GetComponent<RectTransform>();
        maxLocalY = yPosTop + 25f;
        minLocalY = yPosBot - 25f;
    }

    void Update()
    {
        if (thisRect.anchoredPosition.y > (yPosTop + yPosBot) / 2f)
            blindOpen = false;
        else
            blindOpen = true;
    }

    void LateUpdate()
    {
        if (!dragging)
        {
            thisRect.anchoredPosition = Vector2.SmoothDamp(thisRect.anchoredPosition, setPos, ref refVector, smoothAmount, Mathf.Infinity, Mathf.Abs(touchSpeed) / 100f * Time.fixedDeltaTime);
        }
        else
        {
            touchSpeed = Input.touches[0].deltaPosition.y / Input.touches[0].deltaTime;

            thisRect.anchoredPosition = new Vector2(thisRect.anchoredPosition.x, thisRect.anchoredPosition.y + (Input.touches[0].deltaPosition.y/1.525f));

             if (thisRect.anchoredPosition.y >= maxLocalY)
             {
                 thisRect.anchoredPosition = new Vector2(thisRect.anchoredPosition.x, maxLocalY);
                 blindOpen = false;
             }

             if (thisRect.anchoredPosition.y <= minLocalY)
             {
                 thisRect.anchoredPosition = new Vector2(thisRect.anchoredPosition.x, minLocalY);
                 blindOpen = true;
             }
        }
    }

    public void OnDrag()
    {
        dragging = true;
    }

    public void OnRelease()
    {
        if (Mathf.Abs(touchSpeed) > touchSpeedReq)
        {
            if (touchSpeed > 0)
                setPos = new Vector2(thisRect.anchoredPosition.x, yPosTop);
            else
                setPos = new Vector2(thisRect.anchoredPosition.x, yPosBot);
        }
        else
        {
            if(thisRect.anchoredPosition.y >= (yPosTop + yPosBot) / 2)
                setPos = new Vector2(thisRect.anchoredPosition.x, yPosTop);
            else
                setPos = new Vector2(thisRect.anchoredPosition.x, yPosBot);

            touchSpeed = 1000f;
        }

        dragging = false;
    }
}