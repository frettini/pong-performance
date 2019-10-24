using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public BallScript ball;
    public PaletteScript palette;
    public TextMeshProUGUI scoreLeftText, scoreRightText;

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

        scoreLeftText.text = scoreLeft.ToString();
        scoreRightText.text = scoreRight.ToString();

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
        
        if(ballIsInst == true)
        {
            if(player == "PaletteRight")
            {
                //increment score of right player
                scoreRight++;
                scoreRightText.text = scoreRight.ToString();

            }
            else if(player == "PaletteLeft")
            {
                //increment score of left player
                scoreLeft++;
                scoreLeftText.text = scoreLeft.ToString();

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
