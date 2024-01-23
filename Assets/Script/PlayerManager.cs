using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MovingObject
{
    static public PlayerManager instance;

    public string currentMapName;

    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false;
    private bool canMove = true;

    public bool notMove = false;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        queue = new Queue<string>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    IEnumerator MoveCouroutine()
    {
        while (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !notMove)
        {
            if (notMove)
            {
                animator.SetBool("Walking", false);
                canMove = true;
                yield break;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
                animator.SetBool("Running", true);
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
                animator.SetBool("Running", false);
            }

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);

            if (vector.x != 0)
                vector.y = 0;

            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            bool checkCollisionFlag = base.CheckCollision();
            if (checkCollisionFlag)
                break;

            animator.SetBool("Walking", true);

            boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount);

            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }
                if (applyRunFlag)
                    currentWalkCount++;
                currentWalkCount++;

                if (currentWalkCount <= walkCount * 0.7f)
                    boxCollider.offset = new Vector2(vector.x * 0.7f * speed * walkCount, vector.y * 0.7f * speed * walkCount) * (1 - currentWalkCount / (walkCount * 0.7f));

                yield return new WaitForSeconds(0.01f);
            }
            boxCollider.offset = Vector2.zero;
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }


    void Update()
    {
        if (canMove && !notMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCouroutine());
            }
        }
    }
}
