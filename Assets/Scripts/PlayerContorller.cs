using System.Collections;
using System.Collections.Generic;
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

  public enum State {Idle, Moving, Dead, Phantom, PhantomMove,winning,next };
  public static bool isInDialogue;
  public static bool isInDialogueTrigger;

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

  void OnTriggerStay2D(Collider2D other)
  {
    if (other.gameObject.CompareTag("WinCheckerbox") && playerState != State.next)
    {
      playerState = State.winning;
      animator.SetBool("isSolved", true);

      AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

      if (stateInfo.normalizedTime >= 1)
      {
        playerState = State.next;
        Debug.Log("Animation has ended");
        game.WinGame();
      }
    }
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
    
    if (isInDialogue) return;
    
    if(playerState == State.Idle && game.playerAllSettle()){
      //.Log("shabi");
      GetNextPos();
    }

    if (playerState == State.Phantom){

      if (Input.anyKeyDown){
        if(!Input.GetKey(KeyCode.F)){
          playNextStep();
        }
      }
    }
  }



  void GetNextPastPos(){
    Debug.Log(gameObject);
    
    if(ReplayIndex >= recordedActions.Count) return;
    Vector3 direction = recordedActions[ReplayIndex];
    if (direction.x > 0)
    {
      transform.localScale = new Vector3(1, 1, 1);  // Facing right
    }
    else if (direction.x < 0)
    {
      transform.localScale = new Vector3(-1, 1, 1);  // Facing left
    }
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

    RaycastHit2D hit1 = Physics2D.Raycast(transform.position + direction*gridSize*0.7f, direction, 0.3f*gridSize);

    if(hit1.collider != null){
      Debug.Log(hit1.collider.gameObject);

      
      if (hit1.collider.CompareTag("Wall")) return false;

      if (hit1.collider.CompareTag("WinCheckerbox")) return true;
      if(hit1.collider.CompareTag("Player") || hit1.collider.CompareTag("Key")) return true;

      if(hit1.collider.CompareTag("MovableBox")){
        Debug.Log("Box In Way");
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + direction*gridSize * 1.7f, direction, 0.3f*gridSize);


        if(hit2.collider == null) {
          Debug.Log("Success");
          Kick(hit1, direction);
          if(playerState != State.Phantom)
          {recordedActions.Add((nextPos - transform.position).normalized);}
          else {ReplayIndex++;}
          return false;
        }
        Debug.Log(hit2.collider.name);
        if(hit2.collider.CompareTag("Player")){
          Debug.Log("Box hit [Player]");

          Kick(hit1, direction);
          if(playerState != State.Phantom)
          {recordedActions.Add((nextPos - transform.position).normalized);}
          else {ReplayIndex++;}
        }
        else if(hit2.collider.CompareTag("WinCheckerbox"))
        {
          Kick(hit1, direction);
          if (playerState != State.Phantom)
          { recordedActions.Add((nextPos - transform.position).normalized); }
          else { ReplayIndex++; }
        }
        
        else{
          return false;
        }
      }
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
    if(playerState != State.Phantom){
      game.DecreaseActionCount();
    }
  }

  public void playNextStep(){
    GetNextPastPos();   
  }

  public void ConvertToPhantom(){
    transform.position = respawnPoint;
    playerState = State.Phantom;
  }

  public void ConvertToDeath()
  {
    if(playerState == State.Dead) {
      animator.SetBool("isExhausted", true);
    }

    StartCoroutine(checkDeathAnimationEnd());
  }

  public IEnumerator checkDeathAnimationEnd()
  {

    while (true)
    {
      AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

      if (stateInfo.IsName("Death") && stateInfo.normalizedTime >= 1.0f)
      {
        Debug.Log("Death animation has ended");
        animator.SetBool("isExhausted", false);
        playerState = State.Idle;
        break;  // Exit the loop once the death animation has ended
      }
      yield return null;  // Wait for the next frame before the loop iterates again


    }
    game.ResetScene();
  }

  public void ResetPhantom(){
    if(playerState != State.Phantom && playerState != State.PhantomMove) {
      Debug.Log(playerState);
      return;
    }
    ConvertToPhantom();
    ReplayIndex = 0;
    playNextStep();
  }


  public bool isMoving(){
    return (playerState == State.PhantomMove && ReplayIndex < 1);
  }
}
