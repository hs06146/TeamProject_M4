using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int remain_bots;
    public GameObject BOTS;
    private Text winner_text;
    private Text remain_oj;
    private GameObject temp_bot;
    private Text endM;



    // Start is called before the first frame update
    void Start()
    {
  
            winner_text = GameObject.Find("WINNER_TEXT").GetComponent<Text>();
        temp_bot = GameObject.Find("TEMP_BOT");
       // temp_bot = GameObject.FindGameObjectWithTag("BOT");
        remain_oj = GameObject.Find("REMAIN_OBJECT").GetComponent<Text>();

        endM = GameObject.Find("endM").GetComponent<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (remain_bots > 1)
        {
            Destroy(temp_bot);
        } 
        remain_bots = GameObject.FindGameObjectsWithTag("BOT").Length-1;
      
        remain_oj.text = "남은 적 " + remain_bots ;


        if (remain_bots == 0)
        {
            winner_text.text = "경기가 종료 되었습니다";
            // Mouse Lock
            Cursor.lockState = CursorLockMode.None;
            // Cursor visible
            Cursor.visible = true;
        }

    }

  //  IEnumerator aa()
  //  {
   //     yield return new WaitForSeconds(3.0f);
   //     remain_bots = GameObject.FindGameObjectsWithTag("BOT").Length;
  //  }
}
