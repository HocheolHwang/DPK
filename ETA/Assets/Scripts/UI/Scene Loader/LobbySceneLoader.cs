using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Lobby Scene으로 전환하는 클래스
/// </summary>
public class LobbySceneLoader : MonoBehaviour
{
    // Lobby Scene을 로드하는 메서드
    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("Lobby");
    }
}
