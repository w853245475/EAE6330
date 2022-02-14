using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TreeInstance1 : MonoBehaviour
{
    private GameObject model;

    public TreeInstance1(Vector3 i_position, Vector3 i_scale, GameObject i_tree)
    {
        model = i_tree;
        gameObject.transform.position = i_position;
        gameObject.transform.localScale = i_scale;
    }
}

public class FlyWeightObjects : MonoBehaviour
{

}

