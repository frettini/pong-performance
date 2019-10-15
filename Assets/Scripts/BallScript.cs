using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallScript : MonoBehaviour
{

    [SerializeField]
    float speed = 6;

    [SerializeField]
    float dividingFac = 100;

    [SerializeField]
    float dirFac = 1.5f;

    float radius;
    Vector2 direction;

    PaletteScript ps;
    float paddleVel;

    public GameManager gm;
    
    

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.one.normalized;
        radius = transform.localScale.x / 2;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if(transform.position.y < GameManager.bottomLeft.y + radius && direction.y < 0)
        {
            direction.y = -direction.y;
        }

        if (transform.position.y > GameManager.topRight.y + radius && direction.y > 0)
        {
            direction.y = -direction.y;
        }

        if(transform.position.x < GameManager.bottomLeft.x + radius && direction.x < 0)
        {
            Debug.Log("Right Player Wins");
            gm.IncrementScore("PaletteRight");
            Destroy(this.gameObject);
        }

        if (transform.position.x > GameManager.topRight.x + radius && direction.x > 0)
        {
            Debug.Log("Left Player Wins");
            gm.IncrementScore("PaletteLeft");
            Destroy(this.gameObject);

        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Palette")
        {

            ps = other.GetComponent<PaletteScript>();

            paddleVel = ps.velocity;
            bool isRight = ps.isRight;

            direction.Normalize();

            if (isRight && direction.x > 0)
            {
                
                direction.x = -direction.x;
                direction.y += paddleVel / dividingFac;


            }
            if (!isRight && direction.x < 0)
            {
                
                direction.x = -direction.x;
                direction.y += paddleVel / dividingFac;
            }

            direction.x *= (1 + Math.Abs(direction.y)) / dirFac;
        }
    }
   
}
