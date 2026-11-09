// SPDX-License-Identifier: AGPL-3.0-or-later
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XiboClient.Logic
{
    internal static class MonitorSelector
    {
        // Windows device interface paths are case-insensitive. Keep their opaque ID
        // separate from DISPLAY1/DISPLAY2, which may change after a reboot.
        internal static string CreateId(string identity)
        {
            using (var hash = SHA256.Create())
            {
                return "monitor:" + BitConverter.ToString(hash.ComputeHash(
                    Encoding.UTF8.GetBytes(identity.ToUpperInvariant()))).Replace("-", "").ToLowerInvariant();
            }
        }

        internal static string GetDeviceName(string selection, IEnumerable<MonitorDescriptor> inventory)
        {
            var monitors = inventory.ToList();
            var primary = monitors.FirstOrDefault(monitor => monitor.IsPrimary)?.DeviceName;
            if (selection != null && selection.StartsWith("monitor:", StringComparison.Ordinal))
                return monitors.FirstOrDefault(monitor => monitor.Id == selection)?.DeviceName ?? primary;

            switch (selection)
            {
                case "primary":
                    return primary;
                case "secondary":
                    return monitors.Where(monitor => !monitor.IsPrimary)
                        .OrderBy(monitor => monitor.DeviceName, StringComparer.OrdinalIgnoreCase)
                        .FirstOrDefault()?.DeviceName ?? primary;
                default:
                    return null;
            }
        }
    }
}
