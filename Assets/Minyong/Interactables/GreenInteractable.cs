// 테스트 전용 스크립트

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenInteractable : Interactable
{
    protected override void Interact()
    {
        base.Interact();
        // 상호작용 로직 추가
    }
}

