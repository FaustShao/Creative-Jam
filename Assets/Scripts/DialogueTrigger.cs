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
    void Start()
    {
        InteractionKeyBind = KeyCode.F;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {   
        Debug.Log(other);
        if(Input.GetKeyDown(InteractionKeyBind) && !dialogue.isInDialogue){

            if(index <= dialogues.Length - 1){
                dialogue.SetDialogue(dialogues[index].dialogue,icons[index].icon);
                dialogue.ActivateDialogue();
                index++;
            }else{
                dialogue.SetDialogue(dialogues[RepeatingIndex].dialogue,icons[RepeatingIndex].icon);
                dialogue.ActivateDialogue();
            }

        }
    }
}
