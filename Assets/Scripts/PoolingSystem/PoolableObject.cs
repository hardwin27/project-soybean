using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PoolingSystem
{
    public class PoolableObject : MonoBehaviour
    {
        public IObjectPool<GameObject> Pool { get; set; }
    }
}
