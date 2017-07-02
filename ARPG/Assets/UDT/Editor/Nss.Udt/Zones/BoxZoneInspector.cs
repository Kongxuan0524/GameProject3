using Nss.Udt.Common;
using Nss.Udt.Zones;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Zones.Editors {
	[CustomEditor(typeof(BoxZone))]
	public class BoxZoneInspector : Editor {
	
		private BoxZone zone;
		private Tool lastTool = Tool.None;
		
		#region Unity3D Events
		/// <summary>
		/// Unity3D OnEnable event
		/// </summary>
		private void OnEnable() {
			lastTool = Tools.current;
			Tools.current = Tool.Move;
			
			zone = (BoxZone)target;
		}

		/// <summary>
		/// Unity3D OnDisable event
		/// </summary>
		private void OnDisable() {
			Tools.current = lastTool;
		}

		/// <summary>
		/// Unity3D OnInspectorGUI event
		/// </summary>
		public override void OnInspectorGUI() {
			RestrictColliderSettings();
			GUI.changed = false;
			
			EditorToolkit.DrawTitle("Zone");
			DrawToolbar();
			
			EditorToolkit.DrawSeparator();
			
			DrawSettings();
			EditorToolkit.DrawFooter();
			
			if(GUI.changed) EditorUtility.SetDirty(target);
		}

		/// <summary>
		/// Unity3D OnSceneGUI event
		/// </summary>
		private void OnSceneGUI() {
			HandlesHelpers.VisibleLabel(
		              zone.transform.position,
		              100f,
		              zone.name,
		              EditorToolkit.ZoneLabelStyle(zone.color)
		          );
		}
		#endregion
		
		/// <summary>
		/// Draws the toolbar
		/// </summary>
		private void DrawToolbar() {
			if(GUILayout.Button("< Back", EditorToolkit.LargeToolbarButtonLayoutOption())) {
				Selection.activeGameObject = zone.transform.parent.gameObject;
			}
		}
		
		/// <summary>
		/// Draws the zone settings
		/// </summary>
		private void DrawSettings() {
			zone.gameObject.name = EditorGUILayout.TextField("Name", zone.gameObject.name);
			zone.color = EditorGUILayout.ColorField("Color", zone.color);
			zone.triggerType = (Zone.ZoneTriggerTypes)EditorGUILayout.EnumPopup("Trigger Type", zone.triggerType);
			
			if(zone.triggerType == Zone.ZoneTriggerTypes.SendMessage) {
				EditorToolkit.DrawSeparator();
				EditorGUILayout.Space();
				EditorGUI.indentLevel++;
				
				zone.messageReceiver = (GameObject)EditorGUILayout.ObjectField("Message Receiver", zone.messageReceiver, typeof(GameObject), true);
				zone.RequireReceivers = EditorGUILayout.Toggle("Require Receivers", zone.RequireReceivers);
				zone.MessageEnterHandler = EditorGUILayout.TextField("OnEnter Handler", zone.MessageEnterHandler);
				zone.MessageStayHandler = EditorGUILayout.TextField("OnStay Handler", zone.MessageStayHandler);
				zone.MessageExitHandler = EditorGUILayout.TextField("OnExit Handler", zone.MessageExitHandler);
				
				EditorGUI.indentLevel--;
				EditorToolkit.DrawSeparator();
			}
			else {
				EditorToolkit.DrawSeparator();
				EditorGUILayout.Space();
				EditorGUI.indentLevel++;
				
				EditorGUILayout.HelpBox(
					"You must provide your own component on this game object to handle OnTriggerEnter, OnTriggerStay and/or OnTriggerExit",
					MessageType.Info
				);
				EditorGUI.indentLevel--;
				EditorToolkit.DrawSeparator();
			}
		}
		
		private void RestrictColliderSettings() {
			var box = zone.GetComponent<BoxCollider>();
			
			box.center = Vector3.zero;
			box.size = Vector3.one;
			box.isTrigger = true;
		}
	}
}