using UnityEngine;
using UnityEngine.AI;
public class NavMeshRenderer : MonoBehaviour
{

    public float updateRatePerSecond = 5;

    private Mesh mesh;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }

        var tris = NavMesh.CalculateTriangulation();

        mesh.vertices = tris.vertices;
        mesh.triangles = tris.indices;

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    private float timeSinceLastUpdate = 0;
    private void Update()
    {
        if (mesh == null)
            return;
        
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate < 1 / updateRatePerSecond)
            return;

        timeSinceLastUpdate -= 1 / updateRatePerSecond;
        var tris = NavMesh.CalculateTriangulation();

        mesh.triangles = null;
        mesh.vertices = tris.vertices;
        mesh.triangles = tris.indices;
    }
}
