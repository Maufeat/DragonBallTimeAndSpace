using System;
using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public void StartShake()
    {
        this.startPos = base.transform.localPosition;
        base.StartCoroutine(this.DoShake());
    }

    private IEnumerator DoShake()
    {
        for (int num = 0; num < 15; num++)
        {
            float offsetX = (float)UnityEngine.Random.Range(-1 * this.range, this.range);
            float offsetY = (float)UnityEngine.Random.Range(-1 * this.range, this.range);
            base.transform.localPosition = this.startPos + new Vector3(offsetX, offsetY, 0f);
            yield return new WaitForSeconds(0.05f);
        }
        base.transform.localPosition = this.startPos;
        yield break;
    }

    private Vector3 startPos;

    private int range = 7;
}
