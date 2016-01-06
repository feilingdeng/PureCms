using System;

namespace PureCms.Core.Configuration
{
    /// <summary>
    /// Marker attribute. Indicates that the settings should
    /// be persisted as a JSON string rather than splitted
    /// into single properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class JsonPersistAttribute : Attribute
    {
    }
}
