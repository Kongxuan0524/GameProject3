using System;
using Nss.Udt.Pooling;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Pooling.Editors {
	[InitializeOnLoad]
	public class PoolHierarchyCallback {
		
		private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;
		private static Texture2D hierarchyIcon;
		
		private static Texture2D HierarchyIcon {
			get {
				if (PoolHierarchyCallback.hierarchyIcon == null){
					PoolHierarchyCallback.hierarchyIcon = (Texture2D)Resources.Load("icon-pools-hier");
				}
				return PoolHierarchyCallback.hierarchyIcon;
			}
		}
		
		static PoolHierarchyCallback() {
			PoolHierarchyCallback.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(PoolHierarchyCallback.DrawHierarchyIcon);
			EditorApplication.hierarchyWindowItemOnGUI =
				(EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(EditorApplication.hierarchyWindowItemOnGUI, PoolHierarchyCallback.hiearchyItemCallback);
		}
		
		private static void DrawHierarchyIcon(int instanceID, Rect selectionRect) {
			GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (gameObject != null && gameObject.GetComponent<PoolController>() != null)
			{
				Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
				GUI.DrawTexture(rect, PoolHierarchyCallback.HierarchyIcon);
			}
		}	
	}
}