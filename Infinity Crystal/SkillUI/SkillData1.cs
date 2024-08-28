using DTT.Utils.Extensions;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Skilleffect
{
    public string effect;
    public float effect_value;
    public string effect_type;

    public Skilleffect(string e, float v, string et)
    {
        effect = e;
        effect_value = v;
        effect_type = et;
    }

    public Skilleffect DeepCopy()
    {
        Skilleffect s = new Skilleffect(effect, effect_value, effect_type);

        return s;
    }
}

[System.Serializable]
public class SkillData1
{
    [JsonProperty("Number")]
    public string Number;

    [JsonProperty("Name")]
    public string Name;

    [JsonProperty("Spr")]
    public string Sprite;

    public Sprite GetSkillIcon()
    {
        return Resources.Load<Sprite>("#08.Skill_Icons/skill icon_original/" + Sprite);
    }

    public SkillAnimData1 GetSkillAnimData1()
    {
        if (string.IsNullOrEmpty(Number))
            return null;
        return JsonDBManager.Instance.GetAllSkillAnimData().Find(
            (_skill) =>
            {
                Debug.Log(_skill.UniqueID);
                Debug.Log(SkillAnimCode);
                return _skill.UniqueID == SkillAnimCode;
            });
    }

    [JsonProperty("Start Level")]
    public int StartLevel;

    [JsonProperty("Skill type 1")]
    public string Skilltype1;

    public SkillType1 GetSkilltype1()
    {
        switch (Skilltype1)
        {
            case "종족 특화":
                return SkillType1.Race;
            case "일반 기술":
                return SkillType1.Skill;
            case "전직 기술":
                return SkillType1.Job;
            case "무기 특성":
                return SkillType1.Weapon;
            default:
                return SkillType1.Skill;
        }
    }

    [JsonProperty("Skill type 2")]
    public string Skilltype2;

    [JsonProperty("Race")]
    public string Race;

    [JsonProperty("WCate")]
    public string WCate;

    [JsonProperty("Consume MP")]
    public string ConsumeMP;

    [JsonProperty("Cooldown<Sec>")]
    public string Cooldown_Sec;

    [JsonProperty("Casting Time<Sec>")]
    public string CastingTime_Sec;

    [JsonProperty("Skill duration<Sec>")]
    public string Skillduration_Sec;

    [JsonProperty("Skill Success rate<%>")]
    public string SkillSuccess_rate_percent;

    [JsonProperty("필요 선행 기술 Code")]
    public string NeedSkillCode;

    [JsonProperty("Cindition 1")]
    public string Cindition1;

    [JsonProperty("Cindition 2")]
    public string Cindition2;

    [JsonProperty("Cindition 3")]
    public string Cindition3;

    [JsonProperty("Cindition 4")]
    public string Cindition4;


    public float ParseLvUpEffectValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return 0;

        value = value.Replace("%", "");
        value = value.Replace("M", "");

        float parsed = 0;
        float.TryParse(value, out parsed);
        return parsed;
    }
    public string GetLvUpEffectValueChar(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "";

        if (value.Contains("%"))
            return "%";

        if (value.Contains("M"))
            return "M";

        return "";
    }

    [JsonProperty("Ef 1")]
    public string Effect1;

    [JsonProperty("EfV 1")]
    public string _EffectValue1;
    public float EffectValue1
    {
        get { return ParseLvUpEffectValue(_EffectValue1); }
    }

    [JsonProperty("Ef 2")]
    public string Effect2;

    [JsonProperty("EfV 2")]
    public string _EffectValue2;
    public float EffectValue2
    {
        get { return ParseLvUpEffectValue(_EffectValue2); }
    }

    [JsonProperty("Ef 3")]
    public string Effect3;

    [JsonProperty("EfV 3")]
    public string _EffectValue3;
    public float EffectValue3
    {
        get { return ParseLvUpEffectValue(_EffectValue3); }
    }

    [JsonProperty("Ef 4")]
    public string Effect4;

    [JsonProperty("EfV 4")]
    public string _EffectValue4;
    public float EffectValue4
    {
        get { return ParseLvUpEffectValue(_EffectValue4); }
    }

    [JsonProperty("Ef 5")]
    public string Effect5;

    [JsonProperty("EfV 5")]
    public string _EffectValue5;
    public float EffectValue5
    {
        get { return ParseLvUpEffectValue(_EffectValue5); }
    }

    [JsonProperty("Ef 6")]
    public string Effect6;

    [JsonProperty("EfV 6")]
    public string _EffectValue6;
    public float EffectValue6
    {
        get { return ParseLvUpEffectValue(_EffectValue6); }
    }

    [JsonProperty("Ef 7")]
    public string Effect7;

    [JsonProperty("EfV 7")]
    public string _EffectValue7;
    public float EffectValue7
    {
        get { return ParseLvUpEffectValue(_EffectValue7); }
    }

    [JsonProperty("Ef 8")]
    public string Effect8;

    [JsonProperty("EfV 8")]
    public string _EffectValue8;
    public float EffectValue8
    {
        get { return ParseLvUpEffectValue(_EffectValue8); }
    }

    [JsonProperty("LEf 1")]
    public string LvUpEffect1;

    [JsonProperty("LEfV 1")]
    public string _LvUpEffectValue1;
    public float LvUpEffectValue1
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue1); }
    }

    [JsonProperty("LEf 2")]
    public string LvUpEffect2;

    [JsonProperty("LEfV 2")]
    public string _LvUpEffectValue2;
    public float LvUpEffectValue2
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue2); }
    }


    [JsonProperty("LEf 3")]
    public string LvUpEffect3;

    [JsonProperty("LEfV 3")]
    public string _LvUpEffectValue3;
    public float LvUpEffectValue3
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue3); }
    }


    [JsonProperty("LEf 4")]
    public string LvUpEffect4;

    [JsonProperty("LEfV 4")]
    public string _LvUpEffectValue4;
    public float LvUpEffectValue4
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue4); }
    }


    [JsonProperty("LEf 5")]
    public string LvUpEffect5;

    [JsonProperty("LEfV 5")]
    public string _LvUpEffectValue5;
    public float LvUpEffectValue5
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue5); }
    }


    [JsonProperty("LEf 6")]
    public string LvUpEffect6;

    [JsonProperty("LEfV 6")]
    public string _LvUpEffectValue6;
    public float LvUpEffectValue6
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue6); }
    }


    [JsonProperty("LEf 7")]
    public string LvUpEffect7;

    [JsonProperty("LEfV 7")]
    public string _LvUpEffectValue7;
    public float LvUpEffectValue7
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue7); }
    }


    [JsonProperty("LEf 8")]
    public string LvUpEffect8;

    [JsonProperty("LEfV 8")]
    public string _LvUpEffectValue8;
    public float LvUpEffectValue8
    {
        get { return ParseLvUpEffectValue(_LvUpEffectValue8); }
    }


    [JsonProperty("Enchant Condition")]
    public string EnchantCondition;

    [JsonProperty("End Level")]
    public float EndLevel;

    [JsonProperty("Skill Description")]
    public string SkillDescription;

    [JsonProperty("Skill Animation")]
    public string SkillAnimation;

    [JsonProperty("Skill Anim Code")]
    public string SkillAnimCode;

    [JsonIgnore]
    public int ForeignKey;  //스킬 찍은 값 찾는데 사용
    [JsonIgnore]
    public List<Skilleffect> skilleffects = new List<Skilleffect>();    //기본 값 데이터 모음 리스트 (참조하기 편하도록 가공)
    [JsonIgnore]
    public List<Skilleffect> skillleveleffects=new List<Skilleffect>(); //레벨 업 값 데이터 모음 리스트 (참조하기 편하도록 가공)

    public void skillleveleffectsSet()
    {
        skilleffects.Add(new Skilleffect(Effect1, EffectValue1, GetLvUpEffectValueChar(_EffectValue1)));
        skilleffects.Add(new Skilleffect(Effect2, EffectValue2, GetLvUpEffectValueChar(_EffectValue2)));
        skilleffects.Add(new Skilleffect(Effect3, EffectValue3, GetLvUpEffectValueChar(_EffectValue3)));
        skilleffects.Add(new Skilleffect(Effect4, EffectValue4, GetLvUpEffectValueChar(_EffectValue4)));
        skilleffects.Add(new Skilleffect(Effect5, EffectValue5, GetLvUpEffectValueChar(_EffectValue5)));
        skilleffects.Add(new Skilleffect(Effect6, EffectValue6, GetLvUpEffectValueChar(_EffectValue6)));
        skilleffects.Add(new Skilleffect(Effect7, EffectValue7, GetLvUpEffectValueChar(_EffectValue7)));
        skilleffects.Add(new Skilleffect(Effect8, EffectValue8, GetLvUpEffectValueChar(_EffectValue8)));
    }

    public void SkilleffectSet()
    {
        Debug.Log("SkilleffectSet");

        skillleveleffects.Add(new Skilleffect(LvUpEffect1, LvUpEffectValue1, GetLvUpEffectValueChar(_LvUpEffectValue1)));
        skillleveleffects.Add(new Skilleffect(LvUpEffect2, LvUpEffectValue2, GetLvUpEffectValueChar(_LvUpEffectValue2)));
        skillleveleffects.Add(new Skilleffect(LvUpEffect3, LvUpEffectValue3, GetLvUpEffectValueChar(_LvUpEffectValue3)));
        skillleveleffects.Add(new Skilleffect(LvUpEffect4, LvUpEffectValue4, GetLvUpEffectValueChar(_LvUpEffectValue4)));
        skillleveleffects.Add(new Skilleffect(LvUpEffect5, LvUpEffectValue5, GetLvUpEffectValueChar(_LvUpEffectValue5)));
        skillleveleffects.Add(new Skilleffect(LvUpEffect6, LvUpEffectValue6, GetLvUpEffectValueChar(_LvUpEffectValue6)));
        skillleveleffects.Add(new Skilleffect(LvUpEffect7, LvUpEffectValue7, GetLvUpEffectValueChar(_LvUpEffectValue7)));
        skillleveleffects.Add(new Skilleffect(LvUpEffect8, LvUpEffectValue8, GetLvUpEffectValueChar(_LvUpEffectValue8)));

        /*
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue1));
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue2));
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue3));
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue4));
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue5));
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue6));
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue7));
        skillEffectType.Add(GetLvUpEffectValueChar(_LvUpEffectValue8));
        */
    }


    //임시값 넣는 생성자
    /*public SkillData1(string name="", string SkillDescription="",string Skilltype2 = "",float EndLevel=0, string Effect1="", float EffectValue1=0, string Effect2 = "", 
        float EffectValue2 = 0, string LvUpEffect1 = "", float LvUpEffectValue1 = 0, string LvUpEffect2 = "", float LvUpEffectValue2 = 0,
        string LvUpEffect3 = "", float LvUpEffectValue3 = 0, string LvUpEffect4 = "", float LvUpEffectValue4 = 0, string LvUpEffect5 = "", float LvUpEffectValue5 = 0, string LvUpEffect6 = "", float LvUpEffectValue6 = 0
        , string LvUpEffect7 = "", float LvUpEffectValue7=0, string LvUpEffect8 = "", float LvUpEffectValue8 = 0)
    {
        this.Name = name;
        this.SkillDescription = SkillDescription;
        this.Skilltype2 = Skilltype2;
        this.EndLevel = EndLevel;

        this.Effect1 = Effect1;
        this.EffectValue1 = EffectValue1;
        this.Effect2 = Effect2;
        this.EffectValue2 = EffectValue2;

        this.LvUpEffect1 = LvUpEffect1;
        this.LvUpEffectValue1 = LvUpEffectValue1;
        this.LvUpEffect2 = LvUpEffect2;
        this.LvUpEffectValue2 = LvUpEffectValue2;
        this.LvUpEffect3 = LvUpEffect3;
        this.LvUpEffectValue3 = LvUpEffectValue3;
        this.LvUpEffect4 = LvUpEffect4;
        this.LvUpEffectValue4 = LvUpEffectValue4;
        this.LvUpEffect5 = LvUpEffect5;
        this.LvUpEffectValue5 = LvUpEffectValue5;
        this.LvUpEffect6 = LvUpEffect6;
        this.LvUpEffectValue6 = LvUpEffectValue6;
        this.LvUpEffect7 = LvUpEffect7;
        this.LvUpEffectValue7 = LvUpEffectValue7;
        this.LvUpEffect8 = LvUpEffect8;
        this.LvUpEffectValue8 = LvUpEffectValue8;
        
        SkilleffectSet();
        skillleveleffectsSet();
        skillleveleffectssumSet();
    }*/

}
