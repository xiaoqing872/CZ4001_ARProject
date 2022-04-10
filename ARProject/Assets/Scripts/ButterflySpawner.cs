using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class ButterflySpawner : MonoBehaviour
{
    [Range(0, 100)]
    public int spawnChance = 2;
    GameObject[] butterflies;
    GameObject currButterfly;
    int index;
    ARPlaneManager arPlaneManager;

    void Awake()
    {
        butterflies = Resources.LoadAll<GameObject>("Butterfly");
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;
    }

    void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null)
        {
            ARPlane arPlane = args.added[0];
            Mesh mesh = arPlane.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            for (int vertId = 0; vertId < vertices.Length; vertId++)
            {
                if (Random.Range(0, 100) > 100 - spawnChance)
                {
                    index = Random.Range(0, butterflies.Length);
                    currButterfly = butterflies[index];
                    Instantiate(currButterfly, vertices[vertId], transform.rotation * Quaternion.Euler(0, Random.Range(0, 360), 0));
                }
            }
        }
    }

}
