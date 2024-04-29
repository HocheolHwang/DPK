using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    PhotonManager _photon;
    NetworkManager _network;
    PlayerManager _player = new PlayerManager();
    PoolManager _pool = new PoolManager();
    EffectManager _effect = new EffectManager();
    CoroutineManager _coroutine;
    
    public static InputManager Input { get { return Instance._input; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static UIManager UI { get { return Instance._ui; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static PhotonManager Photon { get; private set; }
    public static NetworkManager Network { get; private set; }
    public static PlayerManager Player { get { return Instance._player; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static CoroutineManager Coroutine { get; private set;}
    public static EffectManager Effect { get { return Instance._effect; } }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            Debug.Log("@@");
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
                Coroutine = go.AddComponent<CoroutineManager>();
                Photon = go.AddComponent<PhotonManager>();
                Network = go.AddComponent<NetworkManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
            
            
            s_instance._pool.Init();
            s_instance._sound.Init();
            s_instance._effect.Init();

            Photon.Connect();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Input.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}