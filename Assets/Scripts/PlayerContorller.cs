using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  public float gridSize = 1f;
  public GameController game;



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
    nextPos = transform.position + direction;

    Debug.Log(nextPos);
    if(CheckNextPos()){

      Debug.Log("check Past Next Pos");
      playerState = State.PhantomMove;
    }
  }
  void GetNextPos(){
    if(Input.GetKeyDown(KeyCode.W)){
      nextPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.S)){
      nextPos = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.A)){
      nextPos = new Vector3(transform.position.x - 1f, transform.position.y , transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.D)){
      nextPos = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
    }else{
      return;
    }


    if(CheckNextPos()){
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
      Kick();
      return false;
    }else{
      return true;
    }

  }
  

  void Kick(){

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
