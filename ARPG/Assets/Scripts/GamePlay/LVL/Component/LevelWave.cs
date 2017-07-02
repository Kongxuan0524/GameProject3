using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CFG;

namespace LVL
{
    public class LevelWave : LevelGroup<LevelMonster>
    {
        [LevelVariable]
        public string               IndexName = string.Empty;
        [LevelVariable]
        public float                Delay;
        [LevelVariable]
        public ELvlMonsterWaveSpawn Spawn;
        [LevelVariable]
        public int                  AddBuffID;


        private LevelRegionWave m_ParentGroup;
        private List<int>       m_MonsterGUIDs    = new List<int>();
        private HashSet<int>    m_HasKillMonsters = new HashSet<int>();


        public override void Startup()
        {

        }

        public override void Trigger()
        {
            List<LevelMonster> list = Elements;
            switch (Spawn)
            {
                case ELvlMonsterWaveSpawn.TYPE_ALONG:
                    for (int i = 0; i < list.Count; i++)
                    {
                        IEnumerator ir = DoTriggerMonster(list[i], OnSpawnMonsterCallback, Delay * i);
                        StartCoroutine(ir);
                    }
                    break;
                case ELvlMonsterWaveSpawn.TYPE_RADOM:
                    {
                        int r = UnityEngine.Random.Range(0, list.Count);
                        LevelMonster lm = list[r];
                        IEnumerator ir = DoTriggerMonster(lm, OnSpawnMonsterCallback, Delay);
                        StartCoroutine(ir);
                    }
                    break;
                case ELvlMonsterWaveSpawn.TYPE_WHOLE:
                    for (int i = 0; i < list.Count; i++)
                    {
                        IEnumerator ir = DoTriggerMonster(list[i], OnSpawnMonsterCallback, Delay);
                        StartCoroutine(ir);
                    }
                    break;
            }
            GTEventCenter.AddHandler<int, int>(GTEventID.TYPE_KILL_MONSTER, OnKillMonster);
        }

        public override void Release()
        {
            GTEventCenter.DelHandler<int, int>(GTEventID.TYPE_KILL_MONSTER, OnKillMonster);
        }

        public override void SetName()
        {
            this.name = string.Format("{0}_{1}", this.GetType().Name, Id);
        }

        public override void Import(DCFG cfg)
        {
            LvlWave data       = cfg as LvlWave;
            this.Id            = data.Id;
            this.IndexName     = data.IndexName;
            this.Spawn         = data.Spawn;
            this.Delay         = data.Delay;
            this.AddBuffID     = data.AddBuffID;
            for (int i = 0; i < data.Monsters.Count; i++)
            {
                LvlMonster   d = data.Monsters[i];
                LevelMonster m = this.AddElement();
                m.Import(d);
                m.DrawScene();
                m.SetName();
            }
        }

        public override DCFG Export()
        {
            LvlWave data       = new LvlWave();
            data.Id            = this.Id;
            data.IndexName     = string.IsNullOrEmpty(data.IndexName) ? string.Format("第{0}波", this.Id) : this.IndexName;
            data.Spawn         = this.Spawn;
            data.Delay         = this.Delay;
            data.AddBuffID     = this.AddBuffID;
            List<LevelMonster> list = Elements;
            for (int i = 0; i < list.Count; i++)
            {
                LevelMonster m = list[i];
                LvlMonster   d = (LvlMonster)m.Export();
                data.Monsters.Add(d);
            }
            return data;
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            this.Id                = UnityEditor.EditorGUILayout.IntField("Index", Id);
            this.IndexName         = UnityEditor.EditorGUILayout.TextField("IndexName", IndexName);
            this.Delay             = UnityEditor.EditorGUILayout.FloatField("Delay", Delay);
            this.Spawn             = (ELvlMonsterWaveSpawn)UnityEditor.EditorGUILayout.EnumPopup("Spawn", Spawn);
            this.AddBuffID         = UnityEditor.EditorGUILayout.IntField("AddBuffID", AddBuffID);
            this.SetName();
#endif
        }

        public IEnumerator   DoTriggerMonster(LevelMonster lm, Callback<Character> callback, float delay = 0)
        {
            Character cc = CharacterManager.Instance.AddMonster(lm.Id, KTransform.Create(lm.Pos, lm.Euler));
            if (cc != null)
            {
                callback(cc);
            }
            if (delay > 0)
            {
                yield return new WaitForSeconds(delay);
            }
            yield return null;
        }

        public          void OnSpawnMonsterCallback(Character cc)
        {
            if (cc == null)
            {
                return;
            }
            m_MonsterGUIDs.Add(cc.GUID);
        }

        public          void OnKillMonster(int guid, int id)
        {
            if (m_HasKillMonsters.Contains(guid) == false)
            {
                m_HasKillMonsters.Add(guid);
            }
            if (m_HasKillMonsters.Count >= m_MonsterGUIDs.Count && m_ParentGroup != null)
            {
                m_ParentGroup.Trigger();
            }
        }

        public          void SetParentGroup(LevelRegionWave lrw)
        {
            this.m_ParentGroup = lrw;
        }
    }
}
