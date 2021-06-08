using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NHotPhrase.Keyboard
{
    //Based on https://gist.github.com/Stasonix
    public class GlobalKeyboardHook : IDisposable
    {
        public delegate IntPtr HookProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        public const int WhKeyboardLl = 13;
        public HookProcDelegate HookProc;
        public IntPtr User32LibraryHandle;
        public IntPtr WindowsHookHandle;

        /// <summary>
        /// </summary>
        /// <param name="registeredKeys">PKey that should trigger logging. Pass null for full logging.</param>
        /// <param name="keyboardPressedEvent"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GlobalKeyboardHook(EventHandler<GlobalKeyboardHookEventArgs> keyboardPressedEvent)
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (keyboardPressedEvent == null)
#pragma warning disable IDE0016 // Use 'throw' expression
                throw new ArgumentNullException(nameof(keyboardPressedEvent));
#pragma warning restore IDE0016 // Use 'throw' expression

            WindowsHookHandle = IntPtr.Zero;
            User32LibraryHandle = IntPtr.Zero;
            HookProc =
                LowLevelKeyboardProc; // we must keep alive _hookProc, because GC is not aware about SetWindowsHookEx behaviour.

            User32LibraryHandle = LoadLibrary("User32");
            if (User32LibraryHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to load library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            WindowsHookHandle = SetWindowsHookEx(WhKeyboardLl, HookProc, User32LibraryHandle, 0);
            if (WindowsHookHandle == IntPtr.Zero)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to adjust keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            KeyboardPressedEvent = keyboardPressedEvent;
        }

        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressedEvent;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                // because we can unhook only in the same thread, not in garbage collector thread
                if (WindowsHookHandle != IntPtr.Zero)
                {
                    if (!UnhookWindowsHookEx(WindowsHookHandle))
                    {
                        var errorCode = Marshal.GetLastWin32Error();
                        throw new Win32Exception(errorCode,
                            $"Failed to remove keyboard hooks for '{Process.GetCurrentProcess().ProcessName}'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
                    }

                    WindowsHookHandle = IntPtr.Zero;

                    // ReSharper disable once DelegateSubtraction
                    HookProc -= LowLevelKeyboardProc;
                }

            if (User32LibraryHandle == IntPtr.Zero) return;

            if (!FreeLibrary(User32LibraryHandle)) // reduces reference to library by 1.
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errorCode,
                    $"Failed to unload library 'User32.dll'. Error {errorCode}: {new Win32Exception(Marshal.GetLastWin32Error()).Message}.");
            }

            User32LibraryHandle = IntPtr.Zero;
        }

        ~GlobalKeyboardHook()
        {
            Dispose(false);
        }

        public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var keystrokeWasHandled = false;
            var keyboardStateAsInt = wParam.ToInt32();

            if (Enum.IsDefined(typeof(KeyboardState), keyboardStateAsInt))
            {
                var rawLowLevelKeyboardInputEvent = Marshal.PtrToStructure(lParam, typeof(LowLevelKeyboardInputEvent));
                if (rawLowLevelKeyboardInputEvent != null)
                {
                    var lowLevelKeyboardInputEvent = (LowLevelKeyboardInputEvent) rawLowLevelKeyboardInputEvent;
                    var eventArguments = new GlobalKeyboardHookEventArgs(lowLevelKeyboardInputEvent,
                        (KeyboardState) keyboardStateAsInt);
                    keystrokeWasHandled = HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
                }
            }

            return keystrokeWasHandled
                ? (IntPtr) 1
                : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        // ReSharper disable once UnusedParameter.Global
#pragma warning disable IDE0060 // Remove unused parameter
        public bool HandleKeyEvent(LowLevelKeyboardInputEvent lowLevelKeyboardInputEvent,
            GlobalKeyboardHookEventArgs eventArguments)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (KeyboardPressedEvent == null)
                return false;

            var handler = KeyboardPressedEvent;
            handler?.Invoke(this, eventArguments);
            return eventArguments.Handled;
        }

        [DllImport("kernel32.dll", SetLastError=true, CharSet = CharSet.Unicode)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        /// <summary>
        ///     The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        ///     You would install a hook procedure to monitor the system for certain types of events. These events are
        ///     associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">hook type</param>
        /// <param name="lpfn">hook procedure</param>
        /// <param name="hMod">handle to application instance</param>
        /// <param name="dwThreadId">thread identifier</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        /// <summary>
        ///     The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx
        ///     function.
        /// </summary>
        /// <param name="hhk">handle to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);

        /// <summary>
        ///     The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        ///     A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hHook">handle to current hook</param>
        /// <param name="code">hook code passed to hook procedure</param>
        /// <param name="wParam">value passed to hook procedure</param>
        /// <param name="lParam">value passed to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}