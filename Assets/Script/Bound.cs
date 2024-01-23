using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    private BoxCollider2D bound;

    private CameraManager theCamera;

    // Start is called before the first frame update
    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            theCamera.SetBound(bound);
        }
    }
}
