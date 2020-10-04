using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayController : MonoBehaviour
{
    public int CurrentDay { get; private set; }
    public float Energy { get; private set; }
    public bool Paused;
    public GameObject NewDayScreen;
    public TaskController TaskController;

    public Sprite[] EnergyFaces;

    private Image energyFaces;
    private GameObject zzz;

    // Start is called before the first frame update
    void Start()
    {
        CurrentDay = 1;
        Paused = true;
        energyFaces = transform.Find("Face").GetComponent<Image>();
        zzz = transform.Find("Zzz").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Energy == 0)
        {
            energyFaces.sprite = EnergyFaces[0];
            zzz.SetActive(true);
        }
        else
        {
            energyFaces.sprite = EnergyFaces[Energy >= 10 ? 2 : 1];
            zzz.SetActive(false);
        }
    }

    public void DoTask()
    {
        Energy = Mathf.Max(0, Energy -1);
    }

    public void StartDay(float multiplier, float energy, bool canDoExtraTasks)
    {
        TaskController.ResetDay(multiplier, canDoExtraTasks);
        NewDayScreen.SetActive(false);
        Energy = energy;
        Paused = false;
    }

    public void EndDay()
    {
        NewDayScreen.SetActive(true);
        CurrentDay++;
        Paused = true;
    }
}
