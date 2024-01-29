using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialog : MonoBehaviour
{
    public Dialog dialog;
    private DialogManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private bool flag = false;

    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theDM = FindObjectOfType<DialogManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!flag)
        {
            StartCoroutine(StartDialogCoroutine());
            flag = true;
        }
    }

    IEnumerator StartDialogCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.notMove();
        theDM.ShowDialog(dialog);
        yield return new WaitUntil(() => !theDM.talking);
        theOrder.Move();
    }
}
