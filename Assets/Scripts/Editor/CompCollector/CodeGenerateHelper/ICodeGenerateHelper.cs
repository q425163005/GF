namespace Fuse.Editor
{
    /// <summary>
    /// 自动生成绑定代码接口
    /// </summary>
    public interface ICodeGenerateHelper
    {
        /// <summary>生成绑定代码</summary>
        void GenerateCode(CompCollector collector);
    }
}
