// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using UnityEditor;
using System;

namespace AmplifyShaderEditor
{
	[Serializable]
	[NodeAttributes( "Texture Array", "Textures", "Texture Array" )]
	public class TextureArrayNode : PropertyNode
	{
		[SerializeField]
		private Texture2DArray m_defaultTextureArray;

		[SerializeField]
		private Texture2DArray m_materialTextureArray;

		[SerializeField]
		private TexReferenceType m_referenceType = TexReferenceType.Object;

		[SerializeField]
		private int m_uvSet = 0;

		[SerializeField]
		private MipType m_mipMode = MipType.Auto;

		private readonly string[] m_mipOptions = { "Auto", "Mip Level" };

		private TextureArrayNode m_referenceSampler = null;

		[SerializeField]
		private int m_referenceArrayId = -1;

		[SerializeField]
		private int m_referenceNodeId = -1;

		private readonly Color ReferenceHeaderColor = new Color( 2.67f, 1.0f, 0.5f, 1.0f );
		private bool m_forceSamplerUpdate = false;

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddOutputColorPorts( "RGBA" );
			AddInputPort( WirePortDataType.FLOAT2, false, "UV" );
			AddInputPort( WirePortDataType.FLOAT, false, "Index" );
			AddInputPort( WirePortDataType.FLOAT, false, "Level" );
			m_inputPorts[ 2 ].Visible = false;
			m_insideSize.Set( 110, 110 + 5 );
			m_drawPrecisionUI = false;
			m_currentParameterType = PropertyType.Property;
			m_freeType = false;
			m_customPrefix = "Texture Array ";
			//m_useCustomPrefix = true;
			m_precisionString = UIUtils.FinalPrecisionWirePortToCgType( m_currentPrecisionType, m_outputPorts[ 0 ].DataType );
		}

		protected override void OnUniqueIDAssigned()
		{
			base.OnUniqueIDAssigned();
			if ( m_referenceType == TexReferenceType.Object )
			{
				UIUtils.RegisterTextureArrayNode( this );
				UIUtils.RegisterPropertyNode( this );
			}
		}

		void ShowDefaults()
		{
			m_uvSet = EditorGUILayout.IntPopup( Constants.AvailableUVSetsLabel, m_uvSet, Constants.AvailableUVSetsStr, Constants.AvailableUVSets );

			MipType newMipMode = ( MipType )EditorGUILayout.Popup( "Mip Mode", ( int )m_mipMode, m_mipOptions );
			if ( newMipMode != m_mipMode )
			{
				m_mipMode = newMipMode;
			}

			if ( m_mipMode == MipType.MipLevel )
			{
				m_inputPorts[ 2 ].Visible = true;
			}
			else
			{
				m_inputPorts[ 2 ].Visible = false;
			}

			if ( !m_inputPorts[ 2 ].IsConnected && m_inputPorts[ 2 ].Visible )
			{
				m_inputPorts[ 2 ].FloatInternalData = EditorGUILayout.FloatField( "Mip Level", m_inputPorts[ 2 ].FloatInternalData );
			}

			if ( !m_inputPorts[ 1 ].IsConnected )
			{
				m_inputPorts[ 1 ].FloatInternalData = EditorGUILayout.FloatField( "Texture Index", m_inputPorts[ 1 ].FloatInternalData );
			}
		}

		public override void DrawMainPropertyBlock()
		{
			EditorGUI.BeginChangeCheck();
			m_referenceType = ( TexReferenceType )EditorGUILayout.EnumPopup( Constants.ReferenceTypeStr, m_referenceType );
			if ( EditorGUI.EndChangeCheck() )
			{
				if ( m_referenceType == TexReferenceType.Object )
				{
					UIUtils.RegisterTextureArrayNode( this );
					UIUtils.RegisterPropertyNode( this );

					SetTitleText( m_propertyInspectorName );
					SetAdditonalTitleText( string.Format( Constants.PropertyValueLabel, GetPropertyValStr() ) );
					m_referenceArrayId = -1;
					m_referenceNodeId = -1;
					m_referenceSampler = null;
				}
				else
				{
					UIUtils.UnregisterTextureArrayNode( this );
					UIUtils.UnregisterPropertyNode( this );
				}
				UpdateHeaderColor();
			}

			if ( m_referenceType == TexReferenceType.Object )
			{
				EditorGUI.BeginChangeCheck();
				base.DrawMainPropertyBlock();
				if ( EditorGUI.EndChangeCheck() )
				{
					OnPropertyNameChanged();
				}
			}
			else
			{
				string[] arr = UIUtils.TextureArrayNodeArr();
				bool guiEnabledBuffer = GUI.enabled;
				if ( arr != null && arr.Length > 0 )
				{
					GUI.enabled = true;
				}
				else
				{
					m_referenceArrayId = -1;
					GUI.enabled = false;
				}

				m_referenceArrayId = EditorGUILayout.Popup( Constants.AvailableReferenceStr, m_referenceArrayId, arr );
				GUI.enabled = guiEnabledBuffer;
				ShowDefaults();
			}
		}

		public override void OnPropertyNameChanged()
		{
			base.OnPropertyNameChanged();
			UIUtils.UpdateTextureArrayDataNode( m_uniqueId, PropertyInspectorName );
		}

		public override void DrawSubProperties()
		{
			ShowDefaults();

			EditorGUI.BeginChangeCheck();
			m_defaultTextureArray = ( Texture2DArray )EditorGUILayout.ObjectField( Constants.DefaultValueLabel, m_defaultTextureArray, typeof( Texture2DArray ), false );
			if ( EditorGUI.EndChangeCheck() )
			{
				SetAdditonalTitleText( string.Format( Constants.PropertyValueLabel, GetPropertyValStr() ) );
			}
		}

		public override void DrawMaterialProperties()
		{
			ShowDefaults();

			EditorGUI.BeginChangeCheck();
			m_materialTextureArray = ( Texture2DArray )EditorGUILayout.ObjectField( Constants.MaterialValueLabel, m_materialTextureArray, typeof( Texture2DArray ), false );
			if ( EditorGUI.EndChangeCheck() )
			{
				SetAdditonalTitleText( string.Format( Constants.PropertyValueLabel, GetPropertyValStr() ) );
				m_requireMaterialUpdate = true;
			}
		}

		void UpdateHeaderColor()
		{
			m_headerColorModifier = ( m_referenceType == TexReferenceType.Object ) ? Color.white : ReferenceHeaderColor;
		}

		public override void Draw( DrawInfo drawInfo )
		{
			EditorGUI.BeginChangeCheck();
			base.Draw( drawInfo );
			if ( m_forceSamplerUpdate )
			{
				m_forceSamplerUpdate = false;
				m_referenceSampler = UIUtils.GetNode( m_referenceNodeId ) as TextureArrayNode;
				m_referenceArrayId = UIUtils.GetTextureArrayNodeRegisterId( m_referenceNodeId );
			}
			if ( EditorGUI.EndChangeCheck() )
			{
				OnPropertyNameChanged();
			}

			bool instanced = CheckReference();

			if ( m_referenceType == TexReferenceType.Instance && m_referenceSampler != null )
			{
				SetTitleText( m_referenceSampler.PropertyInspectorName + Constants.InstancePostfixStr );
				SetAdditonalTitleText( m_referenceSampler.AdditonalTitleContent.text );
			}
			else
			{
				SetTitleText( PropertyInspectorName );
				SetAdditonalTitleText( AdditonalTitleContent.text );
			}

			Rect newRect = m_remainingBox;

			newRect.width = 110 * drawInfo.InvertedZoom;
			newRect.height = 110 * drawInfo.InvertedZoom;

			if ( instanced )
			{
				if ( GUI.Button( newRect, string.Empty, UIUtils.CustomStyle( CustomStyle.SamplerTextureRef ) ) )
				{
					UIUtils.FocusOnNode( m_referenceSampler, 1, true );
				}
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				if ( m_materialMode )
					m_materialTextureArray = ( Texture2DArray )EditorGUI.ObjectField( newRect, m_materialTextureArray, typeof( Texture2DArray ), false );
				else
					m_defaultTextureArray = ( Texture2DArray )EditorGUI.ObjectField( newRect, m_defaultTextureArray, typeof( Texture2DArray ), false );

				if ( EditorGUI.EndChangeCheck() )
				{
					SetAdditonalTitleText( string.Format( Constants.PropertyValueLabel, GetPropertyValStr() ) );
					BeginDelayedDirtyProperty();
					m_requireMaterialUpdate = true;
				}
			}

		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalvar )
		{
			OnPropertyNameChanged();

			CheckReference();

			bool instanced = false;
			if ( m_referenceType == TexReferenceType.Instance && m_referenceSampler != null )
				instanced = true;

			if ( !instanced )
				base.GenerateShaderForOutput( outputId, ref dataCollector, ignoreLocalvar );

			string level = string.Empty;
			if ( InputPorts[ 2 ].Visible )
			{
				level = InputPorts[ 2 ].GeneratePortInstructions( ref dataCollector );
			}


			string uvs = string.Empty;
			if ( InputPorts[ 0 ].IsConnected )
			{
				uvs = InputPorts[ 0 ].GeneratePortInstructions( ref dataCollector );
			}
			else
			{
				uvs = TexCoordVertexDataNode.GenerateFragUVs( ref dataCollector, m_uniqueId, m_uvSet, ( instanced ? m_referenceSampler.PropertyName : PropertyName ) );
			}
			string index = InputPorts[ 1 ].GeneratePortInstructions( ref dataCollector );

			int connectionNumber = 0;
			for ( int i = 0; i < m_outputPorts.Count; i++ )
			{
				connectionNumber += m_outputPorts[ i ].ConnectionCount;
			}

			string propertyName = string.Empty;
			if ( !instanced )
				propertyName = PropertyName;
			else
				propertyName = m_referenceSampler.PropertyName;

			string result = "UNITY_SAMPLE_TEX2DARRAY" + ( InputPorts[ 2 ].Visible ? "_LOD" : "" ) + "(" + propertyName + ", float3(" + uvs + ", " + index + ") " + ( InputPorts[ 2 ].Visible ? ", " + level : "" ) + " )";
			if ( connectionNumber > 1 )
			{
				dataCollector.AddToLocalVariables( m_uniqueId, "float4 texArray" + m_uniqueId + " = " + result + ";" );
				return GetOutputVectorItem( 0, outputId, "texArray" + m_uniqueId );
			}
			else
			{

				return GetOutputVectorItem( 0, outputId, result );
			}
		}

		public override string GetPropertyValue()
		{
			return PropertyAttributes + m_propertyName + "(\"" + m_propertyInspectorName + "\", 2DArray ) = \"\" {}";
		}

		public override void GetUniformData( out string dataType, out string dataName )
		{
			dataType = "UNITY_DECLARE_TEX2DARRAY(";
			dataName = m_propertyName + " )";
		}

		public override void ReadFromString( ref string[] nodeParams )
		{
			base.ReadFromString( ref nodeParams );
			string textureName = GetCurrentParam( ref nodeParams );
			m_defaultTextureArray = AssetDatabase.LoadAssetAtPath<Texture2DArray>( textureName );
			m_uvSet = Convert.ToInt32( GetCurrentParam( ref nodeParams ) );
			m_referenceType = ( TexReferenceType )Enum.Parse( typeof( TexReferenceType ), GetCurrentParam( ref nodeParams ) );
			m_referenceNodeId = Convert.ToInt32( GetCurrentParam( ref nodeParams ) );
			if ( UIUtils.CurrentShaderVersion() > 3202 )
				m_mipMode = ( MipType )Enum.Parse( typeof( MipType ), GetCurrentParam( ref nodeParams ) );

			if ( m_referenceType == TexReferenceType.Instance )
			{
				UIUtils.UnregisterTextureArrayNode( this );
				UIUtils.UnregisterPropertyNode( this );
				m_forceSamplerUpdate = true;
			}

			UpdateHeaderColor();

			if ( m_defaultTextureArray )
			{
				m_materialTextureArray = m_defaultTextureArray;
			}
		}

		public override void WriteToString( ref string nodeInfo, ref string connectionsInfo )
		{
			base.WriteToString( ref nodeInfo, ref connectionsInfo );
			IOUtils.AddFieldValueToString( ref nodeInfo, ( m_defaultTextureArray != null ) ? AssetDatabase.GetAssetPath( m_defaultTextureArray ) : Constants.NoStringValue );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_uvSet.ToString() );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_referenceType );
			IOUtils.AddFieldValueToString( ref nodeInfo, ( ( m_referenceSampler != null ) ? m_referenceSampler.UniqueId : -1 ) );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_mipMode );
		}

		public override void UpdateMaterial( Material mat )
		{
			base.UpdateMaterial( mat );
			if ( UIUtils.IsProperty( m_currentParameterType ) )
			{
				OnPropertyNameChanged();
				if ( mat.HasProperty( PropertyName ) )
				{
					mat.SetTexture( PropertyName, m_materialTextureArray );
				}
			}
		}

		public override void SetMaterialMode( Material mat )
		{
			base.SetMaterialMode( mat );
			if ( m_materialMode && UIUtils.IsProperty( m_currentParameterType ) )
			{
				if ( mat.HasProperty( PropertyName ) )
				{
					m_materialTextureArray = ( Texture2DArray )mat.GetTexture( PropertyName );
					if ( m_materialTextureArray == null )
						m_materialTextureArray = m_defaultTextureArray;
				}
			}
		}

		public override void ForceUpdateFromMaterial( Material material )
		{
			if ( UIUtils.IsProperty( m_currentParameterType ) && material.HasProperty( PropertyName ) )
			{
				m_materialTextureArray = ( Texture2DArray )material.GetTexture( PropertyName );
				if ( m_materialTextureArray == null )
					m_materialTextureArray = m_defaultTextureArray;
			}
		}

		public override bool UpdateShaderDefaults( ref Shader shader, ref TextureDefaultsDataColector defaultCol )
		{
			if ( m_defaultTextureArray != null )
			{
				defaultCol.AddValue( PropertyName, m_defaultTextureArray );
			}

			return true;
		}

		public override string GetPropertyValStr()
		{
			return m_materialMode ? ( m_materialTextureArray != null ? m_materialTextureArray.name : IOUtils.NO_TEXTURES ) : ( m_defaultTextureArray != null ? m_defaultTextureArray.name : IOUtils.NO_TEXTURES );
		}

		public bool CheckReference()
		{
			if ( m_referenceType == TexReferenceType.Instance && m_referenceArrayId > -1 )
			{
				m_referenceSampler = UIUtils.GetTextureArrayNode( m_referenceArrayId );

				if ( m_referenceSampler == null )
					m_referenceArrayId = -1;
			}

			return m_referenceSampler != null;
		}

		public Texture2DArray TextureArray
		{
			get { return ( m_materialMode ? m_materialTextureArray : m_defaultTextureArray ); }
		}

		public override string DataToArray { get { return PropertyInspectorName; } }

		public override void Destroy()
		{
			base.Destroy();
			m_defaultTextureArray = null;
			m_materialTextureArray = null;

			if ( m_referenceType == TexReferenceType.Object )
			{
				UIUtils.UnregisterTextureArrayNode( this );
				UIUtils.UnregisterPropertyNode( this );
			}
		}
	}
}
