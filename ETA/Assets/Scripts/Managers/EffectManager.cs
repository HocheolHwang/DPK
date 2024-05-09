using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager
{
    private GameObject[] _effectPrefabs = new GameObject[(int)Define.Effect.MaxCount];
    private Quaternion[] _effectQuaternion = new Quaternion[(int)Define.Effect.MaxCount];
    public void Init()
    {

        string[] effectNames = System.Enum.GetNames(typeof(Define.Effect));
        for (int i = 0; i < effectNames.Length - 1; i++)
        {
            GameObject go = Managers.Resource.Load<GameObject>($"Prefabs/Effect/{effectNames[i]}");
            Managers.Pool.CreatePool(go);

            _effectPrefabs[i] = go;
            _effectQuaternion[i] = go.transform.rotation;
        }

    }
    public ParticleSystem Play(Define.Effect effect, Transform starter = null)
    {
        GameObject original = _effectPrefabs[(int)effect];
        GameObject go = Managers.Resource.Instantiate($"Effect/{original.name}");

        go.transform.position = starter.position + starter.transform.up;
        go.transform.rotation = starter.rotation * _effectQuaternion[(int)effect];
        ParticleSystem ps = go.GetComponent<ParticleSystem>();

        ps.Play();

        return ps;
    }

    public ParticleSystem Play(Define.Effect effect, float duration = 0.0f, Transform starter = null)
    {
        GameObject original = _effectPrefabs[(int)effect];
        GameObject go = Managers.Resource.Instantiate($"Effect/{original.name}");

        if (starter != null)
        {
            go.transform.position = starter.position + starter.transform.up;
            go.transform.rotation = starter.rotation * _effectQuaternion[(int)effect];
        }

        ParticleSystem ps = go.GetComponent<ParticleSystem>();

        ps.Play();
        if (duration == 0) duration = ps.main.duration;
        if(duration > 0) Managers.Coroutine.Run(StopEffect(ps, duration));
        
        return ps;
    }

    IEnumerator StopEffect(ParticleSystem ps, float duration)
    {
        yield return new WaitForSeconds(duration);

        Managers.Resource.Destroy(ps.gameObject);

    }

    public void Stop(ParticleSystem ps)
    {
        ps.Stop();
        Managers.Resource.Destroy(ps.gameObject);
    }


}
