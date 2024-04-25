using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Data
{
    public string name;
    public float castTime;
    public float cooldown;
    public int damage;
    public Sprite icon;
    public GameObject effect;
    public string animator;
}

public abstract class ISkill : MonoBehaviour
{
    public Data data;
    public abstract void InitSetting();
    public abstract void Using();
}
