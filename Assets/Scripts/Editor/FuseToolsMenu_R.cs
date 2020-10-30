using UnityEditor;

namespace Fuse.Editor
{
    public class FuseToolsMenu_R
    {
        [MenuItem("GameObject/���Զ��������/UIPolygonImage", false, 10)]
        public static void CreateUIPolygonImage()
        {
            UIPolygonImageHelper.CreateUIPolygonImage();
        }

        [MenuItem("GameObject/���Զ��������/UICurvedText", false, 11)]
        public static void CreateUICurvedText()
        {
            var goRoot = Selection.activeGameObject;
            if (goRoot == null)
                return;
            if (goRoot.GetComponent<UICurvedText>()==null)
            {
                goRoot.AddComponent<UICurvedText>();
            }
        }
    }
}