using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeModel1
{
    [SerializeField]
    private GameObject treeModel;
}

public class Tree1 : MonoBehaviour
{
    private TreeModel1 model;

    private MeshCollider collider = new MeshCollider();
    private Rigidbody rigidBody = new Rigidbody();

    public Tree1(Vector3 i_position, Vector3 i_scale)
    {
        gameObject.transform.position = i_position;
        gameObject.transform.localScale = i_scale;
    }

    private void Awake()
    {
        rigidBody.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
    }

}

public class FlyWeightObjects : MonoBehaviour
{

}

