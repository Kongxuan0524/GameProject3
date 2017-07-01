using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml;

namespace BIE
{
    [System.Serializable]
    public class GuideBase: DCFG
    {
        public UInt16                   Id;
        public string                   TypeName                 = string.Empty;
        public bool                     IsLocked                 = true;
        public bool                     IsPause                  = false;
        public bool                     IsShowSkip               = false;
        public bool                     IsSavePoint              = false;

        public string                   TipSound                 = string.Empty;
        public string                   TipText                  = string.Empty;
        public Vector3                  TipPosition              = Vector3.zero;
        public EGuideConditionRelation  TriggerConditionRelation = EGuideConditionRelation.AND;
        public List<CheckCondition>     TriggerConditions        = new List<CheckCondition>();

        public EGuideState              State
        {
            get; set;
        }

        public AudioSource              Audio
        {
            get; private set;
        }

        public GuideSystem              Manager
        {
            get; set;
        }

        public UIGuide                  GuideWindow
        {
            get; private set;
        }

        public virtual bool Check()
        {
            switch(TriggerConditionRelation)
            {
                case EGuideConditionRelation.AND:
                    for (int i = 0; i < TriggerConditions.Count; i++)
                    {
                        if(TriggerConditions[i].Check() == false)
                        {
                            return false;
                        }
                    }
                    break;
                case EGuideConditionRelation.OR:
                    for (int i = 0; i < TriggerConditions.Count; i++)
                    {
                        if (TriggerConditions[i].Check() == true)
                        {
                            return true;
                        }
                    }
                    break;
            }
            return true;
        }

        public virtual void Enter()
        {
            GTEventCenter.FireEvent(GTEventID.TYPE_GUIDE_ENTER, Id);
            this.GuideWindow = (UIGuide)GTWindowManager.Instance.OpenWindow(EWindowID.UI_GUIDE);
            this.GuideWindow.ShowViewByGuideBaseData(this);
            this.Audio = GTAudioManager.Instance.PlaySound(TipSound);
        }

        public virtual void Execute()
        {

        }
        
        public virtual void Stop()
        {
            if (this.Audio != null)
            {
                Audio.Stop();
                Audio = null;
            }
        }

        public virtual void Finish()
        {
            if (this.Audio != null)
            {
                Audio.Stop();
                Audio = null;
            }
            GTEventCenter.FireEvent(GTEventID.TYPE_GUIDE_EXIT, Id);
            this.State = EGuideState.TYPE_FINISH;
        }

        public override void Read(XmlElement os)
        {
            this.Id                       = os.GetUInt16("Id");
            this.TypeName                 = os.GetString("TypeName");
            this.IsLocked                 = os.GetBool("IsLocked");
            this.IsPause                  = os.GetBool("IsPause");
            this.IsShowSkip               = os.GetBool("IsShowSkip");
            this.TipSound                 = os.GetString("TipSound");
            this.TipPosition              = os.GetVector3("TipPosition");
            this.TipText                  = os.GetString("TipText");
            this.TriggerConditionRelation = (EGuideConditionRelation)os.GetInt32("TriggerConditionRelation");
            foreach(var current in GetChilds(os))
            {
                Type type = System.Type.GetType("BIE." + current.Name);
                CheckCondition cc = (CheckCondition)System.Activator.CreateInstance(type);
                cc.Read(current);
                TriggerConditions.Add(cc);
            }
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {

        }
    }
}