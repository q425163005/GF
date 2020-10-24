using Fuse.Tasks;

namespace Fuse
{
    public static class CTaskExtension
    {
        public static CTaskHandle Run(this CTask task)
        {
            return GameEntry.CTask.Manager.Run(task);
        }
    }
}
