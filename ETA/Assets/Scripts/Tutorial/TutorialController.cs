using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    // 순차적으로 진행할 튜토리얼
    [SerializeField]
    private List<TutorialBase>  tutorials;

    // 튜토리얼이 끝나면 넘어갈 씬 이름
    [SerializeField]
    private string              nextSceneName = "";

    // 현재 진행하고 있는 튜토리얼 
    private TutorialBase        currentTutorial = null;
    private int                 currentIndex = -1;

    private void Start()
    {
        SetNextTutorial();
    }

    private void Update()
    {
        if (currentTutorial != null) 
        {
            currentTutorial.Execute(this);
        }
    }


    public void SetNextTutorial()
    {
        // 현재 튜토리얼의 Exit() 메소드 호출
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        // 마지막 튜토리얼을 진행했으면 CompletedAllTutorials() 메소드 호출
        if (currentIndex >= tutorials.Count-1)
        {
            CompletedAllTutorials();
            return;
        }

        // 튜토리얼이 남아있으면 다음 튜토리얼 과정을 currentTutorial로 등록
        currentIndex ++;
        currentTutorial = tutorials[currentIndex];

        // 새로 바뀐 튜토리얼의 Enter() 메소드 호출
        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;

        // 행동 양식이 여러 종류가 되었을 때 코드 추가 작성
        // 현재는 씬 전환
        Debug.Log("Complete All!");

        if ( !nextSceneName.Equals(""))
        {
            // Scene 이동 전에 모든 팝업 창 닫은 뒤 이동
            Managers.UI.CloseAllPopupUI();
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
