using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Day
{
    public string Story;
    public float Multiplier;
    public float Energy;

    public Day(string story, float mult, float energy)
    {
        Story = story;
        Multiplier = mult;
        Energy = energy;
    }
}

public class StoryController : MonoBehaviour
{
    public DayController DayController;

    private Text storyText;
    private Dictionary<int, Day> storiesByDay = new Dictionary<int, Day>()
    {
        {1, new Day("Use the <b><color=white>ARROW KEYS</color></b> to move and <b><color=white>SPACE</color></b> to interact with objects.\n\nGet everything done before your energy runs out.", 1, 6) },
        {2, new Day("Get everything done before your energy runs out.", 1, 6) },
        {3, new Day("Get everything done before your energy runs out.", 1, 2) },
        {4, new Day("It's ok. Some days are harder than others.\n\n Just keep going.", 1, 8) },
        {5, new Day("Sometimes it's the small things that make a big difference.", 1, 10) },
        {6, new Day("Every little bit counts.", 1, 3) },
        {7, new Day("It's ok, you did your best. Some days are just harder than others.\n Maybe tomorrow will be better?", 1, 14) },
        {8, new Day("Things aren't always hard. Sometimes it's easy to see the good.", 1, 16) },
        {9, new Day("Keep looking until you find every last piece of joy.", 1, 18) },
        {10, new Day("After a while the small things add up and you find yourself in a different place from where you started.\n There might still be hard days but it gets easier to see past them.", 1, 18) },
    };

    // Start is called before the first frame update
    void Start()
    {
        storyText = transform.Find("StoryText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DayController.CurrentDay > 9)
        {
            transform.Find("ContinueText").gameObject.SetActive(false);
            transform.Find("DayLabel").gameObject.SetActive(false);
            transform.Find("PlayAgain").gameObject.SetActive(true);
        }
        Day day = storiesByDay[DayController.CurrentDay];
        storyText.text = day.Story;

        if (Input.GetKeyDown(KeyCode.Return) && DayController.CurrentDay <= 9)
        {
            DayController.StartDay(day.Multiplier, day.Energy, DayController.CurrentDay >= 4);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
