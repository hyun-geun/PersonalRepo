using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHiddenground : MonoBehaviour
{
    private float maxYPos;
    private float minYPos;
    private float directionY;
    void Awake()
    {
        maxYPos = 0.3f;
        minYPos = -1.3f;
        this.transform.position = new Vector2(this.transform.position.x, maxYPos);
    }

    
    void Update()
    {
        if (this.transform.position.y >= maxYPos)
        {
            directionY = -0.2f;
        }
        else if (this.transform.position.y <= minYPos)
        {
            directionY = 0.2f;
        }
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y+(directionY*Time.deltaTime));
    }
}
