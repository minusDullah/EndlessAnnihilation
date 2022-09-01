// Created by Ronis Vision. All rights reserved
// 03.07.2021.

using System;
using RVHonorAI.Combat;
using RVModules.RVCommonGameLibrary.Effects;
using RVModules.RVCommonGameLibrary.Pooling;
using RVModules.RVLoadBalancer;
using RVModules.RVUtilities;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Collections;

namespace RVHonorAI
{
    public class Honor_Projectile : MonoBehaviour, IProjectile
    {
        #region Public methods

        public void Shoot(Component _shooter, float _damage, Vector3 pos, Vector3 dir, ITarget _target, bool _dmgEnemyOnly)
        {
            dmgEnemyOnly = _dmgEnemyOnly;
            target = _target;
            transform.position = pos;
            var transformRotation = Quaternion.LookRotation(dir);
            transform.rotation = transformRotation;
            if (!guidedMissile) rigidbody.velocity = dir.normalized * speed;
            shooter = _shooter;
            //damage = _damage;
            if (guidedMissile) LB.Register(this, GuidedMissile, loadBalancerConfig);
            //if (lifeTime > 0) LB.Register(this, LifeTimeTimer, loadBalancerConfig);
        }

        #endregion

        #region Fields

        private static LoadBalancerConfig loadBalancerConfig = new LoadBalancerConfig(LoadBalancerType.EveryXFrames, 0) { dontRemoveWhenEmpty = true };

        [Header("Stats")]
        [SerializeField]
        private float damage;

        [SerializeField]
        private DamageType damageType;

        [SerializeField]
        private float speed = 20f;

        [Tooltip("Will move toward target and always hit it")]
        [SerializeField]
        private bool guidedMissile;

        [Header("Lifetime")]
        [Tooltip("If enabled the bullet destroys on impact")]
        public bool destroyOnImpact = false;

        [Tooltip("Minimum time after impact that the bullet is destroyed")]
        public float minDestroyTime;

        [Tooltip("Maximum time after impact that the bullet is destroyed")]
        public float maxDestroyTime;

        [Tooltip("How long before destroying bullet in seconds")]
        [SerializeField]
        private float lifeTime = 10;

        [Header("References")]
        [SerializeField]
        private Component shooter;

        private new Rigidbody rigidbody;
        private ITarget target;
        private new Transform transform;
        private bool dmgEnemyOnly;
        private float actualLifeTime;

        /// <summary>
        /// Collision can be null !!
        /// </summary>
        internal Action<Transform, Collision> onHit;

        #endregion

        #region Properties

        Action IPoolable.OnSpawn { get; set; }

        Action IPoolable.OnDespawn { get; set; }

        /// <summary>
        /// Collision can be null !!
        /// </summary>
        public Action<Transform, Collision> OnHit
        {
            get => onHit;
            set => onHit = value;
        }

        #endregion

        #region Not public methods

        private void Awake()
        {
            transform = base.transform;
            rigidbody = GetComponent<Rigidbody>();

            if (rigidbody == null)
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
            }

            ((IPoolable)this).OnSpawn += () =>
            {
                gameObject.SetActive(true);
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.isKinematic = guidedMissile;
                actualLifeTime = 0;
            };

            ((IPoolable)this).OnDespawn += () =>
            {
                LB.Unregister(this);

                Destroy(gameObject);
                onHit = null;
            };
        }

        private void Start()
        {
            damage = gameObject.GetComponentInParent<WeaponBehaviour>().GetBulletDamage();

            gameObject.transform.SetParent(null);

            var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(),
                GetComponent<Collider>());

            StartCoroutine(DestroyAfter());
        }

        
        private void LifeTimeTimer(float _dt)
        {
            actualLifeTime += UnityTime.DeltaTime;
            if (actualLifeTime > lifeTime) ((IPoolable)this).OnDespawn?.Invoke();
        }

        protected virtual void OnDestroy() => LB.Unregister(this);

        private void OnCollisionEnter(Collision other) => Hit(other.transform);



        private void Hit(Transform other, Collision _collision = null)
        {
            if (!destroyOnImpact)
            {
                StartCoroutine(DestroyTimer());
            }
            //Otherwise, destroy bullet on impact
            else
            {
                ((IPoolable)this).OnDespawn?.Invoke();
            }

            onHit?.Invoke(other, _collision);

            if (dmgEnemyOnly && shooter != null)
            {
                var otherRel = other.transform.GetComponent<IRelationship>();
                var ourRel = shooter.GetComponent<IRelationship>();
                if (otherRel != null && ourRel != null)
                    if (otherRel.IsEnemy(ourRel))
                    {
                        ((IPoolable)this).OnDespawn?.Invoke();
                        return;
                    }
            }

            var damageable = other.transform.GetComponent<IDamageable>();
            if (damageable == null)
            {
                ((IPoolable)this).OnDespawn?.Invoke();
                return;
            }

            damageable.ReceiveDamage(damage, shooter, damageType);
            ((IPoolable)this).OnDespawn?.Invoke();
        }

        private void GuidedMissile(float dt)
        {
            if (target as Object == null)
            {
                ((IPoolable)this).OnDespawn?.Invoke();
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, target.AimTransform.position, speed * UnityTime.DeltaTime);
            if (Vector3.Distance(transform.position, target.AimTransform.position) < .1f) Hit(target.Transform);
        }

        private IEnumerator DestroyTimer()
        {
            //Wait random time based on min and max values
            yield return new WaitForSeconds
                (Random.Range(minDestroyTime, maxDestroyTime));
            //Destroy bullet object
            ((IPoolable)this).OnDespawn?.Invoke();
        }

        private IEnumerator DestroyAfter()
        {
            //Wait for set amount of time
            yield return new WaitForSeconds(lifeTime);
            //Destroy bullet object
            Destroy(gameObject);
        }
        #endregion
    }
}