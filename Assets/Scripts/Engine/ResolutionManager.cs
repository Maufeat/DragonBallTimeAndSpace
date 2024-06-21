using System;
using UnityEngine;

namespace Engine
{
    public class ResolutionManager : MonoBehaviour
    {
        public static ResolutionManager Instance
        {
            get
            {
                object mutex = ResolutionManager.m_mutex;
                ResolutionManager instance;
                lock (mutex)
                {
                    if (null == ResolutionManager.m_instance)
                    {
                        ResolutionManager.m_instance = new ResolutionManager();
                    }
                    instance = ResolutionManager.m_instance;
                }
                return instance;
            }
        }

        private GameObject uiRoot
        {
            get
            {
                if (this._uiRoot == null)
                {
                    this._uiRoot = GameObject.Find("UIRoot");
                }
                return this._uiRoot;
            }
        }

        public void Awake()
        {
            ResolutionManager.m_instance = this;
            this.ReadResolution();
        }

        public Ray MainCameraScreenPointToRay(Vector3 point)
        {
            Camera main = Camera.main;
            float num = Mathf.Clamp((float)main.pixelWidth / (float)Screen.width, 720f / (float)Screen.width, 1f);
            float num2 = Mathf.Clamp((float)main.pixelHeight / (float)Screen.height, 720f / (float)Screen.width, 1f);
            point.x *= num;
            point.y *= num2;
            return main.ScreenPointToRay(point);
        }

        public void GetResolution(ref int width, ref int height, ref int fullscreen)
        {
            width = (height = 0);
            fullscreen = 1;
            if (this._info == null)
            {
                return;
            }
            width = int.Parse(this._info.Width);
            height = int.Parse(this._info.Height);
            fullscreen = this._info.FullScreen;
        }

        public void SetResolutionWidthHeight(int width, int height)
        {
            if (this._info == null)
            {
                this._info = new ResolutionInfo();
            }
            this._info.Width = width.ToString();
            this._info.Height = height.ToString();
            this.WriteResolution();
        }

        public void SetResolutionFull(int fullscreen)
        {
            if (this._info == null)
            {
                this._info = new ResolutionInfo();
            }
            this._info.FullScreen = fullscreen;
            this.WriteResolution();
        }

        public void SetResolution(int width, int height, int fullscreen)
        {
            if (this._info == null)
            {
                this._info = new ResolutionInfo();
            }
            this._info.Width = width.ToString();
            this._info.Height = height.ToString();
            this._info.FullScreen = fullscreen;
            this.WriteResolution();
        }

        public void WriteResolution()
        {
            if (int.Parse(this._info.Width) > 0 && int.Parse(this._info.Height) > 0)
            {
                this.SetWindowStyle(this._info);
                if (LuaScriptMgr.Instance != null && LuaScriptMgr.Instance.lua != null && Util.GetLuaTable("MainUICtrl") != null)
                {
                    LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.OnEnterOrExitGuild", new object[]
                    {
                        Util.GetLuaTable("MainUICtrl")
                    });
                    LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.ShowTaskList", new object[]
                    {
                        Util.GetLuaTable("MainUICtrl")
                    });
                }
            }
            string value = ServerStorageManager.Instance.SerializeClassLocal<ResolutionInfo>(this._info);
            MyPlayerPrefs.SetString("resolution_info", value);
        }

        private void SetWindowStyle(ResolutionInfo info)
        {
            WindowMod component = this.uiRoot.GetComponent<WindowMod>();
            component.SetWindowStyle((this._info.FullScreen != 1) ? WindowMod.appStyle.Windowed : WindowMod.appStyle.WindowedFullScreenWithoutBorder, int.Parse(this._info.Width), int.Parse(this._info.Height), this._info.pixelpercent / 100f);
            component.SetMouseSpeed(this._info.mouseSpeed);
        }

        public void ReadResolution()
        {
			#if !UNITY_EDITOR
            string @string = MyPlayerPrefs.GetString("resolution_info");
            if (@string == string.Empty)
            {
                this.CreateResolutionInfo();
                this.WriteResolution();
            }
            else
            {
                ResolutionInfo resolutionInfo = ServerStorageManager.Instance.DeserializeClassLocal<ResolutionInfo>(@string);
                if (resolutionInfo != null && !this.HaveZeroData(resolutionInfo))
                {
                    this._info = resolutionInfo;
                    this.SetWindowStyle(this._info);
                }
                else
                {
                    this.CreateResolutionInfo();
                    this.WriteResolution();
                }
            }
			#endif
        }

        private bool HaveZeroData(ResolutionInfo info)
        {
            return info.CameraMaxdistance == 0 || info.UIScale == 0 || info.mouseSpeed == 0U || info.pixelpercent == 0U;
        }

        private void CreateResolutionInfo()
        {
            this._info = new ResolutionInfo();
            this._info.FullScreen = 1;
            this._info.Height = Screen.resolutions[Screen.resolutions.Length - 1].height.ToString();
            this._info.Width = Screen.resolutions[Screen.resolutions.Length - 1].width.ToString();
            this._info.ReferenceResolution = (this._info.CurResolution = Screen.resolutions.Length - 1);
            this._info.ModeIndex = 0;
            this._info.CameraMaxdistance = 10;
            this._info.UIScale = 100;
            this._info.mouseSpeed = 10U;
            this._info.pixelpercent = 1U;
        }

        public void SaveResolutionInfo(ResolutionInfo info)
        {
            this._info = info;
            this.WriteResolution();
        }

        public ResolutionInfo GetResolutionInfo()
        {
            return this._info;
        }

        private static readonly object m_mutex = new object();

        private static ResolutionManager m_instance;

        private ResolutionInfo _info;

        private GameObject _uiRoot;
    }
}
