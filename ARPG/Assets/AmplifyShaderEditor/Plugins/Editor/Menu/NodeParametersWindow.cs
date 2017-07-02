// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using UnityEditor;

namespace AmplifyShaderEditor
{
	public sealed class NodeParametersWindow : MenuParent
	{
		private int m_lastSelectedNode = -1;
		private Vector2 m_currentScrollPos;
		private const string TitleStr = "Node Properties";
		private GUIStyle m_nodePropertiesStyle;

		// width and height are between [0,1] and represent a percentage of the total screen area
		public NodeParametersWindow() : base( 0, 0, 250, 0, string.Empty, MenuAnchor.TOP_LEFT, MenuAutoSize.MATCH_VERTICAL )
		{
			m_currentScrollPos = Vector2.zero;
			SetMinimizedArea( -225, 0, 260, 0 );
		}

		public bool Draw( Rect parentPosition, ParentNode selectedNode, Vector2 mousePosition, int mouseButtonId )
		{
			bool changeCheck = false;
			base.Draw( parentPosition, mousePosition, mouseButtonId );
			if ( m_nodePropertiesStyle == null )
			{
				m_nodePropertiesStyle = UIUtils.CustomStyle( CustomStyle.NodePropertiesTitle );
				m_nodePropertiesStyle.normal.textColor = m_nodePropertiesStyle.active.textColor = EditorGUIUtility.isProSkin ? new Color( 1f, 1f, 1f ) : new Color( 0f, 0f, 0f );
			}

			if ( m_isMaximized )
			{
				GUILayout.BeginArea( m_transformedArea, m_content, m_style );
				{
					//Draw selected node parameters
					if ( selectedNode != null )
					{
						// this hack is need because without it the several FloatFields/Textfields/... would show wrong values ( different from the ones they were assigned to show )
						if ( m_lastSelectedNode != selectedNode.UniqueId )
						{
							m_lastSelectedNode = selectedNode.UniqueId;
							GUI.FocusControl( "" );
						}

						EditorGUILayout.BeginVertical();
						{
							EditorGUILayout.Separator();
							EditorGUILayout.LabelField( TitleStr, m_nodePropertiesStyle );
							EditorGUILayout.Separator();
							UIUtils.RecordObject( selectedNode );
							m_currentScrollPos = EditorGUILayout.BeginScrollView( m_currentScrollPos, GUILayout.Width( 0 ), GUILayout.Height( 0 ) );
							float labelWidth = EditorGUIUtility.labelWidth;
							if ( selectedNode.TextLabelWidth > 0 )
								EditorGUIUtility.labelWidth = selectedNode.TextLabelWidth;

							changeCheck = selectedNode.SafeDrawProperties();
							EditorGUIUtility.labelWidth = labelWidth;
							EditorGUILayout.EndScrollView();
						}
						EditorGUILayout.EndVertical();

						if ( changeCheck )
						{
							if ( selectedNode.ConnStatus == NodeConnectionStatus.Connected )
								UIUtils.CurrentWindow.SetSaveIsDirty();
						}
					}
				}
				// Close window area
				GUILayout.EndArea();
			}
			
			PostDraw();
			return changeCheck;
		}
	}
}
