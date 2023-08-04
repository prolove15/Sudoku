#pragma warning disable 0162 // code unreached.
#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used
#pragma warning disable 0429 //never used

using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class AdmobChecker : EditorWindow 
{
	private const string Admob = "ADMOB";
    private static AdmobChecker Instance;

	public static void OpenWelcomeWindow()
	{
		var window = GetWindow<AdmobChecker>(true);
        window.position = new Rect(700, 400, 350, 200);
    }

    public static bool IsOpen
    {
        get { return Instance != null; }
    }

    static AdmobChecker()
	{
	}

	//call from Autorun
	public static void OpenPopupAdmobStartup()
	{
		EditorApplication.update += CheckItNow;
	}

	public static void CheckItNow()
	{
		if (Directory.Exists ("Assets/GoogleMobileAds"))
		{
			SetScriptingDefineSymbols ();

            if (Instance != null)
            {
                Instance.Close();
            }
		}
		else
		{ 
			OpenWelcomeWindow();
		}

		EditorApplication.update -= CheckItNow;
	}

	static void SetScriptingDefineSymbols () 
	{
		SetSymbolsForTarget (BuildTargetGroup.Android, Admob);
		SetSymbolsForTarget (BuildTargetGroup.iOS, Admob); 
	}

	public void OnGUI()
    {
        GUILayoutUtility.GetRect(position.width, 20);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Download and import Google Mobile Ads (Admob)",  GUILayout.Width(320), GUILayout.Height(50)))
		{
            Application.OpenURL("https://github.com/googleads/googleads-mobile-unity/releases/tag/v3.13.1");
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        GUI.skin.label.wordWrap = true;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("1. Download the GoogleMobileAds.unitypackage", GUILayout.MaxWidth(350));
        GUILayout.Space(10);
        GUILayout.Label("2. In Unity, go to menu Asset -> Import Package -> Custom Package", GUILayout.MaxWidth(350));
        GUILayout.Space(10);
        GUILayout.Label("3. This dialog message will disappear after it's done", GUILayout.MaxWidth(350)); 
    }

    static void SetSymbolsForTarget(BuildTargetGroup target, string scriptingSymbol)
	{
		var s = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

		string sTemp = scriptingSymbol;

		if(!s.Contains(sTemp))
		{

			s = s.Replace(scriptingSymbol + ";","");

			s = s.Replace(scriptingSymbol,"");  

			s = scriptingSymbol + ";" + s;

			PlayerSettings.SetScriptingDefineSymbolsForGroup(target,s);
		}
	}

	void OnEnable()
	{
        Instance = this;
#if UNITY_5_3_OR_NEWER
        titleContent = new GUIContent("Please install Admob to use this asset"); 
#endif
	}	
}
