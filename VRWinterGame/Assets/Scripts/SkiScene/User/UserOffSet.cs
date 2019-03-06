using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserOffSet : MonoBehaviour
{

    // The target of the user 
    public GameObject target;
    public float xOffset = -0.2f, yOffset = -0.2f, zOffset = -0.7f;

    private Vector3 totalOffSet;

    // Start is called before the first frame update
    void Start()
    {
        totalOffSet = new Vector3(xOffset, yOffset, zOffset);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + totalOffSet;
    }
}
