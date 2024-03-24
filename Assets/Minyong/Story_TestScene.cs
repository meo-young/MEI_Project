using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_TestScene : StoryController
{
    public DialogueManager dialogueManager;

    // 이 씬에 등장하는 상호작용 대상들
    public Interactable red;
    public Interactable blue;
    public Interactable green;
    private int currentStep = 1; // 스토리 진행 단계, 1부터 시작

    void Start()
    {
        Proceed(); // 씬 시작 시 첫 대화 진행
    }

    public override void Proceed()
    {
        switch (currentStep)
        {
            case 1:
                dialogueManager.StartDialogue(1, 1);
                //상호작용 과제가 주어졌으므로 대상과의 상호작용 활성화
                red.isInteractable = true;
                blue.isInteractable = true;
                break;
            case 2:
                if (!red.isInteracted || !blue.isInteracted) // 조건을 만족할 경우에만 다음 대화 진행
                {
                    return;
                }
                //a.isInteractable = false;
                //b.isInteractable = false;
                dialogueManager.StartDialogue(2, 3);
                green.isInteractable = true;
                break;
            case 3:
                if (!green.isInteracted)
                {
                    return;
                }
                //c.isInteractable = false;
                dialogueManager.StartDialogue(4, 5);
                break;
        }
        currentStep++; // 다음 단계로 넘어가기
    }
}

