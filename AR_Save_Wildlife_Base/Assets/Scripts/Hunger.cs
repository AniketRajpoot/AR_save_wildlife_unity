using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;


public class Hunger : MonoBehaviour
{
    public float hunger;
    public List<Hunger2Sort> sorts = new List<Hunger2Sort>();
    public float currentHunger;
    private bool feed;
    public GameObject child;
    GameObject childAF;
    public bool inst = false;
    private string DATA_URL = "https://chuu-89699.firebaseio.com/";
    //private ScenceController scence = new ScenceController();

    private DatabaseReference databaseReference;

    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATA_URL);
        child = this.gameObject.transform.GetChild(0).gameObject;
        child.SetActive(false);
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }


    void Update()
    {
        //isRecieving();
        if(hunger <= 20 && hunger >= 10 && inst == false)
        {
            child.SetActive(true);
            inst = true;
        }
       
        RecieveHunger();
        RemoveHunger();
    }
    //Hunger System
    public int HungerTimer = 20;
    /*public void isRecieving()
    {
        if(HungerTimer != 0)
        {
            sorts[1].isReceiving = true;
          
        }
        else
        {
            sorts[1].isReceiving = false;
        }
    }*/

    public void RecieveHunger()
    {
        foreach(Hunger2Sort h in sorts)
        {
            if (h.isReceiving)
            {
                currentHunger += h.hunger / 5000;
            }

            hunger -= currentHunger * Time.deltaTime;

            databaseReference.Child("Health of " + this.gameObject.name + ": ").SetValueAsync(hunger);


            if (hunger <= 0)
            {
                //destroy
                string str = "Your animal died!!";
                databaseReference.Child("Message:").SetValueAsync(str);
                Destroy(this.gameObject);
                
                
            }
        }
    }

    public void RemoveHunger()
    {
      
    if (feed)
        {
            hunger += 100;
            child.SetActive(false);
            feed = false;
        }
    }
    public void feedPressed()
    {
        feed = true;
    }
}
[System.Serializable]
public class Hunger2Sort
{
    public string name;
    public float hunger;
    public bool isReceiving = false;
}
