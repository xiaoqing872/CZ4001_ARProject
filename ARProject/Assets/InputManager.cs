using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit.AR;
using UnityEngine.XR.ARSubsystems;

public class InputManager : ARBaseGestureInteractable
{

    [SerializeField] private Camera arCam;
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject crosshair;

    List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private Touch touch;
    private Pose pose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override bool CanStartManipulationForGesture(TapGesture gesture) {
        if (gesture.targetObject == null)
        {
            return true;
        }
        else { return false; }
    }

    protected override void OnEndManipulation(TapGesture gesture) {
        if (gesture.isCanceled)
            return;
        if (gesture.targetObject != null || IsPointerOverUI(gesture)) {
            return;
        }
        if (GestureTransformationUtility.Raycast(gesture.startPosition, _hits, TrackableType.PlaneWithinPolygon)) {
            GameObject placedObj = Instantiate(DataHandler.Instance.GetOptionItems(), pose.position, pose.rotation);
            var anchorObject = new GameObject("PlacementAnchor");
            anchorObject.transform.position = pose.position;
            anchorObject.transform.rotation = pose.rotation;
            placedObj.transform.parent = anchorObject.transform;
        
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CrosshairCalculation();


    }

    bool IsPointerOverUI(TapGesture touch) {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.startPosition.x, touch.startPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    //calculating where to put crosshair
    void CrosshairCalculation() {
        Vector3 origin = arCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));

        if (GestureTransformationUtility.Raycast(origin, _hits, TrackableType.PlaneWithinPolygon))
        {
            pose = _hits[0].pose;
            crosshair.transform.position = pose.position;
            crosshair.transform.eulerAngles = new Vector3(90, 0, 0);

        }
    }
}
