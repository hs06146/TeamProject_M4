  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class billboard : MonoBehaviour
{
    public Transform cm;
    // Start is called before the first frame update
    void Start()
    {
        cm = GameObject.FindGameObjectWithTag("aa").transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cm);
    }
}
