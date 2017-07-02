using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Nss.Udt.Zones.Editors {
	[CustomEditor(typeof(ZoneController))]
	public class ZoneControllerInspector : Editor {
	
		private ZoneController controller;
		private Tool lastTool = Tool.None;
		
		private void OnEnable() {
			controller = (ZoneController)target;
			lastTool = Tools.current;
		}
		
		private void OnDisable() {
			Tools.current = lastTool;
		}
		
		public override void OnInspectorGUI ()
		{
			EditorToolkit.DrawTitle("Zone Controller");
			
			DrawToolbar();
			EditorToolkit.DrawSeparator();
			
			DrawZones();
			EditorToolkit.DrawSeparator();
			
			EditorToolkit.DrawFooter();
		}
		
		private void DrawToolbar() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();

            if (GUILayout.Button("New Zone", EditorToolkit.LargeToolbarButtonLayoutOption())) {
                AddBoxZone();
            }
			
			EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
		}
		
		private void DrawZones() {
			var zones = controller.GetComponentsInChildren<Zone>();
			
			EditorGUILayout.Space();
            EditorGUI.indentLevel++;
			
			if(zones.Length > 0) {
				for(int i=0; i < zones.Length; i++) {
					DrawZone(zones[i]);
				}
			}
			else {
				EditorGUILayout.LabelField("no zones defined",
					EditorToolkit.ErrorLabelStyle()
				);
			}
			
			EditorGUI.indentLevel--;
		}
		
		private void DrawZone(Zone zone) {
			EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", EditorToolkit.CloseButtonLayoutOption())) {
                GameObject.DestroyImmediate(zone.gameObject);
                return;
            }
			
			if(zone.messageReceiver == null && zone.triggerType == Zone.ZoneTriggerTypes.SendMessage) {
				EditorGUILayout.LabelField(
					string.Format("{0} (no receiver)", zone.gameObject.name),
					EditorToolkit.ErrorLabelStyle()
				);
			}
			else {
	            zone.gameObject.name = EditorGUILayout.TextField(
					zone.gameObject.name,
					EditorToolkit.BoundaryGroupSytle(zone.color)
				);
			}

            if (GUILayout.Button("Go >", EditorToolkit.GoButtonLayoutOption())) {
                Selection.activeGameObject = zone.gameObject;
            }

            EditorGUILayout.EndHorizontal();
		}
		
		private void AddBoxZone() {
			var root = new GameObject("new-zone");
            var c = root.AddComponent<BoxZone>();
            root.transform.parent = controller.gameObject.transform;

            c.color = Color.cyan;
            c.color.a = 0.5f;
		}
	}
}