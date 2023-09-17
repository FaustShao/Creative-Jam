using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  public float gridSize = 1f;
  public GameController game;
  public Animator animator;


  Vector3 respawnPoint;
  public Vector3 nextPos;


  
  public List<Vector3> pastPositions;
  public int ReplayIndex;
  bool canMove;

  public enum State {Idle, Moving, Dead, Phantom, PhantomMove};
  public bool isInDialogue;

  public State playerState;
  public List<Vector3> recordedActions;
  void Start()
  {
    respawnPoint = transform.position;
    ReplayIndex = 0;
    
  }

  void FixedUpdate()
  {
    MoveToNextPos();
    ReplayMoveToPos();
  }

  
  void MoveToNextPos(){
    if(playerState == State.Moving){
      if(transform.position != nextPos){
        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
      }else{
        animator.SetBool("isMoving", false);
        playerState = State.Idle;
        
        game.DecreaseActionCount();
        
      }
    }
  }

  void ReplayMoveToPos(){

    if(playerState != State.PhantomMove){
      return;
    }
   
    if(transform.position != nextPos){
        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
      }else{
        ReplayIndex++;
        Debug.Log("Finish 1 Move");
        playerState = State.Phantom;
      }

  }


  void Update()
  {
    if(playerState == State.Idle && game.playerAllSettle()){
      //.Log("shabi");
      GetNextPos();
    }
    if(playerState == State.Phantom){
      if (Input.anyKeyDown){
        palyNextStep();
      }
    }
  }



  void GetNextPastPos(){
    Debug.Log(gameObject);
    
    if(ReplayIndex >= recordedActions.Count) return;
    Vector3 direction = recordedActions[ReplayIndex];
    nextPos = transform.position + direction * gridSize;

    Debug.Log(nextPos);
    if(CheckNextPos()){

      Debug.Log("check Past Next Pos");
      playerState = State.PhantomMove;
    }
  }
  void GetNextPos(){
    if(Input.GetKeyDown(KeyCode.W)){
      nextPos = new Vector3(transform.position.x, transform.position.y + gridSize, transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.S)){
      nextPos = new Vector3(transform.position.x, transform.position.y - gridSize, transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.A)){
      nextPos = new Vector3(transform.position.x - gridSize, transform.position.y , transform.position.z);
      transform.localScale = new Vector3(-1, 1, 1);
    }
    else if(Input.GetKeyDown(KeyCode.D)){
      nextPos = new Vector3(transform.position.x + gridSize, transform.position.y, transform.position.z);
      transform.localScale = new Vector3(1, 1, 1);
    }
    else{
      return;
    }


    if(CheckNextPos()){
      animator.SetBool("isMoving", true);
      playerState = State.Moving;
      recordedActions.Add((nextPos - transform.position).normalized);
    }
  }

  bool CheckNextPos(){
    Vector3 direction = (nextPos - gameObject.transform.position).normalized;

    RaycastHit2D hit = Physics2D.Raycast(transform.position + direction/2, direction, 0.5f);

    if(hit.collider != null){
      Debug.Log(hit.collider.gameObject);

      if(hit.collider.CompareTag("Player")) return true;
      if (hit.collider.CompareTag("Wall")) return false;
      Kick(hit, direction);
      return false;
    }else{
      return true;
    }

  }


  void Kick(RaycastHit2D hit, Vector3 direction)
  {
    animator.SetBool("isKicking", true);
    StartCoroutine(MoveBoxOverTime(hit.collider.transform, direction));
  }

  IEnumerator MoveBoxOverTime(Transform boxTransform, Vector3 direction)
  {
    Vector3 startPosition = boxTransform.position;
    Vector3 endPosition = startPosition + direction * gridSize;
    float time = 0f;
    float duration = 0.5f; // Adjust this value to control the speed of the movement

    while (time < duration)
    {
      time += Time.deltaTime;
      boxTransform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
      yield return null;
    }

    boxTransform.position = endPosition;
    animator.SetBool("isKicking", false);
  }

  void NextAction(){

  }


  void RespawnAsPhantom(){
    playerState = State.Phantom;
    
  }

  public void palyNextStep(){
    
    GetNextPastPos();
    
  }

  public void ConvertToPhantom(){
    transform.position = respawnPoint;
    playerState = State.Phantom;
  }


  public void ResetPhantom(){
    if(playerState != State.Phantom) return;
    ConvertToPhantom();
    ReplayIndex = 0;
    palyNextStep();
  }


  public bool isMoving(){
    return (playerState == State.PhantomMove && ReplayIndex < 1);
  }
}
