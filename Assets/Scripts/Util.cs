using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Util : MonoBehaviour
{
    public static Vector3 FindAveragePosition( Component[] vectors )
    {
        Vector3 average = Vector3.zero;
        for (int i = 0; i < vectors.Length; ++i)
        {
            average += vectors[i].transform.position;
        }

        return average / vectors.Length;
    }
}
