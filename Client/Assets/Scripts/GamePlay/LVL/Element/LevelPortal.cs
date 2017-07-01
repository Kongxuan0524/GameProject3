using UnityEngine;
using System.Collections;
using CFG;
using BVT.AI;

namespace LVL
{
    public class LevelPortal : LevelElement
    {
        [LevelVariable]
        public string    Name = string.Empty;
        [LevelVariable]
        public int       RegionID;
        [LevelVariable]
        public int       DestMapID;
        [LevelVariable]
        public Vector3   DestPos;
        [LevelVariable]
        public bool      DisplayText;
        [LevelVariable]
        public ELvlCR       CR;
        [LevelVariable]
        public int       OpenLevel;
        [LevelVariable]
        public int       OpenItemID;
        [LevelVariable]
        public int       OpenVIP;

        private LevelRegion m_Region;


        public override void Startup()
        {
  
        }

        public override void Trigger()
        {
            this.DrawScene();
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Import(DCFG cfg)
        {
            LvlPortal data   = cfg as LvlPortal;
            this.Id          = data.Id;
            this.Name        = data.Name;
            this.RegionID    = data.RegionID;
            this.DestMapID   = data.DestMapID;
            this.DestPos     = data.DestPos;
            this.DisplayText = data.DisplayText;
            this.CR          = data.CR;
            this.OpenItemID  = data.OpenItemID;
            this.OpenLevel   = data.OpenLevel;
            this.OpenVIP     = data.OpenVIP;
        }

        public override DCFG Export()
        {
            LvlPortal data   = new LvlPortal();
            data.Id          = this.Id;
            data.Name        = this.Name;
            data.RegionID    = this.RegionID;
            data.DestMapID   = this.DestMapID;
            data.DestPos     = this.DestPos;
            data.DisplayText = this.DisplayText;
            data.CR          = this.CR;
            data.OpenItemID  = this.OpenItemID;
            data.OpenLevel   = this.OpenLevel;
            data.OpenVIP     = this.OpenVIP;
            return data;
        }

        public override void DrawScene()
        {
            NGUITools.DestroyChildren(transform);
            GroupRegion group = LvlSystem.GetGroup<GroupRegion>();
            if (group == null)
            {
                return;
            }
            m_Region = group.FindElement(RegionID);
            if (m_Region == null)
            {
                return;
            }
            GameObject effect = GTResourceManager.Instance.Load<GameObject>(GTPrefabKey.PRE_PORTALEFFECT, true);
            effect.transform.parent = transform;
            effect.transform.position = m_Region.transform.position;
            effect.transform.eulerAngles = m_Region.transform.eulerAngles;
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            int id = UnityEditor.EditorGUILayout.IntField("Id", Id);
            if (id != Id)
            {
                this.Id = id;
                this.SetName();
            }
            this.Name = UnityEditor.EditorGUILayout.TextField("Name", Name);
            int regionID = UnityEditor.EditorGUILayout.IntField("RegionID", RegionID);
            if (regionID != RegionID)
            {
                this.RegionID = regionID;
                this.DrawScene();
                this.SetName();
            }
            this.DestMapID = UnityEditor.EditorGUILayout.IntField("DestMapID", DestMapID);
            this.DestPos = UnityEditor.EditorGUILayout.Vector3Field("DestPos", DestPos);
            this.DisplayText = UnityEditor.EditorGUILayout.Toggle("DisplayText", DisplayText);
            this.CR = (ELvlCR)UnityEditor.EditorGUILayout.EnumPopup("CR", CR);
            this.OpenLevel = UnityEditor.EditorGUILayout.IntField("OpenLevel", OpenLevel);
            this.OpenItemID = UnityEditor.EditorGUILayout.IntField("OpenItemID", OpenItemID);
            this.OpenVIP = UnityEditor.EditorGUILayout.IntField("OpenVIP", OpenVIP);
            this.m_Region = (LevelRegion)UnityEditor.EditorGUILayout.ObjectField("Region", m_Region, typeof(LevelRegion), true);
#endif
        }
    }
}

