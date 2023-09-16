using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  public float gridSize = 1f;
  public Animator animator;

  private Vector3 targetPosition;
  private Vector3 respawnPoint;
  private Vector3 currentTransform = new Vector3(1, 1, 1);
  private bool isBlockedByWall = false;

  public bool isInDialogue = false;
  public bool isInReplay = false;
  public bool isMoving = false;
  public bool isKicking = false;
  public bool exhausted = false;

  public List<string> playerActionsList;

  void Start()
  {
    targetPosition = transform.position;
    respawnPoint = transform.position;
    playerActionsList = new List<string>();
  }

  void FixedUpdate()
  {
    CheckPlayerInput();
  }

  void Update()
  {
    UpdateMovementAndAnimation();
  }

  public void CheckPlayerInput()
  {
    if (isMoving || exhausted || isInDialogue || isInReplay)
      return;

    float h = Input.GetAxisRaw("Horizontal");
    float v = Input.GetAxisRaw("Vertical");

    // Prevent diagonal movement
    if (h != 0 && v != 0) { h = 0; v = 0; }

    if (h != 0 || v != 0)
    {
      SetTransformScale(h, v);
      isBlockedByWall = false;
      Move(new Vector3(h, v, 0));
      Debug.Log(v);
    }
  }

  void SetTransformScale(float h, float v)
  {
    if (v == 0)
    {
      transform.localScale = h > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
      currentTransform = transform.localScale;
    }
    else
    {
      transform.localScale = currentTransform;
    }
  }

  void Move(Vector3 direction)
  {
    Debug.Log(direction);
    if ((isMoving || isKicking) || (exhausted && !isInReplay) || isInDialogue || isInReplay)
      return;

    PerformRaycastAndMove(direction);
    if (!isBlockedByWall) RecordPlayerMotion(direction);
  }

  void PerformRaycastAndMove(Vector3 direction)
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position + direction/2, direction/2, gridSize);
    if (hit.collider == null) { SetTargetPosition(direction); return; }
    Debug.Log(hit.collider.gameObject);
    if (hit.collider.CompareTag("MovableBox")) { KickBox(hit, direction); return; }
    if (hit.collider.CompareTag("Wall")) { isBlockedByWall = true; }
  }

  void SetTargetPosition(Vector3 direction)
  {
    targetPosition = transform.position + direction * gridSize;
    isMoving = true;
    animator.SetBool("isMoving", true);
  }

  void KickBox(RaycastHit2D hit, Vector3 direction)
  {
    isKicking = true;
    animator.SetBool("isKicking", true);
    hit.collider.transform.position += direction * gridSize;
  }

  public void RecordPlayerMotion(Vector3 direction)
  {
    playerActionsList.Add(direction == Vector3.up ? "w" : direction == Vector3.left ? "a" : direction == Vector3.down ? "s" : "d");
  }

  public void UpdateMovementAndAnimation()
  {
    if (isMoving && transform.position == targetPosition)
    {
      isMoving = false;
      animator.SetBool("isMoving", false);
    }
    else if (isMoving)
    {
      transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    CheckKickAnimationStatus();
    animator.SetBool("isExhausted", exhausted);
  }

  void CheckKickAnimationStatus()
  {
    if (!isKicking) return;

    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Kicking") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.001f)
    {
      isKicking = false;
      animator.SetBool("isKicking", false);
    }
  }

  public void PlayStep(int stepIndex)
  {
    if (stepIndex >= playerActionsList.Count) return;

    isInReplay = true;
    animator.SetBool("isReplayed", true);

    string action = playerActionsList[stepIndex];
    Vector3 direction = ActionToDirection(action);

    isInReplay = false;
    Move(direction);
    isInReplay = true;
  }

  private Vector3 ActionToDirection(string action)
  {
    return action switch
    {
      "w" => Vector3.up,
      "a" => Vector3.left,
      "s" => Vector3.down,
      "d" => Vector3.right,
      _ => Vector3.zero,
    };
  }

  public void EndKick()
  {
    isKicking = false;
    animator.SetBool("isKicking", false);
  }
}
