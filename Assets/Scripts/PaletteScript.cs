using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;


public class PaletteScript : MonoBehaviour
{

    [SerializeField]
    float speed = 5f;
    float height;
    
    string input;
    public bool isRight;
    public float velocity;
    public string rxPadPosRight = "/paddle/right/pos/y";
    public string rxPadPosLeft = "/paddle/left/pos/y";

    private OSCTransmitter _transmitter;
    private OSCReceiver _receiver;

    private Transform paddleLeftPos;
    private float paddleLeftY;

    private float oldpos, newpos, predictedpos;

    private float axis;
    public float buffer = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;
        oldpos = transform.position.y;

        

    }

    public void Init(bool isRightPaddle)
    {
        Vector2 pos = Vector2.zero;

        if (isRightPaddle)
        {
            //place and scale paddle
            pos = new Vector2(GameManager.topRight.x, 0);
            pos -= Vector2.right * transform.localScale.x;
            //init bool
            isRight = isRightPaddle;
            input = "PaletteRight";
            //init transmitters
            _transmitter = GameObject.Find("OSCTxRight").GetComponent<OSCTransmitter>();
            _receiver = GameObject.Find("OSCRx").GetComponent<OSCReceiver>();
            _receiver.Bind(rxPadPosRight, PadPosOSC);

        }
        else
        {
            pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;

            isRight = isRightPaddle;
            input = "PaletteLeft";

            _transmitter = GameObject.Find("OSCTxLeft").GetComponent<OSCTransmitter>();
            _receiver = GameObject.Find("OSCRx").GetComponent<OSCReceiver>();
            _receiver.Bind(rxPadPosLeft, PadPosOSC);
        }

        //assign position and name
        transform.position = pos;
        transform.name = input;
        predictedpos = pos.y;
    }


    private void calVelocity()
    {
        //calculate velocity of paddle (because kinematic)
        newpos = transform.position.y;
        velocity = (newpos - oldpos) / Time.deltaTime;
        oldpos = newpos;

    }


    // Update is called once per frame
    void Update()
    {
        float difference = predictedpos - transform.position.y; 
        //get axis from either Input system or OSC
        if( difference > buffer)
        {
            axis = 1;
        }else if(difference < -buffer)
        {
            axis = -1;
        }
        else if(difference < buffer && difference > -buffer)
        {
            axis = 0;
        }


        float move = axis * Time.deltaTime * speed;
        
        if(transform.position.y < GameManager.bottomLeft.y + height / 2 && move < 0)
        {
            move = 0;
        }

        if (transform.position.y > GameManager.topRight.y - height / 2 && move > 0)
        {
            move = 0;
        }

        transform.Translate(move * Vector2.up);


        //Send OSC message
        var message = new OSCMessage(string.Format("/paddle/pos/y", input));
        // Populate values.
        message.AddValue(OSCValue.Float(transform.position.y));

        if (isRight)
        {
            paddleLeftPos = GameObject.Find("PaletteLeft").GetComponent<Transform>();
            if (paddleLeftPos != null)
            {
                message.AddValue(OSCValue.Float(paddleLeftPos.position.y));
            }
        }
        _transmitter.Send(message);


        calVelocity();
    }

    // c#
    float map(float s, float from1, float from2, float to1, float to2)
    {
        return to1 + (s - from1) * (to2 - to1) / (from2 - from1);
    }

    private void PadPosOSC(OSCMessage message)
    {
        float x = (float)message.Values[0].Value;
        x = map(x, 0f,1f, GameManager.bottomLeft.y + height / 2, GameManager.topRight.y - height / 2);
        predictedpos = x;
    }
}
