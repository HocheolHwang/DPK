using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillSO : ScriptableObject
{
    public string SkillName;
    public string SkillDescription;
    public string SkillCode;
    public Define.SkillType SkillType;
    public int Damage;
    public float CoolDownTime;
    public Sprite Icon;
}
