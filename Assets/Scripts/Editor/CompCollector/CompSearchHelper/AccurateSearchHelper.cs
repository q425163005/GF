namespace Fuse.Editor
{
    public class AccurateSearchHelper : ICompSearchHelper
    {
        public string CustomName()
        {
            return "精准搜索";
        }

        public bool IsAccord(string inputStr, string targetStr)
        {
            return targetStr.ToLower().Equals(inputStr.ToLower());
        }
    }
}
