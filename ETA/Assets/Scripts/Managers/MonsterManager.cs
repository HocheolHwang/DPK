using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Dungeon_Scene 클래스에서 생성
// 몬스터는 던전 씬에서만 로직이 필요하기 때문이다.
public class MonsterManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _existPlayerList;

    // 싱글톤
    private static MonsterManager _instance;
    public static MonsterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MonsterManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("@MonsterManager");
                    _instance = go.AddComponent<MonsterManager>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _existPlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        //Debug.Log("Monster Manager 생성");
    }

    // --------------------------- Center Distance From Players ----------------------------------
    public Vector3 GetCenterPos(Transform monster)
    {
        List<GameObject> list = ExistPlayerList;
        if (list == null || list.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 sumPos = Vector3.zero;
        foreach (GameObject player in list)
        {
            Vector3 tempPos = new Vector3(player.transform.position.x, monster.position.y, player.transform.position.z);
            sumPos += tempPos;
        }

        return sumPos / list.Count;
    }

    #region Setter/Getter
    public List<GameObject> ExistPlayerList
    {
        get
        {
            _existPlayerList.RemoveAll(player => player.GetComponent<Stat>().Hp <= 0);
            return _existPlayerList;
        }
    }
    #endregion
}
