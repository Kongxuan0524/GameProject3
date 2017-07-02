// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using System;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Clamp", "Operators", "Value clamped to the range [min,max]" )]
	public sealed class ClampOpNode : ParentNode
	{

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT, false, "Value" );
			AddInputPort( WirePortDataType.FLOAT, false, "Min" );
			AddInputPort( WirePortDataType.FLOAT, false, "Max" );
			AddOutputPort( WirePortDataType.FLOAT, Constants.EmptyPortValue );
			m_useInternalPortData = true;
			m_textLabelWidth = 55;
			m_previewId = 32;
			m_inputPorts[ 0 ].PreviewSamplerName = "_Value";
			m_inputPorts[ 1 ].PreviewSamplerName = "_Min";
			m_inputPorts[ 2 ].PreviewSamplerName = "_Max";
		}

		public override void OnInputPortConnected( int portId, int otherNodeId, int otherPortId, bool activateNode = true )
		{
			base.OnInputPortConnected( portId, otherNodeId, otherPortId, activateNode );
			if ( portId == 0 )
			{
				m_inputPorts[ 0 ].MatchPortToConnection();
				m_inputPorts[ 1 ].ChangeType( m_inputPorts[ 0 ].DataType, false );
				m_inputPorts[ 2 ].ChangeType( m_inputPorts[ 0 ].DataType, false );

				m_outputPorts[ 0 ].ChangeType( m_inputPorts[ 0 ].DataType, false );
			}
			//else
			//{
			//	_inputPorts[ portId ].MatchPortToConnection();
			//}
		}

		public override void OnConnectedOutputNodeChanges( int outputPortId, int otherNodeId, int otherPortId, string name, WirePortDataType type )
		{
			base.OnConnectedOutputNodeChanges( outputPortId, otherNodeId, otherPortId, name, type );
			if ( outputPortId == 0 )
			{
				m_inputPorts[ 0 ].MatchPortToConnection();
				m_inputPorts[ 1 ].ChangeType( m_inputPorts[ 0 ].DataType, false );
				m_inputPorts[ 2 ].ChangeType( m_inputPorts[ 0 ].DataType, false );
				m_outputPorts[ 0 ].ChangeType( m_inputPorts[ 0 ].DataType, false );
			}
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalVar )
		{
			WirePortDataType valueType = m_inputPorts[ 0 ].ConnectionType();
			WirePortDataType minType = m_inputPorts[ 1 ].ConnectionType();
			WirePortDataType maxType = m_inputPorts[ 2 ].ConnectionType();

			string value = m_inputPorts[ 0 ].GeneratePortInstructions( ref dataCollector );
			string min = m_inputPorts[ 1 ].GeneratePortInstructions( ref dataCollector );
			if ( minType != valueType )
			{
				min = UIUtils.CastPortType( dataCollector.PortCategory, m_currentPrecisionType, new NodeCastInfo( m_uniqueId, outputId ), null, m_inputPorts[ 1 ].DataType, m_inputPorts[ 0 ].DataType, min );
			}

			string max = m_inputPorts[ 2 ].GeneratePortInstructions( ref dataCollector );
			if ( maxType != valueType )
			{
				max = UIUtils.CastPortType( dataCollector.PortCategory, m_currentPrecisionType, new NodeCastInfo( m_uniqueId, outputId ), null, m_inputPorts[ 2 ].DataType, m_inputPorts[ 0 ].DataType, max );
			}

			string result = string.Empty;
			switch ( valueType )
			{
				case WirePortDataType.FLOAT:
				case WirePortDataType.FLOAT2:
				case WirePortDataType.FLOAT3:
				case WirePortDataType.FLOAT4:
				case WirePortDataType.INT:
				case WirePortDataType.COLOR:
				case WirePortDataType.OBJECT:
				{
					result =  "clamp( " + value + " , " + min + " , " + max + " )";
				}break;
				case WirePortDataType.FLOAT3x3:
				case WirePortDataType.FLOAT4x4:
				{
					result = UIUtils.InvalidParameter(this);
				} break;
			}

			return CreateOutputLocalVariable( 0, result, ref dataCollector );
		}

	}
}
