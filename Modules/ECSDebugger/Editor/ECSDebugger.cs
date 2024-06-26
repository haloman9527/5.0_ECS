// #region 注 释
//
// /***
//  *
//  *  Title:
//  *  
//  *  Description:
//  *  
//  *  Date:
//  *  Version:
//  *  Writer: 半只龙虾人
//  *  Github: https://github.com/haloman9527
//  *  Blog: https://www.haloman.net/
//  *
//  */
//
// #endregion
//
// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using CZToolKitEditor;
// using CZToolKitEditor.IMGUI.Controls;
// using UnityEditor.IMGUI.Controls;
// using UnityEngine;
// using UnityEditor;
// using Unity.Collections;
//
// namespace CZToolKit.ECS.Editors
// {
//     public class ESCDebugger : BaseMenuEditorWindow
//     {
//         private static string[] Pages = new string[] { "Entities", "Systems" };
//
//         private Context selectWorld;
//         private int selectPage;
//
//         protected override void OnEnable()
//         {
//             titleContent = new GUIContent("ECS Debugger");
//             base.OnEnable();
//             EditorApplication.playModeStateChanged += OnPlayModeChanged;
//             foreach (var world in Context.AllWorlds)
//             {
//                 SelectWorld(world);
//                 break;
//             }
//
//             RefreshTreeView();
//         }
//
//         private void OnPlayModeChanged(PlayModeStateChange obj)
//         {
//             switch (obj)
//             {
//                 case PlayModeStateChange.EnteredPlayMode:
//                     foreach (var world in Context.AllWorlds)
//                     {
//                         SelectWorld(world);
//                         break;
//                     }
//
//                     RefreshTreeView();
//                     break;
//                 case PlayModeStateChange.ExitingPlayMode:
//                     SelectWorld(null);
//                     RefreshTreeView();
//                     Repaint();
//                     break;
//             }
//         }
//
//         protected override CZTreeView BuildMenuTree(TreeViewState treeViewState)
//         {
//             CZTreeView treeView = null;
//             switch (selectPage)
//             {
//                 case 0:
//                 {
//                     GenericTreeView<Entity> worldTreeView = new GenericTreeView<Entity>(treeViewState);
//                     if (selectWorld != null)
//                     {
//                         foreach (var entity in selectWorld.Entities.GetValueArray(Allocator.Temp))
//                         {
//                             worldTreeView.AddItem(entity.index.ToString(), entity);
//                         }
//                     }
//
//                     treeView = worldTreeView;
//                     break;
//                 }
//                 case 1:
//                 {
//                     GenericTreeView<ISystem> worldTreeView = new GenericTreeView<ISystem>(treeViewState);
//                     if (selectWorld != null)
//                     {
//                         for (int i = 0; i < selectWorld.Systems.Count; i++)
//                         {
//                             var system = selectWorld.Systems[i];
//                             worldTreeView.AddItem($"{i} : {system.GetType().Name}", system);
//                         }
//                     }
//
//                     worldTreeView.onItemRowGUI += OnTreeViewItemRowGUI;
//                     treeView = worldTreeView;
//                     break;
//                 }
//             }
//
//             treeView.RowHeight = 20;
//             treeView.ShowBoder = true;
//             treeView.ShowAlternatingRowBackgrounds = true;
//             return treeView;
//         }
//
//         protected override void OnTreeViewGUI()
//         {
//             switch (selectPage)
//             {
//                 case 0:
//                     CheckEntitiesTreeView();
//                     break;
//                 case 1:
//                     CheckSystemsTreeView();
//                     break;
//             }
//
//             base.OnTreeViewGUI();
//             Repaint();
//         }
//
//         protected void OnTreeViewItemRowGUI(CZTreeView.RowGUIArgsBridge args)
//         {
//             var item = args.item as CZTreeViewItem;
//             if (item is EntityTreeViewItem<ISystem> systemItem)
//             {
//                 var activeToggleRect = args.rowRect;
//                 activeToggleRect.xMin = activeToggleRect.xMax - activeToggleRect.height;
//                 var selectedSystem = systemItem.data;
//                 var newActive = EditorGUI.Toggle(activeToggleRect, selectedSystem.Active);
//                 if (newActive != selectedSystem.Active)
//                     selectedSystem.Active = newActive;
//             }
//         }
//
//         protected override void OnLeftGUI()
//         {
//             var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true));
//             var worldSelectButtonRect = new Rect(rect.x, rect.y, rect.width / 2, rect.height);
//             var worldSelectDropDownLabel = selectWorld == null ? EditorGUIUtility.TrTextContent("None") : EditorGUIUtility.TrTextContent(selectWorld.name);
//             if (GUI.Button(worldSelectButtonRect, worldSelectDropDownLabel, EditorStyles.toolbarDropDown))
//             {
//                 GenericMenu worldMenu = new GenericMenu();
//                 foreach (var world in Context.AllWorlds)
//                 {
//                     worldMenu.AddItem(new GUIContent(world.name), false, () =>
//                     {
//                         SelectWorld(world);
//                         RefreshTreeView();
//                     });
//                 }
//
//                 worldMenu.DropDown(worldSelectButtonRect);
//             }
//
//             var pageDropDownLabel = EditorGUIUtility.TrTextContent(Pages[selectPage]);
//             var pageSelectButtonRect = new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, rect.height);
//             if (GUI.Button(pageSelectButtonRect, pageDropDownLabel, EditorStyles.toolbarDropDown))
//             {
//                 GenericMenu pageMenu = new GenericMenu();
//                 for (int i = 0; i < Pages.Length; i++)
//                 {
//                     int index = i;
//                     pageMenu.AddItem(new GUIContent(Pages[index]), false, () =>
//                     {
//                         selectPage = index;
//                         RefreshTreeView();
//                     });
//                 }
//
//                 pageMenu.DropDown(pageSelectButtonRect);
//             }
//
//             EditorGUILayout.BeginVertical();
//             base.OnLeftGUI();
//             EditorGUILayout.EndHorizontal();
//         }
//
//         protected override void OnRightGUI(IList<int> selection)
//         {
//             base.OnRightGUI(selection);
//
//             {
//                 // 绘制Component界面
//                 if (selection.Count != 0 && MenuTreeView.RootItem.children.Count != 0)
//                 {
//                     CZTreeViewItem item = null;
//                     foreach (var selectedID in selection)
//                     {
//                         item = MenuTreeView.FindItem(selectedID);
//                         if (item != null)
//                             break;
//                     }
//
//                     switch (item)
//                     {
//                         case EntityTreeViewItem<Entity> entityItem:
//                         {
//                             var selectedEntity = entityItem.data;
//                             foreach (var componentContainer in selectWorld.ComponentContainers.GetValueArray(Allocator.Temp))
//                             {
//                                 var componentType = TypeManager.GetType(componentContainer.typeInfo.id);
//                                 if (selectWorld.ComponentContainers.ContainsKey(componentContainer.typeInfo.id)
//                                     && selectWorld.TryGetComponent(selectedEntity, componentType, out var component))
//                                 {
//                                     EditorGUILayout.BeginVertical("FrameBox");
//                                     var componentDrawer = ComponentDrawerFactory.GetComponentDrawer(componentType);
//                                     componentDrawer.Foldout = EditorGUILayout.BeginFoldoutHeaderGroup(componentDrawer.Foldout, componentType.Name);
//
//                                     if (componentDrawer.Foldout)
//                                     {
//                                         EditorGUI.indentLevel++;
//                                         componentDrawer.OnGUI(selectWorld, selectedEntity, component);
//                                         EditorGUI.indentLevel--;
//                                     }
//
//                                     EditorGUILayout.EndFoldoutHeaderGroup();
//                                     EditorGUILayout.EndVertical();
//                                 }
//                             }
//
//                             break;
//                         }
//                     }
//                 }
//             }
//         }
//
//         private void CheckEntitiesTreeView()
//         {
//             if (selectWorld == null)
//                 return;
//             int count = MenuTreeView.RootItem.children.Count;
//             var entities = selectWorld.Entities.GetValueArray(Allocator.Temp);
//             foreach (var entity in entities)
//             {
//                 (MenuTreeView as GenericTreeView<Entity>).AddItem(entity.index.ToString(), entity);
//             }
//
//             entities.Dispose();
//             for (int i = MenuTreeView.RootItem.children.Count - 1; i >= 0; i--)
//             {
//                 var item = MenuTreeView.RootItem.children[i];
//                 if (item is EntityTreeViewItem<Entity> entityItem)
//                 {
//                     if (!selectWorld.Exists(entityItem.data))
//                         (MenuTreeView as GenericTreeView<Entity>).RemoveItem(entityItem.data);
//                 }
//             }
//
//             if (count != MenuTreeView.RootItem.children.Count)
//                 MenuTreeView.Reload();
//         }
//
//         private void CheckSystemsTreeView()
//         {
//             if (selectWorld == null)
//                 return;
//             int count = MenuTreeView.RootItem.children.Count;
//             foreach (var system in selectWorld.Systems)
//             {
//                 (MenuTreeView as GenericTreeView<ISystem>).AddItem(system.GetType().Name, system);
//             }
//
//             for (int i = MenuTreeView.RootItem.children.Count - 1; i >= 0; i--)
//             {
//                 var item = MenuTreeView.RootItem.children[i];
//                 if (item is EntityTreeViewItem<ISystem> entityItem)
//                 {
//                     if (!selectWorld.SystemsMap.TryGetValue(entityItem.data.GetType(), out var index) || index != i)
//                     {
//                         RefreshTreeView();
//                         break;
//                     }
//                 }
//             }
//         }
//
//         public void SelectWorld(Context world)
//         {
//             selectWorld = world;
//         }
//
//         [MenuItem("Tools/CZToolKit/ECS/Debugger")]
//         public static void Open()
//         {
//             GetWindow<ESCDebugger>();
//         }
//     }
//
//     public class GenericTreeView<T> : CZTreeView
//     {
//         public GenericTreeView(TreeViewState state) : base(state)
//         {
//         }
//
//         public GenericTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
//         {
//         }
//
//         Dictionary<T, EntityTreeViewItem<T>> itemMap = new Dictionary<T, EntityTreeViewItem<T>>();
//
//         protected override TreeViewItem BuildRoot()
//         {
//             Sort();
//             return base.BuildRoot();
//         }
//
//         public override void OnGUI(Rect rect)
//         {
//             base.OnGUI(rect);
//             var currentEvent = Event.current;
//             if (rect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
//             {
//                 if (HasSelection())
//                     this.SetSelection(new int[0]);
//                 currentEvent.Use();
//             }
//         }
//
//         public EntityTreeViewItem<T> AddItem(string path, T data)
//         {
//             if (itemMap.ContainsKey(data))
//                 return null;
//             var item = new EntityTreeViewItem<T>();
//             item.data = data;
//             AddMenuItem(path, item);
//             itemMap[data] = item;
//             return item;
//         }
//
//         public void RemoveItem(T data)
//         {
//             if (!itemMap.TryGetValue(data, out var item))
//                 return;
//             RootItem.children.Remove(item);
//             itemMap.Remove(data);
//             state.selectedIDs.Remove(item.id);
//         }
//
//         public void Sort()
//         {
//             if (typeof(T) == typeof(Entity))
//             {
//                 (RootItem as CZTreeViewItem).children.QuickSort((a, b) => { return (a as EntityTreeViewItem<Entity>).data.index.CompareTo((b as EntityTreeViewItem<Entity>).data.index); });
//             }
//             else
//             {
//             }
//         }
//     }
//
//     public class EntityTreeViewItem<T> : CZTreeViewItem
//     {
//         public T data;
//     }
//
//     [AttributeUsage(AttributeTargets.Class)]
//     public class CustomComponentDrawerAttribute : Attribute
//     {
//         public Type componentType;
//
//         public CustomComponentDrawerAttribute(Type componentType)
//         {
//             this.componentType = componentType;
//         }
//     }
//
//     public abstract class ComponentDrawer
//     {
//         public bool Foldout { get; set; } = true;
//
//         public abstract void OnGUI(Context world, Entity entity, IComponent component);
//     }
//
//     public class ComponentDrawerFactory
//     {
//         private class DefaultComponentDrawer : ComponentDrawer
//         {
//             public override void OnGUI(Context world, Entity entity, IComponent component)
//             {
//                 EditorGUILayout.LabelField(component.ToString());
//             }
//         }
//
//         private static Dictionary<Type, ComponentDrawer> drawers = new Dictionary<Type, ComponentDrawer>();
//         private static Dictionary<Type, Type> drawerTypes = new Dictionary<Type, Type>();
//
//         static ComponentDrawerFactory()
//         {
//             foreach (var type in TypeCache.GetTypesWithAttribute<CustomComponentDrawerAttribute>())
//             {
//                 if (type.IsAbstract)
//                     continue;
//                 var attribute = type.GetCustomAttribute<CustomComponentDrawerAttribute>();
//                 drawerTypes[attribute.componentType] = type;
//             }
//         }
//
//         public static ComponentDrawer GetComponentDrawer(Type componentType)
//         {
//             if (!drawers.TryGetValue(componentType, out var drawer))
//             {
//                 if (drawerTypes.TryGetValue(componentType, out var drawerType))
//                     drawer = Activator.CreateInstance(drawerType) as ComponentDrawer;
//                 if (drawer == null)
//                     drawer = new DefaultComponentDrawer();
//                 drawers[componentType] = drawer;
//             }
//
//             return drawer;
//         }
//     }
// }