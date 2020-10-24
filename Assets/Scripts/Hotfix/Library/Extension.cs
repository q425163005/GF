using Fuse.Tasks;

namespace Fuse.Hotfix
{
    public static class Extension
    {
        public static CTaskHandle Run(this CTask task)
        {
            return GameEntry.CTask.Manager.Run(task);
        }

        
    }
}
