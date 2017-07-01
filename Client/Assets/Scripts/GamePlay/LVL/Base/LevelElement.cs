using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace LVL
{
    public class LevelElement : MonoBehaviour, ILevelElement
    {
        [LevelVariable]
        public int             Id
        {
            get; set;
        }

        [LevelVariable]
        public Vector3         Pos
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        [LevelVariable]
        public Vector3         Scale
        {
            get { return transform.localScale; }
            set { transform.localScale = value; }
        }

        [LevelVariable]
        public Vector3         Euler
        {
            get { return transform.eulerAngles; }
            set { transform.eulerAngles = value; }
        }

        public LevelSystem     LvlSystem
        {
            get; set;
        }

        public virtual void Startup()
        {

        }

        public virtual void Trigger()
        {

        }

        public virtual void SetName()
        {
            this.name =string.Format("{0}_{1}", this.GetType().Name, Id);
        }

        public virtual void Release()
        {
            gameObject.SetActive(false);
        }

        public virtual void DrawScene()
        {

        }

        public virtual void DrawGUI()
        {

        }

        public virtual void Import(DCFG cfg)
        {

        }

        public virtual DCFG Export()
        {
            return null;
        }

        public virtual bool AutoDrawInspector()
        {
            return false;
        }
    }
}