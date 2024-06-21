using System;
using UnityEngine;

public class DramaAction : SingletonForMono<DramaAction>
{
    public void PlayAction(Transform tran, DramaAction.ActionType type)
    {
        this.target = tran;
        this.originPosition = this.target.localPosition;
        if (type == DramaAction.ActionType.Shake)
        {
            this.StartShake();
        }
    }

    private void StartShake()
    {
        this.enableShake = true;
        this.temptime = 0f;
        this.temptime1 = 0f;
    }

    private void OnShake()
    {
        if (this.enableShake)
        {
            this.temptime += Time.deltaTime;
            if (this.temptime > this.temptime1)
            {
                this.temptime1 += this.shakeInterval;
                this.target.transform.localPosition = this.originPosition + new Vector3(UnityEngine.Random.Range(-this.shakerange, this.shakerange), UnityEngine.Random.Range(-this.shakerange, this.shakerange), 0f);
            }
            if (this.temptime >= this.shakeDuration)
            {
                this.target.transform.localPosition = this.originPosition;
                this.enableShake = false;
            }
        }
    }

    private void Update()
    {
        this.OnShake();
    }

    public Transform rt;

    private Transform target;

    private Vector3 originPosition;

    private bool enableShake;

    private float shakerange = 5f;

    private float shakeInterval = 0.05f;

    private float shakeDuration = 0.5f;

    private float temptime;

    private float temptime1;

    public enum ActionType
    {
        Shake = 1
    }
}
