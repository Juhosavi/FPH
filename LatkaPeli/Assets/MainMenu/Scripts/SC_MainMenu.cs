using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SC_MainMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject optionsMenu, howToPlayMenu, creditsMenu;
    public GameObject optionsFirstButton, howToPlayFirstButton, creditsFirstButton;
    public GameObject optionsClosedButton, howToPlayClosedButton, creditsClosedButton;

    //Äänisäätö
    FMOD.Studio.Bus bus;
    [SerializeField]
    [Range(-80f, 10f)]
    private float busVolume;
    public float sliderValue;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/Music/MenuMusic", transform.position);
        MainMenu.SetActive(true);
        //MainMenuButton();
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.9f); // Slideri toimii väärinpäin. Koska on tehty päin ¤#!"%¤¤#"&%&¤! T: Matti
        bus = FMODUnity.RuntimeManager.GetBus("bus:/");
        volumeSlider.onValueChanged.AddListener(delegate { SliderVolumeChange(); });
        bus.setVolume(Mathf.Log10(volumeSlider.value) * 20);
    }
    void OnGUI()
    {
        //Delete all of the PlayerPrefs settings by pressing this button.
        if (GUI.Button(new Rect(100, 200, 200, 60), "Delete"))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void PlayNowButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HockeyKit");
    }
    // public void TrainingButton()
    // {
    //     UnityEngine.SceneManagement.SceneManager.LoadScene("Training");
    // }

    public void Options()
    {
        optionsMenu.SetActive(true);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);


    }


    public void CloseOptions()
    {
        optionsMenu.SetActive(false);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);

    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
        Debug.Log("Exiting Game");
    }
    public void SliderVolumeChange()
    {
        sliderValue = volumeSlider.value;
        bus.setVolume(Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }

    public void HowToPlay()
    {
        howToPlayMenu.SetActive(true);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(howToPlayFirstButton);
    }

    public void CloseHowToPlay()
    {
        howToPlayMenu.SetActive(false);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(howToPlayClosedButton);
    }

    public void CreditsMenu()
    {
        creditsMenu.SetActive(true);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(creditsFirstButton);
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);

        // Clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        // Set a new selected object
        EventSystem.current.SetSelectedGameObject(creditsClosedButton);
    }

    //public void OnEnable()
    //{
    //    optionsMenu.SetActive(true);
    //    // Clear selected object
    //    //EventSystem.current.SetSelectedGameObject(null);
    //    // Set a new selected object
    //    EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    //}

    //public void OnDisable()
    //{
    //    optionsMenu.SetActive(false);

    //}

}











