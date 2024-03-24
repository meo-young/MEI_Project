using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInteractable = false; // ��ȣ�ۿ� Ȱ��ȭ/��Ȱ��ȭ
    private bool isPlayerInRange = false; // �÷��̾ ��ȣ�ۿ� ���� ���� �ִ��� ����
    public bool isInteracted = false; // ��ȣ�ۿ� �Ϸ� ����

    void Update()
    {
        if (isInteractable && isPlayerInRange && Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    protected virtual void Interact()
    {
        if (!isInteracted && isInteractable)
        {
            Debug.Log($"{gameObject.name}��(��) ��ȣ�ۿ��߽��ϴ�.");
            isInteracted = true;
            StoryController.Instance.Proceed();
        }
    }
}

