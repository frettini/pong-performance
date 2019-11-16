using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using extOSC;

public class GameManager : MonoBehaviour
{

    public BallScript ball;
    public PaletteScript palette;
    public GameObject circleBg;
    public float partitionX = 10f;

    public TextMeshProUGUI scoreLeftText, scoreRightText;

    public static Vector2 bottomLeft;
    public static Vector2 topRight;

    public string addrColorRight = "/color/right";
    public string addrColorLeft = "/color/left";
    public string addrScaleRight = "/scale/right";
    public string addrScaleLeft = "/scale/left";
    public float multiplier = 1f;

    private int scoreRight, scoreLeft;
    private bool ballIsInst = true;
    private float partitionsize;
    private float partitionY;

    private GameObject[] circleArr;
    private CircleScript cs;
    private SpriteRenderer sr;

    private BallScript ballS;

    private Color colorRight = new Color(1f,1f,1f,0.5f);
    private Color colorLeft = new Color(1f, 1f, 1f, 0.5f);
    private Color oldColR, oldColL;
    private float amp = 0;


    private int count = 0;

    private OSCReceiver _receiver;

    private void Awake()
    {
        //get coordinate to create level relative to screenwidth
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;


        partitionsize = (float)topRight.x / partitionX;
        partitionY = (float)topRight.y / partitionsize;

        int x = (int)(partitionX * (partitionY + 1));


        circleArr = new GameObject[x];

        float offset = partitionsize;

        oldColL = colorLeft;
        oldColR = colorRight;

        

        //place circles on scene
        int t = 0;
        for (int i = 0; i < (int)(partitionY + 1); i++)
        {
            for (int j = 0; j < (int)partitionX; j++)
            {
                //instantiate with position decided by j and i on a grid relative to screen
                circleArr[t] = Instantiate(circleBg, new Vector3(bottomLeft.x + j * (float)width / partitionX + offset, bottomLeft.y + i * (float)height / partitionY + offset, 0), Quaternion.identity);
                cs = circleArr[t].GetComponent<CircleScript>();
                sr = circleArr[t].GetComponent<SpriteRenderer>();

                if (Random.Range(0f, 2f) > 1)
                {
                    cs.isRight = true;
                }
                else
                {
                    cs.isRight = false;
                }

                sr.color = new Color(1f, 1f, 1f, 0.5f);
                t++;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreRight = 0;
        scoreLeft = 0;

        scoreLeftText.text = scoreLeft.ToString();
        scoreRightText.text = scoreRight.ToString();

        

        //instantiate first ball
        ballS = Instantiate(ball);
        ballS.gm = this;

        //instantiate both palette and assign sides
        PaletteScript palette1 =  Instantiate(palette) as PaletteScript;
        PaletteScript palette2 = Instantiate(palette) as PaletteScript;
        palette1.Init(true);
        palette2.Init(false);

        

       

        _receiver = GameObject.Find("OSCRx").GetComponent<OSCReceiver>();
        _receiver.Bind(addrColorLeft, ColorChange);
        _receiver.Bind(addrColorRight, ColorChange);
        _receiver.Bind(addrScaleRight, ScaleChange);
        _receiver.Bind(addrScaleLeft, ScaleChange);



    }


    private void Update()
    {
        if(colorRight != oldColR)
        {
            ColorManager(true);
            oldColR = colorRight;
        }
        if (colorLeft != oldColL)
        {
            ColorManager(false);
            oldColL = colorLeft;
        }
    }

    private void ColorManager(bool isRight)
    {
        foreach (GameObject circle in circleArr)
        {

            if(circle != null)
            {


            sr = circle.GetComponent<SpriteRenderer>();
            cs = circle.GetComponent<CircleScript>();

            

            if (cs.isRight && isRight)
            {
                sr.color = colorRight;
            }
            else if(!cs.isRight && !isRight)
            {

                sr.color = colorLeft;
            }
            }
            
        }

        if (isRight)
        {
            sr = GameObject.Find("PaletteRight").GetComponent<SpriteRenderer>();
            sr.color = new Color(colorRight.r, colorRight.g, colorRight.b);
            
        }
        else
        {
            sr = GameObject.Find("PaletteLeft").GetComponent<SpriteRenderer>();
            sr.color = new Color(colorLeft.r, colorLeft.g, colorLeft.b);
        }

        ballS.GetComponent<SpriteRenderer>().color = new Color((colorRight.r + colorLeft.r) / 2f, (colorRight.g + colorLeft.g) / 2f, (colorRight.b + colorLeft.b) / 2f);
        

    }

    private void ScaleManager(bool isRight)
    {
        foreach(GameObject circle in circleArr)
        {
            if(circle != null)
            {

                cs = circle.GetComponent<CircleScript>();
                if (cs.isRight && isRight)
                {
                    circle.transform.localScale = new Vector3(1f+amp*multiplier, 1f+amp*multiplier, 1f);
                }
                else if(!cs.isRight && !isRight)
                {
                    circle.transform.localScale = new Vector3(1f + amp * multiplier, 1f + amp * multiplier, 1f);
                }
            }
        }
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
            ballS.GetComponent<SpriteRenderer>().color = new Color((colorRight.r + colorLeft.r) / 2f, (colorRight.g + colorLeft.g) / 2f, (colorRight.b + colorLeft.b) / 2f);
            ballS.gm = this;
            ballIsInst = true;

        }
        
        foreach (GameObject circle in circleArr)
        {
            cs = circle.GetComponent<CircleScript>();

            if (Random.Range(0f, 2f) > 1)
            {
                cs.isRight = true;
            }
            else
            {
                cs.isRight = false;
            }
        }


    }

    private void ColorChange(OSCMessage message)
    {
        float r = (float)message.Values[0].Value;
        float g = (float)message.Values[1].Value;
        float b = (float)message.Values[2].Value;

        if (message.Address.Contains("right"))
        {
            colorRight = new Color(r, g, b);
        }
        else
        {
            colorLeft = new Color(r, g, b);
        }


    }

    private void ScaleChange(OSCMessage message)
    {
        amp = (float)message.Values[0].Value;
        if (message.Address.Contains("right"))
        {
            ScaleManager(true);
        }
        else
        {
            ScaleManager(false);
        }

    }

}
