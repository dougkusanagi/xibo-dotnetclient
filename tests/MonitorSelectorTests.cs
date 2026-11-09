// SPDX-License-Identifier: AGPL-3.0-or-later
using System;
using XiboClient.Logic;

namespace XiboClient.Tests
{
    public static class MonitorSelectorTests
    {
        private static int assertions;

        private static void Equal(string expected, string actual)
        {
            assertions++;
            if (expected != actual)
                throw new Exception("Expected " + (expected ?? "<manual>")
                    + ", got " + (actual ?? "<manual>"));
        }

        public static void Run()
        {
            var secondary = new[] { "DISPLAY3", "DISPLAY2" };
            Equal(null, MonitorSelector.GetDeviceName(null, "DISPLAY1", secondary));
            Equal(null, MonitorSelector.GetDeviceName("default", "DISPLAY1", secondary));
            Equal(null, MonitorSelector.GetDeviceName("unknown", "DISPLAY1", secondary));
            Equal("DISPLAY1", MonitorSelector.GetDeviceName("primary", "DISPLAY1", secondary));
            Equal("DISPLAY2", MonitorSelector.GetDeviceName("secondary", "DISPLAY1", secondary));
            // Enumeration order changes must not change which secondary is selected.
            Equal("DISPLAY2", MonitorSelector.GetDeviceName("secondary", "DISPLAY1",
                new[] { "DISPLAY2", "DISPLAY3" }));
            // Disconnect, reconnect, and a different primary after reboot.
            Equal("DISPLAY1", MonitorSelector.GetDeviceName("secondary", "DISPLAY1", new string[0]));
            Equal("DISPLAY2", MonitorSelector.GetDeviceName("secondary", "DISPLAY1", new[] { "DISPLAY2" }));
            Equal("DISPLAY1", MonitorSelector.GetDeviceName("secondary", "DISPLAY2", new[] { "DISPLAY1" }));
            Equal(null, MonitorSelector.GetDeviceName("secondary", null, new string[0]));
            Console.WriteLine("Monitor selection: " + assertions + " assertions passed.");
        }
    }
}
