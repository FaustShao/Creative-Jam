using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  public float gridSize = 1f; // Set this to your grid size
  private Vector3 targetPosition;
  private bool isMoving;
  private bool isBlockedByWall = false;
  private Vector3 currentTransform = new Vector3(1,1,1);
  public bool isInDialogue = false;
  public int playerGeneration = 0;
  public Animator animator;
  public List<string> playerActionsList;
  public bool isInReplay = false;
  private Vector3 respawnPoint;
  public GameController gameController; // Reference to the GameController script

  private bool isKicking = false;
  public bool exhausted = false;
  public int remain;
  void Start()
  {
    targetPosition = transform.position;
    playerActionsList = new List<string>();
    respawnPoint = transform.position;
    //remain = gameController.maxActionCount;
    Debug.Log("remain is:"+remain);
  }

  void FixedUpdate()
  {
    if (isMoving || exhausted || isInDialogue || isInReplay)
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
      isBlockedByWall = false;
      Move(new Vector3(h, v, 0));
      gameController.ProceedGame();
    }
  }

  void Move(Vector3 direction)
  {
    if ((isMoving || isKicking) || (exhausted && !isInReplay) || isInDialogue || isInReplay)
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
       // remain -= 1;
      }

      else if (hit.collider != null && hit.collider.CompareTag("Wall")) {
        // do nothing
          Debug.Log("Hit wall");
          isBlockedByWall = true;
      }

      else
      {
        Debug.Log("Hit nothing");
        // Move the player
        targetPosition = transform.position + direction * gridSize;
        isMoving = true;
        animator.SetBool("isMoving", true);
        //remain -= 1;
      }
      if(!isBlockedByWall){
        if (direction == Vector3.up)
        {
          playerActionsList.Add("w");
        }
        else if (direction == Vector3.left)
        {
          playerActionsList.Add("a");
        }
        else if (direction == Vector3.down)
        {
          playerActionsList.Add("s");
        }
        else if (direction == Vector3.right)
        {
          playerActionsList.Add("d");
        }
      }
      
    }
    Debug.Log(remain);
    Debug.Log("Player actions: " + string.Join(", ", playerActionsList));
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
    //exhausted = remain <= 0;
    //Debug.Log(remain);
    //Debug.Log(gameController.remainingActionCount);
    animator.SetBool("isExhausted", exhausted);
  }

  public void EndKick()
  {
    isKicking = false;
    animator.SetBool("isKicking", false);
  }

  public void PlayStep(int stepIndex)
  {
    isInReplay = true;
    animator.SetBool("isReplayed", true);
    //ResetToRespawnPoint();

    if (stepIndex < playerActionsList.Count)
    {
      string action = playerActionsList[stepIndex];

      // Convert the action string to a direction and move the player
      Vector3 direction = ActionToDirection(action);
      isInReplay = false;
      Move(direction);
      isInReplay = true;
    }
  }

  public void ResetToRespawnPoint()
  {
    transform.position = respawnPoint;  // Set position to respawn point
    animator.SetBool("isReplaying", false);  // Reset replaying flag
    animator.SetBool("isMoving", false);  // Reset other animator flags
    animator.SetBool("isKicking", false);  // Reset other animator flags
    isInReplay = false;  // Reset replay flag
  }

  private Vector3 ActionToDirection(string action)
  {
    switch (action)
    {
      case "w": return Vector3.up;
      case "a": return Vector3.left;
      case "s": return Vector3.down;
      case "d": return Vector3.right;
      default: return Vector3.zero;
    }
  }

  public void SoftReset(Vector3 pos){
    isInReplay = true;
    transform.position = pos;
  }



}
