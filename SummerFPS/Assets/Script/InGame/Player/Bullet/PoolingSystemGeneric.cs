
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public abstract class PoolingSystemGeneric<T> : MonoBehaviour where T: Component
{
    [SerializeField]
    private T prefab;
[SerializeField]
    private int maxObjects;
    [SerializeField] private TMP_Text text;
    public static PoolingSystemGeneric<T> Instance {get;private set;}
    private Queue<T> objects = new Queue<T>();

    private void Awake() 
    {
     Instance = this;   
    }

    private void CountUpdate() {
        if(text != null)
        text.text = "Objects in Pool = "+objects.Count.ToString("D2");

    }
   
    public T Get()
    {
        if(objects.Count == 0)
            AddObject(1);
        
        CountUpdate();
        return objects.Dequeue();
    }
    public void ReturnToPool(T objectToReturn)
    {
        if(objects.Count >= maxObjects)
        {
            Destroy(objectToReturn.gameObject);
                    CountUpdate();

            return;
        }
        objectToReturn.gameObject.SetActive(false);
        CountUpdate();

        objects.Enqueue(objectToReturn);
    }
    private void AddObject(int count)
    {
        for (int i = 0; i < count; i++)
        {
        var newObject = GameObject.Instantiate(prefab,transform);
        newObject.gameObject.SetActive(false);
        objects.Enqueue(newObject); 
        }
     
    }
}
