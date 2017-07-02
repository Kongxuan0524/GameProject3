// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEditor;
using UnityEngine;

namespace AmplifyShaderEditor
{
	[System.Serializable]
	public sealed class OutputPort : WirePort
	{
		[SerializeField]
		private bool m_connectedToMasterNode;

		[SerializeField]
		private bool m_isLocalValue;

		[SerializeField]
		private string m_localOutputValue;

		[SerializeField]
		private int m_isLocalWithPortType = 0;

		private RenderTexture m_outputPreview = null;

		private int m_indexPreviewOffset = 0;

		public OutputPort( int nodeId, int portId, WirePortDataType dataType, string name ) : base( nodeId, portId, dataType, name ) { LabelSize = Vector2.zero; }

		public bool ConnectedToMasterNode
		{
			get { return m_connectedToMasterNode; }
			set { m_connectedToMasterNode = value; }
		}

		public override void FullDeleteConnections()
		{
			UIUtils.DeleteConnection( false, m_nodeId, m_portId, true, true );
		}

		public override void NotifyExternalRefencesOnChange()
		{
			for ( int i = 0; i < m_externalReferences.Count; i++ )
			{
				ParentNode node = UIUtils.GetNode( m_externalReferences[ i ].NodeId );
				if ( node )
				{
					node.MarkForPreviewUpdate();
					InputPort port = node.GetInputPortByUniqueId( m_externalReferences[ i ].PortId );
					port.UpdateInfoOnExternalConn( m_nodeId, m_portId, m_dataType );
					port.UpdatedPreview = false;
					node.OnConnectedOutputNodeChanges( m_externalReferences[ i ].PortId, m_nodeId, m_portId, m_name, m_dataType );
				}
			}
		}

		public string ConfigOutputLocalValue( PrecisionType precisionType, string value, string customName = null , MasterNodePortCategory category = MasterNodePortCategory.Fragment )
		{
			m_localOutputValue = string.IsNullOrEmpty( customName ) ? ( "temp_output_" + m_nodeId + "_" + PortId ) : customName;
			m_isLocalValue = true;
			m_isLocalWithPortType |= ( int ) category;
			return string.Format( Constants.LocalValueDecWithoutIdent, UIUtils.PrecisionWirePortToCgType( precisionType, DataType ), m_localOutputValue, value );
		}

		public void SetLocalValue( string value, MasterNodePortCategory category = MasterNodePortCategory.Fragment )
		{
			m_isLocalValue = true;
			m_localOutputValue = value;
			m_isLocalWithPortType |= ( int ) category;
		}

		public void ResetLocalValue()
		{
			m_isLocalValue = false;
			m_localOutputValue = string.Empty;
			m_isLocalWithPortType = 0;
		}

		public bool IsLocalOnCategory( MasterNodePortCategory category )
		{
			return ( m_isLocalWithPortType & ( int ) category ) != 0; ;
		}

		public override void ForceClearConnection()
		{
			UIUtils.DeleteConnection( false, m_nodeId, m_portId, false, true );
		}

		public bool IsLocalValue { get { return m_isLocalValue; } }
		public int LocalWithPortType { get { return m_isLocalWithPortType; } }

		public string LocalValue { get { return m_localOutputValue; } }

		public RenderTexture OutputPreviewTexture
		{
			get { return m_outputPreview; }
			set { m_outputPreview = value; }
		}

		public int IndexPreviewOffset
		{
			get { return m_indexPreviewOffset; }
			set { m_indexPreviewOffset = value; }
		}

		public void GeneratePortPreview( int index, int pass )
		{
			if ( m_outputPreview == null || !m_outputPreview.IsCreated() )
				m_outputPreview = new RenderTexture( 128, 128, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear );

			if ( index + m_indexPreviewOffset == 0 )
			{
				//Debug.Log("output Texture");
				RenderTexture temp = RenderTexture.active;
				RenderTexture beforeMask = RenderTexture.GetTemporary( 128, 128, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear );
				RenderTexture.active = beforeMask;
				Graphics.Blit( null, beforeMask, UIUtils.CurrentWindow.PreviewMaterial, pass );

				Vector4 mask = Vector4.zero;

				switch ( DataType )
				{
					case WirePortDataType.INT:
					case WirePortDataType.FLOAT:
					mask.Set( 1, 1, 1, 1 );
					break;
					case WirePortDataType.FLOAT2:
					mask.Set( 1, 1, 0, 0 );
					break;
					case WirePortDataType.FLOAT3:
					mask.Set( 1, 1, 1, 0 );
					break;
					case WirePortDataType.COLOR:
					case WirePortDataType.FLOAT4:
					mask.Set( 1, 1, 1, 1 );
					break;
					default:
					mask.Set( 1, 1, 1, 1 );
					break;
				}

				if ( DataType == WirePortDataType.FLOAT3x3 || DataType == WirePortDataType.FLOAT4x4 )
				{
					RenderTexture.active = m_outputPreview;
					UIUtils.CurrentWindow.PreviewMaterial.SetTexture( "_MainTex", EditorGUIUtility.whiteTexture );
					UIUtils.CurrentWindow.PreviewMaterial.SetVector( "_Ports", mask );
					Graphics.Blit( null, m_outputPreview, UIUtils.CurrentWindow.PreviewMaterial, 85 );
				}
				else
				{
					RenderTexture.active = m_outputPreview;
					UIUtils.CurrentWindow.PreviewMaterial.SetTexture( "_MainTex", beforeMask );
					UIUtils.CurrentWindow.PreviewMaterial.SetVector( "_Ports", mask );
					Graphics.Blit( beforeMask, m_outputPreview, UIUtils.CurrentWindow.PreviewMaterial, 85 );
				}

				RenderTexture.ReleaseTemporary( beforeMask );
				RenderTexture.active = temp;
			}
			else
			{
				//Debug.Log( "output Texture not 0" );
				RenderTexture temp = RenderTexture.active;
				RenderTexture.active = m_outputPreview;

				if ( m_indexPreviewOffset > 0 )
				{
					UIUtils.CurrentWindow.PreviewMaterial.SetTexture( "_MaskTex", UIUtils.GetNode( NodeId ).InputPorts[ 0 ].InputPreviewTexture );
				}
				else
				{
					UIUtils.CurrentWindow.PreviewMaterial.SetTexture( "_MaskTex", UIUtils.GetNode( NodeId ).PreviewTexture );
				}

				UIUtils.CurrentWindow.PreviewMaterial.SetFloat( "_Port", index + m_indexPreviewOffset );

				Graphics.Blit( null, m_outputPreview, UIUtils.CurrentWindow.PreviewMaterial, 2 ); // 2 is the masking pass

				RenderTexture.active = temp;
			}



			NotifyExternalRefencesOnChange();
		}

		public override void Destroy()
		{
			base.Destroy();
			if ( m_outputPreview != null )
				UnityEngine.ScriptableObject.DestroyImmediate( m_outputPreview );
			m_outputPreview = null;
		}
	}
}
