using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MummyManAnimationData : MonsterAnimationData
{
    // ---------------------------- Animation Clip Name -----------------------------------
    [Header("Mummy Man Animation Data")]

    [SerializeField] protected string groggyParamName = "GROGGY";

    // 아무도 죽지 않은 경우 가지는 자동 공격 및 스킬
    [SerializeField] protected string throwParamName = "THROW";
    [SerializeField] protected string shoutingParamName = "SHOUTING";               // 원거리 몬스터가 죽은 경우 FORE -> SHOUT -> RUSH -> 제자리
    [SerializeField] protected string jumpParamName = "JUMP";
    

    // 원거리 몬스터가 죽은 경우
    [SerializeField] protected string foreShadowingParamName = "FORE_SHADOWING";
    [SerializeField] protected string rushParamName = "RUSH";

    // 근거리 몬스터가 죽은 경우
    [SerializeField] protected string windMillParamName = "WIND_MILL";              // 근거리 몬스터가 죽은 경우 JUMP -> WIND -> 제자리

    [SerializeField] protected string clapParamName = "CLAP";                       // 처음 플레이어와 조우했을 때, 일행을 소환한다.
    

    public int GroggyParamHash { get; private set; }
    public int ClapParamHash { get; private set; }
    public int JumpParamHash { get; private set; }
    public int RushParamHash { get; private set; }
    public int ShoutingParamHash { get; private set; }
    public int ThrowParamHash { get; private set; }
    public int WindMillParamHash { get; private set; }
    public int ForeShadowingParamHash { get; private set; }


    // ---------------------------- Animation Clip -----------------------------------
    //Skill
    [SerializeField] private AnimationClip clapAnim;
    [SerializeField] private AnimationClip jumpAnim;
    [SerializeField] private AnimationClip rushAnim;
    [SerializeField] private AnimationClip shoutingAnim;
    [SerializeField] private AnimationClip throwAnim;
    [SerializeField] private AnimationClip windMillAnim;
    [SerializeField] private AnimationClip foreShadowingAnim;

    public AnimationClip ClapAnim { get => clapAnim; }
    public AnimationClip JumpAnim { get => jumpAnim; }
    public AnimationClip RushAnim { get => rushAnim; }
    public AnimationClip ShoutingAnim { get => shoutingAnim; }
    public AnimationClip ThrowAnim { get => throwAnim; }
    public AnimationClip WindMillAnim { get => windMillAnim; }
    public AnimationClip ForeShadowingAnim { get => foreShadowingAnim; }


    public override void StringAnimToHash()
    {
        base.StringAnimToHash();

        GroggyParamHash = Animator.StringToHash(groggyParamName);
        // Skill
        ClapParamHash = Animator.StringToHash(clapParamName);
        JumpParamHash = Animator.StringToHash(jumpParamName);
        RushParamHash = Animator.StringToHash(rushParamName);
        ShoutingParamHash = Animator.StringToHash(shoutingParamName);
        ThrowParamHash = Animator.StringToHash(throwParamName);
        WindMillParamHash = Animator.StringToHash(windMillParamName);
        ForeShadowingParamHash = Animator.StringToHash(foreShadowingParamName);
    }
}
