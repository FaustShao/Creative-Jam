using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float gridSize = 1f; // Set this to your grid size
    private Vector3 targetPosition;
    private bool isMoving;
    private Vector3 currentTransform;

    public Animator animator;


    void Start()
    {
        targetPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (isMoving)
            return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
    

        // Prevent diagonal movement
        if (h != 0 && v != 0)
        {
            h = 0;
            v = 0;
        }

        if (h != 0 || v != 0)
        {
            if(v == 0){
                if(h>0) {
                    transform.localScale = new Vector3(1,1,1);
                    currentTransform = transform.localScale;
                }
                else {
                    transform.localScale = new Vector3(-1,1,1);
                    currentTransform = transform.localScale;
                }
            }else{
                transform.localScale = currentTransform;
            }
            Move(new Vector3(h, v, 0));
        }


    }

    void Move(Vector3 direction)
    {
        if (!isMoving)
        {
            targetPosition = transform.position + direction * gridSize;
            isMoving = true;
            animator.SetBool("isMoving",true);
        }
    }

    void Update()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            isMoving = false;
            animator.SetBool("isMoving",false);
        }
    }
}
