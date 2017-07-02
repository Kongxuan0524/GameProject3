using UnityEditor;
using UnityEngine;

namespace Nss.Udt.Pooling.Editors {
	[CustomEditor(typeof(PoolController))]
	public class PoolControllerInspector : Editor {
		
		private PoolController controller;
		private Tool lastTool = Tool.None;
		
		private Pool[] pools;
		
		private void OnEnable() {
			lastTool = Tools.current;
			controller = (PoolController)target;
		}
		
		private void OnDisable() {
			Tools.current = lastTool;
		}
	
		public override void OnInspectorGUI ()
		{
			Tools.current = Tool.None;
			GUI.changed = false;
			
			EditorToolkit.DrawTitle("Pool Controller");
			DrawToolbar();
			
			if(!controller) return; // handle a DestroyAll call
			
			EditorToolkit.DrawSeparator();
			DrawPools();
			
			EditorToolkit.DrawSeparator();
			EditorToolkit.DrawFooter();
			
			if(GUI.changed) EditorUtility.SetDirty(controller);
		}
		
		private void DrawToolbar() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			
			if(GUILayout.Button("Add Pool", EditorToolkit.LargeToolbarButtonLayoutOption())) {
				CreatePool();
			}
			
			if(GUILayout.Button("Destroy All", EditorToolkit.LargeToolbarButtonLayoutOption())) {
				DestroyAll();
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
		}
		
		private void DrawPools() {
			pools = controller.GetComponentsInChildren<Pool>();
			
			for(int i=0; i < pools.Length; i++) {
				bool active = DrawPool(pools[i]);
				
				if(!active) break;
			}
		}
		
		private bool DrawPool(Pool pool) {
			// Draw the header info
			var cont = DrawPoolHeader(pool);
			
			if(!cont) return false;
			
			// Draw pool details
			DrawPoolQuickEdit(pool);
			
			return true;
		}

		private bool DrawPoolHeader(Pool pool) {
			EditorGUILayout.BeginHorizontal();
			
			if(GUILayout.Button("X", EditorToolkit.CloseButtonLayoutOption())) {
				Undo.RegisterSceneUndo("Deleted UDT Pool");
				GameObject.DestroyImmediate(pool.gameObject);
				
				return false;
			}
			
			if(pool.prefab == null) {
				var title = string.Format("{0} (no prefab)", pool.name);
				var style = new GUIStyle(EditorStyles.boldLabel);
				
				style.normal.textColor = Color.red;
				
				EditorGUILayout.LabelField(title,
					style
				);
			}
			else {
				var style = new GUIStyle(EditorStyles.label);
				style.normal.textColor = Color.green;
				
				pool.name = pool.prefab.name;
				
				EditorGUILayout.LabelField(pool.name,
					style
				);
			}
			
			if(GUILayout.Button("Go >", EditorToolkit.GoButtonLayoutOption())) {
				Selection.activeGameObject = pool.gameObject;
			}
			
			EditorGUILayout.EndHorizontal();
			
			return true;
		}

		private void DrawPoolQuickEdit(Pool pool) {
			EditorGUI.indentLevel++;
			
			EditorGUILayout.BeginHorizontal();
			
			pool.prefab = EditorGUILayout.ObjectField(pool.prefab, typeof(GameObject), false) as GameObject;
			
			if(pool.prefab) {
				pool.name = string.Format("pool-{0}", pool.prefab.name);
			}
			
			pool.size = EditorGUILayout.IntField(pool.size, GUILayout.Width(40f));
			
			if(pool.limit && pool.size > pool.limitSize) {
				pool.limitSize = pool.size;
				Debug.Log(
					string.Format("UDT: Increased limit to match new pool size. [Pool: '{0}']", pool.name)
				);
			}
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUI.indentLevel--;
		}
		
		private void CreatePool() {
			var pool = new GameObject("new-pool");
			pool.AddComponent<Pool>();
			pool.transform.parent = controller.transform;
		}
		
		private void DestroyAll() {
			Undo.RegisterSceneUndo("Destroyed UDT Pools Controller");
			
			var pools = controller.GetComponentsInChildren<Pool>();
			
			for(int i=0; i < pools.Length; i++) {
				GameObject.DestroyImmediate(pools[i].gameObject);
			}
			
			GameObject.DestroyImmediate(controller.gameObject);
		}
	}
}