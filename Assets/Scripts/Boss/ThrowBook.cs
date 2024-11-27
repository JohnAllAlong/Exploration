using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBook : MonoBehaviour
{
    [SerializeField] GameObject bookPrefab;
    
    public void ThrowBookTrigger() {
        Instantiate(bookPrefab, transform.position, Quaternion.identity);
    }
}
