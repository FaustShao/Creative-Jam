using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    
    public TextMeshProUGUI textComponent;
    public Image imageComponent;
    public string[] lines;  
    public bool isInDialogue;
    public Sprite[] icons;
    public float textspeed;


    public KeyCode SkipKey;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        isInDialogue = false;
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        //StartDialog();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(textComponent.text == lines[index])
            {
                NextLine();
            }else{
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialog(){
        index = 0;

        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine(){

        imageComponent.sprite = icons[index];
        foreach (char c in lines[index].ToCharArray()){

            textComponent.text += c;
            yield return new WaitForSeconds(textspeed);

        }
    }


    void SkipDialogue(){

        
        StopAllCoroutines();
        index = 0;
        textComponent.text = string.Empty;
        
        DeactivateDialogue();
    }

    void NextLine(){
        if(index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }else{
            DeactivateDialogue();
        }
    }
    
    public void ActivateDialogue(){
        isInDialogue = true;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);
        StartDialog();
    }

    public void DeactivateDialogue(){
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        isInDialogue = false;
    }
    public void SetDialogue(string[] str, Sprite[] sprites){
        lines = str;
        icons = sprites;
    }

    

}
