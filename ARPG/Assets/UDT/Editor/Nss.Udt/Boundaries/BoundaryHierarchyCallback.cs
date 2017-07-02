using System;
using Nss.Udt.Boundaries;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Boundaries.Editors {
	[InitializeOnLoad]
	public class BoundaryHierarchyCallback {
		
		private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;
		private static Texture2D hierarchyIcon;
		
		private static Texture2D HierarchyIcon {
			get {
				if (BoundaryHierarchyCallback.hierarchyIcon == null){
					BoundaryHierarchyCallback.hierarchyIcon = (Texture2D)Resources.Load("icon-boundaries-hier");
				}
				return BoundaryHierarchyCallback.hierarchyIcon;
			}
		}
		
		static BoundaryHierarchyCallback() {
			BoundaryHierarchyCallback.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(BoundaryHierarchyCallback.DrawHierarchyIcon);
			EditorApplication.hierarchyWindowItemOnGUI =
				(EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(EditorApplication.hierarchyWindowItemOnGUI, BoundaryHierarchyCallback.hiearchyItemCallback);
		}
		
		private static void DrawHierarchyIcon(int instanceID, Rect selectionRect) {
			GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (gameObject != null && gameObject.GetComponent<BoundaryController>() != null)
			{
				Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
				GUI.DrawTexture(rect, BoundaryHierarchyCallback.HierarchyIcon);
			}
		}	
	}
}