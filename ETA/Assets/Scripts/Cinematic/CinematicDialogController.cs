using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class CinematicDialogController : MonoBehaviour
{
    [SerializeField]
    private GameObject      dialogPanel; // 대화 패널 UI
    [SerializeField]
    private TextMeshProUGUI textName;   // 캐릭터 이름을 표시할 텍스트
    [SerializeField]
    private TextMeshProUGUI textDialogue;   // 대사를 표시할 텍스트

    [Header("Dialogue Settings")]
    public string characterName;  // 인스펙터에서 설정할 캐릭터 이름
    [TextArea(3, 5)]
    public string dialogueText;   // 인스펙터에서 설정할 대사

    void Start()
    {
        HideDialogue(); // 게임 시작 시 대화창을 숨깁니다.
    }

    // 대화 UI를 화면에 보여주는 함수
    public void ShowDialogue()
    {
        textName.text = characterName;
        textDialogue.text = dialogueText;
        dialogPanel.SetActive(true);
    }


    // 대화 UI를 화면에서 숨기는 함수
    public void HideDialogue()
    {
        dialogPanel.SetActive(false);
    }
}
