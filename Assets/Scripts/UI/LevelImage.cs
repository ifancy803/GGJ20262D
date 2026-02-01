using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelImage : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(开始离开());
    }

    IEnumerator 开始离开()
    {
        yield return new WaitForSeconds(1f);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 300f, transform.position.z), 0.2f).SetEase(Ease.OutBounce);
        transform.GetComponent<CanvasGroup>().DOFade(0, 1f);
    }
}
