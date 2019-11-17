using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class SlowMotion : MonoBehaviour
{
    public float slowDownLength = 2f;

    public string slowMoAddress = "/scene/slowmo";

    private OSCReceiver _receiver;
    private float slowDownFactor;


    private void Start()
    {
        _receiver = GameObject.Find("OSCRx").GetComponent<OSCReceiver>();
        _receiver.Bind(slowMoAddress, RxSlowMo);
        slowDownFactor = 1f;
    }

   

    public void doSlowDown()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime * (slowDownFactor+ 0.05f);
    }
    

    private void RxSlowMo(OSCMessage message)
    {
        float x = (float)message.Values[0].Value;

        if (x > 0.4f)
        {
            
            slowDownFactor = x;
        }
        else
        {
            Debug.Log(x);

            slowDownFactor = 0.4f;
        }

        doSlowDown();

    }
}
