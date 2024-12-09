using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSystem : MonoBehaviour
{
    internal int currentProgress;
    internal int requiredProgress;
    public TextMeshProUGUI currentProgressDisplay;
    public TextMeshProUGUI requiredProgressDisplay;
    public GameObject questCanvas;
    internal bool alreadyTookQuest = false;
    internal bool questCompleted = false;
    internal bool isQuestCompleted = false;

    private void Awake()
    {
        questCanvas.SetActive(false);
    }
}
