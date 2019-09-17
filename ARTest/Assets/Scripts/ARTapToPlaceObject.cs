using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    public GameObject wayText;
    public GameObject cubeText;
    public GameObject waypoint;
    public GameObject contentAppear;
    
    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool isValidPose;
    private bool gameMode;
    private GameObject placement;
    private float textTime = 5.0f;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        isValidPose = false;
        gameMode = false;
        wayText.SetActive(true);
        cubeText.SetActive(false);
        StartCoroutine(TextTime());
        arOrigin.MakeContentAppearAt(contentAppear.transform, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        
        if (isValidPose && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && gameMode)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                PlaceObject();
            }
        }

        if (!gameMode)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    Instantiate(waypoint, placementPose.position, placementPose.rotation);
                }
            }
        }
    }

    public void StartGame(bool value)
    {
        gameMode = value;
        StartCoroutine(TextTime());
    }

    private IEnumerator TextTime()
    {
        if(gameMode)
        {
            cubeText.SetActive(true);
            wayText.SetActive(false);
        }
        else
        {
            wayText.SetActive(true);
            cubeText.SetActive(false);
        }
        yield return new WaitForSeconds(textTime);
        
        cubeText.SetActive(false);
        wayText.SetActive(false);
    }
    
    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (isValidPose)
        {
            if (placement == null)
            {
                placement = Instantiate(placementIndicator, placementPose.position, placementPose.rotation);
            }
            Debug.Log(isValidPose + " e true ");
            placement.SetActive(true);
            placement.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            Debug.Log(isValidPose + " e false ");
            placement.SetActive(false);
        }
    }
    
    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits, TrackableType.Planes);

        isValidPose = hits.Count > 0;

        if (isValidPose)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }
}
