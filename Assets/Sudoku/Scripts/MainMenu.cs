using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject pauseDialoag;
    [Space(10)]
    public Text timeLable;
    public Text modeLable;

    [Header("NewGameMenu")]
    public GameObject newGameBG;
    public GameObject newGameMenu;

    [Header("Setting Menu")]
    public Toggle soundToggle;
    public Toggle duplicateToggle;
    public Toggle identicalNumToggle;
    public Toggle autoToggle;

    public static MainMenu intance;
    void Awake()
    {
        intance = this;
    }

    void Start()
    {
        SetupToggles();
        StartCoroutine(UpdateTime());
        UpdateModeUI();
    }

    IEnumerator UpdateTime()
    {
        while (true)
        {
            if (!GameManager.IsGamePause)
            {
                GameManager.GameTime += 1f;
                UpdateTimeUI();
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }

    public void playSound(AudioClip clip)
    {
        GameManager.PlaySound(clip);
    }

    public void UpdateTimeUI()
    {
        timeLable.text = GameManager.GetTimeString(GameManager.GameTime);
    }

    public void UpdateModeUI()
    {
        modeLable.text = GameManager.GameMode.ToString();
    }

    public void OpenNewMenu()
    {
        newGameBG.SetActive(true);
        LeanTween.moveLocalY(newGameMenu, -1280 / 2f, .2f);
    }

    public void CloseNewMenu()
    {
        newGameBG.SetActive(false);
        LeanTween.moveLocalY(newGameMenu, -1400, .2f);
    }

    public void PauseGame()
    {
        GameManager.IsGamePause = true;
        pauseDialoag.SetActive(true);
        SudokuManager.intance.UpdateAllBlockUI();
    }

    public void ResumeGame()
    {
        GameManager.IsGamePause = false;
        pauseDialoag.SetActive(false);
        SudokuManager.intance.UpdateAllBlockUI();
    }

    public void StartEasyGame()
    {
        GameManager.GameMode = GameManager.GameModeType.Easy;
        SudokuManager.intance.LoadNewGame();
    }

    public void StartMediumGame()
    {
        GameManager.GameMode = GameManager.GameModeType.Medium;
        SudokuManager.intance.LoadNewGame();
    }

    public void StartHardGame()
    {
        GameManager.GameMode = GameManager.GameModeType.Hard;
        SudokuManager.intance.LoadNewGame();
    }

    public void StartExpertGame()
    {
        GameManager.GameMode = GameManager.GameModeType.Expert;
        SudokuManager.intance.LoadNewGame();
    }

    public void RestartGame()
    {
        SudokuManager.intance.RestartGame();
    }

    void SetupToggles()
    {
        soundToggle.onValueChanged.AddListener((arg0) => GameManager.IsSound = arg0);
        soundToggle.isOn = GameManager.IsSound;

        duplicateToggle.onValueChanged.AddListener((arg0) => GameManager.HighlightDuplicate = arg0);
        duplicateToggle.isOn = GameManager.HighlightDuplicate;

        identicalNumToggle.onValueChanged.AddListener((arg0) => GameManager.HighlightIdenticalNumbers = arg0);
        identicalNumToggle.isOn = GameManager.HighlightIdenticalNumbers;

        autoToggle.onValueChanged.AddListener((arg0) => GameManager.AutoRemoveNotes = arg0);
        autoToggle.isOn = GameManager.AutoRemoveNotes;
    }
}
