using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Editors {
	[CustomEditor(typeof(UdtController))]
	public class UdtControllerInspector : Editor {
	
		/// <summary>
		/// Unity3D OnInspectorGUI event
		/// </summary>
		public override void OnInspectorGUI ()
		{
			EditorToolkit.DrawTitle("Unity Design Tools");
			
			DrawControlPanel();
			
			EditorToolkit.DrawSeparator();
			
			DrawSupportInfo();
		}

		private void DrawControlPanel() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			
			if(GUILayout.Button("", EditorToolkit.BoundariesIconButton())) {
				Selection.activeGameObject = MenuExtensions.GetBoundariesControllerRoot();
			}
			
			EditorGUILayout.Space();
			
			if(GUILayout.Button("", EditorToolkit.ZonesIconButton())) {
				Selection.activeGameObject = MenuExtensions.GetZonesControllerRoot();
			}
			
			EditorGUILayout.Space();
			
			if(GUILayout.Button("", EditorToolkit.PoolsIconButton())) {
				Selection.activeGameObject = MenuExtensions.GetPoolControllerRoot();
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}
		
		private void DrawSupportInfo() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("Forum Link", EditorToolkit.LargeToolbarButtonLayoutOption())) {
				Application.OpenURL("http://forum.unity3d.com/threads/201034-uDesign-Tools-Released");
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if(GUILayout.Button("Contact Support", EditorToolkit.LargeToolbarButtonLayoutOption())) {
				Application.OpenURL("mailto:support@nothingsoftstudios.com");
			}
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space();
		}
	}
}