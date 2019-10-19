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

    private OSCTransmitter _transmitter;

    private float oldpos, newpos;


    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;

        // Creating a transmitter

        oldpos = transform.position.y;
        

    }

    public void Init(bool isRightPaddle)
    {
        Vector2 pos = Vector2.zero;

        if (isRightPaddle)
        {
            pos = new Vector2(GameManager.topRight.x, 0);
            pos -= Vector2.right * transform.localScale.x;

            isRight = isRightPaddle;
            input = "PaletteRight";

            _transmitter = GameObject.Find("OSCTxRight").GetComponent<OSCTransmitter>();
            


        }
        else
        {
            pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;

            isRight = isRightPaddle;
            input = "PaletteLeft";

            _transmitter = GameObject.Find("OSCTxLeft").GetComponent<OSCTransmitter>();
        }

        transform.position = pos;
        transform.name = input;
    }


    private void calVelocity()
    {
        newpos = transform.position.y;
        velocity = (newpos - oldpos) / Time.deltaTime;
        oldpos = newpos;

    }


    // Update is called once per frame
    void Update()
    {
        float move = Input.GetAxis(input) * Time.deltaTime * speed;

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
        message.AddValue(OSCValue.Int((int)transform.position.y));
        _transmitter.Send(message);


        calVelocity();
    }
}
