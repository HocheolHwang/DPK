using UnityEngine;

/// <summary>
/// 게임을 종료하는 컴포넌트
/// 에디터에서 실행 중인 경우 UnityEditor를 이용하여 종료하고,
/// 에디터가 아닌 경우 Application.Quit()을 호출하여 어플리케이션을 종료함.
/// </summary>
public class GameExitHandler : MonoBehaviour
{
    // 게임을 종료하는 메서드
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
