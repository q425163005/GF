using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fuse
{
    public class SkillCanceller : MonoBehaviour {
        public UniversalButton[] associateBtns;
        public bool isAnyFingerDown;
        public List<Image> imgs;

        public List<Text> texts;

        [HideInInspector]
        public Color colorActive;
        public Color colorPressed;

        public UniversalButton.ButtonState state;
        protected Image img;

        protected virtual void Start() {
            state = UniversalButton.ButtonState.Active;
            img = GetComponent<Image>();
            colorActive = img.color;

        }

        protected virtual void Update() {
            isAnyFingerDown = false;
            for (int i = 0; i < associateBtns.Length; i++) {
                isAnyFingerDown = isAnyFingerDown || associateBtns[i].isFingerDown;
            }

            foreach (Image im in imgs) {
                im.enabled = isAnyFingerDown;
            }

            foreach (Text t in texts) {
                t.enabled = isAnyFingerDown;
            }

            this.UpdateColor();
        }

        protected virtual void UpdateColor() {
            if (state == UniversalButton.ButtonState.Active) {
                img.color = colorActive;
            } else if (state == UniversalButton.ButtonState.Pressed) {
                img.color = colorPressed;
            }
        }
    }
}