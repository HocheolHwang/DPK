using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonReqDto
{
    public string partyId;
    public string dungeonCode;
    public bool cleared;
    public int clearTime; // null or number
}
