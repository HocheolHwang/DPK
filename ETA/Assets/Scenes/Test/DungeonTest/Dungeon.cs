using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dungeon : MonoBehaviour
{
    public GameObject dungeonSelectPanel;

    public GameObject dungeononExitButton;

    public void DungeonSelectButtonClick ()
    {
        dungeonSelectPanel.SetActive (true);
    }

    public void DungeonExitButton ()
    {
        dungeonSelectPanel.SetActive(false);
    }

    public void DungeonLoadButtonClick ()
    {
        SceneManager.LoadScene("DungeonTest");
    }
}
