using System;
using Nss.Udt.Zones;
using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Zones.Editors {
	[InitializeOnLoad]
	public class ZoneHierarchyCallback {
		
		private static readonly EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;
		private static Texture2D hierarchyIcon;
		
		private static Texture2D HierarchyIcon {
			get {
				if (ZoneHierarchyCallback.hierarchyIcon == null){
					ZoneHierarchyCallback.hierarchyIcon = (Texture2D)Resources.Load("icon-zones-hier");
				}
				return ZoneHierarchyCallback.hierarchyIcon;
			}
		}
		
		static ZoneHierarchyCallback() {
			ZoneHierarchyCallback.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(ZoneHierarchyCallback.DrawHierarchyIcon);
			EditorApplication.hierarchyWindowItemOnGUI =
				(EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(EditorApplication.hierarchyWindowItemOnGUI, ZoneHierarchyCallback.hiearchyItemCallback);
		}
		
		private static void DrawHierarchyIcon(int instanceID, Rect selectionRect) {
			GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (gameObject != null && gameObject.GetComponent<ZoneController>() != null)
			{
				Rect rect = new Rect(selectionRect.x + selectionRect.width - 16f, selectionRect.y, 16f, 16f);
				GUI.DrawTexture(rect, ZoneHierarchyCallback.HierarchyIcon);
			}
		}	
	}
}