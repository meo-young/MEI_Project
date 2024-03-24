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

    // �ڽ� Ŭ�������� �� �� ���丮 ���� ���� ����
    public abstract void Proceed();
}
