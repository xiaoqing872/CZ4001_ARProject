using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FoxAnimator : MonoBehaviour
{
    GameObject spawnedObject;
    
    ARRaycastManager raycaster;

    [SerializeField]
    ARSessionOrigin arSessionOrigin;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        raycaster = arSessionOrigin.GetComponent<ARRaycastManager>();
        animator.SetBool("onPlane", true);
    }

    // Update is called once per frame
    void Update()
    {
        spawnedObject = arSessionOrigin.GetComponent<PlaceObjectOnPlane>().spawnedObject;

        if (spawnedObject != null)
        {
            var ray = new Ray(spawnedObject.transform.position, Vector3.down);
            var insidePlane = raycaster.Raycast(ray, hits, TrackableType.PlaneWithinPolygon);

            if (insidePlane)
                animator.SetBool("onPlane", true);
            else
                animator.SetBool("onPlane", false);
        }
    }
}
