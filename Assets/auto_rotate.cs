using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class auto_rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        this.transform.Rotate(Time.deltaTime*(new Vector3(0, 50, 0)));
    }
}
