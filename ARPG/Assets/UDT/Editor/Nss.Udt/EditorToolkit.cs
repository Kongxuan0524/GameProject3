using UnityEditor;
using UnityEngine;

namespace Nss.Udt {
	public class EditorToolkit {
		
		public static Texture2D gradientTexture;
		public static Texture2D gradientChildTexture;
		public static Texture2D colorTexture;
		
		public static GUILayoutOption LargeToolbarButtonLayoutOption() {
			return GUILayout.Width(120f);
		}
		
		public static GUILayoutOption NormalToolbarButtonLayoutOption() {
			return GUILayout.Width(80f);
		}
		
		public static GUILayoutOption CloseButtonLayoutOption() {
			return GUILayout.Width(20f);
		}
		
		public static GUILayoutOption GoButtonLayoutOption() {
			return GUILayout.Width(40f);
		}
		
		public static GUIStyle ErrorLabelStyle() {
			var style = new GUIStyle(EditorStyles.boldLabel);
			style.normal.textColor = Color.red;
			
			return style;
		}
		
		public static GUIStyle ValidatedLabelStyle() {
			var style = new GUIStyle(EditorStyles.boldLabel);
			style.normal.textColor = Color.green;
			style.fontStyle = FontStyle.Bold;
			
			return style;
		}
		
		public static GUIStyle BoundaryGroupSytle(Color color) {
			GUIStyle groupStyle = new GUIStyle(EditorStyles.textField);
			
			color.a = 0.75f;
			
			groupStyle.normal.background = GetColorTexture(color);
			groupStyle.focused.background = GetColorTexture(color);
			groupStyle.normal.textColor = Color.black;
			groupStyle.focused.textColor = Color.black;
			
			return groupStyle;
		}
		
		public static GUIStyle ZoneLabelStyle(Color color) {
			var col = color;
			col.a = 1f;
			
			var style = new GUIStyle() {
                normal = new GUIStyleState() {
                    textColor = col
                },
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
			
			return style;
		}
		
		public static GUIStyle BoundariesIconButton() {
			var style = new GUIStyle(GUI.skin.button);
			
			style.normal.background = Resources.Load("icon-boundaries") as Texture2D;
			style.active.background = Resources.Load("icon-boundaries-down") as Texture2D;
			style.fixedWidth = 64f;
			style.fixedHeight = 64f;
			
			return style;
		}
		
		public static GUIStyle ZonesIconButton() {
			var style = new GUIStyle(GUI.skin.button);
			
			style.normal.background = Resources.Load("icon-zones") as Texture2D;
			style.active.background = Resources.Load("icon-zones-down") as Texture2D;
			style.fixedWidth = 64f;
			style.fixedHeight = 64f;
			
			return style;
		}
		
		public static GUIStyle PoolsIconButton() {
			var style = new GUIStyle(GUI.skin.button);
			
			style.normal.background = Resources.Load("icon-pools") as Texture2D;
			style.active.background = Resources.Load("icon-pools-down") as Texture2D;
			style.fixedWidth = 64f;
			style.fixedHeight = 64f;
			
			return style;
		}
		
		public static void DrawTitle(string text) {
			text = string.Format("{0} ({1} v{2})", text, Config.SHORTNAME, Config.VERSION);
			
			Rect lastRect = DrawTitleGradient();
			GUI.color = Color.white;
			EditorGUI.LabelField(
				new Rect(lastRect.x + 3, lastRect.y+1, lastRect.width - 5, lastRect.height),
				text,
				new GUIStyle(EditorStyles.label) {
					fontStyle = FontStyle.Bold,
				}
			);
			
			GUI.color = Color.white;
		}
		
		public static void DrawFooter() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			
			if(GUILayout.Button("UDT Control Panel", GUILayout.Width(140f))) {
				Selection.activeObject = GameObject.Find(Config.ROOT_NAME);
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
		
		public static void DrawSeparator(int padding = 0) {
		    GUILayout.Space(10);
	        Rect lastRect = GUILayoutUtility.GetLastRect();
			
			GUI.color = Color.gray;
		    GUI.DrawTexture(new Rect(padding, lastRect.yMax-0, Screen.width, 1f), EditorGUIUtility.whiteTexture);
			GUI.color = Color.white;
		}
		
		public static bool DrawTitleFoldOut( bool foldOut,string text){

			Rect lastRect = DrawTitleGradient();
			GUI.color = Color.white;
			
			bool value = EditorGUI.Foldout(
				new Rect(lastRect.x + 3, lastRect.y+1, lastRect.width - 5, lastRect.height),
				foldOut,
				text,
				new GUIStyle(EditorStyles.foldout) {
					fontStyle = FontStyle.Bold
				}
			);
			GUI.color = Color.white;
			
			return value;
		}
		
		#region Private Draw Implementation
		private static Rect DrawTitleGradient() {
		    GUILayout.Space(30);
			Rect lastRect = GUILayoutUtility.GetLastRect();
		    lastRect.yMin = lastRect.yMin + 5;
		    lastRect.yMax = lastRect.yMax - 5;
		    lastRect.width =  Screen.width;
			
			GUI.DrawTexture(new Rect(0,lastRect.yMin,Screen.width, lastRect.yMax- lastRect.yMin), GetGradientTexture());
			
			GUI.color = new Color(0.54f,0.54f,0.54f);
			GUI.DrawTexture(new Rect(0,lastRect.yMin,Screen.width,1f),  EditorGUIUtility.whiteTexture);
			GUI.DrawTexture(new Rect(0,lastRect.yMax- 1f,Screen.width,1f),  EditorGUIUtility.whiteTexture);
			
			return lastRect;
		}

		private static Rect DrawTitleChildGradient(int padding = 0) {
		    GUILayout.Space(30);
			Rect lastRect = GUILayoutUtility.GetLastRect();
		    lastRect.yMin = lastRect.yMin + 5;
		    lastRect.yMax = lastRect.yMax - 5;
		    lastRect.width =  Screen.width;
			
			GUI.DrawTexture(new Rect(padding,lastRect.yMin,Screen.width, lastRect.yMax- lastRect.yMin), GetChildGradientTexture());
			
			GUI.color = new Color(0.54f,0.54f,0.54f);
			GUI.DrawTexture(new Rect(padding,lastRect.yMin,Screen.width,1f),  EditorGUIUtility.whiteTexture);
			GUI.DrawTexture(new Rect(padding,lastRect.yMax- 1f,Screen.width,1f),  EditorGUIUtility.whiteTexture);
			
			return lastRect;		
		}

		private static Texture2D GetColorTexture(Color color){
			Texture2D myTexture = new Texture2D(1, 16);
			myTexture.name = "editor_texture";
			myTexture.hideFlags = HideFlags.HideInInspector;
			myTexture.filterMode = FilterMode.Bilinear;
			myTexture.hideFlags = HideFlags.DontSave;		
			
			for (int i = 0; i < 16; i++) {
				myTexture.SetPixel(0, i, color);
			}
			
			myTexture.Apply();
			return myTexture;	
		}

		private static Texture2D GetGradientTexture() {
			
			if (EditorToolkit.gradientTexture==null) {
				EditorToolkit.gradientTexture = CreateGradientTexture();
			}
			
			return gradientTexture;
		}
			
		private static Texture2D CreateGradientTexture() {
			float start=0.4f;
			float end= 0.8f;
			float step = (end-start)/16;
			Color color = new Color(start,start,start);
			Color pixColor = color;
			Texture2D myTexture = new Texture2D(1, 16);
			
			myTexture.name="editor_gradient";
			myTexture.hideFlags = HideFlags.HideInInspector;
			myTexture.filterMode = FilterMode.Bilinear;
			myTexture.hideFlags = HideFlags.DontSave;
			
			for (int i = 0; i < 16; i++) {
				pixColor = new Color (pixColor.r+step, pixColor.b+step, pixColor.b+step,0.5f);
				myTexture.SetPixel(0, i, pixColor);
			}
			
			myTexture.Apply();
			return myTexture;
		}

		private static Texture2D GetChildGradientTexture() {
			
			if (EditorToolkit.gradientChildTexture==null) {
				EditorToolkit.gradientChildTexture = CreateChildGradientTexture();
			}
			
			return gradientChildTexture;		
		}

		private static Texture2D CreateChildGradientTexture() {
			float start=0.4f;
			float end= 0.8f;
			float step = (end-start)/16;
			Color color = new Color(start,start,start);
			Color pixColor = color;
			Texture2D myTexture = new Texture2D(1, 16);
			
			myTexture.name = "editor_gradient";
			myTexture.hideFlags = HideFlags.HideInInspector;
			myTexture.filterMode = FilterMode.Bilinear;
			myTexture.hideFlags = HideFlags.DontSave;
			
			for (int i = 0; i < 16; i++) {
				pixColor = new Color (pixColor.r+step, pixColor.b+step, pixColor.b+step,0.2f);
				myTexture.SetPixel(0, i, pixColor);
			}
			
			myTexture.Apply();
			return myTexture;
		}
		#endregion
	}
}