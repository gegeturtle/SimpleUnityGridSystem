using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private Grid grid;
    void Start()
    {
        grid = new(50, 50, gameObject);
    }

}
