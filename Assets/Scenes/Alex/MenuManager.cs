using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public TMP_Text mainText;
    public GameObject clickButton;
    public GameObject closeButton;
    private int clickCount = 0;

    public void Start()
    {
        closeButton.SetActive(false);
    }

    public void OnClickButton()
    {
        Debug.Log("Button is being clicked!... ClickCount=" + clickCount + (clickCount == 1));

        clickCount++;

        if (clickCount == 1)
        {
            mainText.text = "You will now experience what it's like to have psychosis";
        }
        else if (clickCount == 2)
        {
            mainText.text = "Move forward to the man in blue and touch him.";
            clickButton.SetActive(false);
            closeButton.SetActive(true);
        }
    }
    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
