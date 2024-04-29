using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPause : MonoBehaviour
{
    public GameObject[] pausePanels; // 인스펙터에서 할당할 UI 팝업 배열
    private int currentPanelIndex = 0; // 현재 표시된 팝업의 인덱스

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // 'Player' 태그를 가진 오브젝트가 트리거에 들어왔을 때
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;  // 게임 일시 정지
        if (pausePanels.Length > 0)
        {
            pausePanels[currentPanelIndex].SetActive(true); // 첫 번째 UI 팝업 활성화
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1; // 게임 재개
        if (currentPanelIndex < pausePanels.Length)
        {
            pausePanels[currentPanelIndex].SetActive(false); // 현재 팝업 비활성화
        }
    }

    public void ShowNextPanel()
    {
        // 현재 패널을 비활성화
        if (currentPanelIndex < pausePanels.Length)
        {
            pausePanels[currentPanelIndex].SetActive(false);
        }

        // 인덱스 증가
        currentPanelIndex++;

        // 다음 패널 활성화
        if (currentPanelIndex < pausePanels.Length)
        {
            pausePanels[currentPanelIndex].SetActive(true);
        }
        else
        {
            // 모든 패널을 보여준 후 게임을 재개
            ResumeGame();
        }
    }
}
