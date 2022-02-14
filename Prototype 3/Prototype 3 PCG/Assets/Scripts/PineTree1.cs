using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineTree1 : MonoBehaviour
{
    [SerializeField]
    private Mesh treeMesh;

    [SerializeField]
    private Material material1;

    [SerializeField]
    private Material material2;


    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Rigidbody rigidBody;

    public PineTree1()
    {

    }

    private void Awake()
    {

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.materials = new Material[2];
        rigidBody = gameObject.AddComponent<Rigidbody>();
        meshCollider = gameObject.AddComponent<MeshCollider>();

    }

    private void Start()
    {
        GetComponent<MeshFilter>().mesh = treeMesh;

        //meshFilter.mesh = treeMesh;

        Material[] mats = GetComponent<MeshRenderer>().materials;

        mats[0] = material1;
        mats[1] = material2;
        GetComponent<MeshRenderer>().materials = mats;

        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        GetComponent<MeshCollider>().convex = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        //
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }
}