using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject infoPanel;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject optionPanel;
    [SerializeField] 
    private Button infoButton;
    [SerializeField] 
    private Button muteButton;
    [SerializeField]
    private Button pauseButton;
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button optionsButton;
    [SerializeField]
    private Button couponButton;
    [SerializeField]
    private Button optionExitButton;
    [SerializeField]
    private Button ballButton;   
    [SerializeField]
    private Button eatButton;
    [SerializeField]
    private Image crossImage;

    GameObject mainDog;

    private TouchScreenKeyboard keyboard;

    void Start()
    {
        infoButton.onClick.AddListener(() => OnInfoButtonClick());
        muteButton.onClick.AddListener(() => OnMuteButtonClick());  
        pauseButton.onClick.AddListener(() => OnPauseButtonClick());
        resumeButton.onClick.AddListener(() => OnResumeButtonClick());
        optionsButton.onClick.AddListener(() => OnOptionsButtonClick());
        couponButton.onClick.AddListener(() => OnCouponButtonClick());
        eatButton.onClick.AddListener(() => OnEatButtonClick());  
        optionExitButton.onClick.AddListener(()=> OnOptionExitButtonClick());
    }

    void Update()
    {
        if (mainDog == null)
        {
            mainDog = GameObject.FindGameObjectWithTag("modelObject_Script");
        }
    }


    public void OnInfoButtonClick()
    {
        if ((infoPanel.activeSelf))
            infoPanel.SetActive(false);
        else
            infoPanel.SetActive(true);
    }

    public void OnMuteButtonClick()
    {
        if (!crossImage.IsActive())
        {
            crossImage.gameObject.SetActive(true);
            AudioListener.volume = 0.0f;
        }
        else
        {
            crossImage.gameObject.SetActive(false);
            AudioListener.volume = 1.0f;
        }

    }
    public void OnPauseButtonClick()
    {
        pausePanel.SetActive(true);
        GameManager.isPause = true;
    }

    public void OnResumeButtonClick()
    {
        pausePanel.SetActive(false);
        GameManager.isPause = false;
    }

    public void OnOptionsButtonClick()
    {
        optionPanel.SetActive(true);
    }

    public void OnCouponButtonClick()
    {

    }

    public void OnOptionExitButtonClick()
    {
        optionPanel.SetActive(false);
    }

    public void BallButtonOn()
    {
        ballButton.gameObject.SetActive(true);
    }

    public void OnEatButtonClick()
    {
        mainDog.GetComponentInChildren<DogCtrl>()?.GiveOrder("Eat");
    }
}
