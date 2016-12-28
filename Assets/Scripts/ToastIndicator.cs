using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToastIndicator : MonoBehaviour
{
    public Text displayText;
    Image toast;

    float toastTimer;
    Vector2 setPosToast;

    Vector2 posDown;
    Vector2 posUp;

    void Start()
    {
        toast = GetComponent<Image>();

        posDown = new Vector2(Screen.width / 2, Screen.height - (Screen.height / 11f));
        posUp = new Vector2(Screen.width / 2, Screen.height + (Screen.height / 11f));
    }

    void Update()
    {
        toastTimer -= Time.deltaTime;

        if(toastTimer > 0f)
        {
            if(setPosToast != posDown)
                setPosToast = posDown;
        }
            
        else
        {
            if(setPosToast != posUp)
                setPosToast = posUp;
        }

        if(toastTimer > -1f)
            transform.position = Vector2.Lerp(transform.position, setPosToast, 11f * Time.deltaTime);
    }

    public void ActivateToast(string toastMessage, Color toastColor)
    {
        if(setPosToast == posDown)
            transform.position = posUp;

        toastTimer = 2.875f;

        displayText.text = toastMessage;
        toast.color = toastColor;
    }
}
