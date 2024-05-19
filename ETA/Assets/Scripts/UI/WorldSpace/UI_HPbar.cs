using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    // ------------------------------ 변수 정의 ------------------------------

    // 열거형 정의
    enum GameObjects
    {
        // 플레이어 상태
        HP_Bar,
        Shield_Bar,
    }

    // UI 컴포넌트 바인딩 변수
    private GameObject hpBar;
    private GameObject shieldBar;

    // 스텟
    Stat _stat;


    // ------------------------------ UI 초기화 ------------------------------
    public override void Init()
    {
        // 컴포넌트 바인드
        Bind<GameObject>(typeof(GameObjects));

        // 오브젝트 초기화
        hpBar = GetObject((int)GameObjects.HP_Bar);
        shieldBar = GetObject((int)GameObjects.Shield_Bar);

        // 스텟 초기화
        _stat = transform.parent.GetComponent<Stat>();
    }


    // ------------------------------ 유니티 생명주기 메서드 ------------------------------

    private void Update()
    {
        // 체력바 위치 및 방향 조정
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;

        // 체력이 0 이하면 체력바 파괴
        float ratio = _stat.Hp / (float)_stat.MaxHp;
        if (ratio <= 0) Destroy(gameObject);

        // 체력바 업데이트
        Dungeon_Popup_UI.UpdateHealthAndShieldBars(hpBar, shieldBar, _stat, 116);
    }
}