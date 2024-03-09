using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SlotReaderManager : MonoBehaviour, ISaveable
{

    private List<ImportJob> jobsList = new();

    [SerializeField] private List<SlotReader> slotReaderList = new();
    [SerializeField] private ImportHolder importHolder;

    private GameTime savedTime;
    private bool isSaved = false;


    private EventManagerRefrence eventManagerRefrence;

    private SlotReaderManagerRefrence refrence;

    private ScannerSlotManagerRefrence scannerSlotManagerRefrence;
    private SaveClassManagerRefrence saveClassManagerRefrence;


    private bool isStarted = false;


    private void SetRefrence()
    {
        refrence = (SlotReaderManagerRefrence)FindSORefrence<SlotReaderManager>.FindScriptableObject("SlotReader Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        Debug.Log("We did find it");
        refrence.val = this;
    }

    private void LoadSORefrence()
    {
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
        scannerSlotManagerRefrence = (ScannerSlotManagerRefrence)FindSORefrence<ScannerSlotManager>.FindScriptableObject("Scanner Slot Manager Refrence");
        saveClassManagerRefrence = (SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence");
    }

    private void Awake()
    {
        SetRefrence();
    }


    private void Start()
    {
        LoadSORefrence();
        AddISaveableToDictionary();
        isStarted = true;
        if(gameObject.activeSelf)
            InitializationUI();
    }

    private void OnEnable()
    {
        SavedTimeChecker();
        if(isStarted)
            InitializationUI();
    }

    private void Update()
    {
        if(jobsList.Count > 0)
        {
            CheckJobRemove();
        }
    }
    private void OnDisable()
    {
        if (eventManagerRefrence.val != null)
        {
            eventManagerRefrence.val.SecondsElapsedRemoveListener(SecondElapsed);
            savedTime = new GameTime().CurrentTime(eventManagerRefrence.val);
            isSaved = true;
        }
    }

    /// <summary>
    /// Save the current time OnDisable so when the next OnEnable method calls will calculate the 
    /// time spent when the UI has been closed.
    /// </summary>
    private void SavedTimeChecker()
    {
        if (isSaved)
        {
            float difTime = new GameTime().RawTimeCurrentMinusSaved(savedTime, eventManagerRefrence.val);
            foreach(var job in jobsList.ToList()) 
            {
                Debug.Log(difTime.ToString());
                job.JumpTick(difTime);
            }
            isSaved = false;
        }
        if(jobsList.Count > 0)
        {
            for(int i = 0; i < jobsList.Count; i++)
            {
                if (!jobsList[i].shouldRemove)
                {
                    eventManagerRefrence.val.SecondsElapsedAddListener(SecondElapsed);
                }
            }
        }
    }


    private void InitializationUI()
    {
        List<ScannerSlotUI> slots = scannerSlotManagerRefrence.val.slots;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].state == ScannerSlotManager.SlotState.canAdd)
            {
                SlotReader target = slotReaderList[i];
                target.SetInteractiveButton(false);
                target.SetButtonText("Empty");
                target.SetImageSprite(null);
                target.SetTimerText("Slot is Empty");
                target.SetNameText("Empty");
            }
            else if (slots[i].state == ScannerSlotManager.SlotState.canRemove)
            {
                SlotReader target = slotReaderList[i];
                target.SetInteractiveButton(true);
                target.SetButtonText("Start");
                target.SetImageSprite(slots[i].holdingItem.IconRefrence());
                target.SetTimerText(" 0 : 0");
                target.SetNameText(slots[i].holdingItem.GetName());
            }
            else if (slots[i].state == ScannerSlotManager.SlotState.passive)
            {
                string n = " ";
                foreach (var job in jobsList)
                {
                    if(job.GetSlotNumber() == i)
                        n = job.GetTimerRemaining().ToString();
                }

                SlotReader target = slotReaderList[i];
                target.SetTimerText(n);
                target.SetInteractiveButton(true);
                target.SetButtonText("Cancle");
                target.SetImageSprite(slots[i].holdingItem.IconRefrence());
                target.SetNameText(slots[i].holdingItem.GetName());
            }
            else if (slots[i].state == ScannerSlotManager.SlotState.isDone)
            {
                SlotReader target = slotReaderList[i];
                target.SetInteractiveButton(false);
                target.SetButtonText("Is Done!");
                target.SetImageSprite(slots[i].holdingItem.IconRefrence());
                target.SetTimerText(" 0 : 0");
                target.SetNameText(slots[i].holdingItem.GetName());
            }
        }
    }



    /// <summary>
    /// To set the timer text on each tick rate for the new value of the text
    /// </summary>
    /// <param name="job"></param>
    private void RefreshTimerText(ImportJob job)
    {
        string textTimer = job.GetTimerRemaining().ToString();
        switch (job.GetSlotNumber())
        {
            case 0:
                slotReaderList[0].SetTimerText(textTimer);break;
            case 1:
                slotReaderList[1].SetTimerText(textTimer);break;
            case 2:
                slotReaderList[2].SetTimerText(textTimer);break;
            default:
                Debug.LogWarning("CHECK HERE ASAP");break;
        }
    }


    /// <summary>
    /// To check if the bool shouldRemove is true to remove it from the list
    /// </summary>
    private void CheckJobRemove()
    {
        foreach(var job in jobsList.ToList())
        {
            if (job.shouldRemove == true)
            {
                jobsList.Remove(job);
            }
        }
    }

    /// <summary>
    /// will get called from the buttons in ui to tell it's functional need
    /// </summary>
    /// <param name="i"></param>
    public void ButtonHit(int i)
    {
        ScannerSlotUI targetSlotUI = scannerSlotManagerRefrence.val.slots[i - 1];
        bool beenAddedBefore = importHolder.IfBluePrintAllreadyImported(targetSlotUI.holdingItem);

        if (beenAddedBefore)
        {
            Debug.Log("Already added to the list");
        }
        else if (targetSlotUI.state == ScannerSlotManager.SlotState.canRemove)
        {
            NewJobStart(i - 1);
        }
        else if (targetSlotUI.state == ScannerSlotManager.SlotState.passive)
        {
            NewJobCancel(i - 1);
        }
        InitializationUI();
    }

    /// <summary>
    /// To add the job in the que and set the slot state to passive
    /// </summary>
    /// <param name="slotNumber"></param>
    private void NewJobStart(int slotNumber)
    {
        if (jobsList.Count == 0) 
        {
            eventManagerRefrence.val.SecondsElapsedAddListener(SecondElapsed);
            Debug.Log("New here");
        }
        ScannerSlotUI targetSlotUI = scannerSlotManagerRefrence.val.slots[slotNumber];
        int totalTime = targetSlotUI.holdingItem.ImportTimer();
        ImportJob importJob = new(slotNumber, totalTime,this);
        jobsList.Add(importJob);
        scannerSlotManagerRefrence.val.slots[slotNumber].state = ScannerSlotManager.SlotState.passive;
    }

    /// <summary>
    /// To remove the job from the que and set the slot state to canRemove
    /// </summary>
    /// <param name="slotNumber"></param>
    private void NewJobCancel(int slotNumber)
    {
        if (jobsList.Count == 1)
        {
            eventManagerRefrence.val.SecondsElapsedRemoveListener(SecondElapsed);
            Debug.Log("New job cancel");
        }
        foreach (var job in jobsList.ToList())
        {
            if (job.GetSlotNumber() == slotNumber)
            {
                scannerSlotManagerRefrence.val.slots[slotNumber].state = ScannerSlotManager.SlotState.canRemove;
                job.shouldRemove = true;
                return;
            }
        }
        Debug.LogWarning("CHECK HERE FAST");
    }



    /// <summary>
    /// to remove the job from the list and set the slot state to isdone
    /// </summary>
    /// <param name="job"></param>
    public void NewJobFinished(ImportJob job)
    {
        if (jobsList.Count == 1)
        {
            eventManagerRefrence.val.SecondsElapsedRemoveListener(SecondElapsed);
            Debug.Log("new job finished");
        }
        ScannerSlotUI targetSlotUI = scannerSlotManagerRefrence.val.slots[job.GetSlotNumber()];
        targetSlotUI.state = ScannerSlotManager.SlotState.isDone;
        job.shouldRemove = true;
        BluePrintItem doneBluePrint = targetSlotUI.holdingItem;
        InitializationUI();
        AddHologramToBluePrintLibrary(doneBluePrint);
    }
    
    /// <summary>
    /// The method to set the listener of time system tickrate.
    /// </summary>
    public void SecondElapsed()
    {
        foreach (var job in jobsList.ToList())
        {
            if(!job.shouldRemove)
                job.Tick();
            CheckJobRemove();
            RefreshTimerText(job);
        }
    }


    private void AddHologramToBluePrintLibrary(BluePrintItem hologram)
    {
        importHolder.ImportNewItem(hologram);
    }

    public void AddISaveableToDictionary()
    {
        saveClassManagerRefrence.val.AddISaveableToDictionary("SlotReader", this, 4);
    }

    public object Save()
    {
        SaveClassesLibrary.SlotReaderManager savedData = new(jobsList);
        return savedData;
    }

    public void Load(object savedData)
    {
        SaveClassesLibrary.SlotReaderManager data = (SaveClassesLibrary.SlotReaderManager)savedData;
        jobsList = data.savedJobsList;
        SavedTimeChecker();
        InitializationUI();
    }

    public string GetName()
    {
        return "SlotReader";
    }
}

/// <summary>
/// This class is for the jobs to import the holograms in to the computer in the game. 
/// it will handle the timer that we need to import the holo in.
/// </summary>
[Serializable]
public class ImportJob
{
    private int slotNumber;
    private int totalTime;
    private int timeLeft;
    private SlotReaderManager manager;
    public bool shouldRemove = false;

    public ImportJob(int _SlotNumber, int _Timer,SlotReaderManager manager)
    {
        slotNumber = _SlotNumber;
        totalTime = _Timer;
        timeLeft = totalTime;
        this.manager = manager;
    }
    
    public void Tick()
    {
        if (timeLeft <= 0)
        {
            manager.NewJobFinished(this);
            shouldRemove = true;
            timeLeft = 0;
            return;
        }
        timeLeft -= 1;
    }

    public void JumpTick(float timeJumpValue)
    {
        timeLeft -= (int)timeJumpValue;
        if (timeLeft <= 0)
        {
            manager.NewJobFinished(this);
            shouldRemove = true;
            timeLeft = 0;
            return;
        }
    }

    public float GetTimerRemaining()
    {
        return timeLeft;
    }

    public int GetSlotNumber()
    {
        return slotNumber;
    }
}