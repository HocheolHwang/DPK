using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Data
{
    [SerializeField]    // 이름
    public string name;
    [SerializeField]    // 시전 시간(콜라보 스킬 아니면 즉발 발동)
    public float casting;
    [SerializeField]    // 쿨타임
    public float cooldown;
    [SerializeField]
    public int damage;
    [SerializeField]    // 아이콘
    public Sprite icon;
    [SerializeField]    // 이펙트
    public GameObject effect;
    [SerializeField]    // 애니메이션
    public string animator;
    public string description;

    public float lastCastTime { get; set; }
    public Transform root { get; set; }
}

public abstract class ISkill : MonoBehaviour
{
    public Data data;
    public abstract void InitSetting();
    public abstract void Using();
}
