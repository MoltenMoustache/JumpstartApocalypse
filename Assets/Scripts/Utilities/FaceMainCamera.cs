using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMainCamera : MonoBehaviour
{
    private void Update()
    {
        transform.forward = Camera.main.transform.position;
    }
}
