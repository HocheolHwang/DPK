using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayer : TutorialBase
{
    private PlayerZone  playerZone;

    [SerializeField] 
    private bool        stopPlayerMovement = false;  // 플레이어 움직임을 멈추기 위한 플래그
    [SerializeField] 
    private bool        startPlayerMovement = false; // 플레이어 움직임을 시작하기 위한 플래그

    public override void Enter()
    {
        playerZone = FindObjectOfType<PlayerZone>();
        if (playerZone != null)
        {
            // 체크박스의 상태에 따라 플레이어의 움직임을 멈추거나 시작
            if (stopPlayerMovement)
            {
                playerZone.StopMovement();
                Debug.Log("플레이어 움직임을 멈춤");
            }

            if (startPlayerMovement)
            {
                playerZone.StartMovement();
            }
        }
    }

    public override void Execute(TutorialController controller)
    {
        controller.SetNextTutorial();
    }

    public override void Exit()
    {
        
    }

    
}
