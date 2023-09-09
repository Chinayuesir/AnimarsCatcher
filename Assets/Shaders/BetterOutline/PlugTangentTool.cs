using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlugTangentTool
{
    [MenuItem("Tools/WriteAverageNormalToTangent")]
    public static void WriteAverageNormalToTangentToos()
    {
        MeshFilter[] meshFilters = Selection.activeGameObject.GetComponentsInChildren<MeshFilter>();
        foreach (var meshFilter in meshFilters)
        {
            Mesh mesh = meshFilter.sharedMesh;
            WriteAverageNormalToTangent(mesh);
        }

        SkinnedMeshRenderer[] skinnedMeshRenderers = Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinMeshRender in skinnedMeshRenderers)
        {
            Mesh mesh = skinMeshRender.sharedMesh;
            WriteAverageNormalToTangent(mesh);
        }
    }

    private static void WriteAverageNormalToTangent(Mesh mesh)
    {
        var averageNormalHash = new Dictionary<Vector3, Vector3>();
        for (int j = 0; j < mesh.vertexCount; j++)
        {
            if (!averageNormalHash.ContainsKey(mesh.vertices[j]))
            {
                averageNormalHash.Add(mesh.vertices[j], mesh.normals[j]);
            }
            else
            {
                averageNormalHash[mesh.vertices[j]] = (averageNormalHash[mesh.vertices[j]] + mesh.normals[j]).normalized;
            }
        }

        var averageNormals = new Vector3[mesh.vertexCount];
        for (int j = 0; j < mesh.vertexCount; j++)
        {
            averageNormals[j] = averageNormalHash[mesh.vertices[j]];
        }

        var tangents = new Vector4[mesh.vertexCount];
        for (int j = 0; j < mesh.vertexCount; j++)
        {
            tangents[j] = new Vector4(averageNormals[j].x, averageNormals[j].y, averageNormals[j].z, 0);
        }
        mesh.tangents = tangents;
    }
}
