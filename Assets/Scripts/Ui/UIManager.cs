using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu Properties")]
    [SerializeField] Canvas _mainMenuCanvas;
    [SerializeField] GameObject _optionsCanvas;
    [SerializeField] Resolution[] _resolutions;
    [SerializeField] TMP_Dropdown _resolutionDropdown;

    [Header("References")]
    [SerializeField] PlayerBehavior _player;

    [Header("In Game Properties")]
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _DeathScreen;
    [SerializeField] Button _highQualityButton;
    [SerializeField] Button _fullScreenButton;

    [SerializeField] bool _isScenePaused;

    private void Start()
    {
        Resolution();
        SetResolution(2);
        _highQualityButton.Select();
        _fullScreenButton.Select();
        _isScenePaused = false;
    }

    private void Update()
    {
        if (CheckSceneIsntMenu())
        {
            PlayerDeadUI();
            PauseMenu();
        }
    }

    //Main Menu
    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void Resolution()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();

        List<string> list = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height + " @ " + _resolutions[i].refreshRate + "hz";
            list.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        _resolutionDropdown.AddOptions(list);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Ingame
    public void PlayerDeadUI()
    {
        if (_player.IsDead == true)
        {
            _DeathScreen.SetActive(true);
        }
        else
        {
            _DeathScreen.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseMenu()
    {
        if (!_optionsCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _isScenePaused == false)
            {
                _pauseMenu.SetActive(true);
                Time.timeScale = 0;
                _isScenePaused = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && _isScenePaused == true) 
            {
                UnpauseGame();
                _pauseMenu.SetActive(false);
                _isScenePaused = false;
            }
        }
    }

    public void UnpauseGame()
    {
        if (CheckSceneIsntMenu())
        {
            Time.timeScale = 1;
        }
    }

    public bool CheckSceneIsntMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
