using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject target;

    public Vector3 offset = new Vector3(-0.181193113f,11.9458961f,43.670475f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offset;
    }

    public void SetTarget(GameObject pl) {
        target = pl;
    }
}
