using Photon.Realtime;
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
    private List<GameObject> _existMonsterList;

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
        _existMonsterList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Monster"));
    }

    // --------------------------- Get the Destination using the players ----------------------------------
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

    public Vector3 GetBackPosPlayer(Transform monster)
    {
        List<GameObject> list = ExistPlayerList;
        if (list == null || list.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 backPos = Vector3.zero;

        float maxDist = float.MinValue;
        foreach (GameObject player in list)
        {
            float prevDist = maxDist;
            maxDist = Mathf.Max(Vector3.Distance(monster.position, player.transform.position), maxDist);
            if (prevDist != maxDist)
            {
                backPos = player.transform.position;
            }
        }
        Debug.Log($"Target Pos : {backPos}");
        return backPos;
    }

    #region Setter/Getter
    public List<GameObject> ExistPlayerList
    {
        get
        {
            _existPlayerList.RemoveAll(player => player.GetComponent<Stat>().Hp <= 0);
            //foreach (GameObject player in _existPlayerList)
            //{
            //    Debug.Log($"exist player: {player.name}");
            //}
            return _existPlayerList;
        }
    }

    public List<GameObject> ExistMonsterList
    {
        get
        {
            _existMonsterList.RemoveAll(monster => monster.GetComponent<Stat>().Hp <= 0);
            foreach (GameObject monster in _existMonsterList)
            {
                Debug.Log($"exist monster: {monster.name}");
            }
            return _existMonsterList;
        }
    }
    #endregion
}
