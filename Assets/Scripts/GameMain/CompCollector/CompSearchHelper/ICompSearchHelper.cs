namespace Fuse
{
    /// <summary>
    /// ��������ӿ�
    /// </summary>
    public interface ICompSearchHelper
    {
        /// <summary>����������</summary>
        string CustomName();

        /// <summary>�Ƿ�ƥ��</summary>
        bool IsAccord(string inputStr, string targetStr);
    }
}
