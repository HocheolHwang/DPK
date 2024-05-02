using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDetector
{
    float AttackRange { get; }
    Transform Target { get; }

    IEnumerator UpdateTarget();
    bool IsArriveToTarget();
}
