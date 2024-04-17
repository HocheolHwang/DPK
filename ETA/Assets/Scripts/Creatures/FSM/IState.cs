using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    // ------------------- 멤버 변수 초기화 --------------

    void Initialize();

    // -------------------- 상태 수행 --------------------

    void Enter();
    void Execute();
    void FixedExecute();
    void Exit();
}
