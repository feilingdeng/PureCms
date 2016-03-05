﻿using System;

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

        public string AliasName
        {
            get;
            set;
        }

        public string LinkFromFieldName { get; set; }
        public string LinkToFieldName { get; set; }
        public string TargetFieldName { get; set; }
    }
}
