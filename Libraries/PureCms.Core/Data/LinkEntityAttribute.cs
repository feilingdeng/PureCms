using System;

namespace PureCms.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LinkEntityAttribute : Attribute
    {
        //public LinkEntityAttribute() { }

        public LinkEntityAttribute(Type target)
        {
            this.Target = target;
        }

        public Type Target
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public string SourceFieldName { get; set; }
    }
}
