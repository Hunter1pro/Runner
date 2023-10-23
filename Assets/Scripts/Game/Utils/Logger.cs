using System;
using UnityEngine;

namespace Game.Utils
{
    public class Logger : ICustomLogger
    {
        private bool _logEnabled = true;

        public void Log(string value)
        {
            if (_logEnabled is false) return;
        
            Debug.Log($"version: {Application.version}\n{value}");
        }
        
        public void LogError(string value)
        {
            if (_logEnabled is false) return;
        
            Debug.LogError($"version: {Application.version}\n{value}");
        }
    }

    public interface ICustomLogger
    {
        void Log(string value);
        void LogError(string value);
    }
}    


