using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollavoSystem : MonoBehaviour
{
    private Dictionary<string, PlayerController> _currentSkills= new Dictionary<string, PlayerController>();

    private void Start()
    {

        //TMP
        //_currentSkills.Add("Cyclone", null);
    }

    public void AddCurrentSkill(PlayerController controller, string skillName)
    {
        //if (PhotonNetwork.IsMasterClient == false) return;
        if(_currentSkills.TryGetValue(skillName, out PlayerController player)) // 있음
        {
            if (player.gameObject.name == controller.name) return;
            _currentSkills.Remove(skillName);
            ChangeToCollavoState(controller);
            ChangeToCollavoState(player);

            
        }
        else // 없음
        {
            _currentSkills.Add(skillName, controller);
        }
        
    }

    public void ChangeToCollavoState(PlayerController controller)
    {
        if (controller == null || !controller.photonView.IsMine) return;
        controller.ChangeState(controller.COLLAVO_STATE);
    }

    public void RemoveCurrentSkill(string skillName)
    {
        return;
        Debug.Log($"Remove {skillName}");
        if (skillName == null) return;
        if(_currentSkills.ContainsKey(skillName)) _currentSkills.Remove(skillName);
    }

    public void Clear()
    {
        _currentSkills.Clear();
    }
}
