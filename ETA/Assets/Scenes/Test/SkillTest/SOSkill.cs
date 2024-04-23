using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOSkill : ScriptableObject
{
    public float damage;
    public float cool;

    public string animationName;
    public GameObject effectName;
    public Sprite icon;
}
