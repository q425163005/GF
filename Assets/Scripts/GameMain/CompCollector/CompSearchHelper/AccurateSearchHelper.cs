namespace Fuse
{
    public class AccurateSearchHelper : ICompSearchHelper
    {
        public string CustomName()
        {
            return "��׼����";
        }

        public bool IsAccord(string inputStr, string targetStr)
        {
            return targetStr.ToLower().Equals(inputStr.ToLower());
        }
    }
}