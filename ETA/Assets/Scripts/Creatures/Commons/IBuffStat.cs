using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damage를 받는 Controller에 부착 -> Stat에 부착하는건 후순위로 하겠슴다. 보스 설계부터!
public interface IBuffStat
{
    void IncreaseHp(int amount);
    void DecreaseHp(int amount);
    void IncreaseDefense(int amount);
    void DecreaseDefense(int amount);
    void GetShield(int amount);
    void RemoveShield(int amount);
    void IncreaseDamage(int amount);
    void DecreaseDamage(int amount);
}
