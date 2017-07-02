using System;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Referee.Editors {
	[InitializeOnLoad]
	public class RefereeHierarchyCallback : MonoBehaviour {
	
		private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;
			private static Texture2D hierarchyIcon;
			
			private static Texture2D HierarchyIcon {
				get {
					if (RefereeHierarchyCallback.hierarchyIcon == null){
						RefereeHierarchyCallback.hierarchyIcon = (Texture2D)Resources.Load("icon-referee");
					}
					return RefereeHierarchyCallback.hierarchyIcon;
				}
			}
			
			static RefereeHierarchyCallback() {
				RefereeHierarchyCallback.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(RefereeHierarchyCallback.DrawHierarchyIcon);
				EditorApplication.hierarchyWindowItemOnGUI =
					(EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(EditorApplication.hierarchyWindowItemOnGUI, RefereeHierarchyCallback.hiearchyItemCallback);
			}
			
			private static void DrawHierarchyIcon(int instanceID, Rect selectionRect) {
				GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
				if (gameObject != null && gameObject.GetComponent<RefereeController>() != null)
				{
					Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
					GUI.DrawTexture(rect, RefereeHierarchyCallback.HierarchyIcon);
				}
			}
	}
}