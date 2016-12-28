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

    public bool dragging;
    public bool blindOpen;

    public float yPosTop;
    public float yPosBot;

    void Start()
    {

    }

    void Update()
    {
        if (transform.localPosition.y > (yPosTop + yPosBot) / 2f)
            blindOpen = false;
        else
            blindOpen = true;
    }

    void LateUpdate()
    {
        if(!dragging)
            transform.localPosition = Vector2.Lerp(transform.localPosition, setPos, Mathf.Sqrt(Mathf.Abs(touchSpeed)) * Time.deltaTime);
    }

    public void OnDrag()
    {
        dragging = true;

        transform.position = new Vector2(transform.position.x, Input.touches[0].position.y);
        touchSpeed = Input.touches[0].deltaPosition.y / Input.touches[0].deltaTime;

        if (transform.localPosition.y <= maxLocalY)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, yPosBot);
            blindOpen = false;
        }

        if (transform.localPosition.y >= minLocalY)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, yPosTop);
            blindOpen = true;
        }
    }

    public void OnRelease()
    {
        if (Mathf.Abs(touchSpeed) > touchSpeedReq)
        {
            if (touchSpeed > 0)
                setPos = new Vector2(transform.localPosition.x, yPosTop);
            else
                setPos = new Vector2(transform.localPosition.x, yPosBot);
        }
        else
        {
            if(transform.position.y >= (yPosTop + yPosBot) / 2)
                setPos = new Vector2(transform.localPosition.x, yPosTop);
            else
                setPos = new Vector2(transform.localPosition.x, yPosBot);

            touchSpeed = 1000f;
        }

        dragging = false;
    }
}