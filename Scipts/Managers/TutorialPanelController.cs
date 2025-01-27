using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialPanelController : MonoBehaviour
{
    public GameObject tutorialPanel; // TutorialPanel

    void Start()
    {
        tutorialPanel.SetActive(false); 
    }

    public void ShowTutorial() 
    {
        tutorialPanel.SetActive(true);

        Cursor.visible = true; 
        Cursor.lockState = CursorLockMode.None; 

        Time.timeScale = 0; 
    }

    public void CloseTutorial() 
    {
        tutorialPanel.SetActive(false);

        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked; 

        Time.timeScale = 1; 
    }
}
