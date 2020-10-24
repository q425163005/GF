using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fuse.Hotfix
{
    public class Lang
    {
        public string key { get; set; }

        public Lang(string _key) => key = _key;

        /// <summary>
        /// о╣мЁЭ]хосОят
        /// </summary>
        public string Value => Mgr.Lang.Get(key);
    }
}