using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UITools
{
    public static float soundVolume
    {
        get
        {
            if (!UITools.mLoaded)
            {
                UITools.mLoaded = true;
                UITools.mGlobalVolume = PlayerPrefs.GetFloat("Sound", 1f);
            }
            return UITools.mGlobalVolume;
        }
        set
        {
            if (UITools.mGlobalVolume != value)
            {
                UITools.mLoaded = true;
                UITools.mGlobalVolume = value;
                PlayerPrefs.SetFloat("Sound", value);
            }
        }
    }

    public static bool fileAccess
    {
        get
        {
            return true;
        }
    }

    public static AudioSource PlaySound(AudioClip clip)
    {
        return UITools.PlaySound(clip, 1f, 1f);
    }

    public static AudioSource PlaySound(AudioClip clip, float volume)
    {
        return UITools.PlaySound(clip, volume, 1f);
    }

    public static AudioSource PlaySound(AudioClip clip, float volume, float pitch)
    {
        volume *= UITools.soundVolume;
        if (clip != null && volume > 0.01f)
        {
            if (UITools.mListener == null)
            {
                UITools.mListener = (UnityEngine.Object.FindObjectOfType(typeof(AudioListener)) as AudioListener);
                if (UITools.mListener == null)
                {
                    Camera camera = Camera.main;
                    if (camera == null)
                    {
                        camera = (UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera);
                    }
                    if (camera != null)
                    {
                        UITools.mListener = camera.gameObject.AddComponent<AudioListener>();
                    }
                }
            }
            if (UITools.mListener != null && UITools.mListener.enabled && UITools.GetActive(UITools.mListener.gameObject))
            {
                AudioSource audioSource = UITools.mListener.GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = UITools.mListener.gameObject.AddComponent<AudioSource>();
                }
                audioSource.pitch = pitch;
                audioSource.PlayOneShot(clip, volume);
                return audioSource;
            }
        }
        return null;
    }

    public static WWW OpenURL(string url)
    {
        WWW result = null;
        try
        {
            result = new WWW(url);
        }
        catch (Exception ex)
        {
            FFDebug.LogError("UITools", ex.Message);
        }
        return result;
    }

    public static WWW OpenURL(string url, WWWForm form)
    {
        if (form == null)
        {
            return UITools.OpenURL(url);
        }
        WWW result = null;
        try
        {
            result = new WWW(url, form);
        }
        catch (Exception ex)
        {
            FFDebug.LogError("UITools", (ex == null) ? "<null>" : ex.Message);
        }
        return result;
    }

    public static int RandomRange(int min, int max)
    {
        if (min == max)
        {
            return min;
        }
        return UnityEngine.Random.Range(min, max + 1);
    }

    public static string GetHierarchy(GameObject obj)
    {
        string text = obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            text = obj.name + "/" + text;
        }
        return "\"" + text + "\"";
    }

    public static Color ParseColor(string text, int offset)
    {
        int num = UIMath.HexToDecimal(text[offset]) << 4 | UIMath.HexToDecimal(text[offset + 1]);
        int num2 = UIMath.HexToDecimal(text[offset + 2]) << 4 | UIMath.HexToDecimal(text[offset + 3]);
        int num3 = UIMath.HexToDecimal(text[offset + 4]) << 4 | UIMath.HexToDecimal(text[offset + 5]);
        float num4 = 0.003921569f;
        return new Color(num4 * (float)num, num4 * (float)num2, num4 * (float)num3);
    }

    public static string EncodeColor(Color c)
    {
        int num = 16777215 & UIMath.ColorToInt(c) >> 8;
        return UIMath.DecimalToHex(num);
    }

    public static int ParseSymbol(string text, int index, List<Color> colors, bool premultiply)
    {
        int length = text.Length;
        if (index + 2 < length)
        {
            if (text[index + 1] == '-')
            {
                if (text[index + 2] == ']')
                {
                    if (colors != null && colors.Count > 1)
                    {
                        colors.RemoveAt(colors.Count - 1);
                    }
                    return 3;
                }
            }
            else if (index + 7 < length && text[index + 7] == ']')
            {
                if (colors != null)
                {
                    Color color = UITools.ParseColor(text, index + 1);
                    if (UITools.EncodeColor(color) != text.Substring(index + 1, 6).ToUpper())
                    {
                        return 0;
                    }
                    color.a = colors[colors.Count - 1].a;
                    if (premultiply && color.a != 1f)
                    {
                        color = Color.Lerp(UITools.mInvisible, color, color.a);
                    }
                    colors.Add(color);
                }
                return 8;
            }
        }
        return 0;
    }

    public static string StripSymbols(string text)
    {
        if (text != null)
        {
            int i = 0;
            int length = text.Length;
            while (i < length)
            {
                char c = text[i];
                if (c == '[')
                {
                    int num = UITools.ParseSymbol(text, i, null, false);
                    if (num > 0)
                    {
                        text = text.Remove(i, num);
                        length = text.Length;
                        continue;
                    }
                }
                i++;
            }
        }
        return text;
    }

    public static T[] FindActive<T>() where T : Component
    {
        return UnityEngine.Object.FindObjectsOfType(typeof(T)) as T[];
    }

    public static Camera FindCameraForLayer(int layer)
    {
        int num = 1 << layer;
        Camera[] array = UITools.FindActive<Camera>();
        int i = 0;
        int num2 = array.Length;
        while (i < num2)
        {
            Camera camera = array[i];
            if ((camera.cullingMask & num) != 0)
            {
                return camera;
            }
            i++;
        }
        return null;
    }

    public static string GetName<T>() where T : Component
    {
        string text = typeof(T).ToString();
        if (text.StartsWith("UI"))
        {
            text = text.Substring(2);
        }
        else if (text.StartsWith("UnityEngine."))
        {
            text = text.Substring(12);
        }
        return text;
    }

    public static GameObject AddChild(GameObject parent)
    {
        GameObject gameObject = new GameObject();
        if (parent != null)
        {
            Transform transform = gameObject.transform;
            transform.SetParent(parent.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            gameObject.layer = parent.layer;
        }
        return gameObject;
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
        if (gameObject != null && parent != null)
        {
            Transform transform = gameObject.transform;
            transform.SetParent(parent.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            gameObject.layer = parent.layer;
        }
        return gameObject;
    }

    public static T AddChild<T>(GameObject parent) where T : Component
    {
        GameObject gameObject = UITools.AddChild(parent);
        gameObject.name = UITools.GetName<T>();
        return gameObject.AddComponent<T>();
    }

    public static GameObject GetRoot(GameObject go)
    {
        Transform transform = go.transform;
        for (; ; )
        {
            Transform parent = transform.parent;
            if (parent == null)
            {
                break;
            }
            transform = parent;
        }
        return transform.gameObject;
    }

    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null)
        {
            return (T)((object)null);
        }
        object obj = go.GetComponent<T>();
        if (obj == null)
        {
            Transform parent = go.transform.parent;
            while (parent != null && obj == null)
            {
                obj = parent.gameObject.GetComponent<T>();
                parent = parent.parent;
            }
        }
        return (T)((object)obj);
    }

    public static void Destroy(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isPlaying)
            {
                if (obj is GameObject)
                {
                    GameObject gameObject = obj as GameObject;
                    gameObject.transform.SetParent(null);
                }
                UnityEngine.Object.Destroy(obj);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
        }
    }

    public static void DestroyImmediate(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isEditor)
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
            else
            {
                UnityEngine.Object.Destroy(obj);
            }
        }
    }

    public static void Broadcast(string funcName)
    {
        GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        int i = 0;
        int num = array.Length;
        while (i < num)
        {
            array[i].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
            i++;
        }
    }

    public static void Broadcast(string funcName, object param)
    {
        GameObject[] array = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        int i = 0;
        int num = array.Length;
        while (i < num)
        {
            array[i].SendMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
            i++;
        }
    }

    public static bool IsChild(Transform parent, Transform child)
    {
        if (parent == null || child == null)
        {
            return false;
        }
        while (child != null)
        {
            if (child == parent)
            {
                return true;
            }
            child = child.parent;
        }
        return false;
    }

    private static void Activate(Transform t)
    {
        UITools.SetActiveSelf(t.gameObject, true);
        int i = 0;
        int childCount = t.childCount;
        while (i < childCount)
        {
            Transform child = t.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                return;
            }
            i++;
        }
        int j = 0;
        int childCount2 = t.childCount;
        while (j < childCount2)
        {
            Transform child2 = t.GetChild(j);
            UITools.Activate(child2);
            j++;
        }
    }

    private static void Deactivate(Transform t)
    {
        UITools.SetActiveSelf(t.gameObject, false);
    }

    public static void SetActive(GameObject go, bool state)
    {
        if (state)
        {
            UITools.Activate(go.transform);
        }
        else
        {
            UITools.Deactivate(go.transform);
        }
    }

    public static void SetActiveChildren(GameObject go, bool state)
    {
        Transform transform = go.transform;
        if (state)
        {
            int i = 0;
            int childCount = transform.childCount;
            while (i < childCount)
            {
                Transform child = transform.GetChild(i);
                UITools.Activate(child);
                i++;
            }
        }
        else
        {
            int j = 0;
            int childCount2 = transform.childCount;
            while (j < childCount2)
            {
                Transform child2 = transform.GetChild(j);
                UITools.Deactivate(child2);
                j++;
            }
        }
    }

    public static void SetActiveChildren(Transform go, bool state)
    {
        if (state)
        {
            int i = 0;
            int childCount = go.childCount;
            while (i < childCount)
            {
                Transform child = go.GetChild(i);
                UITools.Activate(child);
                i++;
            }
        }
        else
        {
            int j = 0;
            int childCount2 = go.childCount;
            while (j < childCount2)
            {
                Transform child2 = go.GetChild(j);
                UITools.Deactivate(child2);
                j++;
            }
        }
    }

    public static bool GetActive(GameObject go)
    {
        return go && go.activeInHierarchy;
    }

    public static void SetActiveSelf(GameObject go, bool state)
    {
        go.SetActive(state);
    }

    public static void SetActiveSelf(Transform go, bool state)
    {
        go.gameObject.SetActive(state);
    }

    public static void SetLayer(GameObject go, int layer)
    {
        go.layer = layer;
        Transform transform = go.transform;
        int i = 0;
        int childCount = transform.childCount;
        while (i < childCount)
        {
            Transform child = transform.GetChild(i);
            UITools.SetLayer(child.gameObject, layer);
            i++;
        }
    }

    public static Vector3 Round(Vector3 v)
    {
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);
        return v;
    }

    public static bool Save(string fileName, byte[] bytes)
    {
        if (!UITools.fileAccess)
        {
            return false;
        }
        string path = Application.persistentDataPath + "/" + fileName;
        if (bytes == null)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return true;
        }
        FileStream fileStream = null;
        try
        {
            fileStream = File.Create(path);
        }
        catch (Exception)
        {
            return false;
        }
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
        return true;
    }

    public static byte[] Load(string fileName)
    {
        if (!UITools.fileAccess)
        {
            return null;
        }
        string path = Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        return null;
    }

    public static Color ApplyPMA(Color c)
    {
        if (c.a != 1f)
        {
            c.r *= c.a;
            c.g *= c.a;
            c.b *= c.a;
        }
        return c;
    }

    private static PropertyInfo GetSystemCopyBufferProperty()
    {
        if (UITools.mSystemCopyBuffer == null)
        {
            Type typeFromHandle = typeof(GUIUtility);
            UITools.mSystemCopyBuffer = typeFromHandle.GetProperty("systemCopyBuffer", BindingFlags.Static | BindingFlags.NonPublic);
        }
        return UITools.mSystemCopyBuffer;
    }

    public static string clipboard
    {
        get
        {
            PropertyInfo systemCopyBufferProperty = UITools.GetSystemCopyBufferProperty();
            return (systemCopyBufferProperty == null) ? null : ((string)systemCopyBufferProperty.GetValue(null, null));
        }
        set
        {
            PropertyInfo systemCopyBufferProperty = UITools.GetSystemCopyBufferProperty();
            if (systemCopyBufferProperty != null)
            {
                systemCopyBufferProperty.SetValue(null, value, null);
            }
        }
    }

    public static Transform FindTransformByName(Transform root, string strName)
    {
        Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            if (componentsInChildren[i].name == strName)
            {
                return componentsInChildren[i];
            }
        }
        return null;
    }

    public static List<Transform> FindTransformContainName(Transform root, string strName)
    {
        Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>(true);
        List<Transform> list = new List<Transform>();
        foreach (Transform transform in componentsInChildren)
        {
            if (transform.name.Contains(strName))
            {
                list.Add(transform);
            }
        }
        return list;
    }

    public static void ClearAllListGameObject(List<GameObject> objList)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            UnityEngine.Object.Destroy(objList[i]);
        }
        objList.Clear();
    }

    public static bool IsPointOverUIObject()
    {
        List<RaycastResult> list = new List<RaycastResult>();
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        EventSystem.current.RaycastAll(pointerEventData, list);
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.touches[i];
            PointerEventData pointerEventData2 = new PointerEventData(EventSystem.current);
            pointerEventData2.position = new Vector2(touch.position.x, touch.position.y);
            EventSystem.current.RaycastAll(pointerEventData2, list);
        }
        return list.Count > 0;
    }

    public static bool IsPointOverUIObject(Vector3 pos)
    {
        UITools.overuiresults.Clear();
        UITools.eventDataCurrentPosition.position = pos;
        EventSystem.current.RaycastAll(UITools.eventDataCurrentPosition, UITools.overuiresults);
        return UITools.overuiresults.Count > 0;
    }

    public static bool IsPointOverGraphic(UITools.PointLayer lay = UITools.PointLayer.Defult)
    {
        return UITools.IsPointOverGraphic(Input.mousePosition, lay);
    }

    public static bool IsPointOverGraphic(Vector3 pos, UITools.PointLayer lay)
    {
        UITools.overuiresults.Clear();
        UITools.eventDataCurrentPosition.position = pos;
        EventSystem.current.RaycastAll(UITools.eventDataCurrentPosition, UITools.overuiresults);
        UITools.pointResults.Clear();
        for (int i = 0; i < UITools.overuiresults.Count; i++)
        {
            Graphic component = UITools.overuiresults[i].gameObject.GetComponent<Graphic>();
            if (component && component.raycastTarget)
            {
                UITools.pointResults.Add(UITools.overuiresults[i]);
            }
        }
        return UITools.pointResults.Count > 0;
    }

    public static Text GenerateUnderLine(Text text)
    {
        if (text == null)
        {
            return null;
        }
        Text text2 = UnityEngine.Object.Instantiate<Text>(text);
        text2.name = "underline";
        text2.transform.SetParent(text.transform);
        RectTransform rectTransform = text2.rectTransform;
        rectTransform.localScale = Vector3.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        text2.text = "_";
        float preferredWidth = text2.preferredWidth;
        float preferredWidth2 = text.preferredWidth;
        int num = (int)Mathf.Round(preferredWidth2 / preferredWidth);
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < num; i++)
        {
            stringBuilder.Append("_");
        }
        text2.text = stringBuilder.ToString();
        rectTransform.localPosition = Vector3.zero;
        return text2;
    }

    public static float GetTextWidth(Text text)
    {
        Font font = text.font;
        font.RequestCharactersInTexture(text.text, text.fontSize, text.fontStyle);
        float num = 0f;
        for (int i = 0; i < text.text.Length; i++)
        {
            CharacterInfo characterInfo;
            font.GetCharacterInfo(text.text[i], out characterInfo, text.fontSize);
            num += (float)characterInfo.advance;
        }
        return num;
    }

    public static float GetTextWidth(Text text, string content)
    {
        Font font = text.font;
        font.RequestCharactersInTexture(content, text.fontSize, text.fontStyle);
        float num = 0f;
        for (int i = 0; i < content.Length; i++)
        {
            CharacterInfo characterInfo;
            font.GetCharacterInfo(content[i], out characterInfo, text.fontSize);
            num += (float)characterInfo.advance;
        }
        return num;
    }

    public static string RemoveRichText(string content)
    {
        string text = content.Replace("<b>", string.Empty);
        text = text.Replace("</b>", string.Empty);
        text = text.Replace("<i>", string.Empty);
        text = text.Replace("</i>", string.Empty);
        StringBuilder stringBuilder = new StringBuilder();
        while (text.Length > 0)
        {
            int num = text.IndexOf("<color=");
            if (num >= 0)
            {
                string value = text.Substring(0, num);
                stringBuilder.Append(value);
                if (text.Length < 15)
                {
                    text = text.Remove(0, text.Length);
                }
                else
                {
                    text = text.Remove(0, num + 15);
                }
            }
            int num2 = text.IndexOf("</color>");
            if (num2 >= 0)
            {
                string value2 = text.Substring(0, num2);
                stringBuilder.Append(value2);
                text = text.Remove(0, num2 + 8);
            }
            else
            {
                string value3 = text.Substring(0, text.Length);
                text = text.Remove(0, text.Length);
                stringBuilder.Append(value3);
            }
        }
        return stringBuilder.ToString();
    }

    private static AudioListener mListener;

    private static bool mLoaded = false;

    private static float mGlobalVolume = 1f;

    private static Color mInvisible = new Color(0f, 0f, 0f, 0f);

    private static PropertyInfo mSystemCopyBuffer = null;

    private static PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);

    private static List<RaycastResult> overuiresults = new List<RaycastResult>();

    private static List<RaycastResult> pointResults = new List<RaycastResult>();

    public enum PointLayer
    {
        Defult,
        All,
        Non_Mask
    }
}
