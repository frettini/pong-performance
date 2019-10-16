﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;
using extOSC.Core.Reflection;
using extOSC.Components.Informers;

public class PaletteScript : MonoBehaviour
{

    [SerializeField]
    float speed = 5f;
    float height;
    
    string input;
    public bool isRight;
    public float velocity;

    OSCTransmitter transmitter;

    float oldpos, newpos;


    // Start is called before the first frame update
    void Start()
    {
        height = transform.localScale.y;

        // Creating a transmitter.
        transmitter = gameObject.AddComponent<OSCTransmitter>();
        // Set remote host address.
        transmitter.RemoteHost = "127.0.0.1";
        // Set remote port;
        transmitter.RemotePort = 7000;

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
            


        }
        else
        {
            pos = new Vector2(GameManager.bottomLeft.x, 0);
            pos += Vector2.right * transform.localScale.x;

            isRight = isRightPaddle;
            input = "PaletteLeft";
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


        var message = new OSCMessage(string.Format("/{0}", input));

        // Populate values.
        message.AddValue(OSCValue.Int((int)transform.position.y));
        

        // Send message.
        transmitter.Send(message);

        calVelocity();
    }
}