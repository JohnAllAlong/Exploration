using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToTarget : MonoBehaviour
{
    private Transform mantisPos;

    private void OnEnable() {
        mantisPos = transform.parent;
    }

    
}
