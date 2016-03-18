namespace PureCms.Core.Components.Form
{
    public sealed class CellLabelSettings
    {
        private CellLabelAlignment _alignment = CellLabelAlignment.Left;
        private CellLabelPosition _position;
        private int _width = 115;
        public CellLabelAlignment Alignment
        {
            get
            {
                return this._alignment;
            }
        }
        public CellLabelPosition Position
        {
            get
            {
                return this._position;
            }
        }
        public int Width
        {
            get
            {
                return this._width;
            }
        }
    }
}
