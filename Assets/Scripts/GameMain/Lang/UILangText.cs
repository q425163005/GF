using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    public class UILangText : MonoBehaviour
    {
        [SerializeField]
        private string key;

        private Text textTarget;
        void Start()
        {
            textTarget = gameObject.GetComponent<Text>();
            if (GameEntry.Localization != null)
                textTarget.text = GameEntry.Localization.GetString(key); //查找Key
        }

        public string Key {
            get { return key; }
            set {
                if (key != value)
                {
                    key = value;
                    Value = GameEntry.Localization.GetString(key);
                    if (textTarget != null) //重新查找值
                        textTarget.text = Value;
                }
            }
        }

        public void Refresh()
        {
            Value = Value = GameEntry.Localization.GetString(key); 
        }

        public string Value {
            get {
                if (textTarget == null)
                    return string.Empty;
                return textTarget.text;
            }
            set {
                if (textTarget == null)
                {
                    textTarget = gameObject.GetComponent<Text>();
                }
                textTarget.text = value;
            }
        }
    }
}
