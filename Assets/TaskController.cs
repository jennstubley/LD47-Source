using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task
{
    public string Name;
    public string Object;
    public float Length; // in seconds
    public bool Heart;
    public int MaskIdx;

    public Task(string name, string obj, float len, bool heart = false, int maskIdx = -1)
    {
        Name = name;
        Object = obj;
        Length = len;
        Heart = heart;
        MaskIdx = maskIdx;
    }
}

public class TaskController : MonoBehaviour
{
    public float TaskSpeedMultiplier;
    public GameObject TaskPrefab;
    public DayController DayController;
    public GameObject[] Masks;
    public Room Room;
    public GameObject AllMask;
    public List<int> MasksForTomorrow = new List<int>();

    private List<Task> todoTasks = new List<Task>();
    private Task inProgressTask = null;
    private Action completionAction = null;
    private float taskTimer = 0;
    private float hintTimer = 0;
    private float hintDelay = 15;

    private bool CanDoExtraTasks = false;

    private List<Task> bonusTasks = new List<Task>()
    {
        new Task("Painting", "Easel", 4, true, 0),
        new Task("Water the plant", "Plant", 2, true, 1),
        new Task("Call a friend", "Phone", 4, true, 2),
        new Task("Pet the cat", "Cat", 2, true, 3),
    };

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (inProgressTask != null)
        {
            float multiplier = inProgressTask.Name == "Sleep" ? 1 : TaskSpeedMultiplier;
            int timerBreakpoint = Mathf.CeilToInt(taskTimer * multiplier);
            taskTimer = Mathf.Max(taskTimer - (Time.deltaTime / multiplier), 0);
            if (taskTimer == 0) CompleteTask();
            if (timerBreakpoint > Mathf.CeilToInt(taskTimer * multiplier)) DayController.DoTask();
        }
        if (inProgressTask == null && DayController.Energy == 0 && !(todoTasks.Count > 0 && todoTasks[0].Name == "Sleep"))
        {
            todoTasks.Clear();
            todoTasks.Add(new Task("Sleep", "Bed", 3));
            hintTimer = hintDelay;
            UpdateTaskUI();
        }
        if (hintTimer != 0)
        {
            hintTimer = Mathf.Max(0, hintTimer - Time.deltaTime);
            if (hintTimer == 0)
            {
                Debug.Log("Showing hint");
                if (todoTasks.Count > 0)
                {
                    Room.ShowHint(todoTasks[0].Object);
                }
            }
        }
    }

    public int RegisterInteraction(string obj, Action action)
    {
        if (inProgressTask != null) return 0;

        foreach (Task task in todoTasks)
        {
            if (!CanDoExtraTasks && todoTasks.IndexOf(task) > 0) return 0;

            if (task.Object.Equals(obj))
            {
                Room.HideHint();
                hintTimer = hintDelay;
                inProgressTask = task;
                completionAction = action;
                taskTimer = inProgressTask.Length;
                return task.Heart ? 2 : 1;
            }
        }
        if (!CanDoExtraTasks || todoTasks.Count > 0) return 0;
        foreach (Task task in bonusTasks)
        {
            if (task.Object.Equals(obj) && DayController.Energy >= task.Length)
            {
                Room.HideHint();
                inProgressTask = task;
                completionAction = action;
                taskTimer = inProgressTask.Length;
                return task.Heart ? 2 : 1;
            }
        }

        return 0;
    }

    public void ResetDay(float multiplier, bool canDoExtraTasks)
    {
        Room.HideHint();
        hintTimer = hintDelay;
        TaskSpeedMultiplier = multiplier;
        this.CanDoExtraTasks = canDoExtraTasks;
        if (inProgressTask != null)
        {
            CompleteTask();
        }
        todoTasks.Clear();
        todoTasks.Add(new Task("Eat", "Fridge", 1));
        todoTasks.Add(new Task("Brush teeth", "Sink", 1));
        todoTasks.Add(new Task("Get Dressed", "Dresser", 1));
        todoTasks.Add(new Task("Do Paperwork", "Computer", 3));
        UpdateTaskUI();

        for (int i = 0; i<Masks.Length; ++i)
        {
            Masks[i].SetActive(MasksForTomorrow.Contains(i));
        }
        AllMask.SetActive(MasksForTomorrow.Count == 4);
        MasksForTomorrow.Clear();

    }

    private void CompleteTask()
    {
        todoTasks.Remove(inProgressTask);
        if (inProgressTask.Name == "Sleep")
        {
            DayController.EndDay();
        }
        if (inProgressTask.MaskIdx != -1)
        {
            Masks[inProgressTask.MaskIdx].SetActive(true);
            if (!MasksForTomorrow.Contains(inProgressTask.MaskIdx))
            {
                MasksForTomorrow.Add(inProgressTask.MaskIdx);
            }
            bool allMasksShowing = true;
            foreach (GameObject mask in Masks)
            {
                if (!mask.activeSelf) allMasksShowing = false;
            }
            if (allMasksShowing)
            {
                AllMask.SetActive(true);
            }
        }
        if (todoTasks.Count == 0)
        {
            hintTimer = 0;
        }

        UpdateTaskUI();
        completionAction.Invoke();
        completionAction = null;
        inProgressTask = null;
    }

    private void UpdateTaskUI()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Task task in todoTasks)
        {
            GameObject obj = GameObject.Instantiate(TaskPrefab);
            obj.GetComponent<Text>().text = task.Name;
            obj.transform.SetParent(transform);
            if ((todoTasks.IndexOf(task) == 0 && !CanDoExtraTasks) || CanDoExtraTasks || task.Name.Equals("Sleep"))
            {
                obj.transform.Find("Marker").gameObject.SetActive(true);
            }
        }
    }

    internal float GetTaskCompletion()
    {
        if (inProgressTask == null) return 1;
        return (inProgressTask.Length - taskTimer) / inProgressTask.Length; 
    }
}
