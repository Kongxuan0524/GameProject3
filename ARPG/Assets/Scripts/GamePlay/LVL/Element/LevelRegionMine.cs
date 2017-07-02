using UnityEngine;
using System.Collections;
using CFG;

namespace LVL
{
    public class LevelRegionMine : LevelElement
    {
        [LevelVariable]
        public int   RegionID;
        [LevelVariable]
        public int   MineID;
        [LevelVariable]
        public int   MaxCount = 1;
        [LevelVariable]
        public float RebornCD = 20;

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
            LvlRegionMine data = cfg as LvlRegionMine;
            this.Id            = data.Id;
            this.RegionID      = data.RegionID;
            this.MineID        = data.MineID;
            this.RebornCD      = data.RebornCD;
            this.MaxCount      = data.MaxCount;
        }

        public override DCFG Export()
        {
            LvlRegionMine data = new LvlRegionMine();
            data.Id            = this.Id;
            data.RegionID      = this.RegionID;
            data.MineID        = this.MineID;
            data.RebornCD      = this.RebornCD;
            data.MaxCount      = this.MaxCount;
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
            this.MineID = UnityEditor.EditorGUILayout.IntField("MineID", MineID);
            this.MaxCount = UnityEditor.EditorGUILayout.IntField("MaxCount", MaxCount);
            this.RebornCD = UnityEditor.EditorGUILayout.FloatField("RebornCD", RebornCD);
            this.m_Region = (LevelRegion)UnityEditor.EditorGUILayout.ObjectField("Region", m_Region, typeof(LevelRegion), true);
#endif
        }

        public void OnDrawGizmos()
        {
            if (m_Region != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(m_Region.Pos, new Vector3(1,5,1));
            }
        }
    }
}