using System;
using UnityEngine;

public class LoopRotation : MonoBehaviour
{
    private void Update()
    {
        base.transform.Rotate(new Vector3(0f, 0f, 1f), (!this.clockwise) ? this.speed : (this.speed * -1f));
    }

    public float speed = 0.3f;

    public bool clockwise = true;
}
