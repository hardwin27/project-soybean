using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PoolingSystem
{
    public interface IPoolable
    {
        public IObjectPool<GameObject> Pool { set; get; }
    }
}
