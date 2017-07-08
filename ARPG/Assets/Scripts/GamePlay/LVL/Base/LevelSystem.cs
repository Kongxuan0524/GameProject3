using UnityEngine;
using System.Collections;
using CFG;
using LVL;
using System.Collections.Generic;
using System;

namespace LVL
{
    public class LevelSystem : LevelElement
    {
        [LevelVariable] public string LvlName = string.Empty;

        private LvlConfig                            m_Config = new LvlConfig();
        private Dictionary<ELvlGroup, LevelElement>  m_Groups = new Dictionary<ELvlGroup, LevelElement>();

        public override void Startup()
        {
            this.Trigger();
            if (InitConfig())
            {
                this.StartCoroutine(DoSceneStartEvents());
            }
            else
            {
                this.StartCoroutine(GTLauncher.Instance.CurScene.OpenWindows());
            }
        }

        public override void Trigger()
        {
            AddGroup<GroupBorn>(ELvlGroup.Born);
            AddGroup<GroupPortal>(ELvlGroup.Portal);
            AddGroup<GroupNpc>(ELvlGroup.Npc);
            AddGroup<GroupBarrier>(ELvlGroup.Barrier);
            AddGroup<GroupObj>(ELvlGroup.Obj);
            AddGroup<GroupMachine>(ELvlGroup.Machine);
            AddGroup<GroupPath>(ELvlGroup.Path);
            AddGroup<GroupMonster>(ELvlGroup.Monster);
            AddGroup<GroupRegion>(ELvlGroup.Region);
            AddGroup<GroupRegionMonster>(ELvlGroup.RegionMonster);
            AddGroup<GroupRegionWave>(ELvlGroup.RegionWave);
            AddGroup<GroupRegionMine>(ELvlGroup.RegionMine);
            foreach (var current in m_Groups)
            {
                current.Value.transform.ResetLocalTransform(transform);
            }
        }

        public override void Release()
        {
            CharacterCtrl.Instance.DelListener();
            CharacterManager.Instance.DelCharacters();
        }

        public void AddGroup<T>(ELvlGroup type) where T : LevelElement, ILevelGroup
        {
            LevelElement container = null;
            m_Groups.TryGetValue(type, out container);
            if (container == null)
            {
                container = new GameObject(typeof(T).Name).AddComponent<T>();
                container.LvlSystem = this;
                m_Groups[type] = container;
            }
        }

        public T    GetGroup<T>() where T : LevelElement, ILevelGroup
        {
            T group = GetComponentInChildren<T>();
            return group;
        }

        bool InitConfig()
        {
            string fsPath = LevelUtil.GetConfigPath(this.Id);
            this.m_Config = new LvlConfig();
            return this.m_Config.Load(fsPath);
        }

        IEnumerator AddMainPlayer()
        {
            int id = GTGlobal.CurPlayerID;
            if (m_Config.A == null)
                yield break;
            KTransform bornData = KTransform.Create(m_Config.A.Pos, m_Config.A.Euler);
            CharacterManager.Instance.AddMainPlayer(bornData);
            yield return null;
        }

        IEnumerator AddMainPet()
        {
            yield return null;
        }

        IEnumerator AddPartner()
        {
            CharacterManager.Instance.AddMainPartner(1);
            yield return null;
            CharacterManager.Instance.AddMainPartner(2);
            yield return null;
        }

        IEnumerator AddMonster()
        {
            for (int i = 0; i < m_Config.Monsters.Count; i++)
            {
                LvlMonster mm = m_Config.Monsters[i];
                KTransform bd = KTransform.Create(mm.Pos, mm.Euler);
                CharacterManager.Instance.AddMonster(mm.Id, bd);
                yield return null;
            }
        }

        IEnumerator AddNpc()
        {
            for (int i = 0; i < m_Config.Npcs.Count; i++)
            {
                LvlNpc data = m_Config.Npcs[i];
                CharacterManager.Instance.AddNpc(data.Id, KTransform.Create(data.Pos, data.Euler, data.Scale));
                yield return null;
            }
        }

        IEnumerator AddFollowCamera()
        {
            if (CharacterManager.Main != null)
            {
                GTCameraManager.Instance.CreateMainCamera(CharacterManager.Main.CacheTransform);
                yield return null;
                GTCameraManager.Instance.CreateMiniCamera(CharacterManager.Main.CacheFixEuler);
            }
        }

        IEnumerator DoSceneStartEvents()
        {
            ImportInPlay(m_Config);
            yield return AddMainPlayer();
            yield return AddMainPet();
            yield return AddPartner();
            yield return AddMonster();
            yield return AddFollowCamera();
            yield return GTLauncher.Instance.CurScene.OpenWindows();      
            CharacterCtrl.Instance.DelListener();
            CharacterCtrl.Instance.AddListener();
            LevelData.StTime = Time.time;
        }

        public void TriggerLevelEvent(LvlEvent lvlEvent)
        {
            switch (lvlEvent.Type)
            {
                case ELvlTrigger.TYPE_NONE:
                    break;
                case ELvlTrigger.TYPE_WAVESET:
                    ActiveWaveSet(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_TASK:
                    ActiveTask(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_PLOT:
                    LvlSystem.ActivePlot(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_MACHINE:
                    ActiveMachine(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_BARRIER:
                    ActiveBarrier(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_REGION:
                    ActiveRegion(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_RESULT:
                    ActiveResultWindow();
                    break;
                case ELvlTrigger.TYPE_CUTSCENE:
                    ActiveCutscene(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_PORTAL:
                    ActivePortal(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_BUFF:
                    AddLevelBuff(lvlEvent.Id);
                    break;
                case ELvlTrigger.TYPE_MONSTEGROUP:
                    ActiveMonsterGroup(lvlEvent.Id, lvlEvent.Show);
                    break;
                case ELvlTrigger.TYPE_MINEGROUP:
                    ActiveMineGroup(lvlEvent.Id, lvlEvent.Show);
                    break;
            }
        }

        public void ActiveMachine(int id, bool active)
        {
            ActiveElement<LevelMachine, GroupMachine>(id, active);
        }

        public void ActiveBarrier(int id, bool active)
        {
            ActiveElement<LevelBarrier, GroupBarrier>(id, active);
        }

        public void ActiveRegion(int id, bool active)
        {
            ActiveElement<LevelRegion, GroupRegion>(id, active);
        }

        public void ActiveWaveSet(int id, bool active)
        {
            ActiveElement<LevelRegionWave, GroupRegionWave>(id, active);
        }

        public void ActivePortal(int id, bool active)
        {
            ActiveElement<LevelPortal, GroupPortal>(id, active);
        }

        public void ActiveMonsterGroup(int id, bool active)
        {
            ActiveElement<LevelRegionMonster, GroupRegionMonster>(id, active);
        }

        public void ActiveMineGroup(int id, bool active)
        {
            ActiveElement<LevelRegionMine, GroupRegionMine>(id, active);
        }

        public void ActiveElement<TElement, TGroup>(int id, bool active)
            where TElement : LevelElement
            where TGroup : LevelGroup<TElement>
        {
            TGroup group = GetGroup<TGroup>();
            TElement element = group.FindElement(id);
            if (element != null)
            {
                if (active)
                {
                    element.Trigger();
                }
                else
                {
                    element.Release();
                }
            }
            else
            {
                Debug.LogError(string.Format("找不到ID = {0}的{1}", id, typeof(TElement)));
            }
        }

        public void ActiveResultWindow()
        {
            LevelData.EdTime = Time.time;
            LevelData.Win = true;
            LevelData.Star = 3;
            GTWindowManager.Instance.OpenWindow(EWindowID.UIMainResult);
        }

        public void AddLevelBuff(int id)
        {
            if (CharacterManager.Main != null)
            {
                CharacterHelper.CalcAddBuff(null, CharacterManager.Main, id);
            }
        }

        public void ActivePlot(int id, bool active)
        {
            if (GTWorld.Instance.Plt != null)
            {
                GTWorld.Instance.Plt.Trigger(id);
            }
        }

        public void ActiveTask(int id, bool active)
        {
            if (GTWorld.Instance.Tas != null)
            {
                GTWorld.Instance.Tas.Trigger(id);
            }
        }

        public void ActiveCutscene(int id, bool active)
        {
            if (GTWorld.Instance.Cut != null)
            {
                GTWorld.Instance.Cut.Trigger(id);
            }
        }

        public void ExportDCFGByElement<TElement, TGroup, TDCFG>(List<TDCFG> cfgList)
        where TElement : LevelElement
        where TGroup : LevelGroup<TElement>
        where TDCFG : LvlElement
        {
            List<TElement> list = GetGroup<TGroup>().Elements;
            for (int i = 0; i < list.Count; i++)
            {
                cfgList.Add(list[i].Export() as TDCFG);
            }
        }

        public void ImportDCFGByElement<TElement, TGroup, TDCFG>(List<TDCFG> cfgList, TGroup group, bool isEditor)
        where TElement : LevelElement
        where TGroup : LevelGroup<TElement>
        where TDCFG : LvlElement
        {
            if(isEditor)
            {
                for (int i = 0; i < cfgList.Count; i++)
                {
                    TElement element = group.AddElement();
                    element.Import(cfgList[i]);
                    element.DrawScene();
                    element.SetName();
                }
            }
            else
            {
                for (int i = 0; i < cfgList.Count; i++)
                {
                    TElement element = group.AddElement();
                    element.Import(cfgList[i]);
                    element.Startup();
                    element.SetName();
                }
            }
        }

        public void     DelAllElements()
        {
            List<LevelGroupBase> list = new List<LevelGroupBase>();
            for (int i = 0; i < transform.childCount; i++)
            {
                LevelGroupBase g = transform.GetChild(i).GetComponent<LevelGroupBase>();
                if (g != null)
                {
                    g.DelAllElements();
                }
            }
        }

        public override DCFG Export()
        {
            LvlConfig cfg = new LvlConfig();
            ExportDCFGByElement<LevelBorn,          GroupBorn,          LvlBorn>(cfg.Borns);
            ExportDCFGByElement<LevelPortal,        GroupPortal,        LvlPortal>(cfg.Portals);
            ExportDCFGByElement<LevelBarrier,       GroupBarrier,       LvlBarrier>(cfg.Barriers);
            ExportDCFGByElement<LevelObj,           GroupObj,           LvlObj>(cfg.Objs);
            ExportDCFGByElement<LevelMonster,       GroupMonster,       LvlMonster>(cfg.Monsters);
            ExportDCFGByElement<LevelNpc,           GroupNpc,           LvlNpc>(cfg.Npcs);
            ExportDCFGByElement<LevelRegion,        GroupRegion,        LvlRegion>(cfg.Regions);
            ExportDCFGByElement<LevelRegionMine,    GroupRegionMine,    LvlRegionMine>(cfg.RegionMines);
            ExportDCFGByElement<LevelRegionWave,    GroupRegionWave,    LvlRegionWave>(cfg.RegionWaves);
            ExportDCFGByElement<LevelRegionMonster, GroupRegionMonster, LvlRegionMonster>(cfg.RegionMonsters);
            ExportDCFGByElement<LevelPath,          GroupPath,          LvlPath>(cfg.Paths);
            return cfg;
        }

        public override void Import(DCFG dd)
        {
            LvlConfig cfg = dd as LvlConfig;
            ImportDCFGByElement<LevelBorn,          GroupBorn,          LvlBorn>(cfg.Borns, GetGroup<GroupBorn>(),true);
            ImportDCFGByElement<LevelPortal,        GroupPortal,        LvlPortal>(cfg.Portals ,GetGroup<GroupPortal>(), true);
            ImportDCFGByElement<LevelBarrier,       GroupBarrier,       LvlBarrier>(cfg.Barriers,GetGroup<GroupBarrier>(), true);
            ImportDCFGByElement<LevelObj,           GroupObj,           LvlObj>(cfg.Objs, GetGroup<GroupObj>(), true);
            ImportDCFGByElement<LevelMonster,       GroupMonster,       LvlMonster>(cfg.Monsters, GetGroup<GroupMonster>(), true);
            ImportDCFGByElement<LevelNpc,           GroupNpc,           LvlNpc>(cfg.Npcs, GetGroup<GroupNpc>(), true);
            ImportDCFGByElement<LevelRegion,        GroupRegion,        LvlRegion>(cfg.Regions, GetGroup<GroupRegion>(), true);
            ImportDCFGByElement<LevelRegionMine,    GroupRegionMine,    LvlRegionMine>(cfg.RegionMines, GetGroup<GroupRegionMine>(), true);
            ImportDCFGByElement<LevelRegionWave,    GroupRegionWave,    LvlRegionWave>(cfg.RegionWaves, GetGroup<GroupRegionWave>(), true);
            ImportDCFGByElement<LevelRegionMonster, GroupRegionMonster, LvlRegionMonster>(cfg.RegionMonsters, GetGroup<GroupRegionMonster>(), true);
            ImportDCFGByElement<LevelPath,          GroupPath,          LvlPath>(cfg.Paths, GetGroup<GroupPath>(), true);
        }

        public          void ImportInPlay(DCFG dd)
        {
            LvlConfig cfg = dd as LvlConfig;
            ImportDCFGByElement<LevelBorn,          GroupBorn,          LvlBorn>(cfg.Borns, GetGroup<GroupBorn>(),false);
            ImportDCFGByElement<LevelPortal,        GroupPortal,        LvlPortal>(cfg.Portals ,GetGroup<GroupPortal>(), false);
            ImportDCFGByElement<LevelBarrier,       GroupBarrier,       LvlBarrier>(cfg.Barriers,GetGroup<GroupBarrier>(), false);
            ImportDCFGByElement<LevelObj,           GroupObj,           LvlObj>(cfg.Objs, GetGroup<GroupObj>(), false);
            ImportDCFGByElement<LevelMonster,       GroupMonster,       LvlMonster>(cfg.Monsters, GetGroup<GroupMonster>(), false);
            ImportDCFGByElement<LevelNpc,           GroupNpc,           LvlNpc>(cfg.Npcs, GetGroup<GroupNpc>(), false);
            ImportDCFGByElement<LevelRegion,        GroupRegion,        LvlRegion>(cfg.Regions, GetGroup<GroupRegion>(), false);
            ImportDCFGByElement<LevelRegionMine,    GroupRegionMine,    LvlRegionMine>(cfg.RegionMines, GetGroup<GroupRegionMine>(), false);
            ImportDCFGByElement<LevelRegionWave,    GroupRegionWave,    LvlRegionWave>(cfg.RegionWaves, GetGroup<GroupRegionWave>(), false);
            ImportDCFGByElement<LevelRegionMonster, GroupRegionMonster, LvlRegionMonster>(cfg.RegionMonsters, GetGroup<GroupRegionMonster>(), false);
            ImportDCFGByElement<LevelPath,          GroupPath,          LvlPath>(cfg.Paths, GetGroup<GroupPath>(), false);
        }

        public override void DrawGUI()
        {
#if UNITY_EDITOR
            GUILayout.Space(10);
            this.Id      = UnityEditor.EditorGUILayout.IntField("Id", this.Id);
            this.LvlName = UnityEditor.EditorGUILayout.TextField("LvlName", this.LvlName);
            GUI.color = new Color(0.8f, 0.8f, 1.0f);
            if (GUILayout.Button("Import", GUILayout.Height(30)))
            {
                bool success = this.InitConfig();
                if(success)
                {
                    this.DelAllElements();
                    this.Import(m_Config);
                }
                else
                {
                    Debug.LogError(string.Format("LvlID = {0}，无法导入数据", this.Id));
                }
            }
            GUI.color = new Color(0.0f, 0.9f, 0.2f);
            GUILayout.Space(5);
            if (GUILayout.Button("Export", GUILayout.Height(30)))
            {
                LvlConfig mc = (LvlConfig)Export();
                mc.Id = Id;
                mc.LvlName = LvlName;
                string fileName = string.Format("{0}/Resources/Text/Lvl/{1}.xml", Application.dataPath, mc.Id);
                mc.Save(fileName);
            }
#endif
        }
    }
}