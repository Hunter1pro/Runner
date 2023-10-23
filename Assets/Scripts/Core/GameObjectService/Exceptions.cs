using System;

namespace GameObjectService
{
    public class SystemNotFoundException : Exception
    {
        public string SystemType { get; }

        public SystemNotFoundException(string systemType)
        {
            SystemType = systemType;
        }

        public override string Message => $"System not found {SystemType}";
    }
}
