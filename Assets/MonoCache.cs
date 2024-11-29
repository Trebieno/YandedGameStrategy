using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace CodeBase.SystemGame
{
    public class MonoCache : MonoBehaviour
    {
        public static List<MonoCache> allUpdate = new List<MonoCache>(10001);
        public static List<MonoCache> allFixedUpdate= new List<MonoCache>(10001);


        private void OnEnable() => allUpdate.Add(this);
        private void OnDisable() => allUpdate.Remove(this);
        private void OnDestroy() => allUpdate.Remove(this);

        protected void AddFixedUpdate() => allFixedUpdate.Add(this);
        protected void RemoveFixedUpdate() => allFixedUpdate.Remove(this);

        public void Tick() => OnTick();

        public void FixedTick() => OnFixedTick();

        public virtual void OnTick() { }
        public virtual void OnFixedTick() { }
    }
}
