using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class gun_sound : NetworkBehaviour
{



    public ParticleSystem my_particle;
    public GameObject particle_target;
    public GameObject gun_sound_object;

    public GameObject blood;
    public AudioSource hited_sound;
    AudioSource gun_sound_;
    AudioSource one_shoot_gun_sound;

    public GameObject my_HPBAR_object;
    public Image my_HPBAR_image;
    public Slider HP_BAR;

    private int Viewpoint = 3;

    //연사 시간간격

    public float fireRate = 0.1f;  //발사간격(0.1초 간격)
    private float nextFire = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        gun_sound_object = GameObject.FindGameObjectWithTag("gunSound");
        gun_sound_ = gun_sound_object.GetComponent<AudioSource>();
        one_shoot_gun_sound = particle_target.GetComponent<AudioSource>();

        hited_sound = GetComponent<AudioSource>();
        blood = GameObject.Find("blood");
        if (isLocalPlayer)
            blood.SetActive(false);

        if (isLocalPlayer)
        {
            my_HPBAR_object = GameObject.FindGameObjectWithTag("HP");
            //my_HPBAR_image = my_HPBAR_object.GetComponent<Image>();
        }

    }



    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }


        //단발 클릭한번
        if (Input.GetMouseButtonDown(0))
        {

            CmdPARTICLE_ON();

            one_shoot_gun_sound.Play();
            gun_sound_.mute = false;
           // gun_sound_.Play();
        }

        //연사 사격
        if (Input.GetMouseButton(0))
        {

           
            CmdPARTICLE_ON();
         //   if (Time.time >= nextFire)
           // {
              
             //   nextFire = Time.time + fireRate;
                if (!gun_sound_.isPlaying)
                {

                    gun_sound_.mute = false;
                    gun_sound_.Play();
                }


            //}
           


        }
     

        //마우스업은 취소
        if (Input.GetMouseButtonUp(0))
        {

            gun_sound_.mute = true;
            CmdPARTICLE_OFF();
        }

        //시점변경

        if (Input.GetMouseButtonDown(1))
        {
            if (Viewpoint == 3)


                Viewpoint = 1;

            else

                Viewpoint = 3;

        }

    }
   

    [Command]
    void CmdPARTICLE_ON()
    {
      
        /*
            //파티클이있을 때
            if (my_particle)
            {

                    my_particle.gameObject.SetActive(true);  
                    my_particle.Play();
                    NetworkServer.Spawn(my_particle.gameObject);

             }
            */
            // 파티클이 없다면 새로 만들어주구요
         //   else
           // {


            
            
                 
                    Transform particleObject = (Transform)Instantiate(Resources.Load("Particles/MuzzleFlash", typeof(Transform)), particle_target.transform.position, Quaternion.identity);

                    particleObject.transform.parent = this.transform;


                    my_particle = (ParticleSystem)particleObject.GetComponent(typeof(ParticleSystem));
                       
                        
                       
                        my_particle.Play();
                         NetworkServer.Spawn(my_particle.gameObject);
       // }
        
    }
    [Command]
    void CmdPARTICLE_OFF()
    {
         Destroy(my_particle.gameObject);

        //스탑,클리어면 안먹히고
        //셋엑티브 펠스로하니 상대입장에선 바로안사라짐
        
        //  my_particle.gameObject.SetActive(false);

        //      my_particle.Stop();
        //     my_particle.Clear();

    }

    private void OnTriggerEnter(Collider other)

    {


        if(!isLocalPlayer)
        {
            return;
        }

        if (other.gameObject.CompareTag("bullet") || other.gameObject.CompareTag("monster_weapon1"))
        {


            blood.SetActive(true);
            StartCoroutine(blood_off());
            hited_sound.Play();


          //  my_HPBAR_image.fillAmount =HP_BAR.value;

        }

    }

   

   


 
    IEnumerator blood_off()
    {
        yield return new WaitForSeconds(0.5f);
        blood.SetActive(false);
    }

  

  

}
