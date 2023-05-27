using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

    public Image loadingImage; // �ε�ȭ�鿡 ����� �̹���
    public float imageDuration = 0.5f;
    public float fadeDuration = 0.1f; // ���̵� ��/�ƿ��� �ɸ��� �ð�

    private void Start()
    {
        // �̹����� �������ϰ� �����մϴ�.
        loadingImage.color = new Color(1f, 1f, 1f, 1f);

        // �ε�ȭ���� ǥ���մϴ�.
        StartCoroutine(DisplayLoadingImage());
    }

    private IEnumerator DisplayLoadingImage()
    {
        // �̹����� �����մϴ�.
        yield return new WaitForSeconds(imageDuration);

        // ���̵� �ƿ� �ڷ�ƾ�� �����մϴ�.
        yield return StartCoroutine(FadeOutAndLoadGameScene());
    }

    private IEnumerator FadeOutAndLoadGameScene()
    {
        // �̹����� ���������� �����ϰ� ����ϴ�.
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            loadingImage.color = new Color(1f, 1f, 1f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �̹����� ������ �����ϰ� ����ϴ�.
        loadingImage.color = new Color(1f, 1f, 1f, 0f);      
    }
}
