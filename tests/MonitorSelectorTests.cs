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
            var one = new MonitorDescriptor { Id = MonitorSelector.CreateId("interface:one"), DeviceName = "DISPLAY1", IsPrimary = true };
            var two = new MonitorDescriptor { Id = MonitorSelector.CreateId("interface:two"), DeviceName = "DISPLAY2" };
            var three = new MonitorDescriptor { Id = MonitorSelector.CreateId("interface:three"), DeviceName = "DISPLAY3" };
            var monitors = new[] { three, one, two };
            Equal(null, MonitorSelector.GetDeviceName(null, monitors));
            Equal(null, MonitorSelector.GetDeviceName("default", monitors));
            Equal(null, MonitorSelector.GetDeviceName("unknown", monitors));
            Equal("DISPLAY1", MonitorSelector.GetDeviceName("primary", monitors));
            Equal("DISPLAY2", MonitorSelector.GetDeviceName("secondary", monitors));
            Equal("DISPLAY1", MonitorSelector.GetDeviceName(one.Id, monitors));
            Equal("DISPLAY2", MonitorSelector.GetDeviceName(two.Id, monitors));
            Equal("DISPLAY3", MonitorSelector.GetDeviceName(three.Id, monitors));
            Equal("DISPLAY3", MonitorSelector.GetDeviceName(three.Id, new[] { two, three, one }));
            // A selected monitor survives disconnect/reconnect and DISPLAY number changes.
            Equal("DISPLAY1", MonitorSelector.GetDeviceName(three.Id, new[] { one, two }));
            Equal("DISPLAY3", MonitorSelector.GetDeviceName(three.Id, monitors));
            three.DeviceName = "DISPLAY7";
            Equal("DISPLAY7", MonitorSelector.GetDeviceName(three.Id, monitors));
            one.IsPrimary = false;
            two.IsPrimary = true;
            Equal("DISPLAY7", MonitorSelector.GetDeviceName(three.Id, monitors));
            Equal("DISPLAY2", MonitorSelector.GetDeviceName(three.Id, new[] { one, two }));
            Equal("DISPLAY1", MonitorSelector.GetDeviceName("secondary", monitors));
            Equal(null, MonitorSelector.GetDeviceName(three.Id, new MonitorDescriptor[0]));
            Equal(MonitorSelector.CreateId("interface:ONE"), one.Id);
            Equal("72", one.Id.Length.ToString());
            Console.WriteLine("Monitor selection: " + assertions + " assertions passed.");
        }
    }
}
