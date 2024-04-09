using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    #region Singleton
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PoolManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PoolManager");
                    instance = obj.AddComponent<PoolManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    [SerializeField]
    private GameObject[] prefabs;

    private List<GameObject>[] objPools;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitObjPool();
    }

    private void InitObjPool()
    {
        objPools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < objPools.Length; i++)
        {
            objPools[i] = new List<GameObject>();
        }
    }

    public GameObject ActivateObj(int index)
    {
        GameObject obj = null;

        foreach (GameObject item in objPools[index])
        {
            if (!item.activeSelf)
            {
                obj = item;
                obj.SetActive(true);
                break;
            }
        }

        if (obj == null)
        {
            obj = Instantiate(prefabs[index], transform);
            objPools[index].Add(obj);
        }

        return obj;
    }

    public void DeactivateAllObjects()
    {
        foreach (List<GameObject> pool in objPools)
        {
            foreach (GameObject obj in pool)
            {
                if (obj.activeSelf)
                {
                    obj.transform.SetParent(transform, false);
                    obj.SetActive(false);
                }
            }
        }
    }
}
