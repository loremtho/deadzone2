using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingManager : MonoBehaviour
{
    public static TypingManager instance;

    [Header("Times for each character")]
    public float timeForCharacter; //0.08�� �⺻.

    [Header("Times for each character when speed up")]
    public float timeForCharacter_Fast; //0.03�� ���� �ؽ�Ʈ.

    float characterTime; // ���� ����Ǵ� ���ڿ� �ӵ�.

    //�ӽ� ����Ǵ� ��ȭ ������Ʈ�� ��ȭ����.
    string[] dialogsSave;
    TextMeshProUGUI tmpSave;

    public static bool isDialogEnd;

    bool isTypingEnd = false; //Ÿ������ �����°�?
    int dialogNumber = 0; //��ȭ ���� ����.

    float timer; //���������� ���ư��� �ð� Ÿ�̸�

    public Button SkipButton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        timer = timeForCharacter;
        characterTime = timeForCharacter;
    }

    public void Typing(string[] dialogs, TextMeshProUGUI textObj)
    {
        isDialogEnd = false;
        dialogsSave = dialogs;
        tmpSave = textObj;
        if (dialogNumber < dialogs.Length)
        {
            char[] chars = dialogs[dialogNumber].ToCharArray(); //�޾ƿ� ���̾� �α׸� char�� ��ȯ.
            StartCoroutine(Typer(chars, textObj)); //���۷����� �Ѱܺ��°� �׽�Ʈ �غ���.
        }
        else
        {
            //������ �������Ƿ� �ٸ� ������ ���� �غ�... ���̾�α� �ʱ�ȭ, ���̾�α� ���̺�� Ƽ���� ���̺� �ʱ�ȭ
            tmpSave.text = "";
            isDialogEnd = true; // ȣ���ڴ� ���̾˷α� ���带 ���� ���� ������ �������ָ� ��.
            dialogsSave = null;
            tmpSave = null;
            dialogNumber = 0;
        }
    }

    public void GetInputDown()
    {
        //��ǲ�� �������� -> �ؽ�Ʈ�� �������̸� ������ ����ǰ� �ؽ�Ʈ�� �����Ǿ������� ���� �ؽ�Ʈ�� �Ѿ.
        //�׸��� ��ǲ�� ĵ���Ǹ� �ٽ� ���ڿ� �ӵ��� ����ȭ ���Ѿ���.
        if (dialogsSave != null)
        {
            if (isTypingEnd)
            {
                tmpSave.text = ""; //����ִ� ���� �Ѱܼ� �ʱ�ȭ. 
                Typing(dialogsSave, tmpSave);
            }
            else
            {
                characterTime = timeForCharacter_Fast; //���� ���� �ѱ�.
            }
        }
    }

    public void GetInputUp()
    {
        //��ǲ�� ��������.
        if (dialogsSave != null)
        {
            characterTime = timeForCharacter;
        }
    }

    IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    {
        int currentChar = 0;
        int charLength = chars.Length;
        isTypingEnd = false;

        while (currentChar < charLength)
        {
            if (timer >= 0)
            {
                yield return null;
                timer -= Time.deltaTime;
            }
            else
            {
                textObj.text += chars[currentChar].ToString();
                currentChar++;
                timer = characterTime; //Ÿ�̸� �ʱ�ȭ
            }
        }
        if (currentChar >= charLength)
        {
            isTypingEnd = true;
            dialogNumber++;
            // ��ư Ȱ��ȭ
            if (dialogNumber >= dialogsSave.Length)
            {
                SkipButton.gameObject.SetActive(true);
            }
            yield break;
        }
    }
}