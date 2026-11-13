using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner Instance => GetInstance();

    private static CoroutineRunner _instance;
    // Quick note: the lists inside the dictionary are not cleaned up when a coroutine finishes.
    // This is probably harmless until we need to check whether a coroutine is running,
    // or if we let it run for a very long time and using up too much memory
    private static readonly Dictionary<Object, List<Coroutine>> _coroutines = new();

    private static CoroutineRunner GetInstance()
    {
        if (!_instance)
        {
            _instance = CreateInstance();
        }
        return _instance;
    }

    private static CoroutineRunner CreateInstance()
    {
        var go = new GameObject(nameof(CoroutineRunner));
        _instance = go.AddComponent<CoroutineRunner>();
        DontDestroyOnLoad(go);
        return _instance;
    }

    public static Coroutine Play(Object requester, IEnumerator sequence)
    {
        if (!_coroutines.ContainsKey(requester))
        {
            _coroutines[requester] = new List<Coroutine>();
        }
        var coroutine = Instance.StartCoroutine(sequence);
        _coroutines[requester].Add(coroutine);
        return coroutine;
    }

    public static void StopAll(Object requester)
    {
        if (!_coroutines.ContainsKey(requester))
            return;

        foreach (var coroutine in _coroutines[requester])
        {
            Instance.StopCoroutine(coroutine);
        }
        _coroutines[requester].Clear();
    }
}
