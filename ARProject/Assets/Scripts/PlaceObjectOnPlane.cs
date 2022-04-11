using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class PlaceObjectOnPlane : MonoBehaviour
{
    [SerializeField]
    GameObject placedPrefab;
    public GameObject spawnedObject;
    ARRaycastManager raycaster;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private void Start()
    {
        raycaster = GetComponent<ARRaycastManager>();
    }
    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        // create a regular anchor at the hit pose
        ARAnchor anchor;
        spawnedObject = Instantiate(placedPrefab, hit.pose.position, hit.pose.rotation);
        anchor = spawnedObject.GetComponent<ARAnchor>();
        if (anchor == null)
        {
            anchor = spawnedObject.AddComponent<ARAnchor>();
        }

        return anchor;
    }

    void OnPlaceObject(InputValue value)
    {
        Vector2 touchPosition = value.Get<Vector2>();
        if (raycaster.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            //get the hit point(pose) on the plane
            Pose hitPose = hits[0].pose;
            //if this is the first time placing an object,
            if (spawnedObject == null)
            {
                //instantiate the prefab at the hit position and rotation
                //spawnedObject = Instantiate(placedPrefab, hitPose.position, Quaternion.Inverse(hitPose.rotation));
                CreateAnchor(hits[0]);
            }
            //else
            else
            {
                //change the position of the previously instantiated object
                spawnedObject.transform.SetPositionAndRotation(hitPose.position, Quaternion.Inverse(hitPose.rotation));
            }
        }
    }
}