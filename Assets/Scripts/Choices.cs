using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Choices : MonoBehaviour
{

    [Header("Class Connections")]
    public GameObject Menu;
    public GameController CurrentGame;

    [Header("Assets")]
    public Sprite UnselectedImage;
    public Sprite SelectedImage;

    [Header("Property")]
    public bool isStart;
    public bool isExit;
    public bool isContinue;
    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetComponent<Image>());
    }


    public void Selected(){    
        GetComponent<Image>().sprite = SelectedImage;

    }

    public void DeSelected(){
         GetComponent<Image>().sprite = UnselectedImage;
    }
    public void ExecuteChoice(){
        if(isStart){
            Debug.Log("Start");
            StartScene();
        }else if(isExit){
            Debug.Log("Exit");
            Application.Quit();
        }else if(isContinue){
            Debug.Log("Continue"); 
            Menu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void StartScene(){
        SceneManager.LoadScene(CurrentGame.nextScene,LoadSceneMode.Single);
    }
}
