namespace PureCms.Core.Components.Form
{
    public sealed class CellDescriptor
    {
        private ControlDescriptor _control;
        private int _colSpan;
        private int _rowSpan;

        public string Label { get; set; }
        public bool IsShowLabel { get; set; }

        public bool IsVisible { get; set; }
        public ControlDescriptor Control
        {
            get
            {
                return this._control;
            }
            private set
            {
                this._control = value;
            }
        }
        public int ColSpan
        {
            get
            {
                return this._colSpan;
            }
        }
        public int RowSpan
        {
            get
            {
                return this._rowSpan;
            }
        }
        public bool Visible
        {
            get;
            private set;
        }
        public bool AutoExpand
        {
            get;
            private set;
        }
    }
}
