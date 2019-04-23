using UnityEngine;

public class HorizontalScrollSnapper : MonoBehaviour
{
    public float objWidth;
    bool isDrag;
    Vector2 setVector;
    float xAlign;
    RectTransform thisRect;

    private void Start()
    {
        thisRect = transform.GetComponent<RectTransform>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            isDrag = true;

        if(Input.GetMouseButtonUp(0))
        {
            xAlign = thisRect.anchoredPosition.x / objWidth;
            setVector = new Vector2(Mathf.RoundToInt(xAlign) * objWidth, thisRect.anchoredPosition.y);
            isDrag = false;
        }

        if (!isDrag)
            thisRect.anchoredPosition = Vector2.Lerp(thisRect.anchoredPosition, setVector, 20f * Time.deltaTime);
    }
}
