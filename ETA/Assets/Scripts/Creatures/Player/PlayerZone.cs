using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveFront();
    }

    void moveFront()
    {
        transform.position += transform.forward * Time.deltaTime * 3;
    }
}
