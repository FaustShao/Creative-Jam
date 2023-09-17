using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
  public float moveSpeed = 1f;
  public float gridSize = 1f;
  public GameController game;

  public Vector3[] RecordedAction;

  Vector3 respawnPoint;
  public Vector3 nextPos;
  public int ReplayIndex;
  bool canMove;
  public bool isPhantom;
  public bool isInDialogue;

  public List<Vector3> recordedActions;
  void Start()
  {
    respawnPoint = gameObject.transform.position;
    ReplayIndex = 0;
    canMove = false;
    isPhantom = false;
    nextPos = transform.position;
  }

  void FixedUpdate()
  {
    MoveToNextPos();
  }

  
  void MoveToNextPos(){
    if(canMove){
      if(transform.position != nextPos){
        transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime);
      }else{
        Debug.Log(canMove);
        canMove = false;
        game.DecreaseActionCount();
      }
    }
  }


  void Update()
  {
    
    //Debug.Log(canMove);
    if(isInDialogue) return;
    if(canMove){
      Debug.Log("NotMoving");
      return;
    }
    
    if(isPhantom){
      if(ReplayIndex == 0){
        ReplayNextStep();
      }
    }
    GetNextPos();
    CheckNextPos();
    
  }

  public void ReplayNextStep(){

    Debug.Log(ReplayIndex);
    Vector3 position = transform.position + RecordedAction[ReplayIndex];
    nextPos = RecordedAction[ReplayIndex];
    CheckNextPos();
    ReplayIndex++;
  }

    void GetNextPos(){

    
    if(Input.GetKeyDown(KeyCode.W)){
      nextPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.S)){
      nextPos = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.A)){
      nextPos = new Vector3(transform.position.x - 1f, transform.position.y , transform.position.z);
    }else if(Input.GetKeyDown(KeyCode.S)){
      nextPos = new Vector3(transform.position.x + 1f, transform.position.y - 1f, transform.position.z);
    }

    recordedActions.Add((nextPos - transform.position).normalized);
  }

  void CheckNextPos(){
    Vector3 direction = (nextPos - gameObject.transform.position).normalized;

    RaycastHit2D hit = Physics2D.Raycast(transform.position + direction/2, direction, 0.5f);

    if(hit.collider != null){

    }else{
      canMove = true;
    }

  }
  
  
  void NextAction(){

  }
}
