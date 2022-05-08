#region 注 释
/***
 *
 *  Title:
 *  
 *  Description:
 *  
 *  Date:
 *  Version:
 *  Writer: 半只龙虾人
 *  Github: https://github.com/HalfLobsterMan
 *  Blog: https://www.crosshair.top/
 *
 */
#endregion
using UnityEditor;
using CZToolKit.Core.Editors;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Unity.Collections;
using System.Collections.Generic;

namespace CZToolKit.ECS.Editors
{
    public class ESCDebugger : BasicMenuEditorWindow
    {
        World selectWorld;

        protected override void OnEnable()
        {
            titleContent = new GUIContent("ECS Debugger");
            base.OnEnable();
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            foreach (var world in World.Worlds.Values)
            {
                SelectWorld(world);
                break;
            }
            RefreshTreeView();
        }

        private void OnPlayModeChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    foreach (var world in World.Worlds.Values)
                    {
                        SelectWorld(world);
                        break;
                    }
                    RefreshTreeView();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    SelectWorld(null);
                    RefreshTreeView();
                    Repaint();
                    break;
            }
        }

        protected override CZTreeView BuildMenuTree(TreeViewState treeViewState)
        {
            EntitiesTreeView worldTreeView = new EntitiesTreeView(treeViewState);
            worldTreeView.RowHeight = 20;
            worldTreeView.ShowBoder = true;
            worldTreeView.ShowAlternatingRowBackgrounds = true;
            if (selectWorld != null)
            {
                foreach (var entity in selectWorld.Entities.GetValueArray(Allocator.Temp))
                {
                    worldTreeView.AddEntityItem(entity);
                }
            }
            return worldTreeView;
        }

        protected override void OnLeftGUI()
        {
            var worldSelectButtonRect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true));
            var dropDownLabel = selectWorld == null ? EditorGUIUtility.TrTextContent("None") : EditorGUIUtility.TrTextContent(selectWorld.name);
            if (GUI.Button(worldSelectButtonRect, dropDownLabel, EditorStyles.toolbarDropDown))
            {
                GenericMenu worldMenu = new GenericMenu();
                foreach (var world in World.Worlds.Values)
                {
                    worldMenu.AddItem(new GUIContent(world.name), false, () =>
                    {
                        SelectWorld(world);
                        RefreshTreeView();
                    });
                }
                worldMenu.DropDown(worldSelectButtonRect);
            }
            base.OnLeftGUI();
        }

        protected unsafe override void OnRightGUI(IList<int> selection)
        {
            base.OnRightGUI(selection);

            {
                // 绘制Component界面
                if (selection.Count != 0 && MenuTreeView.RootItem.children.Count != 0)
                {
                    CZTreeViewItem item = null;
                    foreach (var selectedID in selection)
                    {
                        item = MenuTreeView.FindItem(selection[0]);
                        if (item != null)
                            break;
                    }
                    if (item is EntityTreeViewItem entityItem)
                    {
                        var selectedEntity = entityItem.entity;
                        foreach (var componentPool in selectWorld.ComponentPools.Values)
                        {
                            if (componentPool.Contains(&selectedEntity))
                            {
                                GUILayout.Label(componentPool.ComponentType.Name);
                            }
                        }
                    }
                }
            }
        }

        protected override void OnTreeViewGUI()
        {
            CheckEntitiesTreeView();
            base.OnTreeViewGUI();
            Repaint();
        }

        private void CheckEntitiesTreeView()
        {
            if (selectWorld == null)
                return;
            int count = MenuTreeView.RootItem.children.Count;
            var entities = selectWorld.Entities.GetValueArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                (MenuTreeView as EntitiesTreeView).AddEntityItem(entity);
            }
            entities.Dispose();
            for (int i = MenuTreeView.RootItem.children.Count - 1; i >= 0; i--)
            {
                var item = MenuTreeView.RootItem.children[i];
                if (item is EntityTreeViewItem entityItem)
                {
                    if (!entityItem.entity.IsValid())
                        (MenuTreeView as EntitiesTreeView).RemoveEntityItem(entityItem.entity);
                }
            }
            if (count != MenuTreeView.RootItem.children.Count)
                MenuTreeView.Reload();
        }

        public void SelectWorld(World world)
        {
            selectWorld = world;
        }

        [MenuItem("Tools/CZToolKit/ECS/Debugger")]
        public static void Open()
        {
            GetWindow<ESCDebugger>();
        }
    }

    public class EntitiesTreeView : CZTreeView
    {
        public EntitiesTreeView(TreeViewState state) : base(state) { }

        public EntitiesTreeView(TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader) { }

        Dictionary<Entity, EntityTreeViewItem> itemMap = new Dictionary<Entity, EntityTreeViewItem>();

        protected override TreeViewItem BuildRoot()
        {
            Sort();
            return base.BuildRoot();
        }

        public override void OnGUI(Rect rect)
        {
            base.OnGUI(rect);
            var currentEvent = Event.current;
            if (rect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                if (HasSelection())
                    this.SetSelection(new int[0]);
                currentEvent.Use();
            }
        }

        public EntityTreeViewItem AddEntityItem(Entity entity)
        {
            if (itemMap.ContainsKey(entity))
                return null;
            var item = new EntityTreeViewItem();
            item.entity = entity;
            AddMenuItem(entity.ID.ToString(), item);
            itemMap[entity] = item;
            return item;
        }

        public void RemoveEntityItem(Entity entity)
        {
            if (!itemMap.TryGetValue(entity, out var item))
                return;
            RootItem.children.Remove(item);
            itemMap.Remove(entity);
            state.selectedIDs.Remove(item.id);
        }

        public void Sort()
        {
            (RootItem as CZTreeViewItem).children.QuickSort((a, b) =>
            {
                return (a as EntityTreeViewItem).entity.ID.CompareTo((b as EntityTreeViewItem).entity.ID);
            });
        }
    }

    public class EntityTreeViewItem : CZTreeViewItem
    {
        public Entity entity;
    }
}
