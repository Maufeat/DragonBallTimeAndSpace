using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ScreenCapture : MonoBehaviour
{
    private void Awake()
    {
        this._uiCamera = base.transform.Find("Camera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            string name = string.Concat(new object[]
            {
                "\\",
                DateTime.Now.Year,
                "_",
                DateTime.Now.Month,
                "_",
                DateTime.Now.Day,
                "_",
                DateTime.Now.Hour,
                "_",
                DateTime.Now.Minute,
                "_",
                DateTime.Now.Second,
                "_Shot.PNG"
            });
            this._path = Application.streamingAssetsPath + "\\ScreenShot";
            this._sceneCamera = Camera.main;
            base.StartCoroutine(this.CaptureCamera(this._sceneCamera, this._uiCamera, new Rect(0f, 0f, 1920f, 1080f), name));
        }
    }

    private IEnumerator CaptureCamera(Camera camera, Camera camera2, Rect rect, string name)
    {
        yield return new WaitForEndOfFrame();
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        rt.depth = 24;
        camera.targetTexture = rt;
        camera.Render();
        camera2.targetTexture = rt;
        camera2.Render();
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        camera.targetTexture = null;
        camera2.targetTexture = null;
        RenderTexture.active = null;
        UnityEngine.Object.Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        if (!Directory.Exists(this._path))
        {
            Directory.CreateDirectory(this._path);
        }
        File.WriteAllBytes(this._path + name, bytes);
        Debug.Log(string.Format("截屏了一张照片: {0}", name));
        yield break;
    }

    public Camera _sceneCamera;

    public Camera _uiCamera;

    private string _path = string.Empty;
}
