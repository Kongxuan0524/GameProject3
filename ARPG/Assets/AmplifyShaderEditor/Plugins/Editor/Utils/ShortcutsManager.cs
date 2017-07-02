// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{
	public class ShortcutItem
	{
		public delegate void ShortcutFunction();
		public ShortcutFunction MyFunctionPtr;
		public string Name;
		public string Description;

		public ShortcutItem( string name, string description, ShortcutFunction myFunctionPtr )
		{
			Name = name;
			Description = description;
			MyFunctionPtr = myFunctionPtr;
		}
	}

	public class ShortcutsManager
	{
		private Dictionary<KeyCode, ShortcutItem> m_items = new Dictionary<KeyCode, ShortcutItem>();
		private List<ShortcutItem> m_itemsList = new List<ShortcutItem>();

		public void RegisterShortcut( KeyCode key, string description, ShortcutItem.ShortcutFunction myFunctionPtr )
		{
			if ( m_items.ContainsKey( key ) )
			{
				if ( DebugConsoleWindow.DeveloperMode )
				{
					Debug.Log( "Attempting to register an already used shortcut key " + key );
				}
				return;
			}

			m_items.Add( key, new ShortcutItem( key.ToString(), description, myFunctionPtr ) );
			m_itemsList.Add( m_items[ key ] );
		}

		public void ActivateShortcut( KeyCode key )
		{
			if ( m_items.ContainsKey( key ) )
			{
				m_items[ key ].MyFunctionPtr();
			}
		}

		public void Destroy()
		{
			foreach ( KeyValuePair<KeyCode, ShortcutItem> kvp in m_items )
			{
				kvp.Value.MyFunctionPtr = null;
			}
			m_items.Clear();
			m_items = null;

			m_itemsList.Clear();
			m_itemsList = null;
		}

		public Dictionary<KeyCode, ShortcutItem> AvailableShortcuts { get { return m_items; } }
		public List<ShortcutItem> AvailableShortcutsList { get { return m_itemsList; } }
	}
}
