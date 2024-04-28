using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Speaker { player = 0,  }

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Dialog[] dialogs;                       // 현재 분기의 대사 목록
    [SerializeField]
    private GameObject imageDialog;                   // 대화창 Image UI
    [SerializeField]
    private TextMeshProUGUI textName;                        // 현재 대사중인 캐릭터 이름 출력 Text UI
    [SerializeField]
    private TextMeshProUGUI textDialogue;                    // 현재 대사 출력 Text UI
    [SerializeField]
    private GameObject objectArrow;                  // 대사가 완료되었을 때 출력되는 커서 오브젝트
    [SerializeField]
    private float typingSpeed;                  // 텍스트 타이핑 효과의 재생 속도
    [SerializeField]
    private KeyCode keyCodeSkip = KeyCode.Space;    // 타이핑 효과를 스킵하는 키

    private int currentIndex = -1;
    private bool isTypingEffect = false;            // 텍스트 타이핑 효과를 재생중인지

    public void Setup()
    {
        SetNextDialog();
    }

    public bool UpdateDialog()
    {
        if (Input.GetKeyDown(keyCodeSkip) || Input.GetMouseButtonDown(0))
        {
            // 텍스트 타이핑 효과를 재생중일때 마우스 왼쪽 클릭하면 타이핑 효과 종료
            if (isTypingEffect == true)
            {
                // 타이핑 효과를 중지하고, 현재 대사 전체를 출력한다
                StopCoroutine("TypingText");
                isTypingEffect = false;
                textDialogue.text = dialogs[currentIndex].dialogue;
                // 대사가 완료되었을 때 출력되는 커서 활성화
                objectArrow.SetActive(true);

                return false;
            }

            // 다음 대사 진행
            if (dialogs.Length > currentIndex + 1)
            {
                SetNextDialog();
            }
            // 대사가 더 이상 없을 경우 true 반환
            else
            {
                imageDialog.SetActive(false);
                textName.gameObject.SetActive(false);
                textDialogue.gameObject.SetActive(false);
                objectArrow.SetActive(false);

                return true;
            }
        }

        return false;
    }


    private void SetNextDialog()
    {
        currentIndex++;

        // 대화창 활성화
        imageDialog.SetActive(true);

        // 화자 이름 텍스트 활성화 및 설정
        textName.text = dialogs[currentIndex].speaker.ToString();
        textName.gameObject.SetActive(true);

        // 대사 텍스트 활성화 및 설정 (Typing Effect)
        textDialogue.gameObject.SetActive(true);
        StartCoroutine(nameof(TypingText));

        // 대화창 활성화
        imageDialog.SetActive(true);

    }


    private IEnumerator TypingText()
    {
        int index = 0;

        isTypingEffect = true;

        string fullText = dialogs[currentIndex].dialogue;

        // 텍스트를 한글자씩 타이핑치듯 재생
        while (index < fullText.Length)
        {
            textDialogue.text = dialogs[currentIndex].dialogue.Substring(0, index);

            index++;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;

        // 대사가 완료되었을 때 출력되는 커서 활성화
        objectArrow.SetActive(true);
    }
}

[System.Serializable]
public struct Dialog
{
    public Speaker speaker; // 화자
    [TextArea(3, 5)]
    public string dialogue;	// 대사
}

