using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonProgress : MonoBehaviour
{
    
    public Slider progressBar;
    public Transform[] checkpoints;
    private int totalCheckpoints;
    private int currentCheckpointIndex = 0;

    void Start()
    {
        totalCheckpoints = checkpoints.Length;  // 씬에 있는 모든 체크포인트 개수 계산
        progressBar.value = 0;
    }

    public void UpdateProgress()
    {
        if (currentCheckpointIndex < totalCheckpoints)
        {
            currentCheckpointIndex++;  // 체크포인트 통과시 인덱스 증가
            // 체크포인트 인덱스에 따라 진행바를 업데이트
            progressBar.value = (float)currentCheckpointIndex / totalCheckpoints;
            Debug.Log("Progress Updated: " + progressBar.value * 100 + "%");
        }
    }
}
