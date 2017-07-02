// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

//#define ASE_PORT_INFO

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	public sealed class PortLegendInfo : EditorWindow
	{
		private const string KeyboardUsageTemplate = "[{0}] - {1}";
		private const string m_lockedStr = "Locked Port";

		private const float WindowSizeX = 250;
		private const float WindowSizeY = 300;
		private const float WindowPosX = 5;
		private const float WindowPosY = 5;

		private Rect m_availableArea;

		private bool m_portAreaFoldout = true;
		private bool m_shortcutAreaFoldout = true;

		private Vector2 m_currentScrollPos;

		private GUIStyle m_portStyle;
		private GUIStyle m_labelStyleBold;
		private GUIStyle m_labelStyle;

		private GUIContent m_content = new GUIContent( "Helper", "Shows helper info for ASE users" );
		private bool m_init = true;

		private List<ShortcutItem> m_overallShortcuts = null;
#if ASE_PORT_INFO
		[MenuItem( "Window/Amplify Shader Editor/Open Help Console" )]
		static void OpenMainShaderGraph()
		{
			OpenWindow();
		}
#endif
		public static PortLegendInfo OpenWindow()
		{
			PortLegendInfo currentWindow = ( PortLegendInfo ) PortLegendInfo.GetWindow( typeof( PortLegendInfo ), false );
			currentWindow.minSize = new Vector2( WindowSizeX, WindowSizeY );
			currentWindow.maxSize = new Vector2( WindowSizeX, 2 * WindowSizeY ); ;
			currentWindow.wantsMouseMove = true;
			return currentWindow;
		}

		public void Init()
		{
			m_init = false;
			wantsMouseMove = false;
			titleContent = m_content;
			m_portStyle = new GUIStyle( UIUtils.CustomStyle( CustomStyle.PortEmptyIcon ) );
			m_portStyle.alignment = TextAnchor.MiddleLeft;
			m_portStyle.imagePosition = ImagePosition.ImageOnly;
			m_portStyle.margin = new RectOffset( 5, 0, 5, 0 );

			m_labelStyleBold = new GUIStyle( UIUtils.CustomStyle( CustomStyle.InputPortlabel ) );
			m_labelStyleBold.fontStyle = FontStyle.Bold;

			m_labelStyle = new GUIStyle( UIUtils.CustomStyle( CustomStyle.InputPortlabel ) );
			m_labelStyle.clipping = TextClipping.Overflow;
			m_labelStyle.imagePosition = ImagePosition.TextOnly;
			m_labelStyle.contentOffset = new Vector2( -10, 0 );

			m_availableArea = new Rect( WindowPosX, WindowPosY, WindowSizeX - 2 * WindowPosX, WindowSizeY - 2 * WindowPosY );
		}

		void DrawPort( WirePortDataType type )
		{
			EditorGUILayout.BeginHorizontal();
			{
				GUI.color = UIUtils.GetColorForDataType( type, false );
				GUILayout.Box( string.Empty, m_portStyle, GUILayout.Width( UIUtils.PortsSize.x ), GUILayout.Height( UIUtils.PortsSize.y ) );

				GUI.color = Color.white;
				EditorGUILayout.LabelField( UIUtils.GetNameForDataType( type ), m_labelStyle );
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}

		void OnGUI()
		{
			if ( m_init )
			{
				Init();
			}
			m_availableArea = new Rect( WindowPosX, WindowPosY, position.width - 2 * WindowPosX, position.height - 2 * WindowPosY );
			GUILayout.BeginArea( m_availableArea );
			{
				m_currentScrollPos = EditorGUILayout.BeginScrollView( m_currentScrollPos, GUILayout.Width( 0 ), GUILayout.Height( 0 ) );
				{
					EditorGUILayout.BeginVertical();
					{
						NodeUtils.DrawPropertyGroup( ref m_portAreaFoldout, "Port Info", DrawPortInfo );
						NodeUtils.DrawPropertyGroup( ref m_shortcutAreaFoldout, "Shortcuts", DrawKeyboardShortcuts );
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndScrollView();
			}
			GUILayout.EndArea();
		}

		void DrawPortInfo()
		{
			Color originalColor = GUI.color;

			DrawPort( WirePortDataType.OBJECT );
			DrawPort( WirePortDataType.INT );
			DrawPort( WirePortDataType.FLOAT );
			DrawPort( WirePortDataType.FLOAT2 );
			DrawPort( WirePortDataType.FLOAT3 );
			DrawPort( WirePortDataType.FLOAT4 );
			DrawPort( WirePortDataType.COLOR );
			DrawPort( WirePortDataType.SAMPLER2D );
			DrawPort( WirePortDataType.FLOAT3x3 );
			DrawPort( WirePortDataType.FLOAT4x4 );

			EditorGUILayout.BeginHorizontal();
			GUI.color = Constants.LockedPortColor;
			GUILayout.Box( string.Empty, m_portStyle, GUILayout.Width( UIUtils.PortsSize.x ), GUILayout.Height( UIUtils.PortsSize.y ) );
			GUI.color = Color.white;
			EditorGUILayout.LabelField( m_lockedStr, m_labelStyle );
			EditorGUILayout.EndHorizontal();

			GUI.color = originalColor;
		}

		public void DrawKeyboardShortcuts()
		{
			AmplifyShaderEditorWindow window = UIUtils.CurrentWindow;
			if ( window != null )
			{
				if ( m_overallShortcuts == null )
				{
					m_overallShortcuts = window.ShortcutManagerInstance.AvailableShortcutsList;
				}

				int count = m_overallShortcuts.Count;
				for ( int i = 0; i < count; i++ )
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField( m_overallShortcuts[ i ].Name, m_labelStyleBold );
					EditorGUILayout.LabelField( m_overallShortcuts[ i ].Description, m_labelStyle );
					EditorGUILayout.EndHorizontal();
				}
			}
			else
			{
				EditorGUILayout.LabelField( "Please Open the ASE to get access to curren shortcuts" );
			}
		}

		private void OnDestroy()
		{
			m_portStyle = null;
			m_labelStyle = null;
			m_init = false;
		}
	}
}
