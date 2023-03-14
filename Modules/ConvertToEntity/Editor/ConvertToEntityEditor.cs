using CZToolKit.Common;
using CZToolKit.Common.IMGUI;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CZToolKit.ECS.Editors
{
    [CustomEditor(typeof(ConvertToEntity))]
    public class ConvertToEntityEditor : BasicEditor
    {
        private static Dictionary<string, Type> ComponentTypes = new Dictionary<string, Type>();

        static ConvertToEntityEditor()
        {
            foreach (var type in Util_TypeCache.GetTypesDerivedFrom<IComponent>())
            {
                var menu = type.GetCustomAttribute<AddComponentMenu>();
                if (menu != null)
                    ComponentTypes[menu.componentMenu] = type;
                else
                    ComponentTypes[type.Name] = type;
            }
        }
        
        protected override void RegisterDrawers()
        {
            base.RegisterDrawers();

            var tTarget = target as ConvertToEntity;
            var componentsProperty = serializedObject.FindProperty("components");
            var componentList = new ReorderableList(tTarget.components, typeof(IComponent), true, false, true, true);
            componentList.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Components");
            };
            componentList.onReorderCallback += (list) =>
            {
                EditorUtility.SetDirty(tTarget);
            };
            componentList.onAddDropdownCallback = (rect, list) =>
            {
                CZAdvancedDropDown components = new CZAdvancedDropDown("Components");
                components.MinimumSize = new Vector2(150, 300);
                foreach (var pair in ComponentTypes)
                {
                    var item = components.Add(ObjectNames.NicifyVariableName(pair.Key));
                    item.userData = pair.Value;
                }
                components.onItemSelected += (selectedItem) =>
                {
                    var type = selectedItem.userData as Type;
                    Undo.RegisterCompleteObjectUndo(tTarget, $"Add {type.Name}");
                    if (tTarget.components.Find(item => item.GetType() == type) == null)
                        tTarget.components.Add(Activator.CreateInstance(type) as IComponent);
                    else
                        EditorUtility.DisplayDialog("Warning", $"Already contains [{type.Name}] component", "OK");
                };
                components.Show(rect, 300);
            };
            componentList.onRemoveCallback = (list) =>
            {
                var type = tTarget.components[list.index].GetType();
                Undo.RegisterCompleteObjectUndo(tTarget, $"Add {type.Name}");
                tTarget.components.RemoveAt(list.index);
                if (list.index >= list.list.Count - 1)
                {
                    list.index = list.list.Count - 1;
                }
            };
            componentList.elementHeightCallback = (index) =>
            {
                if (componentsProperty.arraySize <= index)
                    return componentList.elementHeight;
                return EditorGUI.GetPropertyHeight(componentsProperty.GetArrayElementAtIndex(index));
            };
            componentList.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                if (componentsProperty.arraySize <= index)
                    return;
                rect.xMin += 10;
                var elementProperty = componentsProperty.GetArrayElementAtIndex(index);
                var name = ObjectNames.NicifyVariableName(tTarget.components[index].GetType().Name);
                EditorGUI.PropertyField(rect, elementProperty, EditorGUIUtility.TrTextContent(name), true);
            };
            RegisterDrawer("components", property =>
            {
                componentList.DoLayoutList();
            });
        }
    }
}