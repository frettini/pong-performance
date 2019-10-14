using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BallScript ball;
    public PaletteScript palette;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    // Start is called before the first frame update
    void Start()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint( new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        Instantiate(ball);

        PaletteScript palette1 =  Instantiate(palette) as PaletteScript;
        PaletteScript palette2 = Instantiate(palette) as PaletteScript;
        palette1.Init(true);
        palette2.Init(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
