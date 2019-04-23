using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StartGuideController : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject pageController;
    public Image[] pageDots;
    int pageNumber;
    int setDot;

    float setDist;

    public Color lightGrey;

    void Start()
    {
        setDist = 0;
        pageNumber = 1;
        backButton.SetActive(false);
        ClickRefresh();
    }

    public void OnClickNext()
    {
        setDist -= 800f;
        pageNumber += 1;

        if (pageNumber == 6)
            nextButton.SetActive(false);
    }

    public void OnClickBack()
    {
        setDist += 800f;
        pageNumber -= 1;

        if (pageNumber == 1)
            backButton.SetActive(false);
    }

    public void ClickRefresh()
    {
        if(pageNumber != 6)
            nextButton.SetActive(true);

        if(pageNumber != 1)
            backButton.SetActive(true);

        RefreshPageDots();
    }

    public void OnClickStart()
    {
        PlayerPrefs.SetInt("guideDone", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }

    void RefreshPageDots()
    {
        foreach (Image pageDot in pageDots)
            pageDot.color = lightGrey;

        pageDots[pageNumber - 1].color = Color.black;
    }

    void Update()
    {
        pageController.transform.localPosition = Vector2.Lerp(pageController.transform.localPosition, new Vector2(setDist, pageController.transform.localPosition.y), 8f * Time.deltaTime);
    }
}
