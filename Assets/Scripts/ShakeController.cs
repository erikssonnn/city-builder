using UnityEngine;
using System.Collections;

public class ShakeController : MonoBehaviour {
    private readonly float shakeRotationTangent = 2.0f;
    private Vector3 startPos = Vector3.zero;
    private Vector3 desiredPos = Vector3.zero;
    private Vector3 startRot = Vector3.zero;
    private Vector3 desiredRot = Vector3.zero;

    public static ShakeController Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Shake(Transform t, float amount, float length) {
        if (t == null) {
            throw new System.Exception("shake transform is null on " + name);
        }

        startPos = t.localPosition;
        startRot = t.localEulerAngles;
        StartCoroutine(ShakeCoroutine(t, amount, length));
    }

    private IEnumerator ShakeCoroutine(Transform t, float amount, float length) {
        float time = 0.0f;

        float step = Time.deltaTime * (amount * 2.0f);
        float rotStep = Time.deltaTime * (amount * shakeRotationTangent);

        while (time < length) {
            float up = Random.Range(-1.0f, 1.0f) * (amount * 0.1f);
            float right = Random.Range(-1.0f, 1.0f) * (amount * 0.1f);

            desiredPos = new Vector3(right, up, 0);

            t.localPosition =
                Vector3.MoveTowards(t.localPosition, desiredPos * 0.2f + startPos, step);

            desiredRot = new Vector3(right, up, 0) * shakeRotationTangent;
            Quaternion dest = Quaternion.Euler(startRot + desiredRot);
            t.localRotation = Quaternion.Slerp(t.localRotation, dest, rotStep);

            time += Time.deltaTime;
            yield return null;
        }

        t.localEulerAngles = startRot;
        StartCoroutine(ReturnHome(t, step));
    }

    private IEnumerator ReturnHome(Transform t, float step) {
        float time = 0.0f;

        while (time < step) {
            time += Time.deltaTime;
            float dist = Vector3.Distance(t.localPosition, startPos);

            if (!(dist > 0.001f)) continue;
            t.localPosition = Vector3.MoveTowards(t.localPosition, startPos, step * 0.1f);
            yield return null;
        }

        t.localPosition = startPos;
    }
}