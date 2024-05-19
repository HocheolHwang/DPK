using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MummyBufferAnimationData : MonsterAnimationData
{
    [Header("Mummy Buffer Animation Data")]
    [SerializeField] private string buffParamName = "BUFF";
    [SerializeField] private string counterEnableParamName = "COUNTER_ENABLE";
    [SerializeField] private string groggyParamName = "GROGGY";

    [SerializeField] private AnimationClip buffAnim;
    [SerializeField] private AnimationClip counterEnableAnim;
    [SerializeField] private AnimationClip groggyAnim;

    public int BuffParamHash { get; private set; }
    public int CounterEnableParamHash { get; private set; }
    public int GroggyParamHash { get; private set; }
    public AnimationClip BuffAnim { get => buffAnim; }
    public AnimationClip CounterEnableAnim { get => buffAnim; }
    public AnimationClip GroggyAnim { get => groggyAnim; }

    public override void StringAnimToHash()
    {
        base.StringAnimToHash();

        BuffParamHash = Animator.StringToHash(buffParamName);
        CounterEnableParamHash = Animator.StringToHash(counterEnableParamName);
        GroggyParamHash = Animator.StringToHash(groggyParamName);
    }
}
