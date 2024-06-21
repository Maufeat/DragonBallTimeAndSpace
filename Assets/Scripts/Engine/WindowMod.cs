using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowMod : MonoBehaviour
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hPos, int x, int y, int cx, int cy, uint nflags);

    [DllImport("User32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("User32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("User32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int dwNewLong);

    [DllImport("User32.dll")]
    private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wP, IntPtr IP);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SetParent(IntPtr hChild, IntPtr hParent);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetParent(IntPtr hChild);

    [DllImport("User32.dll")]
    public static extern IntPtr GetSystemMetrics(int nIndex);

    private void Awake()
    {
        this.ui3DParent = base.transform.Find("LayerGame").gameObject;
        if (this.ui3DParent != null)
        {
            this.ui3D = this.ui3DParent.GetComponentInChildren<UI3D>(true);
            this.ui3DParent.SetActive(false);
        }
        this.mCurPtr = CWin32Help.GetProcessWnd();
        if (!WindowMod.SystemParametersInfo(93U, 0U, ref this.origSpeed, 0U))
        {
            this.origSpeed = 10U;
        }
    }

    private void Start()
    {
        Input.imeCompositionMode = IMECompositionMode.On;
        this.Xscreen = (int)WindowMod.GetSystemMetrics(0);
        this.Yscreen = (int)WindowMod.GetSystemMetrics(1);
    }

    public void SetWindowStyle(WindowMod.appStyle style, int width, int height, float percent = 1f)
    {
        Debug.LogError(string.Concat(new object[]
        {
            "width:",
            width,
            ",height:",
            height
        }));
        if (this.Xscreen == 0)
        {
            this.Xscreen = (int)WindowMod.GetSystemMetrics(0);
            this.Yscreen = (int)WindowMod.GetSystemMetrics(1);
        }
        this.AppWindowStyle = style;
        if (this.AppWindowStyle == WindowMod.appStyle.Windowed)
        {
            this.windowWidth = width;
            this.windowHeight = height;
        }
        else if (this.AppWindowStyle == WindowMod.appStyle.WindowedFullScreenWithoutBorder)
        {
            this.windowWidth = this.Xscreen;
            this.windowHeight = this.Yscreen;
        }
        Screen.SetResolution(this.windowWidth, this.windowHeight, false);
        this.i = 0;
        if (this.ui3DParent != null)
        {
            if ((float)this.windowWidth * percent < 720f)
            {
                percent = 720f / (float)this.windowWidth;
            }
            if (percent >= 1f)
            {
                this.ui3DParent.SetActive(false);
            }
            else
            {
                this.ui3D.SetRenderRect(((float)this.windowWidth * percent).ToInt(), ((float)this.windowHeight * percent).ToInt());
            }
        }
    }

    private void Update()
    {
        if (this.i < 5)
        {
            if (this.AppWindowStyle == WindowMod.appStyle.Windowed)
            {
                WindowMod.SetWindowLong(this.mCurPtr, -16, WindowMod.GetWindowLong(this.mCurPtr, -16) | 12582912 | 8388608 | 4194304);
                WindowMod.SetWindowPos(this.mCurPtr, this.HWND_NORMAL, 0, 0, this.windowWidth, this.windowHeight, 2U);
                WindowMod.SetWindowPos(this.mCurPtr, this.HWND_NORMAL, 0, 0, this.windowWidth, this.windowHeight, 34U);
            }
            if (this.AppWindowStyle == WindowMod.appStyle.WindowedFullScreenWithoutBorder)
            {
                WindowMod.SetWindowLong(this.mCurPtr, -16, WindowMod.GetWindowLong(this.mCurPtr, -16) & -12582913 & -8388609 & -4194305);
                WindowMod.SetWindowPos(this.mCurPtr, this.HWND_NORMAL, 0, 0, this.Xscreen, this.Yscreen, 64U);
            }
            this.i++;
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

    public void SetMouseSpeed(uint mouseSpeed)
    {
        if (mouseSpeed < 1U || mouseSpeed > 20U)
        {
            mouseSpeed = 10U;
        }
        this.curSpeed = mouseSpeed;
        if (!WindowMod.SystemParametersInfo(113U, 0U, mouseSpeed, 0U))
        {
            FFDebug.LogWarning(this, "SetMouseSpeedError: mouseSpeed:" + mouseSpeed);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            this.SetMouseSpeed(this.curSpeed);
        }
        else
        {
            this.SetMouseSpeed(this.origSpeed);
        }
    }

    private void OnApplicationQuit()
    {
        this.SetMouseSpeed(this.origSpeed);
    }

    private const uint SWP_SHOWWINDOW = 64U;

    private const int GWL_STYLE = -16;

    private const int WS_BORDER = 1;

    private const int GWL_EXSTYLE = -20;

    private const int WS_CAPTION = 12582912;

    private const int WS_SYSMENU = 8388608;

    private const int WS_SIZEBOX = 4194304;

    private const int WS_POPUP = 8388608;

    private const int SM_CXSCREEN = 0;

    private const int SM_CYSCREEN = 1;

    private const int SPI_SETMOUSESPEED = 113;

    private const int SPI_GETMOUSESPEED = 93;

    private GameObject ui3DParent;

    private UI3D ui3D;

    private WindowMod.appStyle AppWindowStyle = WindowMod.appStyle.Windowed;

    public WindowMod.zDepth ScreenDepth;

    public int windowLeft = 10;

    public int windowTop = 10;

    public int windowWidth = 800;

    public int windowHeight = 600;

    private IntPtr HWND_TOP = new IntPtr(0);

    private IntPtr HWND_TOPMOST = new IntPtr(-1);

    private IntPtr HWND_NORMAL = new IntPtr(-2);

    private int Xscreen;

    private int Yscreen;

    private IntPtr mCurPtr;

    private int i = 5;

    private uint curSpeed;

    private uint origSpeed;

    public enum appStyle
    {
        Windowed = 2,
        WindowedFullScreenWithoutBorder = 4
    }

    public enum zDepth
    {
        Normal,
        Top,
        TopMost
    }
}
