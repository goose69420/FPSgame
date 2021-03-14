using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public bool paused = false;
    public Vector3 MakeVector3Positive( Vector3 vector)
    {
        vector.x = Mathf.Abs(vector.x);
        vector.y = Mathf.Abs(vector.y);
        vector.z = Mathf.Abs(vector.z);
        return vector;
    }

    // Update is called once per frame

}
