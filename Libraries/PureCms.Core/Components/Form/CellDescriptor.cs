namespace PureCms.Core.Components.Form
{
    public sealed class CellDescriptor
    {
        private ControlDescriptor _control;

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
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
    }
}
