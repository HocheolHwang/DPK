using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damage를 받는 Controller에 부착 -> Stat에 부착하는건 후순위로 하겠슴다. 보스 설계부터!
public interface IBuffStat
{
    void GetShield(int amount);
    void RemoveShield(int amount);
}
