using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class MOB2_enemy_control : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator anim;
    public Transform[] points;

    public int nextIdx;

    public float speed = 3.0f;
    public float damping = 5.0f;

    private Transform tr;
    private Transform playerTr;

    private Vector3 movePos;

    private bool isAttack = false;
    private bool move_enable = true;
    float hp = 100;

    public GameObject designation_point;

    private bool revenge = false;

    private Rigidbody rb;

    int count = 0;

   // float limit_y;
   // Vector3 limit;

  //  public Image HP_BAR;
  //  public GameObject HP;
  //  private AudioSource SOUND;


    public GameObject Bullet;
    public Transform FirePos;
    private bool start_attack = false;

    private bool start_attackaa = false;


    public ParticleSystem my_particle;
    public GameObject particle_target;
    public GameObject gun_sound_object;

    public GameObject blood;
    public AudioSource hited_sound;
    AudioSource gun_sound_;
    AudioSource one_shoot_gun_sound;

    private bool is_water=false;

    
    //AI
    NavMeshAgent nav;
    GameObject target;
    // public Collider[] colliders;
    // Start is called before the first frame update

    int anim_count = 0;
    void Start()
    {

       // gameObject.transform.position = new Vector3(Random.Range(-500, 500), 0, Random.Range(-500, 500));
       
        //   SOUND = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        points = designation_point.GetComponentsInChildren<Transform>();
        nextIdx = Random.Range(1, points.Length-1);
        anim = GetComponent<Animator>();
        playerTr = GameObject.Find("TEMP_PLAYER").GetComponent<Transform>();

        StartCoroutine(MOGPO());
        //   limit_y = tr.position.y;

        StartCoroutine(aa());

        gun_sound_object = GameObject.FindGameObjectWithTag("gunSound");
        gun_sound_ = gun_sound_object.GetComponent<AudioSource>();
       one_shoot_gun_sound = particle_target.GetComponent<AudioSource>();

        nav = GetComponent<NavMeshAgent>();

        






    }

    // Update is called once per frame
    void Update()
    {
        //공격중이아닐땐 목적지로이동 (waypoint OR player)
        //공격중일 때는 멈춤 (만약 이렇게 안나누고 공격중일때movepos를 자기위치로두면 문제가 되는게 공격중일 때 플레이어를 안쳐다봄)
        //왜냐면 얜 항상 무브포스 즉 목적지를 쳐다보니까 무브포스가 자기가 되면 안되거든
        if (!isAttack)
        {
            nav.enabled = true;
            nav.SetDestination(movePos);
        }
        else
        {
            // nav.SetDestination(tr.position);
             nav.enabled = false;
        }

      
        //사격
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") && start_attack && GameObject.FindWithTag("Player") != null)
        {

            //복제한다. //'Bullet'을 'FirePos.transform.position' 위치에 'FirePos.transform.rotation' 회전값으로.

            GameObject bullet = Instantiate(Bullet, FirePos.transform.position, FirePos.transform.rotation);
            StartCoroutine(aa());
            start_attack = false;
            Destroy(bullet, 2f);

           
            CmdPARTICLE_ON();
            one_shoot_gun_sound.Play();
            /*if (!gun_sound_.isPlaying)
            {

                gun_sound_.mute = false;
                gun_sound_.Play();

            }*/
        }
        // HP_BAR.fillAmount = hp / 100f;

        else
        {
         

            CmdPARTICLE_OFF();
        }


        anim.SetBool("SHOOT", isAttack);

        //transform.eulerAngles = new Vector3(0, transform.rotation.y, transform.rotation.z);
        // if (tr.position.y > limit_y)
        //  {
        //   tr.position = new Vector3(tr.position.x, limit_y, tr.position.z);
        //  }



        float dist = Vector3.Distance(tr.position, playerTr.position);
        float monster_home_dist = Vector3.Distance(tr.position, points[0].position);
        
        if ( (dist <= 20.0f || revenge) && !is_water)
        {
           // movePos = tr.position;
            movePos = playerTr.position;
            isAttack = true;
          //  nav.SetDestination(tr.position);

            if (dist >= 60f)
                revenge = false;

        }
        else if (dist <= 35.0f )                        // movePos는 이놈이 갈 곳임 
        {                                             //  이즈어택이 트루면 공격하고이동은안함
            movePos = playerTr.position;              // 펠스면 movePos로감 
            isAttack = false;
            //  speed = 4.5f;
            //anim.speed = 2f;
          
            // anim.SetBool("realrun", true);

            /* if (monster_home_dist > 60f)
             {
                // HP.SetActive(false);
                 revenge = false;
                 movePos = points[nextIdx].position;

             }
             */
        }                                               //근데 주인공과 거리가 4이하면 목적지가 플레이어고
        else                                             // 4초과면 넥스트포인트임
        {

            movePos = points[nextIdx].position;
            isAttack = false;
            speed = 3.0f;
          

            //  anim.SetBool("Run", false);

        }
  
       
    

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Damage") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {


            Quaternion rot = Quaternion.LookRotation(movePos - tr.position);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);

        //    if (!isAttack)
            
          //      nav.SetDestination(movePos);

                // tr.Translate(Vector3.forward * Time.deltaTime * speed );

        

        }


/*
        if (nav.destination != playerTr.position)
        {
            nav.SetDestination(playerTr.position);
        }
        else
        {
            nav.SetDestination(transform.position);
        }
*/

    }
 
    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("pointA"))
        {
            //포인트에 딱 들어왔어
            //그럼 넥스트인덱스를 증가시키는데 이게 포인트배열의 길이 보다크거나같으면 즉 끝까지갔으면
            //다시 1로(처음) 아니면 1이증가된 넥스트인덱스로(이미 선처리로 1증가시킴)

          
            nextIdx = (++nextIdx >= points.Length) ? 1 : Random.Range(1, points.Length-1);
         
        }






        if (other.gameObject.CompareTag("bullet"))
        {

           // if (!SOUND.isPlaying)
            {
          //      SOUND.Play();
            }

          //  HP.SetActive(true);
            revenge = true;
          //   anim.SetTrigger("HITED");
            
            if (anim_count == 0 || anim_count>6 )
            {
                anim.SetTrigger("HITED");
                anim_count = 1;
              
            }
            else
            {
                anim_count += 1;
               
            }

            /*  if (hp > 0)
              {
                  hp = hp - 4f;

                  if (hp <= 1f)
                  {
                      anim.SetTrigger("Death");
                      anim.SetBool("isDeath", true);

                      StartCoroutine(Death());
                  }
              }*/

        }



    }


    //물에서는 공격불가

    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.CompareTag("water"))
        {
            is_water = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {


        if (other.gameObject.CompareTag("water"))
        {
            is_water = false;
        }

    }
    void CmdPARTICLE_ON()
    {


        //파티클이있을 때
        if (my_particle)
        {

            my_particle.gameObject.SetActive(true);
            my_particle.Play();
          

        }
        // 파티클이 없다면 새로 만들어주구요
        else
        {





            Transform particleObject = (Transform)Instantiate(Resources.Load("Particles/MuzzleFlash", typeof(Transform)), particle_target.transform.position, Quaternion.identity);

            particleObject.transform.parent = this.transform;


            my_particle = (ParticleSystem)particleObject.GetComponent(typeof(ParticleSystem));



            my_particle.Play();
          
        }

    }
    
    void CmdPARTICLE_OFF()
    {
        if(my_particle)
        Destroy(my_particle.gameObject);

        //스탑,클리어면 안먹히고
        //셋엑티브 펠스로하니 상대입장에선 바로안사라짐

        //  my_particle.gameObject.SetActive(false);

        //      my_particle.Stop();
        //     my_particle.Clear();

    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }

    IEnumerator aa()
    {
        yield return new WaitForSeconds(0.3f);
        start_attack = true;

    }

    IEnumerator MOGPO()
    {
        yield return new WaitForSeconds(0.1f);
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

    }

 
}
