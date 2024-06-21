using System;
using UnityEngine;

public class GameObjectDestroyListener : MonoBehaviour
{
    public static GameObjectDestroyListener Get(GameObject obj)
    {
        GameObjectDestroyListener gameObjectDestroyListener = obj.GetComponent<GameObjectDestroyListener>();
        if (gameObjectDestroyListener == null)
        {
            gameObjectDestroyListener = obj.AddComponent<GameObjectDestroyListener>();
        }
        return gameObjectDestroyListener;
    }

    private void OnDestroy()
    {
        if (this.onDes != null)
        {
            this.onDes();
        }
    }

    public Action onDes;
}
