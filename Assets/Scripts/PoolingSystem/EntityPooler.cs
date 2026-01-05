using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PoolingSystem
{
    [System.Serializable]
    public class EntityPooler
    {
        [SerializeField] private GameObject _entityToPool;
        [SerializeField] private int _defaultCapacity;
        [SerializeField] private int _maxSize;
        [SerializeField] private List<GameObject> _createdEntities = new List<GameObject>();

        private IObjectPool<GameObject> _pool;

        public GameObject EntityToPool { get => _entityToPool; }
        public IObjectPool<GameObject> Pool { get => _pool; }
        public List<GameObject> CreatedEntities { get => _createdEntities; }

        public EntityPooler(GameObject entityToPool, int defaultCapacity, int maxSize)
        {
            _entityToPool = entityToPool;
            _defaultCapacity = defaultCapacity;
            _maxSize = maxSize;
            _createdEntities = new List<GameObject>();
        }

        public void InitiateEntityPool(Func<GameObject, IObjectPool<GameObject>, GameObject> createFunc, Action<GameObject> actionOnGet, Action<GameObject> actionOnRelease, Action<GameObject> actionOnDestroy)
        {
            _pool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject createdObject = createFunc(_entityToPool, _pool);
                    CreatedEntities.Add(createdObject);
                    return createdObject;
                },
                actionOnGet,
                actionOnRelease,
                actionOnDestroy,
                false,
                _defaultCapacity,
                _maxSize
            );
        }

        public void RemovePool()
        {
            if (_pool == null)
            {
                return;
            }

            _createdEntities.RemoveAll(entity => !entity.activeSelf);
            _pool.Clear();
            if (_createdEntities.Count <= 0)
            {
                _pool = null;
            }
        }
    }
}
