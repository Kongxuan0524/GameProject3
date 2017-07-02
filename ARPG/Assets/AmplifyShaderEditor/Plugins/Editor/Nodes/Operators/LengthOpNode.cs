// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

namespace AmplifyShaderEditor
{
	[System.Serializable]
	[NodeAttributes( "Length", "Operators", "Scalar Euclidean length of a vector" )]
	public sealed class LengthOpNode : SingleInputOp
	{
		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			m_opName = "length";
			m_previewId = 42;
			m_inputPorts[ 0 ].PreviewSamplerName = "_A";
			m_inputPorts[ 0 ].CreatePortRestrictions(	WirePortDataType.OBJECT,
														WirePortDataType.FLOAT ,
														WirePortDataType.FLOAT2,
														WirePortDataType.FLOAT3,
														WirePortDataType.FLOAT4,
														WirePortDataType.COLOR ,
														WirePortDataType.INT);
			m_inputPorts[ 0 ].ChangeType( WirePortDataType.FLOAT4, false );
			m_autoUpdateOutputPort = false;
		}
	}
}
