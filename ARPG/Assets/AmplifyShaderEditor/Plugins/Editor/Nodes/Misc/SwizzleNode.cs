// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
//
// Custom Node Swizzle 
// Donated by Tobias Pott - @ Tobias Pott
// www.tobiaspott.de

using System;
using UnityEditor;
using UnityEngine;

namespace AmplifyShaderEditor
{
    [Serializable]
    [NodeAttributes("Swizzle", "Misc", "swizzle components of vector types ")]
    public sealed class SwizzleNode : SingleInputOp
    {
        public enum SwizzleComponent
        {
            X = 0,
            Y = 1,
            Z = 2,
            W = 3
        }

        private const string OutputTypeStr = "Output type";

        [SerializeField]
        private WirePortDataType _selectedOutputType = WirePortDataType.FLOAT4;

        [SerializeField]
        private int _selectedOutputTypeInt = 4;
        [SerializeField]
        private SwizzleComponent[] _selectedOutputSwizzleTypes = new SwizzleComponent[] { SwizzleComponent.X, SwizzleComponent.Y, SwizzleComponent.Z, SwizzleComponent.W };

        private readonly string[] _outputValueTypes ={  "Float",
                                                        "Vector2",
                                                        "Vector3",
                                                        "Vector4",
                                                        "Color"};

        protected override void CommonInit(int uniqueId)
        {
            base.CommonInit(uniqueId);
			m_inputPorts[ 0 ].CreatePortRestrictions(	WirePortDataType.FLOAT,
														WirePortDataType.FLOAT2, 
														WirePortDataType.FLOAT3, 
														WirePortDataType.FLOAT4, 
														WirePortDataType.COLOR, 
														WirePortDataType.INT);


            m_inputPorts[0].DataType = WirePortDataType.FLOAT4;
            m_outputPorts[0].DataType = _selectedOutputType; // (_selectedOutputType, Constants.EmptyPortValue);
            m_textLabelWidth = 90;
			m_autoWrapProperties = true;
		}

        public override void DrawProperties()
        {

            EditorGUILayout.BeginVertical();
            EditorGUI.BeginChangeCheck();
            _selectedOutputTypeInt = EditorGUILayout.Popup(OutputTypeStr, _selectedOutputTypeInt, _outputValueTypes);
            if (EditorGUI.EndChangeCheck())
            {
                switch (_selectedOutputTypeInt)
                {
                    case 0: _selectedOutputType = WirePortDataType.FLOAT; break;
                    case 1: _selectedOutputType = WirePortDataType.FLOAT2; break;
                    case 2: _selectedOutputType = WirePortDataType.FLOAT3; break;
                    case 3: _selectedOutputType = WirePortDataType.FLOAT4; break;
                    case 4: _selectedOutputType = WirePortDataType.COLOR; break;
                }

                UpdatePorts();
            }
            EditorGUILayout.EndVertical();

            // Draw base properties
            base.DrawProperties();

            EditorGUILayout.BeginVertical();

            int count = 0;
            switch (_selectedOutputType)
            {
                case WirePortDataType.FLOAT4:
                case WirePortDataType.COLOR:
                    count = 4;
                    break;
                case WirePortDataType.FLOAT3:
                    count = 3;
                    break;
                case WirePortDataType.FLOAT2:
                    count = 2;
                    break;
                case WirePortDataType.FLOAT:
                    count = 1;
                    break;
                case WirePortDataType.OBJECT:
                case WirePortDataType.INT:
                case WirePortDataType.FLOAT3x3:
                case WirePortDataType.FLOAT4x4:
                    break;
            }

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < count; i++)
                _selectedOutputSwizzleTypes[i] = (SwizzleComponent)EditorGUILayout.EnumPopup(_selectedOutputSwizzleTypes[i]);
            if (EditorGUI.EndChangeCheck())
            {
                UpdatePorts();
            }

            EditorGUILayout.EndVertical();
        }
        void UpdatePorts()
        {
            m_sizeIsDirty = true;
            ChangeOutputType(_selectedOutputType, false);
            switch (_selectedOutputType)
            {
                case WirePortDataType.COLOR:
                case WirePortDataType.FLOAT4:
                case WirePortDataType.FLOAT3:
                case WirePortDataType.FLOAT2:
                case WirePortDataType.FLOAT:
                case WirePortDataType.INT:
                case WirePortDataType.OBJECT:
                case WirePortDataType.FLOAT3x3:
                case WirePortDataType.FLOAT4x4:
                    { }
                    break;
            }
        }
        public override string GenerateShaderForOutput(int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalVar)
        {
            string value = string.Empty;

            string inName = m_inputPorts[0].IsConnected ? m_inputPorts[0].GenerateShaderForOutput(ref dataCollector, m_inputPorts[ 0 ].DataType, ignoreLocalVar) : string.Empty;
            Vector4 defaultValue = InputPorts[0].Vector4InternalData;
            bool useInput = !inName.Equals(string.Empty);

            switch (_selectedOutputType)
            {
                case WirePortDataType.OBJECT:
                case WirePortDataType.FLOAT4:
                case WirePortDataType.COLOR:
                    {
                        value = "float4( ";
                        for (int i = 0; i < 4; i++)
                        {
                            if (!useInput) value += defaultValue[(int)_selectedOutputSwizzleTypes[i]].ToString();
                            else value += inName + "." + _selectedOutputSwizzleTypes[i].ToString().ToLower();

                            if (i != 3)
                                value += " , ";
                        }
                        value += " )";
                    }
                    break;
                case WirePortDataType.FLOAT3:
                    {
                        value = "float3( ";
                        for (int i = 0; i < 3; i++)
                        {
                            if (!useInput) value += defaultValue[(int)_selectedOutputSwizzleTypes[i]].ToString();
                            else value += inName + "." + _selectedOutputSwizzleTypes[i].ToString().ToLower();

                            if (i != 2)
                                value += " , ";
                        }
                        value += " )";
                    }
                    break;
                case WirePortDataType.FLOAT2:
                    {
                        value = "float2( ";
                        for (int i = 0; i < 2; i++)
                        {
                            if (!useInput) value += defaultValue[(int)_selectedOutputSwizzleTypes[i]].ToString();
                            else value += inName + "." + _selectedOutputSwizzleTypes[i].ToString().ToLower();

                            if (i != 1)
                                value += " , ";
                        }
                        value += " )";
                    }
                    break;
                case WirePortDataType.FLOAT:
                    {
                        if (!useInput) value = defaultValue[(int)_selectedOutputSwizzleTypes[0]].ToString();
                        else value = inName + "." + _selectedOutputSwizzleTypes[0].ToString().ToLower();
                    }
                    break;
                case WirePortDataType.INT:
                case WirePortDataType.FLOAT3x3:
                case WirePortDataType.FLOAT4x4:
                    { }
                    break;
            }
            return CreateOutputLocalVariable(0, value, ref dataCollector);
        }

        public override void ReadFromString(ref string[] nodeParams)
        {
            base.ReadFromString(ref nodeParams);
            _selectedOutputType = (WirePortDataType)Enum.Parse(typeof(WirePortDataType), GetCurrentParam(ref nodeParams));
            switch (_selectedOutputType)
            {
                case WirePortDataType.FLOAT: _selectedOutputTypeInt = 0; break;
                case WirePortDataType.FLOAT2: _selectedOutputTypeInt = 1; break;
                case WirePortDataType.FLOAT3: _selectedOutputTypeInt = 2; break;
                case WirePortDataType.FLOAT4: _selectedOutputTypeInt = 3; break;
                case WirePortDataType.COLOR: _selectedOutputTypeInt = 4; break;
            }
            for (int i = 0; i < _selectedOutputSwizzleTypes.Length; i++)
            {
                _selectedOutputSwizzleTypes[i] = (SwizzleComponent)Convert.ToInt32(GetCurrentParam(ref nodeParams));
            }


            UpdatePorts();
        }

        public override void WriteToString(ref string nodeInfo, ref string connectionsInfo)
        {
            base.WriteToString(ref nodeInfo, ref connectionsInfo);
            IOUtils.AddFieldValueToString(ref nodeInfo, _selectedOutputType);
            for (int i = 0; i < _selectedOutputSwizzleTypes.Length; i++)
            {
                IOUtils.AddFieldValueToString(ref nodeInfo, (int)_selectedOutputSwizzleTypes[i]);
            }
        }
    }
}
