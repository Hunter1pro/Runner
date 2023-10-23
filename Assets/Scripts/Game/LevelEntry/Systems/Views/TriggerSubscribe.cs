using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Level.Views
{
    public class TriggerSubscribe : MonoBehaviour
    {
        private Dictionary<string, Action<GameObject>> _actions = new ();
        
        private void OnTriggerEnter(Collider collider)
        {
            foreach (var action in _actions)
            {
                if (collider.CompareTag(action.Key))
                {
                    action.Value?.Invoke(collider.gameObject);
                }
            }
        }
        
        public void Subscribe(string trigger, Action<GameObject> action)
        {
            _actions.Add(trigger, action);
        }

        public void Subscribe(Action<GameObject> action, params string[] triggers)
        {
            foreach(var trigger in triggers)
            {
                _actions.Add(trigger, action);
            }
        }
    }
}

