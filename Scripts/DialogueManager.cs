using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    private DialogueTrigger dialogueTrigger;
    private Queue<string> npcGreetings;
    private Queue<string> alternativeText;
    private Queue<string> rewardText;
    public Button dialogueContinueBtn;
    public Image npcImage;
    public Image dialogueCloud;
    private PlayerAttack playerAttack;
    private soldierAttack soldierAttack;
    private PlayerSpawner playerSpawner;
    private QuestSystem questSystem;

    private void Awake()
    {
        dialogueContinueBtn.interactable = false;
        npcGreetings = new Queue<string>();
        alternativeText = new Queue<string>();
        rewardText = new Queue<string>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        questSystem = GameObject.Find("QuestCanvas").GetComponent<QuestSystem>();
    }

    internal void ShowNpc()
    {
        npcImage.gameObject.SetActive(true);
        dialogueCloud.gameObject.SetActive(true);
        dialogueContinueBtn.gameObject.SetActive(true);
        playerSpawner = GameObject.Find("GameManager").GetComponent<PlayerSpawner>();
        if (playerSpawner.spartanChosen)
        {
            playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
            playerAttack.canAttack = false;
        }
        else
        {
            soldierAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<soldierAttack>();
            soldierAttack.canAttack = false;
            soldierAttack.canRotate = false;
        }
    }

    internal void StartDialogue(Dialogue dialogue)
    {
        npcGreetings.Clear();
        foreach(string npcGreeting in dialogue.npcGreetings)
        {
            npcGreetings.Enqueue(npcGreeting);
        }
        DisplayNextGreeting();
    }

    internal void StartRewardText(Dialogue dialogue)
    {
        rewardText.Clear();
        foreach (string rewardSpeech in dialogue.wellDone)
        {
            rewardText.Enqueue(rewardSpeech);
        }
        DisplayRewardText();
    }

    internal void StartAlternative(Dialogue dialogue)
    {
        alternativeText.Clear();
        foreach(string alterText in dialogue.areYouStillHere)
        {
            alternativeText.Enqueue(alterText);
        }
        DisplayNextAlternative();
    }

    public void DisplayNextCheck()
    {
        if(!questSystem.alreadyTookQuest && !questSystem.isQuestCompleted)
        {
            DisplayNextGreeting();
        }
        else if(questSystem.alreadyTookQuest && !questSystem.isQuestCompleted)
        {
            DisplayNextAlternative();
        }
        else if(questSystem.isQuestCompleted && questSystem.isQuestCompleted)
        {
            DisplayRewardText();
        }
    }

    private void DisplayRewardText()
    {
        dialogueContinueBtn.interactable = false;
        if (rewardText.Count == 0)
        {
            EndReward();
            return;
        }
        string rewardSpeech = rewardText.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeRewarded(rewardSpeech));
    }

    public void DisplayNextAlternative()
    {
        dialogueContinueBtn.interactable = false;
        if (alternativeText.Count == 0)
        {
            EndAlternative();
            return;
        }
        string alterText = alternativeText.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeAlternative(alterText));
    }

    public void DisplayNextGreeting()
    {
        dialogueContinueBtn.interactable = false;
        if(npcGreetings.Count == 0)
        {
            EndDialogue();
            return;
        }
        string npcGreeting = npcGreetings.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(npcGreeting));
    }

    private IEnumerator TypeRewarded(string rewardSpeech)
    {
        dialogueText.text = "";

        yield return new WaitForSeconds(0.1f);
        foreach (char letter in rewardSpeech.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(1f);
        dialogueContinueBtn.interactable = true;
    }

    private IEnumerator TypeAlternative(string alterText)
    {
        dialogueText.text = "";

        yield return new WaitForSeconds(0.1f);
        foreach(char letter in alterText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(1f);
        dialogueContinueBtn.interactable = true;
    }

    private IEnumerator TypeSentence(string npcGreeting)
    {
        dialogueText.text = "";

        yield return new WaitForSeconds(0.1f);
        foreach(char letter in npcGreeting.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(1f);
        dialogueContinueBtn.interactable = true;
    }

    private void EndReward()
    {
        dialogueText.text = "";
        npcImage.gameObject.SetActive(false);
        dialogueContinueBtn.gameObject.SetActive(false);
        dialogueCloud.gameObject.SetActive(false);
        questSystem.questCanvas.SetActive(false);
        if (playerSpawner.spartanChosen)
        {
            playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
            playerAttack.canAttack = true;
        }
        else
        {
            soldierAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<soldierAttack>();
            soldierAttack.canAttack = true;
            soldierAttack.canRotate = true;
        }
    }

    internal void EndAlternative()
    {
        npcImage.gameObject.SetActive(false);
        dialogueContinueBtn.gameObject.SetActive(false);
        dialogueCloud.gameObject.SetActive(false);
        if (playerSpawner.spartanChosen)
        {
            playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
            playerAttack.canAttack = true;
        }
        else
        {
            soldierAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<soldierAttack>();
            soldierAttack.canAttack = true;
            soldierAttack.canRotate = true;
        }
        dialogueText.text = "";
    }

    private void EndDialogue()
    {
        HideNPC();
    }

    public void HideNPC()
    {
        npcImage.gameObject.SetActive(false);
        dialogueContinueBtn.gameObject.SetActive(false);
        dialogueCloud.gameObject.SetActive(false);
        if (playerSpawner.spartanChosen)
        {
            playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
            playerAttack.canAttack = true;
        }
        else
        {
            soldierAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<soldierAttack>();
            soldierAttack.canAttack = true;
            soldierAttack.canRotate = true;
        }
        questSystem.alreadyTookQuest = true;
        questSystem.questCanvas.SetActive(true);
        questSystem.currentProgress = 0;
        questSystem.currentProgressDisplay.text = questSystem.currentProgress.ToString();
        questSystem.requiredProgress = 10;
        questSystem.requiredProgressDisplay.text = questSystem.requiredProgress.ToString();
        dialogueText.text = "";
    }
}
