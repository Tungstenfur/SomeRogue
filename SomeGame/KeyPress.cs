using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
namespace SomeGame;

public class KeyPress
{
    private static readonly Dictionary<ConsoleKey, (int WinCode, byte[] LinuxCode)> KeyMappings =
        new Dictionary<ConsoleKey, (int, byte[])>
    {
        [ConsoleKey.UpArrow] = (0x26, new byte[] { 0x1B, 0x5B, 0x41 }),
        [ConsoleKey.DownArrow] = (0x28, new byte[] { 0x1B, 0x5B, 0x42 }),
        [ConsoleKey.LeftArrow] = (0x25, new byte[] { 0x1B, 0x5B, 0x44 }),
        [ConsoleKey.RightArrow] = (0x27, new byte[] { 0x1B, 0x5B, 0x43 }),
        [ConsoleKey.Enter] = (0x0D, new byte[] { 0x0A }),
        [ConsoleKey.Escape] = (0x1B, new byte[] { 0x1B }),
        [ConsoleKey.Spacebar] = (0x20, new byte[] { 0x20 }),
        [ConsoleKey.Backspace] = (0x08, new byte[] { 0x7F })
    };

    public static bool IsKeyPressed(ConsoleKey key)
    {
        if (!KeyMappings.TryGetValue(key, out var codes))
            throw new NotSupportedException($"Key {key} is not supported");

        if (OperatingSystem.IsWindows())
            return WindowsCheck(codes.WinCode);

        if (OperatingSystem.IsLinux())
            return LinuxCheck(codes.LinuxCode);
        
        throw new PlatformNotSupportedException();
    }

    #region Windows Implementation
    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    private static bool WindowsCheck(int virtualKey)
    {
        return (GetAsyncKeyState(virtualKey) & 0x8000) != 0;
    }
    #endregion

    #region Linux Implementation
    private const int STDIN_FILENO = 0;
    private const int FIONREAD = 0x541B;

    [DllImport("libc", EntryPoint = "ioctl")]
    private static extern int Ioctl(int fd, int request, out int count);

    [DllImport("libc", EntryPoint = "read")]
    private static extern int Read(int fd, byte[] buffer, int count);

    private static bool LinuxCheck(byte[] expectedSequence)
    {
        try
        {
            // Check how many bytes are available
            if (Ioctl(STDIN_FILENO, FIONREAD, out int bytesAvailable) != 0 || bytesAvailable < 1)
                return false;

            // Read available bytes without blocking
            byte[] buffer = new byte[bytesAvailable];
            int bytesRead = Read(STDIN_FILENO, buffer, buffer.Length);

            // Check if the read bytes match the expected sequence
            if (bytesRead < expectedSequence.Length)
                return false;

            for (int i = 0; i < expectedSequence.Length; i++)
            {
                if (buffer[i] != expectedSequence[i])
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion
}