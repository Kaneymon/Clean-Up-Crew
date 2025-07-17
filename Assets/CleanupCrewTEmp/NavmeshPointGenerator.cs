using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class NavmeshPointGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private float areaWidth = 20f;
    [SerializeField] private float areaDepth = 20f;
    [SerializeField] private float spacing = 2f;

    [Header("NavMesh Sampling")]
    [SerializeField] private float sampleHeight = 1f;
    [SerializeField] private float maxSampleDistance = 2f;

    [Header("Debug Settings")]
    [SerializeField] private float gizmoSphereSize = 0.3f;
    [SerializeField] private Color gizmoColor = Color.green;

    private static List<Vector3> generatedPoints = new List<Vector3>();
    public static IReadOnlyList<Vector3> Points => generatedPoints;

    [ContextMenu("Generate NavMesh Points")]
    public void GeneratePoints()
    {
        generatedPoints.Clear();

        Vector3 origin = transform.position - new Vector3(areaWidth / 2f, 0f, areaDepth / 2f);

        for (float x = 0; x <= areaWidth; x += spacing)
        {
            for (float z = 0; z <= areaDepth; z += spacing)
            {
                Vector3 samplePoint = new Vector3(origin.x + x, sampleHeight, origin.z + z);
                
                if (NavMesh.SamplePosition(samplePoint, out NavMeshHit hit, maxSampleDistance, NavMesh.AllAreas))
                {
                    generatedPoints.Add(hit.position);
                }
            }
        }

        Debug.Log($"[NavmeshPointGenerator] Generated {generatedPoints.Count} NavMesh points.");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        foreach (var point in generatedPoints)
        {
            Gizmos.DrawSphere(point, gizmoSphereSize);
        }
    }
}
