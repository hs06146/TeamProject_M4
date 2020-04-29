using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class fire : NetworkBehaviour
{
    public GameObject Bulletprefab;
    public Transform FirePos;



 

    // Start is called before the first frame update
    void Start()
    {

        if (!isLocalPlayer)
        {
            return;
        }

       
      //  FirePos = AIM_CAMVAS.transform.GetChild(0).GetChild(1);
    }

    // Update is called once per frame
    void Update()

    {

        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            CmdFire();
     
        }
        if (Input.GetMouseButtonDown(0))
        {
            CmdFire();
            
        }


    }

    [Command]
  
    void CmdFire()
    {
        GameObject bullet = Instantiate(Bulletprefab, FirePos.transform.position, FirePos.transform.rotation);
       
        // bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 1000f * Time.deltaTime;
       //  bullet.transform.Translate(Vector3.forward * 200f * Time.deltaTime);
        NetworkServer.Spawn(bullet);

        
      Destroy(bullet, 3f);
    }

}
