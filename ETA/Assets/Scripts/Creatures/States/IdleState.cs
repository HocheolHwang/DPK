using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NormalMonster
{
    public class IdleState : State
    {
        [Header("해당 상태에서 사용할 속성")]
        [SerializeField] public AnimationClip anim;

        public override void Enter()
        {
        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
        }


    }
}

