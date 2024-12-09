using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public TextMeshProUGUI dialogueText;
    public DialogueManager dialogueManager;
    private QuestSystem questSystem;

    private void Awake()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueManager = GetComponent<DialogueManager>();
        questSystem = GameObject.Find("QuestCanvas").GetComponent<QuestSystem>();
    }

    public void CallDialogue()
    {
        if (!questSystem.alreadyTookQuest && !questSystem.isQuestCompleted)
        {
            StartCoroutine(TriggerDialogue());
        }
        if (questSystem.alreadyTookQuest && !questSystem.isQuestCompleted)
        {
            StartCoroutine(TriggerAlternative());
        }
        if (questSystem.isQuestCompleted)
        {
            StartCoroutine(TriggerRewardText());
        }
    }

    private IEnumerator TriggerAlternative()
    {
        yield return new WaitForSeconds(0.1f);
        dialogueManager.StartAlternative(dialogue);
        StartCoroutine(textAppearing());
    }

    private IEnumerator TriggerRewardText()
    {
        yield return new WaitForSeconds(0.1f);
        dialogueManager.StartRewardText(dialogue);
        StartCoroutine(textAppearing());
    }

    private IEnumerator TriggerDialogue()
    {
        yield return new WaitForSeconds(0.1f);
        dialogueManager.StartDialogue(dialogue);
        StartCoroutine(textAppearing());
    }

    public IEnumerator textAppearing()
    {
        yield return new WaitForSeconds(0.1f);
        dialogueText.gameObject.SetActive(true);
    }
}
