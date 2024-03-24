using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_TestScene : StoryController
{
    public DialogueManager dialogueManager;

    // �� ���� �����ϴ� ��ȣ�ۿ� ����
    public Interactable red;
    public Interactable blue;
    public Interactable green;
    private int currentStep = 1; // ���丮 ���� �ܰ�, 1���� ����

    void Start()
    {
        Proceed(); // �� ���� �� ù ��ȭ ����
    }

    public override void Proceed()
    {
        switch (currentStep)
        {
            case 1:
                dialogueManager.StartDialogue(1, 1);
                //��ȣ�ۿ� ������ �־������Ƿ� ������ ��ȣ�ۿ� Ȱ��ȭ
                red.isInteractable = true;
                blue.isInteractable = true;
                break;
            case 2:
                if (!red.isInteracted || !blue.isInteracted) // ������ ������ ��쿡�� ���� ��ȭ ����
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
        currentStep++; // ���� �ܰ�� �Ѿ��
    }
}

