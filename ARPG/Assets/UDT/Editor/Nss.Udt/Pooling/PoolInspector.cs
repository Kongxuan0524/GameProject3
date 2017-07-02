using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Pooling.Editors {
	[CustomEditor(typeof(Pool))]
	public class PoolInspector : Editor {
	
		private Pool pool;
		private Tool lastTool = Tool.None;
		
		private void OnEnable() {
			lastTool = Tools.current;
			pool = (Pool)target;
		}
		
		private void OnDisable() {
			Tools.current = lastTool;
		}
	
		public override void OnInspectorGUI ()
		{
			Tools.current = Tool.None;
			GUI.changed = false;
			
			EditorToolkit.DrawTitle("Pool");
			DrawToolbar();
			EditorToolkit.DrawSeparator();
			
			pool.prefab = EditorGUILayout.ObjectField("Prefab", pool.prefab, typeof(GameObject), false) as GameObject;
			
			if(pool.prefab) {
				pool.name = string.Format("pool-{0}", pool.prefab.name);
			}
			
			pool.size = EditorGUILayout.IntField("Pool Size", pool.size);
			pool.limit = EditorGUILayout.Toggle("Limit Growth", pool.limit);
			
			if(pool.limit) {
				pool.limitSize = EditorGUILayout.IntField("Limit Size", pool.limitSize);
				pool.suppressLimitErrors = EditorGUILayout.Toggle("Ignore Limit Errors", pool.suppressLimitErrors);
				
				if(pool.limitSize < pool.size) pool.limitSize = pool.size;
			}
			
			pool.shrinkBack = EditorGUILayout.Toggle("Shrink Back", pool.shrinkBack);
			pool.hideInHierarchy = EditorGUILayout.Toggle("Hide in Hierarchy", pool.hideInHierarchy);
			
			EditorToolkit.DrawSeparator();
			EditorToolkit.DrawFooter();
			
			if(GUI.changed) EditorUtility.SetDirty(pool);
		}
		
		private void DrawToolbar() {
			if(GUILayout.Button("< Back", EditorToolkit.LargeToolbarButtonLayoutOption())) {
				Selection.activeGameObject = pool.transform.parent.gameObject;
			}
		}
	}
}