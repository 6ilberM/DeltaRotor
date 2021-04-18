using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public Action onGameWon, onGameLost;

    public static GameManager Instance
    {
        get
        {
            if (ReferenceEquals(instance, null))
            {
                instance = new GameObject("Game Manager").AddComponent<GameManager>();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (!ReferenceEquals(instance, null))
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

}
