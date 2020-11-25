using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRotatingBox : MonoBehaviour
{
    public float rotationSpeed;

    public float size0;
    public float sizeDecaySpeed;

    protected float boxHeight;

    private void Start()
    {
        this.transform.localScale = Vector3.one * size0;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.localScale = transform.localScale - Vector3.one * sizeDecaySpeed * Time.deltaTime;

        boxHeight = transform.localScale.y / 2f;

        transform.position = new Vector3(
            transform.position.x,
            boxHeight,
            transform.position.z
            );

        if (transform.localScale.x < 0.01f)
        {
            Destroy(this.gameObject);
        }
    }
}
