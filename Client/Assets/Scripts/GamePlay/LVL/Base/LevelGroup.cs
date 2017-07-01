using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LVL
{
    public class LevelGroup<T> : LevelGroupBase, ILevelGroup where T : LevelElement
    {
        private List<T>  m_Elements = new List<T>();

        public List<T>   Elements
        {
            get
            {
                m_Elements.Clear();
                GTTools.GetAllComponents<T>(transform, m_Elements);
                return m_Elements;
            }
        }

        public string    GroupName
        {
            get { return typeof(T).Name; }
        }

        public virtual T AddElement()
        {
            T pElem = new GameObject().AddComponent<T>();
            pElem.transform.parent = transform;
            pElem.LvlSystem = LvlSystem;
            return pElem;
        }

        public virtual T DelElement(T item)
        {
            if (item != null)
            {
                GameObject.DestroyImmediate(item.gameObject);
            }
            return null;
        }

        public virtual T FindElement(int id)
        {
            m_Elements.Clear();
            GTTools.GetAllComponents<T>(transform, m_Elements);
            for (int i = 0; i < m_Elements.Count; i++)
            {
                if (m_Elements[i].Id == id)
                {
                    return m_Elements[i];
                }
            }
            return null;
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            base.DrawGUI();
            GUI.color = Color.green;
            GUILayout.Space(10);
            if (GUILayout.Button("Add " + GroupName, GUILayout.Height(30)))
            {
                T element = this.AddElement();
                element.DrawScene();
                element.SetName();
            }
            GUI.color = Color.white;
            GUILayout.Space(5);
            List<T> list = Elements;
            for (int i = 0; i < list.Count; i++)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                UnityEditor.EditorGUILayout.LabelField(list[i].name);
                GUI.color = Color.green;
                if (GUILayout.Button("G", GUILayout.Width(40)))
                {
                    UnityEditor.Selection.activeObject = list[i];
                }
                GUI.color = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(40)))
                {
                    this.DelElement(list[i]);
                }
                GUI.color = Color.white;
                UnityEditor.EditorGUILayout.EndHorizontal();
            }
#endif
        }
    }
}

