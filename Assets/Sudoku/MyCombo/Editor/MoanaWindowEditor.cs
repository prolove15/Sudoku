using UnityEngine;
using UnityEditor;

public class MoanaWindowEditor
{
    [MenuItem("Moana Games/Reset game")]
    static void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    [MenuItem("Moana Games/Set hint = 100")]
    static void SetHint100()
    {
        GameManager.Hints = 100;
        PlayerPrefs.Save();
    }

    [MenuItem("Moana Games/Set hint = 0")]
    static void SetHint0()
    {
        GameManager.Hints = 0;
        PlayerPrefs.Save();
    }

    [MenuItem("Moana Games/Set hint = 3")]
    static void SetHint3()
    {
        GameManager.Hints = 3;
        PlayerPrefs.Save();
    }
}