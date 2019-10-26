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
        Debug.Log("bullet time!");
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowDownFactor;
    }
    

    private void RxSlowMo(OSCMessage message)
    {
        float x = (float)message.Values[0].Value;

        if (x > 0.05f)
        {
            slowDownFactor = x;
        }
        else 
        {
            slowDownFactor = 0.05f;
        }

        doSlowDown();

    }
}
