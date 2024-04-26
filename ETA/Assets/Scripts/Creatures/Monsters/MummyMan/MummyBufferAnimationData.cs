using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MummyBufferAnimationData : MonsterAnimationData
{
    [Header("Mummy Buffer Animation Data")]
    [SerializeField] private string buffParamName = "BUFF";
    [SerializeField] private AnimationClip buffAnim;

    public int BuffParamHash { get; private set; }
    public AnimationClip BuffAnim { get => buffAnim; }

    public override void StringAnimToHash()
    {
        base.StringAnimToHash();

        BuffParamHash = Animator.StringToHash(buffParamName);
    }
}
