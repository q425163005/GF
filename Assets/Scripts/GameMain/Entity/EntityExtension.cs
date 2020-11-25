using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Fuse
{
    public static class EntityExtension
    {
        public static void Show(this EntityComponent entityComponent, int    entityId, string path, string group,
                                int                  prioity,         object userData)
        {
            entityComponent.ShowEntity<HotfixEntityLogic>(entityId, path, group, prioity, userData);
        }
    }
}