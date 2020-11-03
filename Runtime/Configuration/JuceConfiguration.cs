using Juce.Utils.Singletons;

namespace Juce.Utils
{
    public class JuceConfiguration : Singleton<JuceConfiguration>
    {
        public bool DeveloperMode { get; set; }
    }
}