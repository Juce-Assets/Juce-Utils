using System;

namespace Juce.Utils.InterfaceImplementation
{
    public class SelectImplementationOptionTooltipAttribute : Attribute
    {
        public string Tooltip { get; }

        public SelectImplementationOptionTooltipAttribute(string tooltip)
        {
            Tooltip = tooltip;
        }
    }
}