﻿namespace PureCms.Core.Components.Grid
{
    public sealed class CellInfo
    {
        public string Name { get; set; }
        public string EntityName { get; set; }
        public string Label { get; set; }

        public bool IsHidden { get; set; }

        public bool IsSortable { get; set; }

        public int Width { get; set; }

    }
}
