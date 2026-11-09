// SPDX-License-Identifier: AGPL-3.0-or-later
namespace XiboClient.Logic
{
    internal sealed class MonitorDescriptor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DeviceName { get; set; }
        public bool IsPrimary { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
    }
}
