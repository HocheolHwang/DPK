using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public DungeonProgress dungeonProgress;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        if (other.CompareTag("Player"))  // 플레이어와의 충돌 감지
        {
            dungeonProgress.UpdateProgress();
            Debug.Log("Progress updated.");
        }
    }
}
