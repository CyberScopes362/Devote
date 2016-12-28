using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartupController : MonoBehaviour
{
    public float topPos;
    public float botPos;
    public GameObject top;
    public GameObject bot;
    public float alignSpeed;

    float timer;
    public Color startColor;
    Image bg;
    public Image logo;

    public float[] sets;


    void Awake()
    {
        transform.localPosition = new Vector2(0f, 0f);
        bg = GetComponent<Image>();

        top.transform.localPosition = new Vector2(top.transform.localPosition.x, topPos);
        bot.transform.localPosition = new Vector2(bot.transform.localPosition.x, botPos);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if(timer > (sets[0] / 2))
            logo.color = Color.Lerp(logo.color, startColor, Time.deltaTime * 3.5f);

        if(timer > sets[0])
        {
            startColor = Color.clear;

            if(timer > sets[1])
            {
                bg.color = Color.Lerp(bg.color, Color.clear, Time.deltaTime * 5f);

                top.transform.localPosition = Vector2.Lerp(top.transform.localPosition, new Vector2(top.transform.localPosition.x, 0f), Time.deltaTime * alignSpeed);
                bot.transform.localPosition = Vector2.Lerp(bot.transform.localPosition, new Vector2(bot.transform.localPosition.x, 0f), Time.deltaTime * alignSpeed);

                if (bg.raycastTarget)
                    bg.raycastTarget = false;

                if(timer > sets[2])
                {
                    top.transform.localPosition = new Vector2(top.transform.localPosition.x, 0f);
                    bot.transform.localPosition = new Vector2(bot.transform.localPosition.x, 0f);

                    Destroy(gameObject);
                }
            }
        }
    }
}