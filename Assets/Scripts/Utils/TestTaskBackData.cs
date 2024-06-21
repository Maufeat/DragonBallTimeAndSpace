using System;
using UnityEngine;

public class TestTaskBackData : MonoBehaviour
{
    private void Start()
    {
        TestTaskBackData.instance = this;
        this.gStyle = new GUIStyle();
        this.gStyle.normal.textColor = Color.red;
    }

    private void OnGUI()
    {
        if (TestTaskBackData.mSwitch)
        {
            GUI.Label(new Rect(200f, 50f, 500f, 500f), TestTaskBackData.testBackData, this.gStyle);
        }
    }

    public static void TaskTestMessageSwitch()
    {
        TestTaskBackData.mSwitch = !TestTaskBackData.mSwitch;
        if (TestTaskBackData.instance == null)
        {
            Camera.main.gameObject.AddComponent<TestTaskBackData>();
        }
    }

    private static TestTaskBackData instance;

    private GUIStyle gStyle;

    public Vector2 scrollPos = Vector2.zero;

    public Vector2 scrollPosTest = Vector2.zero;

    public static string testBackData = "START";

    public static bool mSwitch;
}
