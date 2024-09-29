/**
 * Copyright (C) 2024 Emilian Roman
 * 
 * This file is part of Shantae.Progress.
 * 
 * Shantae is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Shantae is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Shantae.  If not, see <https://www.gnu.org/licenses/>.
 */

namespace Miris.Shantae;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

class Transfer
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(int hProcess,
      int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
      byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

    const int PROCESS_WM_READ      = 0x0010;
    const int PROCESS_VM_WRITE     = 0x0020;
    const int PROCESS_VM_OPERATION = 0x0008;

    public required Progress Source { get; set; }
    public required Progress Target { get; set; }

    public void Commit()
    {
        var sourceProcess = Process.GetProcessesByName(Source.Game.Process)[0];
        var sourceHandle  = OpenProcess(PROCESS_WM_READ, false, sourceProcess.Id);

        var sourceAddress = IntPtr.Add(BaseAddress(sourceProcess), Source.Start);
        var sourceBuffer  = new byte[Source.Length];
        int sourceRead    = 0;

        if (ReadProcessMemory((int)sourceHandle, (int)sourceAddress, sourceBuffer, sourceBuffer.Length, ref sourceRead) == false)
        {
            throw new Exception("Unable to read from source process memory.");
        }

        var targetProcess = Process.GetProcessesByName(Target.Game.Process)[0];
        var targetHandle  = OpenProcess(PROCESS_VM_WRITE | PROCESS_VM_OPERATION, false, targetProcess.Id);

        var targetAddress = IntPtr.Add(BaseAddress(targetProcess), Target.Start);
        var targetBuffer  = sourceBuffer;
        int targetWritten = 0;

        if(WriteProcessMemory((int)targetHandle, (int)targetAddress, targetBuffer, targetBuffer.Length, ref targetWritten) == false)
        {
            throw new Exception("Unable to write to process memory.");
        }
    }

    private static nint BaseAddress(Process process)
    {
        if (process.MainModule == null)
        {
            return 0;
        }

        return process.MainModule.BaseAddress;
    }
}
