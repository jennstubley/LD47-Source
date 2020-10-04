using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Room Room;
    public TaskController TaskController;
    public GameObject TaskBubble;
    public GameObject HeartBubble;
    public bool Paused;
    public Sprite[] Sprites;
    public Sprite[] GreySprites;

    private Vector2Int facingDir;
    private SpriteRenderer playerSprite;
    private SpriteRenderer greyPlayerSprite;

    // Start is called before the first frame update
    void Start()
    {
        facingDir = new Vector2Int(0, -1);
        playerSprite = transform.Find("PlayerImage").GetComponent<SpriteRenderer>();
        greyPlayerSprite = transform.Find("PlayerImage_Grey").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Paused) return;

        int deltaX = 0, deltaY = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            deltaY = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            deltaY = -1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            deltaX = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            deltaX = 1;
        }

        if (deltaX != 0 || deltaY != 0)
        {
            ValidateAndSetPosition(new Vector3(transform.position.x + deltaX, transform.position.y + deltaY, transform.position.z));
            SetFacingDir(deltaX, deltaY);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            string item = Room.GetItemAtPosition(transform.position, facingDir);
            int result = TaskController.RegisterInteraction(item, () => Unpause());
            if (result > 0)
            {
                Paused = true;
                if (result == 1)
                    TaskBubble.SetActive(true);
                else
                    HeartBubble.SetActive(true);
            }
        }

    }

    private void ValidateAndSetPosition(Vector3 newPos)
    {
        if (Room.CanMoveTo(newPos))
        {
            transform.position = newPos;
        }
    }

    private void SetFacingDir(int x, int y)
    {
        if (x != 0)
        {
            facingDir = new Vector2Int(x, 0);
            playerSprite.sprite = x > 0 ? Sprites[0] : Sprites[1];
            greyPlayerSprite.sprite = x > 0 ? GreySprites[0] : GreySprites[1];
        }
        else if (y != 0)
        {
            facingDir = new Vector2Int(0, -y);
            playerSprite.sprite = y > 0 ? Sprites[2] : Sprites[3];
            greyPlayerSprite.sprite = y > 0 ? GreySprites[2] : GreySprites[3];
        }
    }

    private void Unpause()
    {
        Paused = false;
        TaskBubble.SetActive(false);
        HeartBubble.SetActive(false);
    }
}
