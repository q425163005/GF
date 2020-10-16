using System.Collections.Generic;
using UnityEngine;

namespace Fuse
{
    public class HotfixProcedureMgr : MonoBehaviour
    {
        [HideInInspector] [SerializeField] public string       nowHotfixProcedure { get; set; }
        [HideInInspector] [SerializeField] public List<string> allHotfixProcedure = new List<string>();
    }
}