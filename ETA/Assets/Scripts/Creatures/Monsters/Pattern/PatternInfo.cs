using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Pattern List를 가지는 Info Class
public class PatternInfo : MonoBehaviour
{
    protected List<IPattern> _patternList;          // 몬스터의 pattern list

    public List<IPattern> PatternList { get => _patternList; set => _patternList = value; }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        _patternList = new List<IPattern>();
    }
}
