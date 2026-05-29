using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPoolSystem<T> where T:Component
{
    private GameObject _gameobject;
    private Queue<(GameObject obj, T Component)> _pool;

    public GenericPoolSystem(GameObject obj, int preloadCount)
    {
        _gameobject = obj;
        _pool = new Queue<(GameObject, T)>(preloadCount);

        for (int i = 0; i < preloadCount; i++)
        {
            _pool.Enqueue(CreateObject());
        }
    }

    public (GameObject obj, T component) SpawnObject(Vector3 position, Quaternion rotation)
    {
        var entry = _pool.Count > 0 ? _pool.Dequeue() : CreateObject();
        entry.obj.transform.SetPositionAndRotation(position,rotation);
        entry.obj.SetActive(true);
        return entry; 
    }

    public void EraseObject(GameObject obj,float time)
    {
        if (!obj.activeSelf) return;
        CoroutineRunner.Instance.StartCoroutine(DelayErase(obj,time));
    }

    private IEnumerator DelayErase(GameObject obj,float delayTime)
    {
        float timer = 0f;
        while (timer < delayTime) 
        {
            timer += Time.deltaTime; 
            yield return null; 
        }
        obj.SetActive(false);
        _pool.Enqueue((obj, obj.GetComponent<T>()));
    }

    public (GameObject obj, T component) CreateObject()
    {
        _gameobject.SetActive(false);
        GameObject obj = Object.Instantiate(_gameobject);
        T component = obj.GetComponent<T>();  // cached ONCE here at instantiation
        _gameobject.SetActive(true);
        return (obj, component);

    }
    
}
public class GenericPoolSystem
{
    private GameObject _gameobject;
    private Queue<GameObject> _pool;

    public GenericPoolSystem(GameObject obj, int preloadCount)
    {
        _gameobject = obj;
        _pool = new Queue<GameObject>(preloadCount);

        for (int i = 0; i < preloadCount; i++)
        {
            _pool.Enqueue(CreateObject());
        }
    }

    public GameObject  SpawnObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : CreateObject();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    public void EraseObject(GameObject obj, float time)
    {
        if (!obj.activeSelf) return;
        CoroutineRunner.Instance.StartCoroutine(DelayErase(obj, time));
    }

    private IEnumerator DelayErase(GameObject obj, float delayTime)
    {
        float timer = 0f;
        while (timer < delayTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }

    public GameObject  CreateObject()
    {
        _gameobject.SetActive(false);
        GameObject obj = Object.Instantiate(_gameobject);
        _gameobject.SetActive(true);
        return (obj);

    }

}
