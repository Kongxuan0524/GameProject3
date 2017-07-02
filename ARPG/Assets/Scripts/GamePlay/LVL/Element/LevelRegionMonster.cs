using UnityEngine;
using System.Collections;
using CFG;

namespace LVL
{
    public class LevelRegionMonster : LevelElement
    {
        [LevelVariable]
        public int   RegionID;
        [LevelVariable]
        public int   MonsterID;
        [LevelVariable]
        public int   MaxCount = 1;
        [LevelVariable]
        public float RebornCD = 5;

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

        }

        public override void Import(DCFG cfg)
        {
            LvlRegionMonster data = cfg as LvlRegionMonster;
            this.Id               = data.Id;
            this.RegionID         = data.RegionID;
            this.MonsterID        = data.MonsterID;
            this.RebornCD         = data.RebornCD;
            this.MaxCount         = data.MaxCount;
        }

        public override DCFG Export()
        {
            LvlRegionMonster data = new LvlRegionMonster();
            data.Id               = this.Id;
            data.RegionID         = this.RegionID;
            data.MonsterID        = this.MonsterID;
            data.RebornCD         = this.RebornCD;
            data.MaxCount         = this.MaxCount;
            return data;
        }

        public override void DrawScene()
        {
            GroupRegion group = LvlSystem.GetGroup<GroupRegion>();
            if (group == null)
            {
                return;
            }
            m_Region = group.FindElement(RegionID);
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
            int regionID = UnityEditor.EditorGUILayout.IntField("RegionID", RegionID);
            if (regionID != RegionID)
            {
                this.RegionID = regionID;
                this.DrawScene();
                this.SetName();
            }
            this.MonsterID = UnityEditor.EditorGUILayout.IntField("MonsterID", MonsterID);
            this.MaxCount = UnityEditor.EditorGUILayout.IntField("MaxCount", MaxCount);
            this.RebornCD = UnityEditor.EditorGUILayout.FloatField("RebornCD", RebornCD);
            this.m_Region = (LevelRegion)UnityEditor.EditorGUILayout.ObjectField("Region", m_Region, typeof(LevelRegion), true);
#endif
        }

        public void OnDrawGizmos()
        {
            if (m_Region != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawCube(m_Region.Pos, new Vector3(1, 5, 1));
            }
        }
    }
}