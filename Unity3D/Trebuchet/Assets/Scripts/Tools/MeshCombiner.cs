using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class MeshCombiner
{
    [MenuItem("Tools/Combine Selected Meshes")]
    static void CombineSelectedMeshes()
    {
        Debug.Log("Combine Meshes");

        GameObject[] objs = Selection.gameObjects;

        List< MeshFilter> meshFilters = new List< MeshFilter>();

        for(int i = 0; i < objs.Length; i++)
        {
            MeshFilter[] mF = objs[i].GetComponentsInChildren<MeshFilter>(true);

            if (mF != null)
            {
                meshFilters.AddRange(mF);
            }
        }

        if(meshFilters.Count == 0) 
        {
            Debug.Log("No mesh filters found!");
            return;
        }

        // Create a new combined mesh to hold all the meshes
        Mesh combinedMesh = new Mesh();

        // Combine the meshes into the new combined mesh
        CombineInstance[] combineInstances = new CombineInstance[meshFilters.Count];

        for (int i = 0; i < meshFilters.Count; i++)
        {
            combineInstances[i].mesh = meshFilters[i].sharedMesh;
            combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        combinedMesh.CombineMeshes(combineInstances);

        // Create a new game object to hold the combined mesh
        GameObject combinedObject = new GameObject("Combined Mesh");
        combinedObject.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
        combinedObject.AddComponent<MeshRenderer>();

        // Save the mesh as an asset in the project
        string folderPath = "Assets/CombinedMeshes";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "CombinedMeshes");
        }

        AssetDatabase.CreateAsset(combinedMesh, folderPath + "/CombinedMesh.asset");
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Tools/Remove Mesh Components From Selected")]
    static void RemoveMeshComponents()
    {
        Debug.Log("Remove Mesh Components From Selected");

        GameObject[] objs = Selection.gameObjects;

        for(int i = 0; i < objs.Length; i++)
        {
            Object.DestroyImmediate(objs[i].GetComponent<MeshFilter>());
            Object.DestroyImmediate(objs[i].GetComponent<MeshRenderer>());
        }
    }
}
#endif