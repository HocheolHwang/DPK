using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Dungeon_Popup_UI dungeonPopupUI;

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        if (other.CompareTag("Player"))  // 플레이어와의 충돌 감지
        {
            if (dungeonPopupUI == null)
            {
                dungeonPopupUI = FindObjectOfType<Dungeon_Popup_UI>();
                if (dungeonPopupUI == null)
                {
                    Debug.LogError("Dungeon_Popup_UI component not found in the scene.");
                    return; // dungeonPopupUI를 찾지 못하면 함수를 더 이상 실행하지 않음
                }
            }

            dungeonPopupUI.UpdateProgress();
            Debug.Log("Progress updated.");
        }

        
    }
}
