using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using extOSC;

public class BallScript : MonoBehaviour
{
    #region Public Vars

    public GameManager gm;

    public string address = "/ball"; 
    #endregion

    #region Private Vars

    [SerializeField]
    private float speed = 6;

    [SerializeField]
    private float dividingFac = 100;

    [SerializeField]
    private float dirFac = 1.5f;

    private float radius;
    private Vector2 direction;

    private PaletteScript ps;
    private float paddleVel;


    private OSCTransmitter _transmitterRight, _transmitterLeft;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.one.normalized;
        radius = transform.localScale.x / 2;

        _transmitterRight = GameObject.Find("OSCTxRight").GetComponent<OSCTransmitter>();
        _transmitterLeft = GameObject.Find("OSCTxLeft").GetComponent<OSCTransmitter>();


    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if(transform.position.y < GameManager.bottomLeft.y + radius && direction.y < 0)
        {
            SendBang(_transmitterLeft, "/ball/hit/bottom");
            SendBang(_transmitterRight, "/ball/hit/bottom");
            direction.y = -direction.y;
        }

        if (transform.position.y > GameManager.topRight.y + radius && direction.y > 0)
        {
            SendBang(_transmitterLeft, "/ball/hit/top");
            SendBang(_transmitterRight, "/ball/hit/top");

            direction.y = -direction.y;
        }

        if(transform.position.x < GameManager.bottomLeft.x + radius && direction.x < 0)
        {

            SendBang(_transmitterLeft, "/point/lose");
            SendBang(_transmitterRight, "/point/win");

            gm.IncrementScore("PaletteRight");
            Destroy(this.gameObject);
        }

        if (transform.position.x > GameManager.topRight.x + radius && direction.x > 0)
        {
            SendBang(_transmitterLeft, "/point/win");
            SendBang(_transmitterRight, "/point/lose");

            gm.IncrementScore("PaletteLeft");
            Destroy(this.gameObject);

        }

        SendFloat(_transmitterLeft, "/ball/pos/x", transform.position.x);
        SendFloat(_transmitterRight, "/ball/pos/x", transform.position.x);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Palette")
        {
            //give more directoin when velocity of the paddle isnt 0
            ps = other.GetComponent<PaletteScript>();

            paddleVel = ps.velocity;
            bool isRight = ps.isRight;

            direction.Normalize();

            if (isRight && direction.x > 0)
            {
                //if right paddle invert  direction x and add paddle speed
                direction.x = -direction.x;
                direction.y += paddleVel / dividingFac;

                SendBang(_transmitterRight, "/paddle/hit/ball" );


            }
            if (!isRight && direction.x < 0)
            {
                //if left paddle invert  direction x and add paddle speed
                direction.x = -direction.x;
                direction.y += paddleVel / dividingFac;

                SendBang(_transmitterLeft, "/paddle/hit/ball");
            }

            direction.x *= (1 + Math.Abs(direction.y)) / dirFac;
        }
    }

    private void SendFloat(OSCTransmitter _transmitter, string address, float value)
    {

        //Send OSC message
        var message = new OSCMessage(string.Format("{0}",address));
        // Populate values.
        message.AddValue(OSCValue.Float((float)value));
        _transmitter.Send(message);

    }

    private void SendBang(OSCTransmitter _transmitter, string address)
    {

        //Send OSC message
        var message = new OSCMessage(string.Format("{0}", address));
        // Populate values.
        message.AddValue(OSCValue.Impulse());
        _transmitter.Send(message);

    }

}
