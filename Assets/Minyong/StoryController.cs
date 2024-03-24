using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryController : MonoBehaviour
{
    public static StoryController Instance { get; protected set; }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 자식 클래스에서 씬 별 스토리 진행 로직 구현
    public abstract void Proceed();
}
