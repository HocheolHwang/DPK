using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum SkillType
    {
        None,
        Target,
        Range,
        Holding,
        Immediately

    }
    public enum SkillKey
    {
        Q,
        W,
        E,
        R,
        A,
        S,
        D,
        F
    }

    public enum Sound
    {
        BGM,
        Effect,
        MaxCount
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Opening,
        Tutorial,
        DeepForest,
        ForgottenTemple,
        StarShardPlain,
    }
    public enum UIEvent
    {
        Click,
        Drag,
    }
    public enum MouseEvent
    {
        Press,
        Click
    }
    public enum CameraMode
    {
        QuarterVeiw
    }

    public enum  UnitType
    {
        // ------------------- 플레이어 -----------------------
        Player = 0,
        // ------------------- 일반 몬스터 -----------------------
        Boar,
        Tree,
        Porin,
        Shaga,
        Butterfly,
        // ------------------- 보스 몬스터 -----------------------
        FlowerDryad,
        KnightG,
        MummyMan,
        MummyManWarrior,
        MummyManBuffer,
    }

    public enum Effect
    {
        WarriorNormalAttackEffect,
        KnightG_CounterEnable,
        KnightG_CounterAttack,
        KnightG_TwoSkillEnergy,
        KnightG_TwoSkillAttack,
        KnightG_PhaseTransition,
        KnightG_PhaseAttack,
        Groggy,
        SwordWaveWhite,
        StoneSlash1,
        StoneSlash2,
        SurfaceExplosionDirtStone,
        WhirlwindEffect1,
        WhirlwindEffect2,
        Porin_Attack,
        MaxCount
    }
}
