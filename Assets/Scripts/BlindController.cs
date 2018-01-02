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
    public float smoothAmount;

    public bool dragging;
    public bool blindOpen;

    public float yPosTop;
    public float yPosBot;
    public float ypsps;


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
        if (!dragging)
        {
            transform.localPosition = Vector2.SmoothDamp(transform.localPosition, setPos, ref refVector, smoothAmount, Mathf.Infinity, Mathf.Abs(touchSpeed) / 100f * Time.fixedDeltaTime);
        }
        else
        {
            touchSpeed = Input.touches[0].deltaPosition.y / Input.touches[0].deltaTime;

            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + (Input.touches[0].deltaPosition.y/1.525f));

            if (transform.localPosition.y <= maxLocalY - 25f)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, yPosBot - 25f);
                blindOpen = false;
            }

            if (transform.localPosition.y >= minLocalY + 25f)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, yPosTop + 25f);
                blindOpen = true;
            }

            ypsps = Input.touches[0].deltaPosition.y;
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