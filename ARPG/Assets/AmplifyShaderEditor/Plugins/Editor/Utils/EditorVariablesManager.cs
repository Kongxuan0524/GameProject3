// Amplify Shader Editor - Advanced Bloom Post-Effect for Unity
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEditor;

namespace AmplifyShaderEditor
{
	public class EditorVariable<T>
	{
		protected string m_labelName;
		protected string m_name;
		protected T m_value;

		public EditorVariable( string name, string labelName ) { m_name = name; m_labelName = labelName; }
		public string Name { get { return m_name; } }

		public virtual T Value
		{
			get { return m_value; }
			set
			{
				m_value = value;
			}
		}
		public string LabelName { get { return m_labelName; } }
	}

	public sealed class EditorVariableFloat : EditorVariable<float>
	{
		public EditorVariableFloat( string name, string labelName ) : base( name, labelName )
		{
			m_value = EditorPrefs.GetFloat( name );
		}

		public override float Value
		{
			get { return m_value; }
			set
			{
				if ( m_value != value )
				{
					m_value = value;
					EditorPrefs.SetFloat( m_name, m_value );
				}
			}
		}
	}

	public sealed class EditorVariableBool : EditorVariable<bool>
	{
		public EditorVariableBool( string name, string labelName ) : base( name, labelName )
		{
			m_value = EditorPrefs.GetBool( name );
		}

		public override bool Value
		{
			get { return m_value; }
			set
			{
				if ( m_value != value )
				{
					m_value = value;
					EditorPrefs.SetBool( m_name, m_value );
				}
			}
		}
	}

	public sealed class EditorVariableInt : EditorVariable<int>
	{
		public EditorVariableInt( string name, string labelName ) : base( name, labelName )
		{
			m_value = EditorPrefs.GetInt( name );
		}

		public override int Value
		{
			get { return m_value; }
			set
			{
				if ( m_value != value )
				{
					m_value = value;
					EditorPrefs.SetInt( m_name, m_value );
				}
			}
		}
	}

	public sealed class EditorVariableString : EditorVariable<string>
	{
		public EditorVariableString( string name, string labelName ) : base( name, labelName )
		{
			m_value = EditorPrefs.GetString( name );
		}

		public override string Value
		{
			get { return m_value; }
			set
			{
				if ( !m_value.Equals( value ) )
				{
					m_value = value;
					EditorPrefs.SetString( m_name, m_value );
				}
			}
		}
	}

	public class EditorVariablesManager
	{
		public static EditorVariableBool LiveMode = new EditorVariableBool( "ASELiveMode", "LiveMode" );
		public static EditorVariableBool OutlineActiveMode = new EditorVariableBool( "ASEOutlineActiveMode", " Outline" );
	}
}
