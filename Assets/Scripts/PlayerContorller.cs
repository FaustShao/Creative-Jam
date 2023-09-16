using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float gridSize = 0.5f;
    private Vector3 targetPosition;

    private bool isMoving;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isMoving)
            return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement
        if (h != 0) v = 0;

        if (h != 0 || v != 0)
        {
            StartCoroutine(Move(new Vector3(h, v, 0)));
        }
    }

    System.Collections.IEnumerator Move(Vector3 direction)
    {
        isMoving = true;
        targetPosition = transform.position + direction * gridSize;

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}