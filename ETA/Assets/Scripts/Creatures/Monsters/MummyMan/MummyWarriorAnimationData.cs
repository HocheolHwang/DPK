using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MummyWarriorAnimationData : MonsterAnimationData
{
    [Header("Mummy Warrior Animation Data")]
    [SerializeField] private string windMillParamName = "WIND_MILL";
    [SerializeField] private AnimationClip windMillAnim;

    public int WindMillParamHash { get; private set; }
    public AnimationClip WindMillAnim { get => windMillAnim; }

    public override void StringAnimToHash()
    {
        base.StringAnimToHash();

        WindMillParamHash = Animator.StringToHash(windMillParamName);
    }
}
