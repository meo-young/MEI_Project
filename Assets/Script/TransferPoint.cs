using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferPoint : MonoBehaviour
{
    public string targetScene;
    public string ID;
    public string targetID;
    public bool transferReady = false;

    public enum Direction { Up, Down, Left, Right }
    public Direction activateDirection;

    private PlayerManager thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            transferReady = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            transferReady = false;
    }

    private void Update()
    {
        if (transferReady)
        {
            Vector2 vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (!thePlayer.animator.GetBool("Walking"))
            {
                switch (activateDirection)
                {
                    case Direction.Up:
                        if (vector == Vector2.up) Transfer();
                        break;
                    case Direction.Down:
                        if (vector == Vector2.down) Transfer();
                        break;
                    case Direction.Right:
                        if (vector == Vector2.right) Transfer();
                        break;
                    case Direction.Left:
                        if (vector == Vector2.left) Transfer();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void Transfer()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            StartCoroutine(ExternalTransfer());
        }
        else
        {
            StartCoroutine(LocalTransfer());
        }
    }

    IEnumerator LocalTransfer()
    {
        TransferPoint transferPoint = FindTargetPortal();
        thePlayer.transform.position = transferPoint.transform.position;
        yield return null;
    }

    IEnumerator ExternalTransfer()
    {
        SceneManager.LoadScene(targetScene);
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == targetScene);
        TransferPoint transferPoint = FindTargetPortal();
        thePlayer.transform.position = transferPoint.transform.position;
    }

    private TransferPoint FindTargetPortal()
    {
        TransferPoint[] transferPoints = FindObjectsOfType<TransferPoint>();
        foreach (TransferPoint transferPoint in transferPoints)
        {
            if (transferPoint.ID == targetID)
            {
                return transferPoint;
            }
        }
        return null;
    }
}

