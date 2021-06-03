using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    
 
interface INode{  }
 
[Serializable]
class RootNode : INode
{
   [SerializeReference] public INode left;
   [SerializeReference] public INode right;
}

class UnityObject : UnityEngine.Object
{
   public string word;
}
 
[Serializable]
class SubNode : RootNode
{
   [SerializeReference] INode parent;
}
 
[Serializable]
class LeafNode : INode
{
   [SerializeReference] public INode parent;
}
 
 
class SomeThing : MonoBehaviour
{
    [SerializeReference]
    public List<RootNode>  m_Trees;

    [FormerlySerializedAs("uo")] public UnityObject unityObject;
}
 
}