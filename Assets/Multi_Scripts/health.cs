using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class health : NetworkBehaviour
{

    public GameObject healpack;

    public Image my_hp_bar;
    //사망
    public GameObject death_camera;
    public Text hardmode;
    private int count = 0;
    private bool isDeath = false;
    Camera cm;
    public GameObject blood;

    public Text kill_text;
    public Text n_kill_text;


    public Transform kill_cal;

    public bool in_magnetic_field = true;

    //콘스트는 변경불가란뜻
    public const double maxHealth = 100;

   


    //이렇게하면 서렌트헬스값 바뀔때마다 자동으로
    //훅이걸려서 온체인지헬스함수가호출됨
    [SyncVar(hook = "onChangeHealth")]
    public double currentHealth = maxHealth;
    public Slider healthSlider;

    private NetworkStartPosition spawnposition;


  
    private void Start()
    {
        StartCoroutine(start_magnetic_field());
        if (isLocalPlayer)
        {  
            //모든스타트포지션찾음
            spawnposition = FindObjectOfType<NetworkStartPosition>();
            healthSlider.enabled = false;

            //내체력만
            my_hp_bar = GameObject.Find("HP_BAR").GetComponent<Image>();
            healthSlider.gameObject.SetActive(false);


            //사망
            hardmode = GameObject.Find("death_text").GetComponent<Text>();
            hardmode.text = "";

           
            death_camera = GameObject.Find("deathCamera");
            cm = GameObject.FindGameObjectWithTag("aa").GetComponent<Camera>();
            

            kill_text = GameObject.Find("killtext").GetComponent<Text>();
            n_kill_text = GameObject.Find("n_kill").GetComponent<Text>();

            kill_cal = GameObject.Find("kill_cal").GetComponent<Transform>();

            StartCoroutine(textreset());
            blood = GameObject.Find("blood");
        }


      //  healpack = GameObject.FindGameObjectWithTag("HEAL_PACK");

    }

    private void Update()
    {
        if(!isLocalPlayer)
            return;

        my_hp_bar.fillAmount =  healthSlider.value/100f;

    
     if (my_hp_bar.fillAmount <= 0)
        {

          
            kill_text.text = "";
            n_kill_text.text = "";
            gameObject.GetComponent<player_control>().enabled = false;
            gameObject.GetComponent<fire>().enabled = false;
            
            
           // blood.SetActive(true);
            if (count ==0)
            {
                death_camera.transform.position = transform.position;
                count++;
                death_camera.GetComponent<Camera>().depth = 15;
              //  kill_cal.position = new Vector3(kill_cal.position.x + 1, 0, 0);
            }
             if (count < 30)
            {
              
                death_camera.transform.Translate(Vector3.back*20f * Time.deltaTime);
                count++;
            }
            if (count == 30)
            {
               // hardmode.text = "사망하셨습니다";

                gameObject.GetComponent<player_control>().gamelose();

                // Mouse Lock
                Cursor.lockState = CursorLockMode.None;
                // Cursor visible
                Cursor.visible = true;


                // if (isLocalPlayer)
                //Time.timeScale = 0;

                //  StartCoroutine(die());
                //  RpcRespawn();

            }
        }

      
       

        //물약
        if (Input.GetKey(KeyCode.Alpha0))
        {
            currentHealth = maxHealth;
        }
    }
    public void LateUpdate()
    {
        if (in_magnetic_field == false)
        {

            TakeDamage(0.5);
            //화면피는플레이어만
            if (isLocalPlayer)
            {
                blood.SetActive(true);
                StartCoroutine(blood_off());
            }
        }
     
    }
    public void TakeDamage(double amount)
    {
       if (!isServer)
        {
         return;
        }
        currentHealth -= amount;
       

       

        if (currentHealth<=0)
        {
          
          

            //RPC는 서버에서발동하면 모든클라이언트들
            //에게도 자동으로 방동됨

            //  gameObject.SetActive(false);

              RpcRespawn();
            // Cmddie();
             StartCoroutine(Death());

            // isDeath = true;
        }

    }

   void onChangeHealth(double health)
    {
        healthSlider.value =(float)health;
       
    }



    //지금 스폰이하나라서이렇지
    //원래는 배열써야되니까
    //그상황오면 꼭 f레트로인강코드다시보기
    [ClientRpc]
    void RpcRespawn()
    {


        //죽으면 힐팩소환
        int rnd = Random.Range(0, 3);
        if(rnd!=2)
        Instantiate(healpack, transform.position+ new Vector3(0,2,0), transform.rotation);
    //   NetworkServer.Spawn(healpack);
        in_magnetic_field = true;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        if (gameObject.GetComponent<CharacterController>())
        gameObject.GetComponent<CharacterController>().enabled = false;
        if (gameObject.GetComponent<fire>())
            gameObject.GetComponent<fire>().enabled = false;
        if (gameObject.GetComponent<gun_sound>())
            gameObject.GetComponent<gun_sound>().enabled = false;

       

        
       
        var anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger("Death");

        // if (gameObject.GetComponent<MOB2_enemy_control>())
         //   gameObject.GetComponent<MOB2_enemy_control>().enabled = false;

        if (isLocalPlayer)
        {
           
            /*
         
            isDeath = false;
            count = 0;
            gameObject.GetComponent<player_control>().enabled = true;
            gameObject.GetComponent<fire>().enabled = true;
            cm.depth = 5;

            blood.SetActive(false);
            hardmode.text = "";
       
            // gameObject.SetActive(true);
        
        
            Vector3 SpawnPOINT = Vector3.zero;
            if(spawnposition!=null )
            {
                SpawnPOINT = spawnposition.transform.position;
            }
        
            transform.position = SpawnPOINT;  */



        }
    }
     [Command]
  void Cmddie()
     {
         //  Destroy(gameObject);
         //  gameObject.SetActive(false);
         //  StartCoroutine(revival());
         RpcRespawn();
     }


    /*   IEnumerator die()
       {
           yield return new WaitForSeconds(1f);

           Cmddie();
         //  RpcRespawn();
           //gameObject.SetActive(false);

       }
       */
    /*
 private void OnTriggerEnter(Collider other)

 {


     if (!isLocalPlayer)
     {
         return;
     }

     if (other.gameObject.CompareTag("monster_weapon1") || other.gameObject.CompareTag("bullet"))
     {


         // TakeDamage(10);

         currentHealth  -=3;
         //  my_HPBAR_image.fillAmount =HP_BAR.value;







     }

 }



*/



    IEnumerator start_magnetic_field()
    {
        yield return new WaitForSeconds(100f);
        in_magnetic_field = false;
        //이때부터자기장줄어듬
      

        if (isLocalPlayer)
        {
            gameObject.GetComponent<player_control>().winner_text.text = "자기장이 가동 되었습니다";
            StartCoroutine(textreset());
        }

        
    }


    IEnumerator textreset()
    {
        yield return new WaitForSeconds(3f);
        gameObject.GetComponent<player_control>().winner_text.text = "";
    }




    IEnumerator Death()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(gameObject);
    }



    IEnumerator revival()
    {
        yield return new WaitForSeconds(2.3f);
        
        currentHealth = maxHealth;
        RpcRespawn();

    }


    IEnumerator blood_off()
    {
        yield return new WaitForSeconds(0.5f);
        blood.SetActive(false);
    }
}

