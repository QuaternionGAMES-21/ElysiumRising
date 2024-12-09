using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractWindowTrigger : MonoBehaviour
{
    public Image dialogueStartImage;
    private int hasPlayer = 0;
    private DialogueTrigger dialogueTrigger;
    private DialogueManager dialogueManager;
    
    private void Awake()
    {
        dialogueTrigger = GameObject.Find("DialogueSystem").GetComponent<DialogueTrigger>();
        dialogueManager = GameObject.Find("DialogueSystem").GetComponent<DialogueManager>();
    }

    private void Update()
    {
        dialogueStartImage.gameObject.SetActive(hasPlayer == 1 ? true : false);
        if(Input.GetKeyDown(KeyCode.E) && hasPlayer == 1)
        {
            dialogueManager.ShowNpc();
            dialogueTrigger.CallDialogue();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasPlayer = 0;
        }
        dialogueManager.EndAlternative();
    }
}
