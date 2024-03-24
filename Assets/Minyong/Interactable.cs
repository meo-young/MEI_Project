using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool isInteractable = false; // 상호작용 활성화/비활성화
    private bool isPlayerInRange = false; // 플레이어가 상호작용 범위 내에 있는지 여부
    public bool isInteracted = false; // 상호작용 완료 여부

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
            Debug.Log($"{gameObject.name}와(과) 상호작용했습니다.");
            isInteracted = true;
            StoryController.Instance.Proceed();
        }
    }
}

