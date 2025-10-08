using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

using SingletonSystem;

namespace PoolingSystem
{
    public class ObjectPoolController : Singleton<ObjectPoolController>
    {
        [SerializeField] private List<EntityPooler> _entityPoolers = new List<EntityPooler>();

        private void Awake()
        {
            InitiatePool();
        }

        public void InitiatePool()
        {
            foreach (EntityPooler entityPooler in _entityPoolers)
            {
                entityPooler.InitiateEntityPool(
                        CreateObjectToPool,
                        GetFromPool,
                        ReleaseToPool,
                        DestroyPooledObject
                    );
            }
        }

        protected GameObject CreateObjectToPool(GameObject entityToPool, IObjectPool<GameObject> entityPool)
        {
            GameObject entityInstance = Instantiate(entityToPool);
            if (entityInstance.TryGetComponent(out IPoolable poolable))
            {
                poolable.Pool = entityPool;
            }
            return entityInstance;
        }

        protected void GetFromPool(GameObject entity)
        {
            entity.SetActive(true);
        }

        protected void ReleaseToPool(GameObject entity)
        {
            entity.SetActive(false);
        }

        protected void DestroyPooledObject(GameObject entity)
        {
            Destroy(entity);
        }

        public GameObject Get(GameObject entityToGet)
        {
            EntityPooler entityPoolerUsed = null;
            foreach (EntityPooler entityPooler in _entityPoolers)
            {
                if (entityPooler.EntityToPool.gameObject == entityToGet.gameObject)
                {
                    entityPoolerUsed = entityPooler;
                    break;
                }
            }

            if (entityPoolerUsed == null)
            {
                return null;
            }
            else
            {
                if (entityPoolerUsed.Pool == null)
                {
                    return null;
                }
                else
                {
                    return entityPoolerUsed.Pool.Get();
                }
            }
        }
    }
}
