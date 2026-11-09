// SPDX-License-Identifier: AGPL-3.0-or-later
using System;
using System.Collections.Generic;
using System.Linq;

namespace XiboClient.Logic
{
    /// <summary>
    /// Resolve a monitor preference without persisting transient enumeration indexes.
    /// A null result leaves the existing manual window configuration in control.
    /// </summary>
    internal static class MonitorSelector
    {
        internal static string GetDeviceName(string selection, string primaryDeviceName,
            IEnumerable<string> secondaryDeviceNames)
        {
            switch (selection)
            {
                case "primary":
                    return primaryDeviceName;
                case "secondary":
                    return secondaryDeviceNames.OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                        .FirstOrDefault() ?? primaryDeviceName;
                default:
                    return null;
            }
        }
    }
}
