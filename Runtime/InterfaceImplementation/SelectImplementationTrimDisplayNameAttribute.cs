using System;

namespace Juce.Utils.InterfaceImplementation
{
    public class SelectImplementationTrimDisplayNameAttribute : Attribute
    {
        public string TrimDisplayNameValue { get; }

        public SelectImplementationTrimDisplayNameAttribute(string trimDisplayNameValue)
        {
            TrimDisplayNameValue = trimDisplayNameValue;
        }
    }
}