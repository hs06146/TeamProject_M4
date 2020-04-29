using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bullet : MonoBehaviour
{

    public ParticleSystem my_particle;
    public AudioSource hit_sound;
    public Text kill_text;
    public Text n_kill_text;
    private int kill_score=0;

    private Text winner_text;

    public Transform kill_cal;

    [SerializeField] private Color gameEndColor;
    [SerializeField] private float gameEndDensity;  
    private void Start()
    {
        hit_sound = GameObject.Find("hit_sound").GetComponent<AudioSource>();
        kill_text = GameObject.Find("killtext").GetComponent<Text>();
        n_kill_text = GameObject.Find("n_kill").GetComponent<Text>();
        kill_cal = GameObject.Find("kill_cal").GetComponent<Transform>();
        winner_text = GameObject.Find("WINNER_TEXT").GetComponent<Text>();
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * 40f * Time.deltaTime);
    }

  /*  private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("bullet"))
        {
            //    Transform bullet_hole = (Transform)Instantiate(Resources.Load("bulletHole", typeof(Transform)), other.transform.position, transform.rotation) ;
            //    Destroy(gameObject);

            PARTICLE2_ON();

        }
    }*/
    private void OnTriggerEnter(Collider other)
    {


       if (!other.gameObject.CompareTag("bullet"))
       {
            //    Transform bullet_hole = (Transform)Instantiate(Resources.Load("bulletHole", typeof(Transform)), other.transform.position, transform.rotation) ;
            //    Destroy(gameObject);

         //   PARTICLE2_ON();

        }


        if (other.gameObject.CompareTag("Player"))
        {
            
            PARTICLE_ON();
            hit_sound.Play();

            
        }


        if (other.gameObject.CompareTag("BOT"))
        {

            PARTICLE_ON();
            hit_sound.Play();
        }


        if (other.gameObject.CompareTag("water"))
        {

            Destroy(gameObject);
        }



        //충돌한상대방에게데미지를주기

        //충돌한상대오브젝트저장
        var hit = other.gameObject;
        var health = hit.GetComponent<health>();

        //이것도서버에서만발동됨 왜냐면
        //테이크데미지함수에서 !isserver에걸리거든
        if (health != null)
        {

            //나 죽이는건 킬수올리면안되지
           // if (hit.CompareTag("Player"))
           //     return;
            health.TakeDamage(7);


                if (health.currentHealth == 0)
            {
               // StartCoroutine(killtext_reset());

            }
            if (health.currentHealth  <= 0)
            {

                StartCoroutine(all_bullet_destroy());
                kill_cal.position = new Vector3(kill_cal.position.x + 1, 0, 0);
                kill_text.text = "적을 처치하였습니다";
                n_kill_text.text = kill_cal.position.x + "킬";
               
              
           


            }
        }
      //  if(other.gameObject.CompareTag("Player"))
     // Destroy(gameObject);
    }


    void PARTICLE_ON( )
    {
       if (my_particle)
        my_particle.Play();
            // 파티클이 없다면 새로 만들어주구요
            else
            {
            Transform particleObject = (Transform)Instantiate(Resources.Load("Particles/CFX2_Blood", typeof(Transform)), transform.position, transform.rotation);
               // particleObject.transform.parent = this.transform;

                my_particle = (ParticleSystem)particleObject.GetComponent(typeof(ParticleSystem));

                my_particle.Play();


            }
        }

    void PARTICLE2_ON()
    {
        if (my_particle)
            my_particle.Play();
        // 파티클이 없다면 새로 만들어주구요
        else
        {
            Transform particleObject = (Transform)Instantiate(Resources.Load("Particles/Explosion_A", typeof(Transform)), transform.position, transform.rotation);
            // particleObject.transform.parent = this.transform;

            my_particle = (ParticleSystem)particleObject.GetComponent(typeof(ParticleSystem));

            my_particle.Play();


        }
    }

    IEnumerator all_bullet_destroy()
    {
        yield return new WaitForSeconds(0.5f);
        //적 죽으면 월드에 있는총알 다 제거
        var b = GameObject.FindGameObjectsWithTag("bullet");
        for (int i = 0; i < b.Length; i++)
        {
            Destroy(b[i]);
        }
    }





}
