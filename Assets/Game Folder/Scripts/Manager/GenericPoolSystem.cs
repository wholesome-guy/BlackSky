using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericPoolSystem
{
    private GameObject _gameobject;
    private Queue<GameObject> _pool = new Queue<GameObject>();

    public GenericPoolSystem(GameObject obj, int preloadCount)
    {
        _gameobject = obj;

        for(int i = 0; i < preloadCount; i++)
        {
            _pool.Enqueue(CreateObject());
        }
    }

    public GameObject SpawnObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : CreateObject();
        obj.transform.SetPositionAndRotation(position,rotation);
        obj.SetActive(true);
        return obj; 
    }

    public void EraseObject(GameObject obj,float time)
    {
        if (!obj.activeSelf) return;
        obj.SetActive(false);
        CoroutineRunner.Instance.StartCoroutine(DelayErase(obj,time));
    }

    private IEnumerator DelayErase(GameObject obj,float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        _pool.Enqueue(obj);
    }

    public GameObject CreateObject()
    {
        _gameobject.SetActive(false);
        GameObject obj = Object.Instantiate(_gameobject);
        _gameobject.SetActive(true);
        return obj;
    }
    public void ClearPool()
    {
        while(_pool.Count > 0)
        {
            Object.Destroy(_pool.Dequeue());
        }
    }
}
