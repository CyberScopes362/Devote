using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    ViewMarksManager viewMarksManager;
    TaskManager taskManager;
    ActionController actionController;
    public Toggle notifToggle;
    public Toggle onClearToggle;
    public Text onClearSubText;
    public InputField inputTitle;

    public Text versionNumberText;
    public GameObject resetPopup;

    void Start()
    {
        Initializer initializer = GameObject.FindGameObjectWithTag("Initializer").GetComponent<Initializer>();
        taskManager = initializer.taskManager;
        viewMarksManager = initializer.viewMarksManager;
        actionController = initializer.actionController;

        versionNumberText.text = "Devote v" + Application.version;

        if (taskManager.stgAutoClear)
            onClearToggle.isOn = true;
        else
            onClearToggle.isOn = false;

        resetPopup.SetActive(false);
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
            onClearSubText.text = "Automatically hide completed homework or assignment tasks (Restart app to take effect)";
        }
        else
        {
            taskManager.stgAutoClear = false;
            onClearSubText.text = "Never hide completed homework or assignment tasks; tasks are hidden automatically after they have exceeded their due date";
        }

        actionController.GeneralFunctions();
    }

    public void OnClick_ResetGrades()
    {
        resetPopup.SetActive(true);
    }

    public void OnClick_ResetOption(int opt)
    {
        if (opt == 1)
        {
            if (inputTitle.text == "")
                actionController.Error("Add a title for your current marks.");
            else
            {
                viewMarksManager.LoadSep(inputTitle.text);
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            resetPopup.SetActive(false);
        }
    }

    public void OnClick_Notifications()
    {
        if (notifToggle.isOn)
            taskManager.stgNotifsOpt = true;
        else
            taskManager.stgNotifsOpt = false;

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