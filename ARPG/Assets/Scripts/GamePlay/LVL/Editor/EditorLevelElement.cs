using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LVL
{
    [CustomEditor(typeof(LevelElement), true)]
    public class EditorLevelElement : Editor
    {
        public override void OnInspectorGUI()
        {
            LevelElement elem = target as LevelElement;
            if(elem.AutoDrawInspector())
            {
                int id = UnityEditor.EditorGUILayout.IntField("Id", elem.Id);
                if (id != elem.Id)
                {
                    elem.Id = id;
                    elem.SetName();
                }
                base.OnInspectorGUI();
                elem.DrawGUI();
            }
            else
            {
                elem.DrawGUI();
            }  
        }
    }
}