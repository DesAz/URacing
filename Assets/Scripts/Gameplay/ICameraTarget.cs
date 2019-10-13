using UnityEngine;

namespace URacing
{
    public interface ICameraTarget
    {
        Transform Follow { get; }
        bool ForceShow { get; }
    }
}