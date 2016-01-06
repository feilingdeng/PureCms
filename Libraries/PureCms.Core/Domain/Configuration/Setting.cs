using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureCms.Core.Domain.Configuration
{
    /// <summary>
    /// Represents a setting
    /// </summary>
    [DebuggerDisplay("{Name}: {Value}")]
    public partial class Setting : BaseEntity
    {
        public Setting() { }

        public Setting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }
    }
}
