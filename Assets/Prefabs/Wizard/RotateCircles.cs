using System.Collections;
using UnityEngine;

public class RotateCircles : MonoBehaviour
{
    public bool isInside;

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    public IEnumerator Rotate()
    {
        while (true)
        {
            float rotationSpeed = isInside ? 50f : -50f;
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }
}

