using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BallScript ball;
    public PaletteScript palette;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    private int scoreRight, scoreLeft;
    private bool ballIsInst = true;

    private BallScript ballS;

    // Start is called before the first frame update
    void Start()
    {
        scoreRight = 0;
        scoreLeft = 0;

        bottomLeft = Camera.main.ScreenToWorldPoint( new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        ballS = Instantiate(ball);
        ballS.gm = this;

        PaletteScript palette1 =  Instantiate(palette) as PaletteScript;
        PaletteScript palette2 = Instantiate(palette) as PaletteScript;
        palette1.Init(true);
        palette2.Init(false);

    }


    public void IncrementScore(string player)
    {
        Debug.Log(player);
        if(ballIsInst == true)
        {
            if(player == "PaletteRight")
            {
                //increment score of right player
                scoreRight++;
                Debug.Log("Right player : " + scoreRight);
                
            }
            else if(player == "PaletteLeft")
            {
                //increment score of left player
                scoreLeft++;
                Debug.Log("Left player : " + scoreLeft);
                
            }
            else
            {
                //print error
                Debug.Log("name not recognized");
            }

            ballS = Instantiate(ball);
            ballS.gm = this;
            ballIsInst = true;

        }
    }

}
