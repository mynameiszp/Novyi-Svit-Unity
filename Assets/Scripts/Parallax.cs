using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform mainCam;
    [SerializeField] Transform middleBG;
    [SerializeField] Transform sideBG;
    [SerializeField] float length = 33f;

    void Update()
    {
        if (mainCam.position.x > middleBG.position.x)
        {
            sideBG.position = middleBG.position + Vector3.right * length;
        }
        if (mainCam.position.x < middleBG.position.x)
        {
            sideBG.position = middleBG.position + Vector3.left * length;
        }
        if (mainCam.position.x > sideBG.position.x || mainCam.position.x < sideBG.position.x)
        {
            Transform temp = middleBG;
            middleBG = sideBG;
            sideBG = temp;
        }
    }
}
