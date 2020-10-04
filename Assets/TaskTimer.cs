using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskTimer : MonoBehaviour
{
    public TaskController TaskController;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(Player.transform.position) + new Vector3(92, 42, 0);
        GetComponent<Slider>().value = TaskController.GetTaskCompletion();
    }
}
