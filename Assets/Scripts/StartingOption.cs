using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingOption : MonoBehaviour
{

    public Choices[] choices;
    public GameController CurrentGame;
    
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        Time.timeScale = 0;

    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckCurrentSelection();


    }

    void CheckCurrentSelection(){  
        //Debug.Log(index);

        for(int i = 0; i < choices.Length; i++){
            if(i == index){
                choices[i].Selected();
            }else{
                choices[i].DeSelected();
            }
        }
        

    }
    public void DisableMenu(){
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void EnableMenu(){
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    void CheckInput(){
        if(Input.GetKeyDown(KeyCode.W)){
            if(index == 0){
                index = choices.Length - 1;
            }else{
                index --;
            }
        }

        if(Input.GetKeyDown(KeyCode.S)){
            if(index == choices.Length - 1){
                index = 0;
            }else{
                index ++;
            }
        }

        if(Input.GetKeyDown(KeyCode.Return)){
            choices[index].ExecuteChoice();

        }
    }
}
