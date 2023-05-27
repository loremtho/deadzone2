using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{

    public Image loadingImage; // 로딩화면에 사용할 이미지
    public float imageDuration = 0.5f;
    public float fadeDuration = 0.1f; // 페이드 인/아웃에 걸리는 시간

    private void Start()
    {
        // 이미지를 불투명하게 설정합니다.
        loadingImage.color = new Color(1f, 1f, 1f, 1f);

        // 로딩화면을 표시합니다.
        StartCoroutine(DisplayLoadingImage());
    }

    private IEnumerator DisplayLoadingImage()
    {
        // 이미지를 유지합니다.
        yield return new WaitForSeconds(imageDuration);

        // 페이드 아웃 코루틴을 시작합니다.
        yield return StartCoroutine(FadeOutAndLoadGameScene());
    }

    private IEnumerator FadeOutAndLoadGameScene()
    {
        // 이미지를 점차적으로 투명하게 만듭니다.
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            loadingImage.color = new Color(1f, 1f, 1f, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 이미지를 완전히 투명하게 만듭니다.
        loadingImage.color = new Color(1f, 1f, 1f, 0f);      
    }
}
