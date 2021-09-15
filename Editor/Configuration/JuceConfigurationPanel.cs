using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Juce.Utils.Editor
{
    public class JuceConfigurationPanel : EditorWindow
    {
        private readonly List<BuildTargetGroup> buildTargetGroups = new List<BuildTargetGroup>();
        private readonly List<ExtensionDefineEntry> extensionsDefines = new List<ExtensionDefineEntry>();

        [MenuItem("Tools/Juce/Configuration")]
        public static void ShowWindow()
        {
            GetWindow<JuceConfigurationPanel>("Juce configuration").Show(true);
        }

        private void OnEnable()
        {
            AddBuildTargetGroups();
            AddExtensionsDefines();
            GetExtensionsDefinesValues();
        }

        private void OnGUI()
        {
            DrawHeader();

            DrawExtensionDefines();

            DrawDeveloperMode();
        }

        private void AddBuildTargetGroups()
        {
            // From https://docs.unity3d.com/ScriptReference/BuildTarget.html
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneOSX));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneWindows));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.iOS));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.Android));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneWindows));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.WebGL));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.WSAPlayer));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneLinux64));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.PS4));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.XboxOne));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.tvOS));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.Switch));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.Stadia));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.CloudRendering));
            buildTargetGroups.Add(BuildPipeline.GetBuildTargetGroup(BuildTarget.PS5));
        }

        private void AddExtensionsDefines()
        {
            extensionsDefines.Clear();
            extensionsDefines.Add(new ExtensionDefineEntry("TextMeshPro", "JUCE_TEXT_MESH_PRO_EXTENSIONS"));
            extensionsDefines.Add(new ExtensionDefineEntry("Timeline", "JUCE_TIMELINE_EXTENSIONS"));
        }

        private void GetExtensionsDefinesValues()
        {
            for (int i = 0; i < extensionsDefines.Count; ++i)
            {
                ExtensionDefineEntry currExtensionDefine = extensionsDefines[i];

                currExtensionDefine.Enabled = ContainsScriptingDefineSymbol(currExtensionDefine.Define);
            }
        }

        private void DrawHeader()
        {
            EditorGUILayout.LabelField("Juce Configuration", EditorStyles.boldLabel);

            EditorGUILayout.Space(2);

            EditorGUILayout.LabelField("Here you can enable or disable the different extensions that can be used with Juce", EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space(2);
        }

        private void DrawExtensionDefines()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                for (int i = 0; i < extensionsDefines.Count; ++i)
                {
                    ExtensionDefineEntry currExtensionDefine = extensionsDefines[i];

                    bool newEnalbed = EditorGUILayout.Toggle($"{currExtensionDefine.Name} extensions", currExtensionDefine.Enabled);

                    if (newEnalbed != currExtensionDefine.Enabled)
                    {
                        currExtensionDefine.Enabled = newEnalbed;

                        if (currExtensionDefine.Enabled)
                        {
                            AddScriptingDefineSymbols(currExtensionDefine.Define);
                        }
                        else
                        {
                            RemoveScriptingDefineSymbols(currExtensionDefine.Define);
                        }
                    }
                }
            }
        }

        private void DrawDeveloperMode()
        {
            EditorGUILayout.Space(5);

            bool developerMode = JuceConfiguration.Instance.DeveloperMode;

            JuceConfiguration.Instance.DeveloperMode = EditorGUILayout.Toggle("Developer mode", developerMode);
        }

        private void AddScriptingDefineSymbols(string define)
        {
            foreach (BuildTargetGroup targetGroup in buildTargetGroups)
            {
                if (targetGroup == BuildTargetGroup.Unknown)
                {
                    continue;
                }

                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

                bool alreadyAdded = ContainsScriptingDefineSymbol(targetGroup, define);

                if (!alreadyAdded)
                {
                    defines += $"; {define}";

                    PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
                }
            }
        }

        private void RemoveScriptingDefineSymbols(string define)
        {
            foreach (BuildTargetGroup targetGroup in buildTargetGroups)
            {
                if (targetGroup == BuildTargetGroup.Unknown)
                {
                    continue;
                }

                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

                defines = defines.Replace(define, "");

                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defines);
            }
        }

        private bool ContainsScriptingDefineSymbol(BuildTargetGroup group, string define)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);

            string definesNoSpaces = defines.Replace(" ", "");

            string[] definesString = definesNoSpaces.Split(';');

            for (int i = 0; i < definesString.Length; ++i)
            {
                if (definesString[i].Equals(define))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsScriptingDefineSymbol(string define)
        {
            foreach (BuildTargetGroup targetGroup in buildTargetGroups)
            {
                if (targetGroup == BuildTargetGroup.Unknown)
                {
                    continue;
                }

                string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

                string definesNoSpaces = defines.Replace(" ", "");

                string[] definesString = definesNoSpaces.Split(';');

                for (int i = 0; i < definesString.Length; ++i)
                {
                    if (definesString[i].Equals(define))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}