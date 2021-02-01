namespace Fuse.Editor
{
    /// <summary>
    /// 组件搜索接口
    /// </summary>
    public interface ICompSearchHelper
    {
        /// <summary>搜索类型名</summary>
        string CustomName();

        /// <summary>是否匹配</summary>
        bool IsAccord(string inputStr, string targetStr);
    }
}
