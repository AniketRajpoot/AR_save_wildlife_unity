using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class just : MonoBehaviour
{
    Animator anim;
    public float timer;  //feeding timer value from inspector
    private bool isWalking = false;
    private float speedTiger = 0.2f;
    private float rotSpeed = 75.0f;

    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWalking)
        {
            anim = this.gameObject.transform.GetComponent<Animator>();
            anim.SetBool("walk", true);
            isWalking = true;
        }
      
        //translation in the forward direction
        this.gameObject.transform.Translate(Vector3.forward * speedTiger * Time.deltaTime);
      
        if (timer < 0)   //when timer == 0 than stop the motion
        {
            speedTiger = 0;
            anim.SetBool("walk", false);   //setting animaiton to idle animaiton
        }
       
        timer -= Time.deltaTime;
        
        //this.transform.Translate(Vector3.forward * speedTiger * Time.deltaTime);
    }
}
