using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Boundaries.Editors {
    [CustomEditor(typeof(BoundaryController))]
    public class BoundaryControllerInspector : Editor {
        #region Serialized Items
        private BoundaryController controller;
        #endregion

        #region Private Members
        private Tool lastTool = Tool.None;
        #endregion

        #region Unity3D Events
        private void OnEnable() {
            controller = (BoundaryController)target;

            lastTool = Tools.current;
            Tools.current = Tool.None;
        }

        private void OnDisable() {
            Tools.current = lastTool;
        }

        public override void OnInspectorGUI() {
            Tools.current = Tool.None;
			
			EditorToolkit.DrawTitle("Boundaries Controller");
            
            ShowToolbar();
			EditorToolkit.DrawSeparator();
			
            ShowGroups();
			EditorToolkit.DrawSeparator();
			
			EditorToolkit.DrawFooter();
        }
        #endregion

        #region Inspector UI
        private void ShowToolbar() {
            EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();

            if (GUILayout.Button("New Group", EditorToolkit.LargeToolbarButtonLayoutOption())) {
                AddGroup();
            }
			
			EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }

        private void ShowGroups() {
            var groups = GetChildGroups();
			
			EditorGUILayout.Space();
            EditorGUI.indentLevel++;
			
			if(groups.Count > 0) {
				for(int i=0; i < groups.Count; i++) {
					ShowGroup(groups[i]);
				}
			}
			else {
				EditorGUILayout.LabelField("no groups defined",
					EditorToolkit.ErrorLabelStyle()
				);
			}
			
			EditorGUI.indentLevel--;
        }

        private void ShowGroup(Group group) {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", EditorToolkit.CloseButtonLayoutOption())) {
                GameObject.DestroyImmediate(group.gameObject);
                return;
            }
			
            group.gameObject.name = EditorGUILayout.TextField(
				group.gameObject.name,
				EditorToolkit.BoundaryGroupSytle(group.color)
			);

            if (GUILayout.Button("Go >", EditorToolkit.GoButtonLayoutOption())) {
                Selection.activeGameObject = group.gameObject;
            }

            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region Private Methods
        private List<Group> GetChildGroups() {
            var result = new List<Group>();
            result.AddRange(controller.GetComponentsInChildren<Group>());

            return result;
        }

        private void AddGroup() {
            var group = new GameObject("new-group");
            var gc = group.AddComponent<Group>();
            group.transform.parent = controller.gameObject.transform;

            gc.color = Color.cyan;
            gc.color.a = 0.5f;
            gc.height = 1f;
        }
        #endregion
    } 
}