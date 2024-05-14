using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllCalssLevelResDto
{
    public string message;
    public MyClasses[] myClasses;
}
[System.Serializable]
public class MyClasses
{
    public string classCode;
    public int level;
}
