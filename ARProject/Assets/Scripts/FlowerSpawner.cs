using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class FlowerSpawner : MonoBehaviour
{
    [Range(0, 100)]
    public int spawnChance = 10;
    GameObject[] flowers;
    GameObject currFlower;
    int index;
    ARPlaneManager arPlaneManager;

    void Awake()
    {
        flowers = Resources.LoadAll<GameObject>("Prefabs");
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;
    }

    void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if(args.added != null)
        {
            foreach (ARPlane arPlane in args.updated)
            {
                // Chance to spawn a flower at each vertex of the mesh.
                Mesh mesh = arPlane.GetComponent<MeshFilter>().mesh;
                for (int vertID = 0; vertID < mesh.vertices.Length; vertID++)
                {
                    Vector3 vert = mesh.vertices[vertID];
                    if (vertID < arPlane.transform.childCount - 1)
                    {
                        Transform item = arPlane.transform.GetChild(vertID);
                        item.localPosition = vert;
                    }
                    else if (Random.Range(0, 100) > 100 - spawnChance)
                    {
                        index = Random.Range(0, flowers.Length);
                        currFlower = flowers[index];
                        GameObject newFlower = Instantiate(currFlower, arPlane.transform);
                        newFlower.transform.localPosition = vert;
                        newFlower.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    }
                    else
                    {
                        GameObject reserved = new GameObject("Reserved");
                        reserved.transform.localPosition = vert;
                        reserved.transform.SetParent(arPlane.transform);
                    }
                }
            }
        }

        // Remove de-spawned planes.
        if(args.removed != null)
        {
            foreach (ARPlane arPlane in args.removed)
            {
                foreach (Transform child in arPlane.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }
}
