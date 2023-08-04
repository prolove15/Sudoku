using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeManager : MonoBehaviour
{
    public static ThemeManager intance;
    public List<Graphic> lightColorElements;
    public List<Graphic> DarkColorElements;

    [Space(20)]
    public List<Toggle> themeToggles;
    public Theme[] themes;

    public Theme CurrentTheme
    {
        get
        {
            return themes[SelectedToggle];
        }
    }

    public int SelectedToggle
    {
        get
        {
            return Mathf.Clamp(themeToggles.FindIndex((obj) => obj.isOn), 0, themes.Length);
        }
    }

    void Awake()
    {
        intance = this;
    }

    void Start()
    {
        SetUpToggle();
    }

    void SetUpToggle()
    {
        for (int index = 0; index < 3; index++)
        {
            themeToggles[index].targetGraphic.color = themes[index].lightColor;
            int temp = index;

            themeToggles[index].onValueChanged.AddListener((arg0) =>
            {
                if (arg0)
                {
                    PlayerPrefs.SetInt("SelectedTheme", temp);
                    UpdateTheme();

                }
            });
        }
        themeToggles[PlayerPrefs.GetInt("SelectedTheme", 0)].isOn = true;
        UpdateTheme();
    }

    public void UpdateTheme()
    {
        foreach (var item in lightColorElements)
        {
            if (item != null)
                item.color = CurrentTheme.lightColor;
        }
        foreach (var item in DarkColorElements)
        {
            if (item != null)
                item.color = CurrentTheme.darkColor;
        }
    }
}

[System.Serializable]
public class Theme
{
    [Space(10)]
    public Color darkColor;
    public Color lightColor;
    [Space(10)]
    public Color defultNumColor;
    public Color rightNumColor;
    public Color wroungNumColor;
    public Color hintNumColor;
    [Space(10)]
    public Color boxNormalColor;
    public Color boxSelectedColor;
    public Color boxHighlight;
    public Color boxWithWorngNum;
}