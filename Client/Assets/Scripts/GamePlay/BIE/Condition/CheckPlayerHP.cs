using UnityEngine;
using System.Collections;
using System.Xml;

namespace BIE
{
    public class CheckPlayerHP : CheckCondition
    {
        public ECompare Compare = ECompare.EQ;
        public int      HPValue;

        public override void Read(XmlElement os)
        {
            base.Read(os);
            this.Compare = os.GetEnum<ECompare>("Compare");
            this.HPValue = os.GetInt32("HPValue");
        }

        public override void Write(XmlDocument doc, XmlElement os)
        {
            base.Write(doc, os);
            DCFG.Write(doc, os, "Compare", Compare.ToString());
            DCFG.Write(doc, os, "HPValue", HPValue);
        }

        public override bool Check()
        {
            if (CharacterManager.Main == null)
            { 
                return false;
            }
            switch (Compare)
            {
                case ECompare.EQ:
                    return CharacterManager.Main.CurrAttr.HP == HPValue;
                case ECompare.GT:
                    return CharacterManager.Main.CurrAttr.HP >  HPValue;
                case ECompare.LT:
                    return CharacterManager.Main.CurrAttr.HP <  HPValue;
                case ECompare.GE:
                    return CharacterManager.Main.CurrAttr.HP >= HPValue;
                case ECompare.LE:
                    return CharacterManager.Main.CurrAttr.HP <= HPValue;
                default:
                    return true;
            }
        }
    }
}