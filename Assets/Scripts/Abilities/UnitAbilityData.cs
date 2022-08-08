using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType 
    {
        RandomTarget,
        AllTargets
    }
    public enum AbilityType 
    {
        Damage,
        Heal
    }
    [System.Serializable]
    public class Segment
    {
        public int minInclusive;
        public int maxExclusive;

        public Segment(int min, int max)
        {
            this.minInclusive = min;
            this.maxExclusive = max;
        }
    }
[CreateAssetMenu(fileName = "UnitData_NAME_Ability_", menuName = "Dragon Crashers/Unit/Ability Data", order = 2)]
public class UnitAbilityData : ScriptableObject
{
    
    [Header("Name of Ability")]
    public string abilityName;

    [Header("CoolDown Time")]
    public float coolDownTime;

    [Header("DamageType")]
    public DamageType damageType;

    [Header("DamageRange")]
    public Segment segment;

    public int GetRandomDamageInRange() 
    {
        return Random.Range(segment.minInclusive, segment.maxExclusive);
    }
}
