using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

// Dungeon_Scene 클래스에서 생성
// 몬스터는 던전 씬에서만 로직이 필요하기 때문이다.
public class MonsterManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _existPlayerList;
    [SerializeField] private List<GameObject> _existMonsterList;

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

        // TODO : 고처야할것

        _existPlayerList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        _existMonsterList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Monster"));
        //_monsterCoList = new List<Coroutine>();
        //_coStateDic = new Dictionary<Coroutine, bool>();
    }

    // --------------------------- Get the Destination using the players ----------------------------------
    #region Get Position
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
    #endregion

    // -------------------------------- Coroutine -------------------------------

    #region Temp
    //[SerializeField] private List<Coroutine> _monsterCoList;
    //[SerializeField] private Dictionary<Coroutine, bool> _coStateDic;

    //private Coroutine SetMonsterCoroutine(IEnumerator routine)
    //{
    //    Coroutine co = StartCoroutine(routine);
    //    _monsterCoList.Add(co);
    //    _coStateDic[co] = true;

    //    Debug.Log($"SetMonsterCoroutine - StartCoroutine : {co}");

    //    return co;
    //}

    //public IEnumerator StartMonsterCoroutine(IEnumerator routine)
    //{
    //    Debug.Log($"StartMonster - routine : {routine}");
    //    Coroutine co = SetMonsterCoroutine(routine);

    //    Debug.Log($"StartMonster - SetMonster - co : {co}");

    //    yield return co;    // 코루틴이 끝나면 다음 로직을 수행

    //    _coStateDic[co] = false;
    //}

    //private void RemoveFinishCoroutines()
    //{
    //    foreach (Coroutine co in _monsterCoList)
    //    {
    //        if (_coStateDic.ContainsKey(co) && !_coStateDic[co])
    //        {
    //            _coStateDic.Remove(co);
    //            _monsterCoList.Remove(co);
    //        }
    //    }
    //}
    #endregion

    #region Setter/Getter
    public List<GameObject> ExistPlayerList
    {
        get
        {
            List<GameObject> tmp = new List<GameObject>();
            foreach(var player in FindObjectsOfType<PlayerController>())
            {
                tmp.Add(player.gameObject);
            }
            //_existPlayerList.RemoveAll(player => player.GetComponent<Stat>().Hp <= 0);
            //foreach (GameObject player in _existPlayerList)
            //{
            //    Debug.Log($"exist player: {player.name}");
            //}
            return tmp;
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

    //public List<Coroutine> MonsterCoList
    //{
    //    get
    //    {
    //        RemoveFinishCoroutines();
    //        return _monsterCoList;
    //    }
    //}
    #endregion
}
