using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsController : MonoBehaviour
{
    TaskManager taskManager;
    ActionController actionController;
    public Toggle onClearToggle;
    public Text onClearSubText;


    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;
        actionController = initializer.actionController;

        if (taskManager.stgAutoClear)
            onClearToggle.isOn = true;
        else
            onClearToggle.isOn = false;

        OnClick_AutoClear();
    }

    void Update()
    {

    }

    public void OnClick_AutoClear()
    {
        if(onClearToggle.isOn)
        {
            taskManager.stgAutoClear = true;
            onClearSubText.text = "Automatically hide completed homework or assignment tasks [Once the interface is refreshed]";
        }
        else
        {
            taskManager.stgAutoClear = false;
            onClearSubText.text = "Never hide completed homework or assignment tasks; tasks are hidden automatically after they have exceeded their due date";
        }

        actionController.GeneralFunctions();
    }

    public void OnClickPlayStore()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.cyberscopes.devote");
    }

    public void OnClickPayPal()
    {
        Application.OpenURL("https://www.paypal.me/MozamelAnwary");
    }

}