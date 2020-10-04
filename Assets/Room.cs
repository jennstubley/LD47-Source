using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject Hint;

    private string[,] FloorPlan;
    // Start is called before the first frame update
    void Awake()
    {
        FloorPlan = new string[6, 6];
        FloorPlan[0, 0] = "Fridge";
        FloorPlan[1, 0] = "Sink";
        FloorPlan[3, 0] = "Easel";
        FloorPlan[5, 0] = "Bed";
        FloorPlan[5, 1] = "Bed_End";
        FloorPlan[5, 3] = "Dresser";
        FloorPlan[5, 4] = "Phone";
        FloorPlan[0, 2] = "Plant";
        FloorPlan[0, 3] = "Computer";
        FloorPlan[0, 5] = "Cat";
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Returns true if the position is free, otherwise false.
    public bool CanMoveTo(Vector3 position)
    {
        Vector2Int coords = CoordsFromPosition(position);
        return AreCoordsValid(coords) && FloorPlan[coords.x, coords.y] == null;
    }

    private Vector2Int CoordsFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x + 4);
        int y = 6 - Mathf.RoundToInt(position.y + 3);
        return new Vector2Int(x, y);
    }

    private Vector3 HintPositionFromCoords(Vector2Int coords)
    {
        float x = coords.x - 2.5f;
        float y = -coords.y + 3.8f;
        return new Vector3(x, y);
    }

    private bool AreCoordsValid(Vector2Int coords)
    {
        return coords.x >= 0 && coords.y >= 0 && coords.x < 6 && coords.y < 6;
    }

    internal string GetItemAtPosition(Vector3 position, Vector2Int facingDir)
    {
        Vector2Int positionCoords = CoordsFromPosition(position);
        Vector2Int coords = positionCoords + facingDir;
        if (!AreCoordsValid(coords) || FloorPlan[coords.x, coords.y] == null) return null;

        return FloorPlan[coords.x, coords.y];
    }

    public void ShowHint(string obj)
    {
        Vector2Int coords = new Vector2Int(-1, -1);
        for (int i = 0; i < 6; ++i)
        {
            for (int j = 0; j < 6; j++)
            {
                if (FloorPlan[i, j] != null && FloorPlan[i, j].Equals(obj))
                {
                    coords = new Vector2Int(i, j);
                    break;
                }
            }
        }
        if (coords != new Vector2Int(-1, -1))
        {
            Hint.transform.position = HintPositionFromCoords(coords);
            Hint.SetActive(true);
        }
    }

    public void HideHint()
    {
        Hint.SetActive(false);
    }
}
