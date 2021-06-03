using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IShape {}
[Serializable]
public class Cube : IShape
{
    public Vector3 size;
}
[ExecuteInEditMode]
public class BuildingBlocks : MonoBehaviour
{
    [SerializeReference]
    public List<IShape> inventory;
   void OnEnable()
    {
        if (inventory == null)
        {
            inventory = new List<IShape>()
            {
                new Cube() {size = new Vector3(1.0f, 1.0f, 1.0f)}
            };
            Debug.Log("Created list");
        }
        else
            Debug.Log("Read list");
    }
}
