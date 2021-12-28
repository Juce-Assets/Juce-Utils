using Juce.Utils.Extensions;
using Juce.Utils.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Juce.Utils.InterfaceImplementation
{
    [CustomPropertyDrawer(typeof(SelectImplementationAttribute))]
    public class SelectImplementationPropertyDrawer : PropertyDrawer
    {
        private readonly PropertyDrawerLayoutHelper layoutHelper = new PropertyDrawerLayoutHelper();
        private readonly Dictionary<string, int> typeIndexMap = new Dictionary<string, int>();

        private Type[] types;
        private GUIContent[] names;
        private PropertyDrawer propertyDrawer;
        private int previousTypeIndex = -1;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var typeAttribute = (SelectImplementationAttribute)attribute;

            float height = layoutHelper.GetElementsHeight(1);

            if(!property.isExpanded && !typeAttribute.ForceExpanded)
            {
                return height;
            }

            if (propertyDrawer == null)
            {
                return height + property.GetVisibleChildHeight();
            }

            return height + propertyDrawer.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SelectImplementationAttribute typeAttribute = (SelectImplementationAttribute)attribute;

            if (types == null)
            {
                GetTypes(typeAttribute, property);
            }

            int typeIndex = GetCurrentTypeIndex(property);

            if (types.Length > 0 && typeIndex == -1)
            {
                bool defaultFound = TryGetDefaultType(out int defaultTypeIndex);

                if (defaultFound)
                {
                    InitializePropertyAtIndex(property, defaultTypeIndex);
                    typeIndex = defaultTypeIndex;
                }
                else
                {
                    InitializePropertyAtIndex(property, 0);
                    typeIndex = 0;
                }
            }

            if (typeIndex != previousTypeIndex)
            {
                previousTypeIndex = typeIndex;
            }

            layoutHelper.Init(position);

            bool useExpand = property.hasVisibleChildren && !typeAttribute.ForceExpanded;

            Rect popupRect = layoutHelper.NextVerticalRect();

            if (typeAttribute.DisplayLabel)
            {
                SplitLabelField(popupRect, out var labelRect, out popupRect, applyIndent: false);
                if (useExpand)
                {
                    property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, label);
                }
                else
                {
                    EditorGUI.LabelField(labelRect, label);
                }
            }
            else if (useExpand)
            {
                SplitLabelField(popupRect, out var labelRect, out popupRect, applyIndent: false);
                property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, GUIContent.none);
            }

            int nextTypeIndex = EditorGUI.Popup(popupRect, typeIndex, names);

            if (nextTypeIndex != typeIndex)
            {
                InitializePropertyAtIndex(property, nextTypeIndex);
            }

            if (useExpand && !property.isExpanded)
            {
                return;
            }

            EditorGUI.indentLevel++;

            if (propertyDrawer == null)
            {
                property.ForeachVisibleChildren(DrawChildPropertyField);
            }
            else
            {
                propertyDrawer.OnGUI(layoutHelper.NextVerticalRect(property), property, names[typeIndex]);
            }

            EditorGUI.indentLevel--;
        }

        private void GetTypes(SelectImplementationAttribute typeAttribute, SerializedProperty property)
        {
            Type baseType = typeAttribute.FieldType;

            SelectImplementationTrimDisplayNameAttribute trimDisplayNameAttribute 
                = (SelectImplementationTrimDisplayNameAttribute)Attribute.GetCustomAttribute(baseType, typeof(SelectImplementationTrimDisplayNameAttribute));
            
            string removeTailString = trimDisplayNameAttribute?.TrimDisplayNameValue ?? string.Empty;

            types = GetTypes(baseType).ToArray();

            names = types.Select(x =>
                new GUIContent(
                    ObjectNames.NicifyVariableName(
                        RemoveTail(x.Name, removeTailString)
                    ),
                    GetTypeTooltip(x)
                )
            ).ToArray();
        }

        private bool TryGetDefaultType(out int defaultTypeIndex)
        {
            for (int i = 0; i < types.Length; ++i)
            {
                Type type = types[i];

                SelectImplementationDefaultTypeAttribute defaultAttribute = Attribute.GetCustomAttribute(
                    type,
                    typeof(SelectImplementationDefaultTypeAttribute)
                    ) as SelectImplementationDefaultTypeAttribute;

                if(defaultAttribute != null)
                {
                    defaultTypeIndex = i;
                    return true;
                }
            }

            defaultTypeIndex = default;
            return false;
        }

        private static string GetTypeTooltip(Type type)
        {
            SelectImplementationOptionTooltipAttribute tooltipAttribute = Attribute.GetCustomAttribute(
                type, 
                typeof(SelectImplementationOptionTooltipAttribute)
                ) as SelectImplementationOptionTooltipAttribute;
            
            return tooltipAttribute != null ? tooltipAttribute.Tooltip : string.Empty;
        }

        private static IEnumerable<Type> GetTypes(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(x => type.IsAssignableFrom(x) && !x.IsAbstract && !x.IsSubclassOf(typeof(UnityEngine.Object)));
        }

        private void DrawChildPropertyField(SerializedProperty childProperty)
        {
            EditorGUI.PropertyField(layoutHelper.NextVerticalRect(childProperty), childProperty, true);
        }

        private int GetCurrentTypeIndex(SerializedProperty property)
        {
            string propertyTypeName = property.managedReferenceFullTypename;

            bool typeFound = typeIndexMap.TryGetValue(propertyTypeName, out int typeIndex);

            if (typeFound)
            {
                return typeIndex;
            }

            for (int i = 0; i < types.Length; i++)
            {
                string typeFullName = types[i].FullName;

                if (propertyTypeName.Contains(typeFullName))
                {
                    typeIndexMap.Add(propertyTypeName, i);
                    return i;
                }
            }

            return -1;
        }

        private void InitializePropertyAtIndex(SerializedProperty property, int typeIndex)
        {
            Type type = types[typeIndex];

            property.managedReferenceValue = Activator.CreateInstance(type);
            property.serializedObject.ApplyModifiedProperties();
        }

        private static void SplitLabelField(
            Rect rect, 
            out Rect labelRect, 
            out Rect fieldRect, 
            float margin = 0, 
            bool applyIndent = true
            )
        {
            Rect indentedContent = EditorGUI.IndentedRect(rect);

            float labelWidth = EditorGUIUtility.labelWidth - (indentedContent.xMin - rect.xMin) - margin;
            float fieldWidth = rect.width - labelWidth;

            Vector2 position = applyIndent ? indentedContent.position : rect.position;

            labelRect = new Rect(
                position, 
                new Vector2(labelWidth, rect.height)
                );

            fieldRect = new Rect(
                position + new Vector2(labelWidth + margin, 0), 
                new Vector2(fieldWidth - margin, rect.height)
                );
        }

        private static string RemoveTail(string source, string tail)
        {
            if (string.IsNullOrEmpty(tail))
            {
                return source;
            }

            var index = source.LastIndexOf(tail);
            if (index == -1)
            {
                return source;
            }

            return source.Substring(0, index);
        }
    }
}

