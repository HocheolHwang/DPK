using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PanelState
{
    public GameObject[] pausePanels;
    public int currentPanelIndex = 0;
}

public class TutorialPause : MonoBehaviour
{
    public PanelState panelState;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // 'Player' 태그를 가진 오브젝트가 트리거에 들어왔을 때
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        if (panelState.pausePanels.Length > 0)
        {
            panelState.pausePanels[panelState.currentPanelIndex].SetActive(true);
        }
    }

    public void ResumeGame()
    {
        foreach (var panel in panelState.pausePanels)
        {
            panel.SetActive(false);
        }
        Time.timeScale = 1;
        panelState.currentPanelIndex = 0;
    }

    public void ShowNextPanel()
    {
        if (panelState.currentPanelIndex < panelState.pausePanels.Length)
        {
            panelState.pausePanels[panelState.currentPanelIndex].SetActive(false);
        }

        panelState.currentPanelIndex++;

        if (panelState.currentPanelIndex < panelState.pausePanels.Length)
        {
            panelState.pausePanels[panelState.currentPanelIndex].SetActive(true);
        }
        else
        {
            panelState.currentPanelIndex = 0;  // Reset index
            ResumeGame();
        }
    }
}
