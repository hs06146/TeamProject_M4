using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_control_ : MonoBehaviour
{
    // Start is called before the first frame update
   
    private Animator anim;
    public Transform[] points;

    public int nextIdx = 1;

    public float speed = 3.0f;
    public float damping = 5.0f;

    private Transform tr;
    private Transform playerTr;

    private Vector3 movePos;

    private bool isAttack=false;
    private bool move_enable = true;
    float hp=100;

    public GameObject designation_point;

    private bool revenge = false;

    private Rigidbody rb;

    int count = 0;

    float limit_y;
    Vector3 limit;

    public Image HP_BAR;
    public GameObject HP;
    private AudioSource SOUND;
    // public Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        SOUND = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        points = designation_point.GetComponentsInChildren<Transform>();
        anim = GetComponent<Animator>();
       playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        limit_y = tr.position.y;
    }

    // Update is called once per frame
    void Update()
    {
    
      

        HP_BAR.fillAmount = hp / 100f;




        //transform.eulerAngles = new Vector3(0, transform.rotation.y, transform.rotation.z);
        if (tr.position.y > limit_y)
        {
            tr.position = new Vector3(tr.position.x, limit_y, tr.position.z);
        }


       
        float dist = Vector3.Distance(tr.position, playerTr.position);
        float monster_home_dist = Vector3.Distance(tr.position, points[0].position);
        if (dist <= 3.3f)
        {
             isAttack = true;
            movePos = playerTr.position;

        }
        else if (dist <= 13.0f || revenge)                        // movePos는 이놈이 갈 곳임 
        {                                             //  이즈어택이 트루면 공격하고이동은안함
            movePos = playerTr.position;              // 펠스면 movePos로감 
            isAttack = false;
            speed = 10f;
            anim.speed = 2f;

            anim.SetBool("Run", true);
            if (monster_home_dist > 60f)
            {
                HP.SetActive(false);
                revenge = false;
                movePos = points[nextIdx].position;
                
            }

        }                                               //근데 주인공과 거리가 4이하면 목적지가 플레이어고
        else                                             // 4초과면 넥스트포인트임
        {
           
            movePos = points[nextIdx].position;
            isAttack = false;
            speed = 3.0f;
            anim.SetBool("Run", false);

        }

        
        anim.SetBool("isAttack",isAttack);

        if ( !anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death")) 
        {


            Quaternion rot = Quaternion.LookRotation(movePos - tr.position);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);

            if(!isAttack)
            tr.Translate(Vector3.forward * Time.deltaTime * speed);
        }


       


    }
    private void OnTriggerEnter(Collider other)
    {

     
        if (other.gameObject.CompareTag("pointA"))
        {
            //포인트에 딱 들어왔어
            //그럼 넥스트인덱스를 증가시키는데 이게 포인트배열의 길이 보다크거나같으면 즉 끝까지갔으면
            //다시 1로(처음) 아니면 1이증가된 넥스트인덱스로(이미 선처리로 1증가시킴)

          
            nextIdx = (++nextIdx >= points.Length) ? 1 : nextIdx;
        }
    
      




        if (other.gameObject.CompareTag("bullet"))
        {
           
            if(!SOUND.isPlaying)
            {
                SOUND.Play();
            } 
           
            HP.SetActive(true);
            revenge = true;
            anim.SetTrigger("Hited");
            if (hp > 0)
            {
                hp=hp-6f;

                if (hp <= 1f)
                {
                    anim.SetTrigger("Death");
                  anim.SetBool("isDeath",true);
                  
                  StartCoroutine(Death());
                }
            }

        }
      

        
    }

 


    IEnumerator Death()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }

    
}
