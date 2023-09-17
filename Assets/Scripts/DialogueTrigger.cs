using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Keybind")]
    public KeyCode InteractionKeyBind;
    private int index;
    public int RepeatingIndex;
    public Dialogue dialogue;

    public bool isOnActive;

    [Serializable]
    public class ConversationList
    {
        public string[] dialogue;
    }
    public ConversationList[] dialogues;

    [Serializable]
    public class ConversationIconList
    {
        public Sprite[] icon;
    }
    public ConversationIconList[] icons;


    private void Awake()
    {
        StartCoroutine(loadStartDelayed());
    }

    IEnumerator loadStartDelayed()
    {
        yield return new WaitForSeconds(0.1f);
        if(isOnActive){
            ForceTrigger();
            isOnActive = false;
            
        }
        
    }
    void Start()
    {   
        InteractionKeyBind = KeyCode.F;
        index = 0;
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other){
        PlayerController Player = other.gameObject.GetComponent<PlayerController>();
        if(Player.playerState != PlayerController.State.Idle ) return;
        PlayerController.isInDialogueTrigger = true;
    }
    void OnTriggerExit2D(Collider2D other){
        PlayerController Player = other.gameObject.GetComponent<PlayerController>();
        if(Player.playerState != PlayerController.State.Idle ) return;
        PlayerController.isInDialogueTrigger = false;
    }
    void OnTriggerStay2D(Collider2D other)
    {   
        
        PlayerController Player = other.gameObject.GetComponent<PlayerController>();
        if(Player.playerState != PlayerController.State.Idle ) return;
        Debug.Log(other.gameObject.CompareTag("Player"));
        PlayerController.isInDialogue = dialogue.isInDialogue;
        if(Input.GetKeyDown(InteractionKeyBind) && !dialogue.isInDialogue){
            
            if(index <= dialogues.Length - 1){
                dialogue.SetDialogue(dialogues[index].dialogue,icons[index].icon);
                dialogue.ActivateDialogue(Player);
                index++;
            }else{
                dialogue.SetDialogue(dialogues[RepeatingIndex].dialogue,icons[RepeatingIndex].icon);
                dialogue.ActivateDialogue(Player);
            }

        }
    }

    public void ForceTrigger(){

        //Time.timeScale = 0;
        PlayerController.isInDialogue = true;
        PlayerController Player = null;
        if(index <= dialogues.Length - 1){
            dialogue.SetDialogue(dialogues[index].dialogue,icons[index].icon);
            dialogue.ActivateDialogue(Player);
            index++;
        }else{
            dialogue.SetDialogue(dialogues[RepeatingIndex].dialogue,icons[RepeatingIndex].icon);
            dialogue.ActivateDialogue(Player);
        }        
            

    }
}
