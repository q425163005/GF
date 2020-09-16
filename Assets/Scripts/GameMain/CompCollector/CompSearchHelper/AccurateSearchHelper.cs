namespace Fuse
{
    public class AccurateSearchHelper : ICompSearchHelper
    {
        public string CustomName()
        {
            return "¾«×¼ËÑË÷";
        }

        public bool IsAccord(string inputStr, string targetStr)
        {
            return targetStr.ToLower().Equals(inputStr.ToLower());
        }
    }
}