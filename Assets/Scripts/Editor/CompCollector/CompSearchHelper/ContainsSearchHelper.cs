namespace Fuse.Editor
{
    public class ContainsSearchHelper : ICompSearchHelper
    {
        public string CustomName()
        {
            return "°üº¬ËÑË÷";
        }

        public bool IsAccord(string inputStr, string targetStr)
        {
            return targetStr.ToLower().Contains(inputStr.ToLower());
        }
    }
}