// SPDX-License-Identifier: AGPL-3.0-or-later
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace XiboClient.Logic
{
    internal static class WindowsMonitorInventory
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DisplayDevice
        {
            public int Size;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string DeviceString;
            public uint StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string DeviceId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string DeviceKey;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumDisplayDevices(string device, uint index,
            ref DisplayDevice displayDevice, uint flags);

        internal static List<MonitorDescriptor> GetMonitors()
        {
            var result = new List<MonitorDescriptor>();
            foreach (var screen in Screen.AllScreens.OrderBy(item => item.DeviceName, StringComparer.OrdinalIgnoreCase))
            {
                var device = new DisplayDevice { Size = Marshal.SizeOf(typeof(DisplayDevice)) };
                // EDD_GET_DEVICE_INTERFACE_NAME supplies the monitor interface path.
                bool identified = EnumDisplayDevices(screen.DeviceName, 0, ref device, 1)
                    && !string.IsNullOrEmpty(device.DeviceId);
                string identity = identified ? "interface:" + device.DeviceId : "gdi:" + screen.DeviceName;
                result.Add(new MonitorDescriptor
                {
                    Id = MonitorSelector.CreateId(identity),
                    Name = string.IsNullOrWhiteSpace(device.DeviceString) ? screen.DeviceName : device.DeviceString,
                    DeviceName = screen.DeviceName,
                    IsPrimary = screen.Primary,
                    Width = screen.Bounds.Width,
                    Height = screen.Bounds.Height,
                    Left = screen.Bounds.Left,
                    Top = screen.Bounds.Top
                });
            }
            return result;
        }
    }
}
