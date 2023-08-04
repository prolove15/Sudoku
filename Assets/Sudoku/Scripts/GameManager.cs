using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameModeType
    {
        Easy,
        Medium,
        Hard,
        Expert
    }

    public static bool IsGamePause
    {
        get
        {
            return PlayerPrefs.GetInt("isGamePause", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("isGamePause", value ? 1 : 0);
        }
    }

    public static AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("sound File not set");
        }
        if (IsSound)
            audioSource.PlayOneShot(clip);
    }

    public static GameModeType GameMode
    {
        get
        {
            return (GameModeType)PlayerPrefs.GetInt("GameMode", 0);
        }
        set
        {
            PlayerPrefs.SetInt("GameMode", (int)value);
        }
    }

    public static float GameTime
    {
        get
        {
            return PlayerPrefs.GetFloat("GameTime", 0f);
        }
        set
        {
            PlayerPrefs.SetFloat("GameTime", value);
        }
    }

    public static bool IsSound
    {
        get
        {
            return PlayerPrefs.GetInt("isSound", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("isSound", value ? 1 : 0);
        }
    }

    public static bool HighlightDuplicate
    {
        get
        {
            return PlayerPrefs.GetInt("HighlightDuplicate", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("HighlightDuplicate", value ? 1 : 0);
        }
    }

    public static bool HighlightIdenticalNumbers
    {
        get
        {
            return PlayerPrefs.GetInt("HighlightIdenticalNumbers", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("HighlightIdenticalNumbers", value ? 1 : 0);
        }
    }

    public static bool AutoRemoveNotes
    {
        get
        {
            return PlayerPrefs.GetInt("AutoRemoveNotes", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("AutoRemoveNotes", value ? 1 : 0);
        }
    }

    public static int Hints
    {
        get
        {
            return PlayerPrefs.GetInt("Hints", 3);
        }
        set
        {
            PlayerPrefs.SetInt("Hints", value);
        }
    }

    public static int LastLoaded
    {
        get
        {
            return PlayerPrefs.GetInt("LastLoaded_" + GameMode, -1);
        }
        set
        {
            PlayerPrefs.SetInt("LastLoaded_" + GameMode, value);
        }
    }

    public static DataGame SavedGame
    {
        get
        {
            return JsonUtility.FromJson<DataGame>(PlayerPrefs.GetString("savedGame", JsonUtility.ToJson(new DataGame())));
        }
        set
        {
            PlayerPrefs.SetString("savedGame", JsonUtility.ToJson(value));
        }
    }

    public static float HighScoreTime
    {
        get
        {
            return PlayerPrefs.GetFloat("HighScore" + GameMode, float.MaxValue);
        }
        set
        {
            PlayerPrefs.SetFloat("HighScore" + GameMode, value);
        }
    }

    public static string GetTimeString(float t)
    {
        int h = Mathf.FloorToInt(t / 3600f);
        int m = Mathf.FloorToInt(t / 60f) % 60;
        int s = Mathf.FloorToInt(t) % 60;
        return (h > 0 ? (h.ToString("00") + " : ") : "") + m.ToString("00") + " : " + s.ToString("00");
    }

    private float lastPressedTime = -10;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.time - lastPressedTime > 2.5f)
            {
                Toast.instance.ShowMessage("Press back again to quit game");
                lastPressedTime = Time.time;
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
