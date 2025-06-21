using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] int initialSize;
    [SerializeField] T prefabToPool;

    protected Queue<T> pooledItems = new();
    protected List<T> activeItems = new();

    protected virtual void Awake() =>
        InitializePool(initialSize);
    protected virtual void InitializePool(int size)
    {
        for (int i = 0; i < size; i++)
        {
            T item = Instantiate(prefabToPool, transform);
            item.gameObject.SetActive(false);
            pooledItems.Enqueue(item);
        }
    }

    public virtual T GetPooledItem()
    {
        if (pooledItems.Count > 0)
        {
            T item = pooledItems.Dequeue();
            activeItems.Add(item);
            item.gameObject.SetActive(true);
            return item;
        }
        return CreateNewPoolItem();

    }
    protected virtual T CreateNewPoolItem()
    {
        T newItem = Instantiate(prefabToPool, transform);
        newItem.gameObject.SetActive(true);
        pooledItems.Enqueue(newItem);
        return newItem;
    }
    public virtual void ReturnActivedItemToPool(T item)
    {
        if (activeItems.Contains(item))
        {
            item.gameObject.SetActive(false);
            activeItems.Remove(item);
            pooledItems.Enqueue(item);
        }
       
    }
    public virtual void ReturnAllTiemsToPool()
    {
        foreach (T item in activeItems)
        {
            item.gameObject.SetActive(false);
            pooledItems.Enqueue(item);
        }
        activeItems.Clear();
    }
}
