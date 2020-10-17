using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuse.Hotfix
{
    public class LoadConfigEventArgs : HotfixGameEventArgs
    {
        public static readonly int EventId = typeof(HotfixTestEventArgs).GetHashCode();

        public override int Id { get; }

        public override void Clear()
        {
        }
    }
}