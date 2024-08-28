using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using static ChStatCalculator.StatNumAlls;

public enum Race
{
    Kelts, Perrt, Nix, Shard, All, Null
}

public static class ChStatCalculator
{
    public class StatLevels // 스텟창에서 찍은 횟수
    {
        public int Strength;
        public int Agile;
        public int Defence;
        public int Vitality;
        public int Magic;
        public int Wisdom;
        public int MDefence;
        public int Recovery;

        public void Reset(StatBaseLevels Statbase)
        {
            Strength = Statbase.B_Strength; Agile = Statbase.B_Agile; Defence = Statbase.B_Defence; Vitality = Statbase.B_Vitality; 
            Magic = Statbase.B_Magic; Wisdom = Statbase.B_Wisdom; MDefence = Statbase.B_MDefence; Recovery = Statbase.B_Recovery;
        }

        public StatLevels DeepCopy()
        {
            StatLevels st = new StatLevels();
            st.Strength = Strength;
            st.Agile = Agile;
            st.Defence = Defence;
            st.Vitality = Vitality;
            st.Magic = Magic;
            st.Wisdom = Wisdom;
            st.MDefence = MDefence;
            st.Recovery = Recovery;

            return st;

        }

        public void SetValue(StatLevels st) //값 전달 (얕은복사 방지)
        {
            Strength = st.Strength;
            Agile = st.Agile;
            Defence = st.Defence;
            Vitality = st.Vitality;
            Magic = st.Magic;
            Wisdom = st.Wisdom;
            MDefence = st.MDefence;
            Recovery = st.Recovery;
        }
    }

    public class StatBaseLevels // 종족별 기본 스텟
    {
        public int B_Strength;
        public int B_Agile;
        public int B_Defence;
        public int B_Vitality;
        public int B_Magic;
        public int B_Wisdom;
        public int B_MDefence;
        public int B_Recovery;

        public StatBaseLevels()
        {
            Reset();
        }

        public void Reset()
        {
            switch (SaveData.GetRace())
            {
                case Race.Kelts:
                    B_Strength = 10; B_Agile = 10; B_Defence = 10; B_Vitality = 10; B_Magic = 10; B_Wisdom = 10; B_MDefence = 10; B_Recovery = 10;
                    break;
                case Race.Perrt:
                    B_Strength = 15; B_Agile = 8; B_Defence = 8; B_Vitality = 9; B_Magic = 15; B_Wisdom = 8; B_MDefence = 8; B_Recovery = 9;
                    break;
                case Race.Nix:
                    B_Strength = 8; B_Agile = 13; B_Defence = 8; B_Vitality = 11; B_Magic = 8; B_Wisdom = 13; B_MDefence = 8; B_Recovery = 11;
                    break;
                case Race.Shard:
                    B_Strength = 9; B_Agile = 8; B_Defence = 9; B_Vitality = 14; B_Magic = 9; B_Wisdom = 8; B_MDefence = 9; B_Recovery = 14;
                    break;
                default:
                    break;
            }
            
        }
    }

    public class StatNumAlls // 스텟 수치 모음
    {
        public StrengthStats Strength { get; set; } = new StrengthStats();
        public AgileStats Agile { get; set; } = new AgileStats();
        public DefenceStats Defence { get; set; } = new DefenceStats();
        public VitalityStats Vitality { get; set; } = new VitalityStats();
        public MagicStats Magic { get; set; } = new MagicStats();
        public WisdomStats Wisdom { get; set; } = new WisdomStats();
        public MDefenceStats MDefence { get; set; } = new MDefenceStats();
        public RecoveryStats Recovery { get; set; } = new RecoveryStats();
        public BasicStats Basic { get; set; } = new BasicStats();
        public RaceStats Race { get; set; } = new RaceStats();
        public JobStats Job { get; set; } = new JobStats();
        public WeaponStats Weapon { get; set; } = new WeaponStats();

        public class StrengthStats
        {
            public float PhysicalDamage; // 물리 피해
            public float CriticalDamage; // 치명타 피해
        }

        public class AgileStats
        {
            public float PhysicalHit; // 물리 명중
            public float AttackSpeed; // 공격 속도
            public float PhysicalDodge; // 물리 회피
            public float CriticalChance; // 치명타 확률
        }

        public class DefenceStats
        {
            public float PhysicalDefence; // 물리 방어
            public float CriticalResistance; // 치명타 저항률
            public float CriticalDamageReduction; // 치명 피해 감소
        }

        public class VitalityStats
        {
            public float HP; // Hp
            public float MP; // Mp
            public float MovementSpeed; // 이동 속도
            public float Stamina; // 스테미나
        }

        public class MagicStats
        {
            public float MagicDamage; // 마법 피해
            public float CriticalMagicDamage; // 치명타 마법 피해
        }

        public class WisdomStats
        {
            public float MagicHit; // 마법 명중
            public float MagicSpeed; // 마법 속도
            public float MagicDodge; // 마법 회피
            public float MagicCriticalChance; // 마법 치명타 확률
        }

        public class MDefenceStats
        {
            public float MagicDefence; // 마법 방어
            public float MagicCriticalResistance; // 마법 치명타 저항률
            public float MagicCriticalDamageReduction; // 마법 치명 피해 감소
        }

        public class RecoveryStats
        {
            public float HPRecovery; // Hp 회복
            public float MPRecovery; // Mp 회복
            public float StaminaRecovery; // 스테미나 회복
        }

        //일반 스킬에 사용되는 스텟
        public class BasicStats
        {
            public float Skill_Cooldown; // 기술 재사용 시간
            public float Shield_Defense_Rate; // 방패 방어율
            public float Bow_Speed; // 활 속도
            public float Bow_Range; // 활 사거리
            public float Crossbow_Range; // 석궁 사거리
            public float Status_Ailment_Probability; // 상태이상 확률
            public float Status_Ailment_Resistance; // 상태이상 저항
            public float Scroll_Effects; // 주문서 효과
            public float Potion_Effects; // 물약 효과
            public float Additional_Gathering_Chance; // 재료 채집 시 추가 획득 확률
            public float Shard_Drop_Rate; // Shard 종족 추가 획득
            public float Gold_From_Monsters; // 몬스터에게 얻는 골드량
            public float Gold_Cost_For_Item_Crafting; // 아이템 제작 시 골드 소모량
            public float Additional_Materials_On_Enhance_Failure; // 강화 실패 시 재료 20%를 추가로 획득 확률
            public float HP_Recovery_On_Move; // 이동 시 HP 회복
            public float Additional_HP_On_Healing_Spell; // 치유 주문 시 추가 HP 회복
            public float MP_Cost_Of_Healing_Skills; // 치유 관련 기술 MP
            public float Spell_Failure_Chance_From_Attack; // 공격으로 인한 시전 실패 확률
            public float Attack_Target; // 공격 대상
            public float Damage_Range; // 피해 거리
        }

        //종족 스킬에 사용되는 스텟
        public class RaceStats
        {
            public float Stun_Effect_on_Hit; // 적중 시 기절 효과 발동
            public float Totem_Effects; // 모든 토템 효과
            public float Enemy_Stats; // 적 능력치
            public float Inventory_Slots; // 가방 칸
            public float Warehouse_Slots; // 창고 칸
            public float Inventory_Weight; // 가방 무게
        }

        //무기 스킬에 사용되는 스텟
        public class WeaponStats
        {
            public float Shield_Physical_Defense; // 방패의 물리 방어
            public float Damage_Recovery_to_MP; // 피해의 일부를 MP로 회복
            public float Max_Shield_Defense_Rate; // 방패 방어율 최대치
            public float Status_Ailment_Duration; // 상태이상 시간
            public float Critical_Hit_Probability; // 스킬 피해가 치명타로 적중 확률
            public float Skill_Duration; // 기술 지속 시간
            public float Total_Damage; // 모든 피해
        }

        //전직 스킬에 사용되는 스텟
        public class JobStats
        {
            public float Reuse_Cooldown_Chance; // 재사용 대기중인 기술 20% 확률로 재사용 시간
            public float Skill_Success_Probability; // 기술 성공 확률
            public float Activation_Time; // 발동 시간
            public float Piercing_Skill_Success_Probability; // 모든 찌르기 속성 기술 성공 확률
            public float Minion_Abilities; // 소환수의 모든 능력
            public float Phys_Evasion_to_Defense; // 물리 회피를 물리 방어로 전환
            public float Cont_Damage_Magic_Accel; // 지속 피해 마법 가속도
            public float Stun_Resistance; // 기절 내성
            public float Bow_Resistance; // 활 내성
            public float Instant_Summoning_of_Minions; // 소환수를 즉시 소환
            public float MP_Required_for_Summoning; // 소환에 필요한 MP
            public float Minion_Summon_Cooldown; // 소환수 소환 재사용 대기 시간
        }


        // + 연산자 오버로딩
        public static StatNumAlls operator +(StatNumAlls a, StatNumAlls b)
        {
            return MergeStats(a, b, true);
        }

        // - 연산자 오버로딩
        public static StatNumAlls operator -(StatNumAlls a, StatNumAlls b)
        {
            return MergeStats(a, b, false);
        }

        private static StatNumAlls MergeStats(StatNumAlls a, StatNumAlls b, bool isAddition)
        {
            // a와 b를 JSON으로 직렬화
            JObject ja = JObject.Parse(JsonConvert.SerializeObject(a));
            JObject jb = JObject.Parse(JsonConvert.SerializeObject(b));

            // ja의 각 필드를 순회하며 jb의 해당 필드와 더하거나 뺌
            foreach (var property in ja)
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    // 재귀적으로 내부 객체 처리
                    MergeStatsInternal((JObject)property.Value, (JObject)jb[property.Key], isAddition);
                }
                else if (property.Value.Type == JTokenType.Float || property.Value.Type == JTokenType.Integer)
                {
                    // 값 더하기 또는 빼기
                    float valueA = property.Value.ToObject<float>();
                    float valueB = jb[property.Key].ToObject<float>();
                    ja[property.Key] = isAddition ? valueA + valueB : valueA - valueB;
                }
            }

            // 결과 JSON 객체를 StatNumAlls 객체로 역직렬화하여 반환
            return ja.ToObject<StatNumAlls>();
        }

        private static void MergeStatsInternal(JObject ja, JObject jb, bool isAddition)
        {
            foreach (var property in ja)
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    // 재귀적으로 내부 객체 처리
                    MergeStatsInternal((JObject)property.Value, (JObject)jb[property.Key], isAddition);
                }
                else if (property.Value.Type == JTokenType.Float || property.Value.Type == JTokenType.Integer)
                {
                    // 값 더하기 또는 빼기
                    float valueA = property.Value.ToObject<float>();
                    float valueB = jb[property.Key].ToObject<float>();
                    ja[property.Key] = isAddition ? valueA + valueB : valueA - valueB;
                }
            }
        }

        //스킬 스텟 찍기
        public void SetStats(Stat stat,string name, float effect_value, string type, int PlusMinus = 1)
        {
                switch (name)
                {
                // Strength
                case "물리 피해":
                    if (type.Contains("%"))
                    {
                        Strength.PhysicalDamage += (float)stat.GetStat(Stat_.Attack_Strength) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Strength.PhysicalDamage += effect_value * PlusMinus;
                    }
                    break;
                case "물리 치명타 피해":
                    if (type.Contains("%"))
                    {
                        Strength.CriticalDamage += (float)stat.GetStat(Stat_.Attack_Critical_Damage) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Strength.CriticalDamage += effect_value * PlusMinus;
                    }
                    break;
                // Agile
                case "물리 명중":
                    if (type.Contains("%"))
                    {
                        Agile.PhysicalHit += (float)stat.GetStat(Stat_.Attacks_Hit_the_target) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Agile.PhysicalHit += effect_value * PlusMinus;
                    }
                    break;
                case "공격 속도":
                    if (type.Contains("%"))
                    {
                        Agile.AttackSpeed += (float)stat.GetStat(Stat_.Attack_Speed) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Agile.AttackSpeed += effect_value * PlusMinus;
                    }
                    break;
                case "물리 회피":
                    if (type.Contains("%"))
                    {
                        Agile.PhysicalDodge += (float)stat.GetStat(Stat_.Attack_Dodge) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Agile.PhysicalDodge += effect_value * PlusMinus;
                    }
                    break;
                case "물리 치명타 확률":
                    if (type.Contains("%"))
                    {
                        Agile.CriticalChance += (float)stat.GetStat(Stat_.Attack_Critical) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Agile.CriticalChance += effect_value * PlusMinus;
                    }
                    break;

                // Defence
                case "물리 방어":
                    if (type.Contains("%"))
                    {
                        Defence.PhysicalDefence += (float)stat.GetStat(Stat_.Attack_Defence) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Defence.PhysicalDefence += effect_value * PlusMinus;
                    }
                    break;
                case "물리 치명타 저항":
                    if (type.Contains("%"))
                    {
                        Defence.CriticalResistance += (float)stat.GetStat(Stat_.Attack_Critical_Resistance) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Defence.CriticalResistance += effect_value * PlusMinus;
                    }
                    break;
                case "물리 치명타 피격 피해":
                    if (type.Contains("%"))
                    {
                        Defence.CriticalDamageReduction += (float)stat.GetStat(Stat_.Attack_Critical_DamageReduction) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Defence.CriticalDamageReduction += effect_value * PlusMinus;
                    }
                    break;

                // Vitality
                case "HP":
                    if (type.Contains("%"))
                    {
                        Vitality.HP += (float)stat.GetStat(Stat_.Hp) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Vitality.HP += effect_value * PlusMinus;
                    }
                    break;
                case "MP":
                    if (type.Contains("%"))
                    {
                        Vitality.MP += (float)stat.GetStat(Stat_.Mp) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Vitality.MP += effect_value * PlusMinus;
                    }
                    break;
                case "이동 속도":
                    if (type.Contains("%"))
                    {
                        Vitality.MovementSpeed += (float)stat.GetStat(Stat_.MoveSpeed) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Vitality.MovementSpeed += effect_value * PlusMinus;
                    }
                    break;
                case "원기":
                    if (type.Contains("%"))
                    {
                        Vitality.Stamina += (float)stat.GetStat(Stat_.Stamina) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Vitality.Stamina += effect_value * PlusMinus;
                    }
                    break;

                // Magic
                case "마법 피해":
                    if (type.Contains("%"))
                    {
                        Magic.MagicDamage += (float)stat.GetStat(Stat_.Spell) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Magic.MagicDamage += effect_value * PlusMinus;
                    }
                    break;
                case "마법 치명타 피해":
                    if (type.Contains("%"))
                    {
                        Magic.CriticalMagicDamage += (float)stat.GetStat(Stat_.Spell_Critical_Damage) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Magic.CriticalMagicDamage += effect_value * PlusMinus;
                    }
                    break;

                // Wisdom
                case "마법 명중":
                    if (type.Contains("%"))
                    {
                        Wisdom.MagicHit += (float)stat.GetStat(Stat_.Spell_Hit_the_target) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Wisdom.MagicHit += effect_value * PlusMinus;
                    }
                    break;
                case "마법 속도":
                    if (type.Contains("%"))
                    {
                        Wisdom.MagicSpeed += (float)stat.GetStat(Stat_.Spell_Speed) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Wisdom.MagicSpeed += effect_value * PlusMinus;
                    }
                    break;
                case "마법 회피":
                    if (type.Contains("%"))
                    {
                        Wisdom.MagicDodge += (float)stat.GetStat(Stat_.Spell_Dodge) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Wisdom.MagicDodge += effect_value * PlusMinus;
                    }
                    break;
                case "마법 치명타 확률":
                    if (type.Contains("%"))
                    {
                        Wisdom.MagicCriticalChance += (float)stat.GetStat(Stat_.Spell_Critical) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Wisdom.MagicCriticalChance += effect_value * PlusMinus;
                    }
                    break;

                // MDefence
                case "마법 방어":
                    if (type.Contains("%"))
                    {
                        MDefence.MagicDefence += (float)stat.GetStat(Stat_.Spell_Defence) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        MDefence.MagicDefence += effect_value * PlusMinus;
                    }
                    break;
                case "마법 치명타 저항":
                    if (type.Contains("%"))
                    {
                        MDefence.MagicCriticalResistance += (float)stat.GetStat(Stat_.Spell_Critical_Resistance) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        MDefence.MagicCriticalResistance += effect_value * PlusMinus;
                    }
                    break;
                case "마법 치명타 피격 피해":
                    if (type.Contains("%"))
                    {
                        MDefence.MagicCriticalDamageReduction += (float)stat.GetStat(Stat_.Spell_Critical_DamageReduction) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        MDefence.MagicCriticalDamageReduction += effect_value * PlusMinus;
                    }
                    break;

                // Recovery
                case "HP 회복":
                    if (type.Contains("%"))
                    {
                        Recovery.HPRecovery += (float)stat.GetStat(Stat_.Hp_recovery) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Recovery.HPRecovery += effect_value * PlusMinus;
                    }
                    break;
                case "MP 회복":
                    if (type.Contains("%"))
                    {
                        Recovery.MPRecovery += (float)stat.GetStat(Stat_.Mp_Recovery) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Recovery.MPRecovery += effect_value * PlusMinus;
                    }
                    break;
                case "원기 회복":
                    if (type.Contains("%"))
                    {
                        Recovery.StaminaRecovery += (float)stat.GetStat(Stat_.Stamina_Recovery) * effect_value * 0.01f * PlusMinus;
                    }
                    else
                    {
                        Recovery.StaminaRecovery += effect_value * PlusMinus;
                    }
                    break;
                //Basic
                case "기술 재사용 시간":
                        Basic.Skill_Cooldown = effect_value * PlusMinus;
                        break;
                    case "방패 방어율":
                        Basic.Shield_Defense_Rate = effect_value * PlusMinus;
                        break;
                    case "활 속도":
                        Basic.Bow_Speed = effect_value * PlusMinus;
                        break;
                    case "활 사거리":
                        Basic.Bow_Range = effect_value * PlusMinus;
                        break;
                    case "석궁 사거리":
                        Basic.Crossbow_Range = effect_value * PlusMinus;
                        break;
                    case "상태이상 확률":
                        Basic.Status_Ailment_Probability = effect_value * PlusMinus;
                        break;
                    case "상태이상 저항":
                        Basic.Status_Ailment_Resistance = effect_value * PlusMinus;
                        break;
                    case "주문서 효과":
                        Basic.Scroll_Effects = effect_value * PlusMinus;
                        break;
                    case "물약 효과":
                        Basic.Potion_Effects = effect_value * PlusMinus;
                        break;
                    case "재료 채집 시 추가 획득 확률":
                        Basic.Additional_Gathering_Chance = effect_value * PlusMinus;
                        break;
                    case "Shard 종족 추가 획득":
                        Basic.Shard_Drop_Rate = effect_value * PlusMinus;
                        break;
                    case "몬스터에게 얻는 골드량":
                        Basic.Gold_From_Monsters = effect_value * PlusMinus;
                        break;
                    case "아이템 제작 시 골드 소모량":
                        Basic.Gold_Cost_For_Item_Crafting = effect_value * PlusMinus;
                        break;
                    case "강화 실패 시 재료 20%를 추가로 획득 확률":
                        Basic.Additional_Materials_On_Enhance_Failure = effect_value * PlusMinus;
                        break;
                    case "이동 시 HP 회복":
                        Basic.HP_Recovery_On_Move = effect_value * PlusMinus;
                        break;
                    case "치유 주문 시 추가 HP 회복":
                        Basic.Additional_HP_On_Healing_Spell = effect_value * PlusMinus;
                        break;
                    case "치유 관련 기술 MP":
                        Basic.MP_Cost_Of_Healing_Skills = effect_value * PlusMinus;
                        break;
                    case "공격으로 인한 시전 실패 확률":
                        Basic.Spell_Failure_Chance_From_Attack = effect_value * PlusMinus;
                        break;
                    case "공격 대상":
                        Basic.Attack_Target = effect_value * PlusMinus;
                        break;
                    case "피해 거리":
                        Basic.Damage_Range = effect_value * PlusMinus;
                        break;
                    // Race
                    case "적중 시 기절 효과 발동":
                        Race.Stun_Effect_on_Hit = effect_value * PlusMinus;
                        break;
                    case "모든 토템 효과":
                        Race.Totem_Effects = effect_value * PlusMinus;
                        break;
                    case "적 능력치":
                        Race.Enemy_Stats = effect_value * PlusMinus;
                        break;
                    case "가방 칸":
                        Race.Inventory_Slots = effect_value * PlusMinus;
                        break;
                    case "창고 칸":
                        Race.Warehouse_Slots = effect_value * PlusMinus;
                        break;
                    case "가방 무게":
                        Race.Inventory_Weight = effect_value * PlusMinus;
                        break;
                    // Weapon
                    case "방패의 물리 방어":
                        Weapon.Shield_Physical_Defense = effect_value * PlusMinus;
                        break;
                    case "피해의 일부를 MP로 회복":
                        Weapon.Damage_Recovery_to_MP = effect_value * PlusMinus;
                        break;
                    case "방패 방어율 최대치":
                        Weapon.Max_Shield_Defense_Rate = effect_value * PlusMinus;
                        break;
                    case "상태이상 시간":
                        Weapon.Status_Ailment_Duration = effect_value * PlusMinus;
                        break;
                    case "스킬 피해가 치명타로 적중 확률":
                        Weapon.Critical_Hit_Probability = effect_value * PlusMinus;
                        break;
                    case "기술 지속 시간":
                        Weapon.Skill_Duration = effect_value * PlusMinus;
                        break;
                    case "모든 피해":
                        Weapon.Total_Damage = effect_value * PlusMinus;
                        break;
                    // JobStats
                    case "재사용 대기중인 기술 20% 확률로 재사용 시간":
                        Job.Reuse_Cooldown_Chance = effect_value * PlusMinus;
                        break;
                    case "기술 성공 확률":
                        Job.Skill_Success_Probability = effect_value * PlusMinus;
                        break;
                    case "발동 시간":
                        Job.Activation_Time = effect_value * PlusMinus;
                        break;
                    case "모든 찌르기 속성 기술 성공 확률":
                        Job.Piercing_Skill_Success_Probability = effect_value * PlusMinus;
                        break;
                    case "소환수의 모든 능력":
                        Job.Minion_Abilities = effect_value * PlusMinus;
                        break;
                    case "물리 회피를 물리 방어로 전환":
                        Job.Phys_Evasion_to_Defense = effect_value * PlusMinus;
                        break;
                    case "지속 피해 마법 가속도":
                        Job.Cont_Damage_Magic_Accel = effect_value * PlusMinus;
                        break;
                    case "기절 내성":
                        Job.Stun_Resistance = effect_value * PlusMinus;
                        break;
                    case "활 내성":
                        Job.Bow_Resistance = effect_value * PlusMinus;
                        break;
                    case "소환수를 즉시 소환":
                        Job.Instant_Summoning_of_Minions = effect_value * PlusMinus;
                        break;
                    case "소환에 필요한 MP":
                        Job.MP_Required_for_Summoning = effect_value * PlusMinus;
                        break;
                    case "소환수 소환 재사용 대기 시간":
                        Job.Minion_Summon_Cooldown = effect_value * PlusMinus;
                        break;
                    default:
                        //포함되지 않은 스텟들 ex) 기술명*발동시간.
                        break;
                

            }
        }
    }

    public static void CalculateAndApplyToMyEntity(bool cancelRecord = false)
    {
        Debug.Log("ChStatCalculator.CalculateAndApplyToMyEntity()");

        if (!cancelRecord)
            Stat.RecordStat(Entity.myMainEntity.finalStat);

        // 캐릭터 스탯 상승
        SaveData.StatNumAll = new StatNumAlls();

        int Level = (int)Entity.myMainEntity.currentStat.GetStat(Stat_.Level);

        SaveData.StatNumAll.Strength = ChStatCalculator.CalculateStrength(Level, SaveData.StatLevels.Strength);

        SaveData.StatNumAll.Agile = ChStatCalculator.CalculateAgile(Level, SaveData.StatLevels.Agile);

        SaveData.StatNumAll.Defence = ChStatCalculator.CalculateDefence(Level, SaveData.StatLevels.Defence);

        SaveData.StatNumAll.Vitality = ChStatCalculator.CalculateVitality(Level, SaveData.StatLevels.Vitality);

        SaveData.StatNumAll.Magic = ChStatCalculator.CalculateMagic(Level, SaveData.StatLevels.Magic);

        SaveData.StatNumAll.Wisdom = ChStatCalculator.CalculateWisdom(Level, SaveData.StatLevels.Wisdom);

        SaveData.StatNumAll.MDefence = ChStatCalculator.CalculateMDefence(Level, SaveData.StatLevels.MDefence);

        SaveData.StatNumAll.Recovery = ChStatCalculator.CalculateRecovery(Level, SaveData.StatLevels.Recovery);


        // 스킬 스탯 상승
        SaveData.SkillStatNumAll = new StatNumAlls();

        for (int i = 0; i < SaveData.SkillPointlist.Count; i++)
        {
            if (SaveData.SkillPointlist[i].PageType2 == SkillType2.Passive)      //패시브 일 때
            {
                for (int n = 0; n < SaveData.SkillPointlist[i].skilleffects.Count; n++)
                {
                    //스킬 스텟에 갱신
                    SaveData.SkillStatNumAll.SetStats(
                        Entity.myMainEntity.entityStat,
                         SaveData.SkillPointlist[i].skilleffects[n].effect,
                         SaveData.SkillPointlist[i].skilleffects[n].effect_value,
                         SaveData.SkillPointlist[i].skilleffects[n].effect_type
                        );
                    //새로운 스텟 적용 (임시 데이터)
                }

            }
        }

        //새로고침
        Entity.myMainEntity.RefreshChStat_01();

        if (!cancelRecord)
            Stat.LogChangedStat(Entity.myMainEntity.finalStat);
    }

    public static StrengthStats CalculateStrength(int level, int strength)
    {
        StrengthStats statnum = new StrengthStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                //물리피해
                statnum.PhysicalDamage = (((level + 9) * 2f) + ((strength - 10) * 0.65f));                
                //치명타 피해
                statnum.CriticalDamage = (statnum.PhysicalDamage + (statnum.PhysicalDamage * 0.65f));
                break;
            case Race.Perrt:
                //물리피해
                statnum.PhysicalDamage = (((level + 14) * 2.5f) + ((strength - 15) * 0.8f));
                //치명타 피해
                statnum.CriticalDamage = (statnum.PhysicalDamage + (statnum.PhysicalDamage * 0.7f));
                break;
            case Race.Nix:
                //물리피해
                statnum.PhysicalDamage = (((level + 7) * 1.5f) + ((strength - 8) * 0.5f));
                //치명타 피해
                statnum.CriticalDamage = (statnum.PhysicalDamage + (statnum.PhysicalDamage * 0.6f));
                break;
            case Race.Shard:
                //물리피해
                statnum.PhysicalDamage = (((level + 8) * 1.75f) + ((strength - 9) * 0.58f));
                //치명타 피해
                statnum.CriticalDamage = (statnum.PhysicalDamage + (statnum.PhysicalDamage * 0.65f));
                break;
            default:
                break;
        }

        

        return statnum;
    }

    public static AgileStats CalculateAgile(int level, int agile)
    {
        AgileStats statnum = new AgileStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                // 물리명중
                statnum.PhysicalHit = (((level + 9) * 0.75f) + ((agile - 10) * 0.3f));
                // 공격속도
                statnum.AttackSpeed = (((level + 9) * 0.5f) + ((agile - 10) * 0.2f));
                // 물리 회피
                statnum.PhysicalDodge = (((level + 9) * 0.7f) + ((agile - 10) * 0.25f));
                // 치명타 확률
                statnum.CriticalChance = (((level + 9) * 0.3f) + ((agile - 10) * 0.1f));
                break;
            case Race.Perrt:
                // 물리명중
                statnum.PhysicalHit = (((level + 7) * 0.55f) + ((agile - 8) * 0.25f));
                // 공격속도
                statnum.AttackSpeed = (((level + 7) * 0.4f) + ((agile - 8) * 0.15f));
                // 물리 회피
                statnum.PhysicalDodge = (((level + 7) * 0.5f) + ((agile - 8) * 0.2f));
                // 치명타 확률
                statnum.CriticalChance = (((level + 7) * 0.25f) + ((agile - 8) * 0.08f));
                break;
            case Race.Nix:
                // 물리명중
                statnum.PhysicalHit = (((level + 12) * 0.85f) + ((agile - 13) * 0.35f));
                // 공격속도
                statnum.AttackSpeed = (((level + 12) * 0.6f) + ((agile - 13) * 0.25f));
                // 물리 회피
                statnum.PhysicalDodge = (((level + 12) * 0.8f) + ((agile - 13) * 0.35f));
                // 치명타 확률
                statnum.CriticalChance = (((level + 12) * 0.35f) + ((agile - 13) * 0.12f));
                break;
            case Race.Shard:
                // 물리명중
                statnum.PhysicalHit = (((level + 7) * 0.65f) + ((agile - 8) * 0.28f));
                // 공격속도
                statnum.AttackSpeed = (((level + 7) * 0.45f) + ((agile - 8) * 0.18f));
                // 물리 회피
                statnum.PhysicalDodge = (((level + 7) * 0.6f) + ((agile - 8) * 0.23f));
                // 치명타 확률
                statnum.CriticalChance = (((level + 7) * 0.28f) + ((agile - 8) * 0.09f));
                break;
            default:
                break;
        }

        return statnum;
    }

    public static DefenceStats CalculateDefence(int level, int defence)
    {
        DefenceStats statnum = new DefenceStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                // 물리방어
                statnum.PhysicalDefence = ((level + 9) * 8.6f) + ((defence - 10) * 2.9f);
                // 치명타 저항률
                statnum.CriticalResistance = (((level + 9) * 0.2f) + ((defence - 10) * 0.06f));
                // 치명 피해 감소
                statnum.CriticalDamageReduction = (statnum.PhysicalDefence * 0.15f);
                break;
            case Race.Perrt:
                // 물리방어
                statnum.PhysicalDefence = ((level + 7) * 6.5f) + ((defence - 8) * 2.2f);
                // 치명타 저항률
                statnum.CriticalResistance = (((level + 9) * 0.12f) + ((defence - 10) * 0.04f));
                // 치명 피해 감소
                statnum.CriticalDamageReduction = (statnum.PhysicalDefence * 0.15f);
                break;
            case Race.Nix:
                // 물리방어
                statnum.PhysicalDefence = ((level + 7) * 6f) + ((defence - 8) * 2f);
                // 치명타 저항률
                statnum.CriticalResistance = (((level + 9) * 0.22f) + ((defence - 10) * 0.0073f));
                // 치명 피해 감소
                statnum.CriticalDamageReduction = (statnum.PhysicalDefence * 0.15f);
                break;
            case Race.Shard:
                // 물리방어
                statnum.PhysicalDefence = ((level + 8) * 7f) + ((defence - 9) * 2.3f);
                // 치명타 저항률
                statnum.CriticalResistance = (((level + 9) * 0.1f) + ((defence - 10) * 0.0033f));
                // 치명 피해 감소
                statnum.CriticalDamageReduction = (statnum.PhysicalDefence * 0.15f);
                break;
            default:
                break;
        }

        return statnum;
    }

    public static VitalityStats CalculateVitality(int level, int vitality)
    {
        VitalityStats statnum = new VitalityStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                // Hp
                statnum.HP = (((level + 9) * 50f) + ((vitality - 10) * 8f));
                // Mp
                statnum.MP = (((level + 9) * 15f) + ((vitality - 10) * 5f));
                // 이동속도
                statnum.MovementSpeed = (((level + 9) * 0.55f) + ((vitality - 10) * 0.183f));
                // 스테미나
                statnum.Stamina = (((level + 9) * 1.5f) + ((vitality - 10) * 0.5f));
                break;
            case Race.Perrt:
                // Hp
                statnum.HP = (((level + 8) * 47f) + ((vitality - 9) * 6f));
                // Mp
                statnum.MP = (((level + 8) * 12f) + ((vitality - 9) * 4f));
                // 이동속도
                statnum.MovementSpeed = (((level + 8) * 0.45f) + ((vitality - 9) * 0.15f));
                // 스테미나
                statnum.Stamina = (((level + 8) * 1.7f) + ((vitality - 9) * 0.567f));
                break;
            case Race.Nix:
                // Hp
                statnum.HP = (((level + 10) * 48f) + ((vitality - 11) * 7f));
                // Mp
                statnum.MP = (((level + 10) * 17f) + ((vitality - 11) * 5.66f));
                // 이동속도
                statnum.MovementSpeed = (((level + 10) * 0.65f) + ((vitality - 11) * 0.217f));
                // 스테미나
                statnum.Stamina = (((level + 10) * 1.3f) + ((vitality - 11) * 0.433f));
                break;
            case Race.Shard:
                // Hp
                statnum.HP = (((level + 13) * 53f) + ((vitality - 14) * 8f));
                // Mp
                statnum.MP = (((level + 13) * 14f) + ((vitality - 14) * 4.66f));
                // 이동속도
                statnum.MovementSpeed = (((level + 13) * 0.5f) + ((vitality - 14) * 0.167f));
                // 스테미나
                statnum.Stamina = (((level + 13) * 1.9f) + ((vitality - 14) * 0.633f));
                break;
            default:
                break;
        }

        return statnum;
    }

    public static MagicStats CalculateMagic(int level, int magic)
    {
        MagicStats statnum = new MagicStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                // 마법 피해
                statnum.MagicDamage = (((level + 9) * 2.1f) + ((magic - 10) * 0.7f));
                // 치명타 피해
                statnum.CriticalMagicDamage = (statnum.MagicDamage + (statnum.MagicDamage * 0.65f));
                break;
            case Race.Perrt:
                // 마법 피해
                statnum.MagicDamage = (((level + 14) * 2.63f) + ((magic - 15) * 0.85f));
                // 치명타 피해
                statnum.CriticalMagicDamage = (statnum.MagicDamage + (statnum.MagicDamage * 0.7f));
                break;
            case Race.Nix:
                // 마법 피해
                statnum.MagicDamage = (((level + 7) * 1.65f) + ((magic - 8) * 0.55f));
                // 치명타 피해
                statnum.CriticalMagicDamage = (statnum.MagicDamage + (statnum.MagicDamage * 0.6f));
                break;
            case Race.Shard:
                // 마법 피해
                statnum.MagicDamage = (((level + 8) * 1.92f) + ((magic - 9) * 0.85f));
                // 치명타 피해
                statnum.CriticalMagicDamage = (statnum.MagicDamage + (statnum.MagicDamage * 0.65f));
                break;
            default:
                break;
        }

        

        return statnum;
    }

    public static WisdomStats CalculateWisdom(int level, int wisdom)
    {
        WisdomStats statnum = new WisdomStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                // 마법명중
                statnum.MagicHit = (((level + 9) * 0.75f) + ((wisdom - 10) * 0.3f));
                // 마법속도
                statnum.MagicSpeed = (((level + 9) * 0.5f) + ((wisdom - 10) * 0.2f));
                // 마법 회피
                statnum.MagicDodge = (((level + 9) * 0.7f) + ((wisdom - 10) * 0.25f));
                // 치명타 확률
                statnum.MagicCriticalChance = (((level + 9) * 0.3f) + ((wisdom - 10) * 0.1f));
                break;
            case Race.Perrt:
                // 마법명중
                statnum.MagicHit = (((level + 7) * 0.55f) + ((wisdom - 8) * 0.25f));
                // 마법속도
                statnum.MagicSpeed = (((level + 7) * 0.4f) + ((wisdom - 8) * 0.15f));
                // 마법 회피
                statnum.MagicDodge = (((level + 7) * 0.5f) + ((wisdom - 8) * 0.2f));
                // 치명타 확률
                statnum.MagicCriticalChance = (((level + 7) * 0.25f) + ((wisdom - 8) * 0.08f));
                break;
            case Race.Nix:
                // 마법명중
                statnum.MagicHit = (((level + 12) * 0.85f) + ((wisdom - 13) * 0.35f));
                // 마법속도
                statnum.MagicSpeed = (((level + 12) * 0.6f) + ((wisdom - 13) * 0.25f));
                // 마법 회피
                statnum.MagicDodge = (((level + 12) * 0.8f) + ((wisdom - 13) * 0.35f));
                // 치명타 확률
                statnum.MagicCriticalChance = (((level + 12) * 0.35f) + ((wisdom - 13) * 0.12f));
                break;
            case Race.Shard:
                // 마법명중
                statnum.MagicHit = (((level + 7) * 0.65f) + ((wisdom - 8) * 0.28f));
                // 마법속도
                statnum.MagicSpeed = (((level + 7) * 0.45f) + ((wisdom - 8) * 0.18f));
                // 마법 회피
                statnum.MagicDodge = (((level + 7) * 0.6f) + ((wisdom - 8) * 0.23f));
                // 치명타 확률
                statnum.MagicCriticalChance = (((level + 7) * 0.28f) + ((wisdom - 8) * 0.09f));
                break;
            default:
                break;
        }

        

        return statnum;
    }
    public static MDefenceStats CalculateMDefence(int level, int mDefence)
    {
        MDefenceStats statnum = new MDefenceStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                // 마법방어
                statnum.MagicDefence = (((level + 9) * 8.3f) + ((mDefence - 10) * 2.8f));
                // 치명타 저항률
                statnum.MagicCriticalResistance = (((level + 9) * 0.18f) + ((mDefence - 10) * 0.06f));
                // 치명 피해 감소
                statnum.MagicCriticalDamageReduction = (statnum.MagicDefence * 0.15f);
                break;
            case Race.Perrt:
                // 마법방어
                statnum.MagicDefence = (((level + 7) * 6f) + ((mDefence - 8) * 2f));
                // 치명타 저항률
                statnum.MagicCriticalResistance = (((level + 9) * 0.14f) + ((mDefence - 10) * 0.05f));
                // 치명 피해 감소
                statnum.MagicCriticalDamageReduction = (statnum.MagicDefence * 0.15f);
                break;
            case Race.Nix:
                // 마법방어
                statnum.MagicDefence = (((level + 7) * 9f) + ((mDefence - 8) * 3f));
                // 치명타 저항률
                statnum.MagicCriticalResistance = (((level + 9) * 0.24f) + ((mDefence - 10) * 0.08f));
                // 치명 피해 감소
                statnum.MagicCriticalDamageReduction = (statnum.MagicDefence * 0.15f);
                break;
            case Race.Shard:
                // 마법방어
                statnum.MagicDefence = (((level + 8) * 6.5f) + ((mDefence - 9) * 2.15f));
                // 치명타 저항률
                statnum.MagicCriticalResistance = (((level + 9) * 0.11f) + ((mDefence - 10) * 0.036f));
                // 치명 피해 감소
                statnum.MagicCriticalDamageReduction = (statnum.MagicDefence * 0.15f);
                break;
            default:
                break;
        }

        

        return statnum;
    }
    public static RecoveryStats CalculateRecovery(int level, int recovery)
    {
        RecoveryStats statnum = new RecoveryStats();

        switch (SaveData.GetRace())
        {
            case Race.Kelts:
                // Hp 회복
                statnum.HPRecovery = (((level + 9) * 0.23f) + ((recovery - 10) * 0.23f)) / 3f;          
                // Mp 회복
                statnum.MPRecovery = (((level + 9) * 0.07f) + ((recovery - 10) * 0.07f) ) / 3f;
                // 스테미나 회복
                statnum.StaminaRecovery = (((level + 9) * 0.025f) + ((recovery - 10) * 0.025f) ) / 3f;
                break;
            case Race.Perrt:
                // Hp 회복
                statnum.HPRecovery = (((level + 8) * 0.21f) + ((recovery - 9) * 0.21f) ) / 3f;
                // Mp 회복
                statnum.MPRecovery = (((level + 8) * 0.05f) + ((recovery - 9) * 0.05f) ) / 3f;
                // 스테미나 회복
                statnum.StaminaRecovery = (((level + 8) * 0.018f) + ((recovery - 9) * 0.018f) ) / 3f;
                break;
            case Race.Nix:
                // Hp 회복
                statnum.HPRecovery = (((level + 10) * 0.25f) + ((recovery - 11) * 0.25f) ) / 3f;
                // Mp 회복
                statnum.MPRecovery = (((level + 10) * 0.09f) + ((recovery - 11) * 0.09f) ) / 3f;
                // 스테미나 회복
                statnum.StaminaRecovery = (((level + 10) * 0.022f) + ((recovery - 11) * 0.022f) ) / 3f;
                break;
            case Race.Shard:
                // Hp 회복
                statnum.HPRecovery = (((level + 13) * 0.22f) + ((recovery - 14) * 0.22f) ) / 3f;
                // Mp 회복
                statnum.MPRecovery = (((level + 13) * 0.05f) + ((recovery - 14) * 0.05f) ) / 3f;
                // 스테미나 회복
                statnum.StaminaRecovery = (((level + 13) * 0.03f) + ((recovery - 14) * 0.03f) ) / 3f;
                break;
            default:
                break;
        }

        return statnum;
    }
}