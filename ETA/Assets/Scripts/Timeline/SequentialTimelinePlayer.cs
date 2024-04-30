using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

    private void OnDirectorStopped(PlayableDirector director)
    {
        if (director == directors[currentDirectorIndex])
        {
            currentDirectorIndex++;
            if ( currentDirectorIndex < directors.Count )
            {
                directors[currentDirectorIndex].Play();
            }
        }
    }

    void OnDestroy()
    {
        foreach (PlayableDirector director in directors)
        {
            director.stopped -= OnDirectorStopped;
        }
    }
}
