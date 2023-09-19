using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("SceneKeyAttributes")]
    public int maxActionCount;
    public string nextScene;
    public int maxRewindCount;
    public string sceneName;
    public Vector2 spawnPoint;
    public int remainingActionCount;
    public int playerGenerationCounter = 0;
    public int remainingRewindCount;
    public bool isSolved;
    public bool ActiveFailCheck;
    public MenuController Menu;
    public Animator AliveAnimator;
    public Animator PhantomAnimator;
    public RuntimeAnimatorController AliveController;
    public RuntimeAnimatorController PhantomController;
    //TOD: Replace GameObject Class with actual class
    [Header("GameObjects")]
    public GameObject Endpoint;
    public List<Vector3> boxPositions;
    public List<GameObject> boxs;
    public PlayerController CurrentActivePlayer;
    public List<PlayerController> Player_Live;
    public GoalDevice Goal;
    public List<GameObject> Walls;
    public List<GameObject> Turrets;
    public UnlockableBarrier Door;
    public Locks DoorTrigger;


    public DialogueTrigger LowLifeWarning;
    int playerIndex = 0;
    bool isPause;

    //[Header("Prefabs")]
    //public GameObject playerPrefab;
    //public GameObject level1Object;

    void Start()
    {
      FailSceneController.previouScene = SceneManager.GetActiveScene().buildIndex;
      FailSceneCollide.lastScene = SceneManager.GetActiveScene().buildIndex;
      ActiveFailCheck = false;

      isSolved = false;
      remainingActionCount = maxActionCount;
      remainingRewindCount = maxRewindCount;
      isPause = false; 

      CurrentActivePlayer = Player_Live[maxRewindCount - remainingRewindCount];
      for(int i =0; i < Player_Live.Count;i++){
        if(i != maxRewindCount - remainingRewindCount){
          Player_Live[i].gameObject.SetActive(false);
        }
      }
    }


    public bool playerAllSettle(){
      
      foreach(PlayerController p in Player_Live){
        if (p.isMoving()) return false;
      }
      return true;
    }
    // Update is called once per frame
    void Update()
    {
      CheckPause();
      CheckCollideFailStatus();
      if(isPause){
        CheckFinishedReplay();
      }
    }

    void CheckRewindAcitivateWarining(){
      if(LowLifeWarning == null) return;
      if(remainingRewindCount == 1){
        LowLifeWarning.ForceTrigger();
      }
      
    }


    void CheckFinishedReplay(){
      if(CurrentActivePlayer.ReplayIndex > 0){
        isPause = false;
        playerIndex++;
        if(playerIndex < Player_Live.Count){
          CurrentActivePlayer = Player_Live[playerIndex];
        }else{
          FailGame();
        }
      }
    }

    public void DecreaseActionCount(){
      remainingActionCount--;

      if(remainingActionCount == 0){
        CurrentActivePlayer.playerState = PlayerController.State.Dead;
        remainingRewindCount--;
        CheckRewindAcitivateWarining();
        //death animation then Convert to Phantom
        remainingActionCount = maxActionCount;
        ResetWalls();
        SetPhantomAnimator();
        CurrentActivePlayer.ConvertToPhantom();
        ResetAllPhantom();
        GetNextPlayer();
        
      }
    }

    public void ResetWalls()
    {
      for(int i=0; i<boxs.Count; i++)
      {
        Debug.Log("reset"+boxs[i]);
        boxs[i].transform.position = boxPositions[i];
      }
    }


     void CheckCollideFailStatus(){
      if(!ActiveFailCheck){
        return;
      }
      foreach(PlayerController p in Player_Live){
        if(CurrentActivePlayer == p) {
          Debug.Log("SameActivePlayer");
        }
        if(p.playerState == PlayerController.State.PhantomMove){ 

          if((CurrentActivePlayer.transform.position - p.transform.position ).magnitude <= 0.1f){
            FailGameCollide();
          }
          
        }
      }
    }
    void FailGameCollide(){
      SceneManager.LoadScene("Assets/Scenes/FailSceneCollide.unity");
    }
    public void SetPhantomAnimator()
    {
        CurrentActivePlayer.animator.runtimeAnimatorController = PhantomController;
    }

    void ResetAllPhantom(){
      ActiveFailCheck = false;
      foreach ( PlayerController p in Player_Live){
        p.ResetPhantom();
      }
      Invoke(nameof(SetToActiveFailCheck),0.2f);
    }
    void SetToActiveFailCheck(){
      ActiveFailCheck = true;
    }
    public void GetNextPlayer(){
      playerIndex++;
      if(playerIndex < Player_Live.Count){
        CurrentActivePlayer = Player_Live[playerIndex];
        CurrentActivePlayer.gameObject.SetActive(true);
      }else{
        FailGame();
      }
    }

    public void FailGame(){
        //TODO: add Game Fail ;
        Debug.Log("GameFail");
        //reload Current Scene;
         

        SceneManager.LoadScene("Assets/Scenes/FailScene.unity");
    }
     public void WinGame(){
        //TODO: add Game Win Event ;
        if(nextScene != null){
          SceneManager.LoadScene(nextScene);
        }
        Debug.Log("TODO: Activate Win Game Effect");
     }

    void CheckPause(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Menu.isActiveOnStart = true;
            Menu.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
