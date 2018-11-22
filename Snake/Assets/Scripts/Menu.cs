using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    public GameObject panel;
    public Slider slider; 
    public Slider sliderSFX;
    public Slider sliderDifficulty;
    public Text highText;
    //private float musicVolume;

    private void Start()
    {
        SetVolume();
        highText.text = "High Score: " + ((int)PlayerPrefs.GetFloat("Highscore")).ToString();
    }

    private void SetVolume()
    {
        slider.value = PlayerPrefs.GetFloat("Volume");
        sliderSFX.value = PlayerPrefs.GetFloat("SFX");
        sliderDifficulty.value = PlayerPrefs.GetFloat("Difficulty");
    }

    public void OnPlay()
    {
        Time.timeScale = 1;
        //paused = false;
        PlayerPrefs.SetFloat("Volume", slider.value);
        PlayerPrefs.SetFloat("SFX", sliderSFX.value);
        PlayerPrefs.SetFloat("Difficulty", sliderDifficulty.value);
        SceneManager.LoadScene(1);
    }

    public void OnSettings()
    {
        if(!panel.gameObject.activeSelf)
        {
            panel.gameObject.SetActive(true);
        }
        else
        {
            panel.gameObject.SetActive(false);
        }
        //Debug.Log(musicVolume);
    }

    /*public void OnSliderValue()
    {
        
    }*/

    public void OnReset()
    {
        slider.value = 0.8f;
        sliderSFX.value = 0.5f;
        sliderDifficulty.value = 0f;
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
