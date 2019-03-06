using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainsGenerator : MonoBehaviour
{
    const float x = -439.011f, y = 23.75f, z = 253.5f;

    public GameObject mountainsPrefab;
    private GameObject mountains;

    void Awake()
    {
        GenerateMountains(x, y, z);
    }


    // Update is called once per frame
    void GenerateMountains(float x, float y, float z)
    {
        mountains = Instantiate(mountainsPrefab) as GameObject;
        mountains.transform.position = new Vector3(x, y, z);
    }
}
