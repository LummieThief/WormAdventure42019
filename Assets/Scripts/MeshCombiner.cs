using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class MeshCombiner : MonoBehaviour
{
	public bool run;
	public bool runAdvanced;
	public bool restore;

	private void Start()
	{
		gameObject.layer = 2;
	}
	private void Update()
	{
		if (run)
		{
			run = false;
			CombineMeshes();
		}
		if (runAdvanced)
		{
			Debug.Log("running advanced");
			runAdvanced = false;
			CombineMeshesAdvanced();
		}
		if (restore)
		{
			restore = false;
			UncombineMeshes();
		}
	}

	public void CombineMeshesAdvanced()
	{
		MeshRenderer myRenderer = GetComponent<MeshRenderer>();
		MeshFilter myFilter = GetComponent<MeshFilter>();

		// All our children (and us)
		MeshFilter[] filters = GetComponentsInChildren<MeshFilter>(false);

		// All the meshes in our children (just a big list)
		List<Material> materials = new List<Material>();
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>(false); // <-- you can optimize this
		foreach (MeshRenderer renderer in renderers)
		{
			if (renderer.transform == transform)
				continue;
			Material[] localMats = renderer.sharedMaterials;
			foreach (Material localMat in localMats)
				if (!materials.Contains(localMat))
					materials.Add(localMat);
		}

		// Each material will have a mesh for it.
		List<Mesh> submeshes = new List<Mesh>();
		foreach (Material material in materials)
		{
			// Make a combiner for each (sub)mesh that is mapped to the right material.
			List<CombineInstance> combiners = new List<CombineInstance>();
			foreach (MeshFilter filter in filters)
			{
				if (filter.transform == transform) continue;
				// The filter doesn't know what materials are involved, get the renderer.
				MeshRenderer renderer = filter.GetComponent<MeshRenderer>();  // <-- (Easy optimization is possible here, give it a try!)
				if(renderer != null)
					renderer.enabled = false;
				if (renderer == null)
				{
					Debug.LogError(filter.name + " has no MeshRenderer");
					continue;
				}

				// Let's see if their materials are the one we want right now.
				Material[] localMaterials = renderer.sharedMaterials;
				for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
				{
					if (localMaterials[materialIndex] != material)
						continue;
					// This submesh is the material we're looking for right now.
					CombineInstance ci = new CombineInstance();
					ci.mesh = filter.sharedMesh;
					ci.subMeshIndex = materialIndex;
					ci.transform = filter.transform.localToWorldMatrix;
					combiners.Add(ci);
				}
			}
			// Flatten into a single mesh.
			Mesh mesh = new Mesh();
			mesh.CombineMeshes(combiners.ToArray(), true);
			submeshes.Add(mesh);
		}

		// The final mesh: combine all the material-specific meshes as independent submeshes.
		List<CombineInstance> finalCombiners = new List<CombineInstance>();
		foreach (Mesh mesh in submeshes)
		{
			CombineInstance ci = new CombineInstance();
			ci.mesh = mesh;
			ci.subMeshIndex = 0;
			ci.transform = Matrix4x4.identity;
			finalCombiners.Add(ci);
		}
		Mesh finalMesh = new Mesh();
		finalMesh.CombineMeshes(finalCombiners.ToArray(), false);
		myFilter.sharedMesh = finalMesh;
		if (materials.ToArray().Length != myRenderer.sharedMaterials.Length)
		{
			myRenderer.sharedMaterials = materials.ToArray();
		}
		Debug.Log("Final mesh has " + submeshes.Count + " materials.");
	}


	public void CombineMeshes()
	{
		Quaternion oldRot = transform.rotation;
		Vector3 oldPos = transform.position;

		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;


		MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

		Debug.Log(name + " is combining meshes! " + filters.Length + " Meshes!");

		Mesh finalMesh = new Mesh();

		CombineInstance[] combiners = new CombineInstance[filters.Length];

		for (int a = 0; a < filters.Length; a++)
		{
			if (filters[a].transform == transform)
				continue;
			combiners[a].subMeshIndex = 0;
			combiners[a].mesh = filters[a].sharedMesh;
			combiners[a].transform = filters[a].transform.localToWorldMatrix;
		}


		finalMesh.CombineMeshes(combiners);

		GetComponent<MeshFilter>().sharedMesh = finalMesh;


		transform.rotation = oldRot;
		transform.position = oldPos;

		for (int a = 0; a < transform.childCount; a++)
		{
			transform.GetChild(a).GetComponent<MeshRenderer>().enabled = false;
		}

	}

	public void UncombineMeshes()
	{

		foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
		{
			renderer.enabled = true;
		}
		GetComponent<MeshFilter>().sharedMesh = null;
	}
}
