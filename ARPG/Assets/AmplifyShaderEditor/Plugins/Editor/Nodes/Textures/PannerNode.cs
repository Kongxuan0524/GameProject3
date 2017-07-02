// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using System;
using UnityEditor;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Panner", "Textures", "Pans UV texture coordinates according to its inputs" )]
	public sealed class PannerNode : ParentNode
	{
		private const string _speedXStr = "Speed X";
		private const string _speedYStr = "Speed Y";
		[SerializeField]
		private float m_speedX = 1f;

		[SerializeField]
		private float m_speedY = 1f;

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT2, false, "UV" );
			AddInputPort( WirePortDataType.FLOAT, false, "Time" );
			AddOutputPort( WirePortDataType.FLOAT2, "Out" );
			m_textLabelWidth = 70;
			m_autoWrapProperties = true;
			m_previewId = 14;
			m_inputPorts[ 0 ].PreviewSamplerName = "_UVs";
			m_inputPorts[ 1 ].PreviewSamplerName = "_PanTime";
		}

		public override void DrawProperties()
		{
			base.DrawProperties();

			m_speedX = EditorGUILayout.FloatField( _speedXStr, m_speedX );
			m_speedY = EditorGUILayout.FloatField( _speedYStr, m_speedY );
		}

		public override void SetPreviewInputs()
		{
			base.SetPreviewInputs();

			if ( m_inputPorts[ 1 ].IsConnected )
			{
				UIUtils.CurrentWindow.PreviewMaterial.SetFloat( "_UsingEditor", 0 );
				m_inputPorts[ 1 ].PreviewSamplerName = "_PanTime";
			}
			else
			{
				UIUtils.CurrentWindow.PreviewMaterial.SetFloat( "_UsingEditor", 1 );
				m_inputPorts[ 1 ].PreviewSamplerName = string.Empty;
			}

			UIUtils.CurrentWindow.PreviewMaterial.SetFloat( "_SpeedX", m_speedX );
			UIUtils.CurrentWindow.PreviewMaterial.SetFloat( "_SpeedY", m_speedY );
		}

		public override void AfterPreviewRefresh()
		{
			base.AfterPreviewRefresh();
			MarkForPreviewUpdate();
			m_inputPorts[ 1 ].UpdatedPreview = false;
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			string timePort = string.Empty;
			if ( m_inputPorts[ 1 ].IsConnected )
			{
				timePort = m_inputPorts[ 1 ].GeneratePortInstructions( ref dataCollector );
			}
			else
			{
				dataCollector.AddToIncludes( m_uniqueId, Constants.UnityShaderVariables );
				timePort = "_Time[1]";
			}

			string result = "(abs( " + m_inputPorts[ 0 ].GeneratePortInstructions( ref dataCollector )+ "+" + timePort + " * float2(" + m_speedX + "," + m_speedY + " )))";
			return CreateOutputLocalVariable( 0, result, ref dataCollector );
		}

		public override void ReadFromString( ref string[] nodeParams )
		{
			base.ReadFromString( ref nodeParams );
			m_speedX = Convert.ToSingle( GetCurrentParam( ref nodeParams ) );
			m_speedY = Convert.ToSingle( GetCurrentParam( ref nodeParams ) );
		}
		public override void WriteToString( ref string nodeInfo, ref string connectionsInfo )
		{
			base.WriteToString( ref nodeInfo, ref connectionsInfo );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_speedX );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_speedY );
		}
	}
}
