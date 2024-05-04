using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SequentialTimelinePlayer : MonoBehaviour
{
    public List<PlayableDirector> directors;
    private int currentDirectorIndex = 0;

    void Start()
    {
        foreach (PlayableDirector director in directors)
        {
            director.stopped += OnDirectorStopped;
        }

        if (directors.Count > 0)
        {
            directors[currentDirectorIndex].Play();
        }
    }

    //void Update()
    //{
    //    // 엔터 키 또는 스페이스바를 누르면 로그인 씬으로 이동
    //    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
    //    {
    //        GoToLoginScene();
    //    }
    //}

    private void OnDirectorStopped(PlayableDirector director)
    {
        if (director == directors[currentDirectorIndex])
        {
            currentDirectorIndex++;
            if (currentDirectorIndex < directors.Count)
            {
                directors[currentDirectorIndex].Play();
            }
            //else
            //{
            //    // 모든 타임라인이 재생된 후 로그인 씬으로 전환
            //    GoToLoginScene();
            //}
        }
    }

    void OnDestroy()
    {
        foreach (PlayableDirector director in directors)
        {
            director.stopped -= OnDirectorStopped;
        }
    }

    public void GoToLoginScene()
    {
        SceneManager.LoadScene("Login");
    }
}
