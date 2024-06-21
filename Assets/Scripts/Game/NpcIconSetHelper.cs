using System;
using UnityEngine;

public class NpcIconSetHelper : MonoBehaviour
{
    private void Start()
    {
        this.ReReadCurData();
    }

    private void OnGUI()
    {
        if (!NpcIconSetHelper.guiSwitch)
        {
            return;
        }
        GUILayout.Space(200f);
        GUI.skin.horizontalSlider.fixedWidth = 180f;
        GUI.skin.horizontalSlider.fixedHeight = 13f;
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label("POSITION_X:", new GUILayoutOption[0]);
        this.x = GUILayout.HorizontalSlider(this.x, -5f, 5f, new GUILayoutOption[0]);
        GUILayout.Label("     X:" + this.FloatToConstLengthStr(this.x), new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label("POSITION_Y:", new GUILayoutOption[0]);
        this.y = GUILayout.HorizontalSlider(this.y, -5f, 5f, new GUILayoutOption[0]);
        GUILayout.Label("     Y:" + this.FloatToConstLengthStr(this.y), new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label("POSITION_Z:", new GUILayoutOption[0]);
        this.z = GUILayout.HorizontalSlider(this.z, -5f, 5f, new GUILayoutOption[0]);
        GUILayout.Label("     Z:" + this.FloatToConstLengthStr(this.z), new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label("ROTATION_X:", new GUILayoutOption[0]);
        this.rx = GUILayout.HorizontalSlider(this.rx, -180f, 180f, new GUILayoutOption[0]);
        GUILayout.Label("     X:" + this.FloatToConstLengthStr(this.rx), new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label("ROTATION_Y:", new GUILayoutOption[0]);
        this.ry = GUILayout.HorizontalSlider(this.ry, -180f, 180f, new GUILayoutOption[0]);
        GUILayout.Label("     Y:" + this.FloatToConstLengthStr(this.ry), new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label("ROTATION_Z:", new GUILayoutOption[0]);
        this.rz = GUILayout.HorizontalSlider(this.rz, -180f, 180f, new GUILayoutOption[0]);
        GUILayout.Label("     Z:" + this.FloatToConstLengthStr(this.rz), new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
        this.rstr = string.Concat(new string[]
        {
            this.x.ToString("f1"),
            "|",
            this.y.ToString("f1"),
            "|",
            this.z.ToString("f1"),
            "|",
            this.rx.ToString("f1"),
            "|",
            this.ry.ToString("f1"),
            "|",
            this.rz.ToString("f1")
        });
        if (GUILayout.Button("RESET", new GUILayoutOption[0]))
        {
            this.x = 0f;
            this.y = -0.7f;
            this.z = 1.5f;
            this.rx = 0f;
            this.ry = 180f;
            this.rz = 0f;
        }
        GUILayout.BeginHorizontal(new GUILayoutOption[0]);
        GUILayout.Label("RESULT_STRING:", new GUILayoutOption[0]);
        this.rstr = GUILayout.TextField(this.rstr, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();
    }

    private void LateUpdate()
    {
        if (base.transform.childCount > 0)
        {
            Vector3 localPosition = new Vector3(this.x, this.y, this.z);
            Transform child = base.transform.GetChild(0);
            child.localPosition = localPosition;
            child.localRotation = Quaternion.Euler(this.rx, this.ry, this.rz);
        }
    }

    private string FloatToConstLengthStr(float f)
    {
        string text = f.ToString("f1");
        while (text.Length < 5)
        {
            text += "0";
        }
        if (text.Length >= 5)
        {
            text = text.Substring(0, 4);
        }
        return text;
    }

    private void ReReadCurData()
    {
        if (base.gameObject.transform.childCount > 0)
        {
            GameObject gameObject = base.transform.GetChild(0).gameObject;
            this.x = gameObject.transform.localPosition.x;
            this.y = gameObject.transform.localPosition.y;
            this.z = gameObject.transform.localPosition.z;
            Vector3 eulerAngles = gameObject.transform.localRotation.eulerAngles;
            this.rx = eulerAngles.x;
            this.ry = eulerAngles.y;
            this.rz = eulerAngles.z;
        }
    }

    public static void EnableSeting()
    {
        NpcIconSetHelper.guiSwitch = !NpcIconSetHelper.guiSwitch;
        if (NpcIconSetHelper.instance == null)
        {
            GameObject gameObject = GameObject.Find("Cam_For3d_Icon");
            if (!(gameObject != null))
            {
                return;
            }
            NpcIconSetHelper.instance = gameObject.AddComponent<NpcIconSetHelper>();
        }
        NpcIconSetHelper.instance.ReReadCurData();
        NpcIconSetHelper.instance.enabled = true;
    }

    private void OnDisable()
    {
        NpcIconSetHelper.instance.enabled = false;
    }

    private float x;

    private float y;

    private float z;

    private float rx;

    private float ry;

    private float rz;

    private string rstr = string.Empty;

    private static bool guiSwitch;

    private static NpcIconSetHelper instance;
}
