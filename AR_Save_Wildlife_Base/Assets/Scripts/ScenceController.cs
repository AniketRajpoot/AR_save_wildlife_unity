using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using TMPro;
using Firebase.Auth;

public class ScenceController : MonoBehaviour
{
    //Main score
    private int score = 100;
    Hunger ScoreHunger;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI DailyTask;
    private bool markClicked = false;
    private float timerScore = -1;
    //firebase
    private string DATA_URL = "https://chuu-89699.firebaseio.com/";
    private DatabaseReference databaseReference;


    public Camera firstPersonCamera;
    public ScoreBoardController scoreboard;
    public SnakeController snakeController;
    public GameObject marker;
    private GameObject just;  //for storing marker update
    public GameObject Gameplane;
    private GameObject GamePlaneInstance;
    //hunger
    private Hunger hunger;
    private bool feedClicked = false;

    public bool clicked = false;
    private bool inst = false;
    private bool checkPlane = false;
    private bool isNotPlaced = false;
    
    //info tab
    private bool infoClicked = false;
    int infoIndex = 0;
    public List<GameObject> Info;
   
    //placement objects 
    private GameObject TreeInstance;
    public List<GameObject> m_Prefabs;
    private int m_currentObjectIndex;
    private GameObject selectedObject;
    
    //using movement
    Animator anim;
    public float timer;
    bool inSync = false;
    private GameObject temp;
    private float speedTiger = 0.2f;
    
    //animals inventory
    private bool animalsClicked = false;
    public GameObject animals;

    //animals inventory
    private bool treeClicked = false;
    public GameObject treeList;

    //Daily Task
    private bool TaskClicked = false;
    public GameObject Task;
   

    

    void QuitOnConnectionErrors()
    {
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "Camera permission is needed to run this application.", 5));
        }
        else if (Session.Status.IsError())
        {
            // This covers a variety of errors.  See reference for details
            // https://developers.google.com/ar/reference/unity/namespace/GoogleARCore
            StartCoroutine(CodelabUtils.ToastAndExit(
                "ARCore encountered a problem connecting. Please restart the app.", 5));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DailyTask.SetText("Feed nearby domestic animals!!" +
            "Like Birds and cats etc.");
        Score.text = score.ToString();
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DATA_URL);

        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child("score").SetValueAsync(score);

        QuitOnConnectionErrors();

    }
    
    // Update is called once per frame
    void Update()
    {
        // The session status must be Tracking in order to access the Frame.
        if (Session.Status != SessionStatus.Tracking)
        {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (clicked == true)
        {

            List<TrackableHit> hits = new List<TrackableHit>();
            //TrackableHit hit;
            TrackableHitFlags raycastFilter =
                    TrackableHitFlags.PlaneWithinBounds |
                    TrackableHitFlags.PlaneWithinPolygon;
            Vector3 fwd = firstPersonCamera.transform.TransformDirection(Vector3.forward);
            if (Frame.RaycastAll(firstPersonCamera.transform.position, fwd, hits, 50f, raycastFilter))
            {
                Pose hitPose = hits[0].Pose;
                var cameraForward = Camera.current.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                hitPose.rotation = Quaternion.LookRotation(cameraBearing);

                if (inst == false)
                {
                    just = Instantiate(marker, hitPose.position, hitPose.rotation);
                    just.SetActive(true);
                    inst = true;
                }
                else
                {
                    just.transform.position = hitPose.position;
                }
            }


        }
        if (checkPlane == true)
        {
            ProcessTouches();
        }
        else
        {
            SSTools.ShowMessage("create a plane first!", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }

       
       

    }

    void ProcessTouches()
    {
        Touch touch;
        if (Input.touchCount != 1 ||
            (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        TrackableHit hit;
        TrackableHitFlags raycastFilter =
            TrackableHitFlags.PlaneWithinBounds |
            TrackableHitFlags.PlaneWithinPolygon;
        Ray raycast = firstPersonCamera.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            /*var selection = raycastHit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();
            if(selectionRenderer != null)
            {
                selectionRenderer.material = highlightMaterial;
            }*/
            //SSTools.ShowMessage("Cast Detected", SSTools.Position.bottom, SSTools.Time.twoSecond);
            if (raycastHit.collider.tag == "Tiger")
            {
                temp = raycastHit.collider.gameObject;
                hunger = temp.transform.GetComponent<Hunger>();
                infoIndex = 0;  //info for tiger only
                displayInfo();
            }else if(raycastHit.collider.tag == "Elephan")
            {

                temp = raycastHit.collider.gameObject;
                hunger = temp.transform.GetComponent<Hunger>();
                infoIndex = 1;  //info for elephant only
                displayInfo();
            }
            else if (raycastHit.collider.tag == "Rhino")
            {

                temp = raycastHit.collider.gameObject;
                hunger = temp.transform.GetComponent<Hunger>();
                infoIndex = 2;  //info for rhino only
                displayInfo();
            }
            else if (raycastHit.collider.tag == "Zebra")
            {

                temp = raycastHit.collider.gameObject;
                hunger = temp.transform.GetComponent<Hunger>();
                infoIndex = 3;  //info for zebra only
                displayInfo();
            }
            
            
        }
        else
        {
            SSTools.ShowMessage("No hit detected", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
       
            Vector3 fwd = firstPersonCamera.transform.TransformDirection(Vector3.forward);
            if (Frame.Raycast(firstPersonCamera.transform.position, fwd, out hit, 50f, raycastFilter))
            {
                Pose hitPose = hit.Pose;
                if (isNotPlaced == true)
                {
                    TreeInstance = Instantiate(m_Prefabs[m_currentObjectIndex], hitPose.position,
                    hitPose.rotation, transform);
                    if (m_currentObjectIndex == 0)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        score = score + 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                }
                    else if (m_currentObjectIndex == 1)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        score = score + 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                }
                    else if (m_currentObjectIndex == 2)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        TreeInstance.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
                    score = score - 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);

                }
                    else if (m_currentObjectIndex == 3)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        TreeInstance.transform.Rotate(0.0f, 180f, 0.0f,Space.Self);
                        score = score - 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                    //anim = TreeInstance.transform.GetComponent<Animator>();
                    //anim.SetBool("walk", true);
                    //walk(TreeInstance);
                    }
                    else if (m_currentObjectIndex == 4)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        TreeInstance.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
                        score = score - 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                    }
                    else if (m_currentObjectIndex == 5)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        TreeInstance.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
                        score = score - 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                    }
                    else if (m_currentObjectIndex == 6)
                    {
                       
                        TreeInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        TreeInstance.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
                        score = score - 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);

                    }
                    else if (m_currentObjectIndex == 7)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                        TreeInstance.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
                        score = score - 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);

                }
                    else if (m_currentObjectIndex == 8)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        score = score + 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                }
                    else if (m_currentObjectIndex == 9)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                        score = score + 2;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                }
                    else if (m_currentObjectIndex == 10)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
                        score = score + 1;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);

                }
                    else if (m_currentObjectIndex == 11)
                    {
                        TreeInstance.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
                        score = score + 1;
                        Score.text = score.ToString();
                        databaseReference.Child("score").SetValueAsync(score);
                }
                isNotPlaced = false;
                }

            }

        

    }

    void SetSelectedPlane(DetectedPlane selectedPlane)
    {
        Debug.Log("Selected plane centered at " + selectedPlane.CenterPose.position);
        scoreboard.SetSelectedPlane(selectedPlane);
        snakeController.SetPlane(selectedPlane);
    }

    public void isClicked(Button button)
    {
        if (button.name.Equals("MarkerButton"))
        {
            clicked = true;
        }
    }

    public void PlacePlane()
    {
        if (checkPlane == false)
        {
            TrackableHit hit;
            TrackableHitFlags raycastFilter =
                    TrackableHitFlags.PlaneWithinBounds |
                    TrackableHitFlags.PlaneWithinPolygon;
            Vector3 fwd = firstPersonCamera.transform.TransformDirection(Vector3.forward);
            if (Frame.Raycast(firstPersonCamera.transform.position, fwd, out hit, 50f, raycastFilter))
            {
                Pose hitPose = hit.Pose;
                GamePlaneInstance = Instantiate(Gameplane, hitPose.position, hitPose.rotation);
                GamePlaneInstance.transform.localScale = new Vector4(0.06f, 0.06f, 0.06f);
            }
            checkPlane = true;
        }
        else
        {
            SSTools.ShowMessage("Can create only one plane!", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }

    }


    public void setIndex(Button button)
    {
        if (button.name.Equals("ButtonTree_1"))
        {
            m_currentObjectIndex = 0;
        }
        else if (button.name.Equals("ButtonTree_2"))
        {
            m_currentObjectIndex = 1;
        }
        else if (button.name.Equals("ButtonTiger"))
        {
            m_currentObjectIndex = 2;
        }
        else if (button.name.Equals("ButtonElephant"))
        {
            m_currentObjectIndex = 3;
        }
        else if (button.name.Equals("ButtonRhino"))
        {
            m_currentObjectIndex = 4;
        }
        else if (button.name.Equals("ButtonZebra"))
        {
            m_currentObjectIndex = 5;
        }
        else if (button.name.Equals("ButtonHorse"))
        {
            m_currentObjectIndex = 6;
        }
        else if (button.name.Equals("ButtonBeagle"))
        {
            m_currentObjectIndex = 7;
        }
        else if (button.name.Equals("ButtonTree_3"))
        {
            m_currentObjectIndex = 8;
        }
        else if (button.name.Equals("ButtonTree_4"))
        {
            m_currentObjectIndex = 9;
        }
        else if (button.name.Equals("ButtonBush_1"))
        {
            m_currentObjectIndex = 10;
        }
        else if (button.name.Equals("ButtonBush_2"))
        {
            m_currentObjectIndex = 11;
        }
        else if (button.name.Equals("BuildButton"))
        {
            SSTools.ShowMessage("Will Destroy The Habitat", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }

    public void isPlaceClicked()
    {
        isNotPlaced = true;
    }

    public void displayTask()
    {
        if (!animalsClicked && !treeClicked && !infoClicked)
        {
            if (TaskClicked == false)
            {
                Task.SetActive(true);
                TaskClicked = true;
            }
            else
            {
                Task.SetActive(false);
                TaskClicked = false;
            }
        }
        {
            SSTools.ShowMessage("Close the other tabs first!", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }
    public void displayInv()
    {
        if (!TaskClicked && !treeClicked && !infoClicked)
        {
            if (animalsClicked == false)
            {
                animals.SetActive(true);
                animalsClicked = true;
            }
            else
            {
                animals.SetActive(false);
                animalsClicked = false;
            }
        }
        else
        {
            SSTools.ShowMessage("Close the other tabs first!", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }
    public void displayInvTrees()
    {
        if (!TaskClicked && !animalsClicked && !infoClicked)
        {
            if (treeClicked == false)
            {
                treeList.SetActive(true);
                treeClicked = true;
            }
            else
            {
                treeList.SetActive(false);
                treeClicked = false;
            }
        }
        else
        {
            SSTools.ShowMessage("Close the other tabs first!", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }
    public void displayInfo()
    {
        if (!TaskClicked && !treeClicked && !animalsClicked)
        {
            if (infoClicked == false)
            {
                Info[infoIndex].SetActive(true);
                infoClicked = true;
            }
            else
            {
                Info[infoIndex].SetActive(false);
                infoClicked = false;
            }
        }
        else
        {
            SSTools.ShowMessage("Close the other tabs first!", SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }
    public void feed()
    {
        if (hunger != null)
        {
            if (!feedClicked)
            {
                score = score + 5;
                Score.text = score.ToString();
                databaseReference.Child("score").SetValueAsync(score);
                SSTools.ShowMessage("Animal Fed Current Hunger:" + hunger.hunger, SSTools.Position.bottom, SSTools.Time.twoSecond);
                hunger.feedPressed();
                StartCoroutine(FeedCoroutine());
            }
        }
        else
        {
            SSTools.ShowMessage("Select an animal to feed!" + hunger.hunger, SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }

    public void dailytask(Button button)
    {

        if (button.name.Equals("completed"))
        {
            //databaseReference.Child("markClicked").SetValueAsync(markClicked);
            if (!markClicked)
            {
                score = score + 20;
                Score.text = score.ToString();
                databaseReference.Child("score").SetValueAsync(score);
                markClicked = true;
                timerScore = 100;
                StartCoroutine(ButtonCoroutine());
            }
        }
    }

   

    public void nofeed()
    {
        score = score - 10;
        Score.text = score.ToString();
        databaseReference.Child("score").SetValueAsync(score);
        //return score;
    }

    IEnumerator ButtonCoroutine()
    {
        yield return new WaitForSeconds(20f);
        // Turn on the button & make it active
        //databaseReference.Child("scoretimeeeee").SetValueAsync(Time.time);
        DailyTask.SetText("Now make shelter for one nearby homeless animals!");
        markClicked = false;
    }

    IEnumerator FeedCoroutine()
    {
        yield return new WaitForSeconds(10f);
        // Turn on the button & make it active
        //databaseReference.Child("scoretimeeeee").SetValueAsync(Time.time);
        feedClicked = false;
    }
}