using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Fuse;

public class ExampleActor : MonoBehaviour {
    public UniversalButton inputMove;
    public SkillCanceller skillCanceller;
    public UniversalButton[] skillButtons;

    public SkillSetting[] skillSettings;
    public float[] cooldowns;

    public bool lerpStopping = false;
    public Transform dirMarker;
    public float moveSpeed;

    public MessageBox msg;

    protected virtual void Start() {
        Application.targetFrameRate = 60;

        cooldowns = new float[3];

        for (int i = 0; i < skillButtons.Length; i++) {
            skillButtons[i].SetActiveState(true);
            skillButtons[i].SetText("");
            skillButtons[i].onPointerDown.AddListener(OnSkillButtonPressed);
            skillButtons[i].onDrag.AddListener(OnSkillButtonDragged);
            skillButtons[i].onActivateSkill.AddListener(OnActivateSkill);
            skillButtons[i].onCancelSkill.AddListener(OnCancelSkill);

            skillSettings[i].skillMarker.transform.position = this.transform.position;
            skillSettings[i].skillMarker.SetActive(false);

            cooldowns[i] = 0f;

        }
    }

    protected Vector3 cachedInput;
    protected virtual void Update() {
        if (inputMove.isFingerDown) {
            cachedInput = inputMove.directionXZ;
            transform.forward = cachedInput;
        } else {
            if (lerpStopping) {
                cachedInput = Vector3.Lerp(cachedInput, Vector3.zero, moveSpeed * Time.deltaTime);
            } else {
                cachedInput = Vector3.zero;
            }
        }

        transform.Translate(cachedInput * moveSpeed * Time.deltaTime, Space.World);
        dirMarker.position = transform.position + cachedInput;
        msg.UpdatePosition(transform.position);

        if (skillCanceller.isAnyFingerDown) {
            for (int i = 0; i < skillButtons.Length; i++) {
                skillSettings[i].skillMarker.transform.position = GetSkillMarkerPosition(i);
            }
        }

        this.UpdateCooldown();
    }

    protected virtual void UpdateCooldown() {
        for (int i = 0; i < cooldowns.Length; i++) {
            if (cooldowns[i] > 0f) {
                cooldowns[i] -= Time.deltaTime;
                if (cooldowns[i] < 1f) {
                    skillButtons[i].SetText(cooldowns[i].ToString("F1"));
                } else {
                    skillButtons[i].SetText("" + (int)cooldowns[i]);
                }


                if (skillButtons[i].state == UniversalButton.ButtonState.Active) {
                    skillButtons[i].SetActiveState(false);
                }
            } else {
                if (skillButtons[i].state == UniversalButton.ButtonState.Inactive) {
                    skillButtons[i].SetText("");
                    skillButtons[i].SetActiveState(true);
                }
            }
        }
    }

    protected virtual void OnSkillButtonPressed(int i) {
        skillSettings[i].skillMarker.SetActive(true);
        this.UpdateSkillMarkersState(i);
    }

    protected virtual void OnSkillButtonDragged(int i) {
        this.UpdateSkillMarkersState(i);
    }

    protected virtual void UpdateSkillMarkersState(int i) {
        if (skillCanceller.state == UniversalButton.ButtonState.Pressed) {
            skillSettings[i].SetMarkerCanCastSkill(false);
        } else {
            skillSettings[i].SetMarkerCanCastSkill(true);
        }
    }

    protected virtual void OnActivateSkill(int i) {
        skillSettings[i].SpawnSkillAt(skillSettings[i].skillMarker.transform.position);
        skillSettings[i].skillMarker.SetActive(false);
        skillSettings[i].skillMarker.transform.position = this.transform.position;
        cooldowns[i] = skillSettings[i].cooldown;
        this.skillButtons[i].directionXZ = Vector3.zero;

        msg.PopText("Activated skill " + i);
    }

    protected virtual void OnCancelSkill(int i) {
        skillSettings[i].skillMarker.SetActive(false);
        skillSettings[i].skillMarker.transform.position = this.transform.position;
        this.skillButtons[i].directionXZ = Vector3.zero;

        msg.PopText("Canceled skill " + i);
    }

    protected Vector3 GetSkillMarkerPosition(int i) {
        return this.transform.position +
            skillButtons[i].directionXZ * skillSettings[i].range;
    }
    public void ResetPosition() {
        this.transform.position = Vector3.up;
    }

    [Serializable]
    public class SkillSetting {
        public GameObject skillPrefab;
        public float rotationSpeed;
        public float startingSize;
        public float sizeDecaySpeed;
        public float range;
        public GameObject skillMarker;
        public Material markerActivateSkillTrue;
        public Material markerActivateSkillFalse;
        public float cooldown;

        protected MeshRenderer renderer;

        protected SkillRotatingBox skill;

        public void SetMarkerCanCastSkill(bool can) {
            if (renderer == null) {
                renderer = skillMarker.GetComponent<MeshRenderer>();
            }

            if (can) {
                renderer.material = markerActivateSkillTrue;
            } else {
                renderer.material = markerActivateSkillFalse;
            }
        }

        public void SpawnSkillAt(Vector3 position) {
            skill = this.skillPrefab.GetComponent<SkillRotatingBox>();
            skill.size0 = this.startingSize;
            skill.sizeDecaySpeed = this.sizeDecaySpeed;

            Instantiate(skillPrefab, position, Quaternion.identity);

        }
    }
}
