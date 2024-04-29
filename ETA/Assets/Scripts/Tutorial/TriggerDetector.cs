using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public TutorialTrigger tutorialTrigger;  // 인스펙터에서 TutorialTrigger 컴포넌트를 할당해야 합니다.

    private void OnTriggerEnter(Collider other)
    {
        // 트리거에 플레이어가 닿았다고 가정합니다.
        // 여기서 "Player"는 플레이어 오브젝트의 태그를 가정한 것입니다.
        if (other.CompareTag("Player"))
        {
            tutorialTrigger.isTrigger = true;
        }
    }
}
