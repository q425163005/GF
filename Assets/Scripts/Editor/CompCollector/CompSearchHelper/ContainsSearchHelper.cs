namespace Fuse.Editor
{
    public class ContainsSearchHelper : ICompSearchHelper
    {
        public string CustomName()
        {
            return "包含搜索";
        }

        public bool IsAccord(string inputStr, string targetStr)
        {
            return targetStr.ToLower().Contains(inputStr.ToLower());
        }
    }
}