using UnityEditor;

namespace Fuse.Editor
{
    public class FuseToolsMenu_R
    {
        [MenuItem("GameObject/★自定义组件★/UIPolygonImage", false, 10)]
        public static void CreateUIPolygonImage()
        {
            UIPolygonImageHelper.CreateUIPolygonImage();
        }

        [MenuItem("GameObject/★自定义组件★/UICurvedText", false, 11)]
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