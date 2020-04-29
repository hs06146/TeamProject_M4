using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class monster_attack_ : MonoBehaviour
{
    GameObject death_camera;

   // public Text hardmode;

    private bool player_death = false;
    int count = 0;

   // public Text player_HP_TEXT;
    private Animator anim;

    public GameObject testmob;

    // Start is called before the first frame update
    void Start()
    {
        
        //hardmode.text = "";

        anim = testmob.GetComponent<Animator>();
        death_camera = GameObject.Find("deathCamera");

      
    }

    // Update is called once per frame
    void Update()
    {




         if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            //   rb.isKinematic = true;
           
            gameObject.GetComponent<BoxCollider>().isTrigger = true;

        }
        else
        {
           
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }

    /*    if (player_HP_TEXT.text.Equals("HP 0/100"))
        {
           
            if (count < 50)
            {

                death_camera.transform.Translate(Vector3.back  * 3f * Time.deltaTime);
                count++;
            }
            if (count == 50)
                hardmode.text = "사망하셨습니다";
        }*/


    
    }
  private void OnTriggerEnter(Collider other)
    {




       if (other.gameObject.CompareTag("Player"))
        {



            //충돌한상대오브젝트저장
            var hit = other.gameObject;
            var health = hit.GetComponent<health>();

            //이것도서버에서만발동됨 왜냐면
            //테이크데미지함수에서 !isserver에걸리거든
            if (health != null)
            {
                health.TakeDamage(3);
               
                
            }

        }

    

    }
    IEnumerator aa()
    {
        yield return new WaitForSeconds(5.5f);
        gameObject.AddComponent<BoxCollider>();

    }


}
