using UnityEngine;
using System.Collections;
using CFG;
using System.Collections.Generic;
using System;

namespace LVL
{
    public class LevelRegionWave : LevelGroup<LevelWave>
    {
        [LevelVariable]
        public List<LvlEvent>   Events = new List<LvlEvent>();

        private Int32           m_HasTriggerIndex = 0;
        private HashSet<int>    m_HasTriggerEvents = new HashSet<int>();

        public override void Startup()
        {
            List<LevelWave> list = Elements;
            for (int i = 0; i < list.Count; i++)
            {
                LevelWave m = list[i];
                m.Startup();
            }
        }

        public override void Trigger()
        {
            m_HasTriggerIndex++;
            LevelWave lw = FindElement(m_HasTriggerIndex);
            if (lw != null)
            {
                lw.Trigger();
            }
            else
            {
                FinishWave();
            }
        }

        public override void Release()
        {
            base.Release();
        }

        private         void FinishWave()
        {
            ActiveEvents(ELvlTriggerCondition.TYPE_WAVES_FINISH);
        }

        private         void ActiveEvents(ELvlTriggerCondition inputTriggerCondition)
        {
            for (int i = 0; i < Events.Count; i++)
            {
                if (m_HasTriggerEvents.Contains(i))
                {
                    continue;
                }
                LvlEvent e = Events[i];
                if (e.TriggerCondition == inputTriggerCondition)
                {
                    LvlSystem.TriggerLevelEvent(e);
                    m_HasTriggerEvents.Add(i);
                }
            }
        }

        public override void Import(DCFG cfg)
        {
            LvlRegionWave data = cfg as LvlRegionWave;
            this.Id            = data.Id;
            this.Events.Clear();
            this.Events.AddRange(data.Events);
            for (int i = 0; i < data.Waves.Count; i++)
            {
                LvlWave d = data.Waves[i];
                LevelWave m = this.AddElement();
                m.Import(d);
                m.DrawScene();
                m.SetName();
                m.SetParentGroup(this);
            }
        }

        public override DCFG Export()
        {
            LvlRegionWave data = new LvlRegionWave();
            data.Id            = this.Id;
            data.Events.AddRange(Events);
            List<LevelWave> list = Elements;
            for (int i = 0; i < list.Count; i++)
            {
                LevelWave m = list[i];
                LvlWave d = (LvlWave)m.Export();
                data.Waves.Add(d);
            }
            return data;
        }

        public override bool AutoDrawInspector()
        {
            return true;
        }

        public override void DrawScene()
        {

        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            base.DrawGUI();
            this.SetName();
#endif
        }
    }
}