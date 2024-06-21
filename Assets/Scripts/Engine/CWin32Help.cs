using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class CWin32Help
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EnumWindows(CWin32Help.Wndenumproc lpEnumFunc, uint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

    [DllImport("kernel32.dll")]
    private static extern void SetLastError(uint dwErrCode);

    public static IntPtr GetProcessWnd()
    {
        IntPtr ptrWnd = IntPtr.Zero;
        uint id = (uint)Process.GetCurrentProcess().Id;
        return (CWin32Help.EnumWindows(delegate (IntPtr hwnd, uint lParam)
        {
            uint num = 0U;
            if (CWin32Help.GetParent(hwnd) != IntPtr.Zero)
            {
                return true;
            }
            CWin32Help.GetWindowThreadProcessId(hwnd, ref num);
            if (num != lParam)
            {
                return true;
            }
            ptrWnd = hwnd;
            CWin32Help.SetLastError(0U);
            return false;
        }, id) || Marshal.GetLastWin32Error() != 0) ? IntPtr.Zero : ptrWnd;
    }

    [DllImport("imm32.dll")]
    private static extern IntPtr ImmGetContext(IntPtr hwnd);

    [DllImport("imm32.dll")]
    private static extern bool ImmGetOpenStatus(IntPtr himc);

    [DllImport("imm32.dll")]
    private static extern bool ImmSetOpenStatus(IntPtr himc, bool b);

    public static IntPtr GetIme(IntPtr tf)
    {
        return CWin32Help.ImmGetContext(tf);
    }

    public static bool GetImeStatus(IntPtr tf)
    {
        return CWin32Help.ImmGetOpenStatus(tf);
    }

    public static bool SetImeStatus(IntPtr tf, bool open)
    {
        return CWin32Help.ImmSetOpenStatus(tf, open);
    }

    private delegate bool Wndenumproc(IntPtr hwnd, uint lParam);
}
