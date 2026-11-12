using System.Collections;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TutorialWindow : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public Coroutine Show()
    {
        gameObject.SetActive(true);
        return StartCoroutine(GetDisplaySequence());
    }

    private IEnumerator GetDisplaySequence()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}