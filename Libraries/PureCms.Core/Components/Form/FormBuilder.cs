using System;
using PureCms.Core.Domain.Schema;

namespace PureCms.Core.Components.Form
{
    public class FormBuilder
    {
        private SystemFormInfo _formEntity;
        private FormDescriptor _form;

        public FormBuilder(SystemFormInfo formEntity)
        {
            _formEntity = formEntity;
            _form = _form.DeserializeFromJson(_formEntity.FormConfig);
        }

        public FormDescriptor Form
        {
            get { return _form; }
            set { _form = value; }
        }
    }
}
