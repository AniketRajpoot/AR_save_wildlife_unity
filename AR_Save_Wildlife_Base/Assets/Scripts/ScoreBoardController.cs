using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;

public class ScoreBoardController : MonoBehaviour
{
    public Camera firstPersonCamera;
    private Anchor anchor;
    private DetectedPlane detectedPlane;
    private float yOffset;
    private int score;

    //trying to make multiple selectable game objects
    private GameObject TreeInstance;
    public List<GameObject> m_Prefabs;
    private int m_currentObjectIndex;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }
        if (detectedPlane == null)
        {
            return;
        }

        // Check for the plane being subsumed.
        // If the plane has been subsumed switch attachment to the subsuming plane.
        while (detectedPlane.SubsumedBy != null)
        {
            detectedPlane = detectedPlane.SubsumedBy;
        }
       
       
    }

    public void SetSelectedPlane(DetectedPlane detectedPlane)
    {
        this.detectedPlane = detectedPlane;
        Place();
    }

    void CreateAnchor()
    {
       
        
        anchor = Session.CreateAnchor(transform.position, transform.rotation);
        TreeInstance = Instantiate(m_Prefabs[m_currentObjectIndex],anchor.transform.position,
        anchor.transform.rotation,anchor.transform);
        TreeInstance.transform.localScale = new Vector4(0.1f, 0.1f, 0.1f);
        
    }

    void Place()
    {
        Vector3 pos = detectedPlane.CenterPose.position;
        TreeInstance = Instantiate(m_Prefabs[m_currentObjectIndex], pos,
                Quaternion.identity, transform);
        TreeInstance.transform.localScale = new Vector4(0.1f,0.1f,0.1f);
    }

    public void setIndex(Button button)
    {
        if (button.name.Equals("TreeButton")){
            m_currentObjectIndex = 0;
        }else if (button.name.Equals("BushButton"))
        {
            m_currentObjectIndex = 1;
        }
    }
}
