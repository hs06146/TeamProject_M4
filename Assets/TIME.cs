using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TIME : MonoBehaviour
{
    Text time_;
    public float tt;

    // Start is called before the first frame update
    void Start()
    {
        time_= GetComponent<Text>();

 
    }

    // Update is called once per frame
    void Update()
    {
        tt += Time.deltaTime;
        time_.text = "시간: "+(int)tt;
    }
}
