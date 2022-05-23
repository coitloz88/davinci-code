using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Manager : MonoBehaviour
{
    public Sprite[] sprites = null;
    private SpriteRenderer spriteRenderer;
    protected Game_Manager game_Manager;

    string color = "";

    void SetColor(int color_index)
    {
        spriteRenderer.sprite = sprites[color_index];
        color = color_index == 0 ? "black" : "white";
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        game_Manager = GameObject.FindObjectOfType<Game_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        
    }
}
