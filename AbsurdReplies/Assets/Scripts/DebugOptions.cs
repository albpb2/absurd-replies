using SPStudios.Tools;
using UnityEngine;

namespace AbsurdReplies
{
    public class DebugOptions : MonoSingleton<DebugOptions>
    {
        [SerializeField] private bool forceUnknownCategory;

        public bool ForceUnknownCategory => forceUnknownCategory;
    }
}