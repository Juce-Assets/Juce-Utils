using System.Collections.Generic;
using UnityEngine;

namespace Juce.Utils.InterfaceImplementation
{
    public class InterfaceImplementationExample : MonoBehaviour
    {
        [SelectImplementation(typeof(IInteraface))]
        [SerializeField, SerializeReference] private List<IInteraface> implementations = new List<IInteraface>();
    }
}
