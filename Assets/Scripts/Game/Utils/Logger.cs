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
        
            Debug.Log($"{value}\nversion: {Application.version}");
        }
        
        public void LogError(string value)
        {
            if (_logEnabled is false) return;
        
            Debug.LogError($"{value}\nversion: {Application.version}");
        }
    }

    public interface ICustomLogger
    {
        void Log(string value);
        void LogError(string value);
    }
}    


