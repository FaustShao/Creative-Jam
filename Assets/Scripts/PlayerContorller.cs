using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  public float gridSize = 1f; // Set this to your grid size
  private Vector3 targetPosition;
  private bool isMoving;
  private Vector3 currentTransform = new Vector3(1,1,1);

  public Animator animator;
  public List<string> playerActionsList;

  public GameController gameController; // Reference to the GameController script

  private bool isKicking = false;
  private bool exhausted = false;
  private int remain = 6;
 
  void Start()
  {
    targetPosition = transform.position;
  }

  void FixedUpdate()
  {
    if (isMoving || exhausted)
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
      if (v == 0)
      {
        if (h > 0)
        {
          transform.localScale = new Vector3(1, 1, 1);
          currentTransform = transform.localScale;
        }
        else
        {
          transform.localScale = new Vector3(-1, 1, 1);
          currentTransform = transform.localScale;
        }
      }
      else
      {
        transform.localScale = currentTransform;
      }
      Move(new Vector3(h, v, 0));
    }
  }

  void Move(Vector3 direction)
  {
    if ((isMoving || isKicking) || exhausted)
      return;

    if (!isMoving && !isKicking)
    {
      RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, gridSize);
      if (hit.collider != null && hit.collider.CompareTag("MovableBox"))
      {
        // kick the box
        isKicking = true;
        animator.SetBool("isKicking", true);
        hit.collider.transform.position += direction * gridSize; // Move the box
        remain -= 1;
      }
      else
      {
        // Move the player
        targetPosition = transform.position + direction * gridSize;
        isMoving = true;
        animator.SetBool("isMoving", true);
        remain -= 1;
      }
    }
    Debug.Log(remain);
  }

  void Update()
  {
    if (isMoving)
    {
      if (transform.position != targetPosition)
      {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
      }
      else
      {
        isMoving = false;
        animator.SetBool("isMoving", false);
      }
    }

    if (isKicking && animator.GetCurrentAnimatorStateInfo(0).IsName("Kicking") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.001f)
    {
      Debug.Log("false!");
      isKicking = false;
      animator.SetBool("isKicking", false);
    }

    // Update exhausted state based on remainingActionCount from GameController
    //exhausted = gameController.remainingActionCount <= 0;
    exhausted = remain <= 0;
    //Debug.Log(gameController.remainingActionCount);
    animator.SetBool("isExhausted", exhausted);
  }

  public void EndKick()
  {
    isKicking = false;
    animator.SetBool("isKicking", false);
  }

  public void PlayStep(int i)
  {
    Debug.Log("TODO: Play actoin step");
  }
}
