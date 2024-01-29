using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    public static DialogManager instance;
    public Text text;
    public SpriteRenderer rendererSprite;
    public SpriteRenderer rendererDialogWindow;
    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogWindows;
    private int count; //��ȭ ���� ��Ȳ ī��Ʈ
    public Animator animSprite;
    public Animator animDialogWindow;
    //public string typeSound;
    //public string enterSound;
    private AudioManager theAudio;
    public bool talking = false;
    private bool keyActivated = false;
    private bool onlyText = false;
    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion Singleton
    void Start()
    {
        count = 0;
        text.text = "";
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogWindows = new List<Sprite>();
        theAudio = FindObjectOfType<AudioManager>();
    }
    public void ShowText(string[] _sentences)
    {
        onlyText = true;
        talking = true;
        for (int i = 0; i < _sentences.Length; i++)
        {
            listSentences.Add(_sentences[i]);
        }
        StartCoroutine(StartTextCoroutine());
    }

    public void ShowDialog(Dialog dialog)
    {
        talking = true;
        onlyText = false;
        for (int i = 0; i < dialog.sentences.Length; i++)
        {
            listSentences.Add(dialog.sentences[i]);
            listSprites.Add(dialog.sprites[i]);
            listDialogWindows.Add(dialog.dialogWindows[i]);
        }
        animSprite.SetBool("Appear", true);
        animDialogWindow.SetBool("Appear", true);
        StartCoroutine(StartDialogCoroutine());
    }

    public void ExitDialog()
    {
        text.text = "";
        count = 0;
        listSentences.Clear();
        listSprites.Clear();
        listDialogWindows.Clear();
        animSprite.SetBool("Appear", false);
        animDialogWindow.SetBool("Appear", false);
        talking = false;
    }

    IEnumerator StartTextCoroutine()
    {
        keyActivated = true;
        // �ؽ�Ʈ ��� �ڵ�
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i]; //�� ������ 1���ھ� �߰�
            /*if (i % 7 == 1)
            {
                theAudio.Play(typeSound);
            }*/
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator StartDialogCoroutine()
    {
        if (count > 0)
        {
            // ��ȭâ�� �ٸ� ��� �� ĳ����, ��ȭâ ����
            if (listDialogWindows[count] != listDialogWindows[count - 1])
            {
                animSprite.SetBool("Change", true);
                animDialogWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                rendererDialogWindow.GetComponent<SpriteRenderer>().sprite = listDialogWindows[count];
                rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
                animDialogWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
            else
            {
                // ĳ���� �̹����� �ٸ� ��� �� ĳ���� ����
                if (listSprites[count] != listSprites[count - 1])
                {
                    animSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count]; //rendererSprite.sprite = listSprites[count]�� ����
                    animSprite.SetBool("Change", false);
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else // ù �̹����� ������ ��ü�� �߻�
        {
            rendererDialogWindow.GetComponent<SpriteRenderer>().sprite = listDialogWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }
        keyActivated = true;
        // �ؽ�Ʈ ��� �ڵ�
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i]; //�� ������ 1���ھ� �߰�
            /*if (i % 7 == 1)
            {
                theAudio.Play(typeSound);
            }*/
            yield return new WaitForSeconds(0.01f);
        }
    }
    void Update()
    {
        if (talking && keyActivated)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            { //ZŰ ������ ���� ��ȭ�� �Ѿ
                keyActivated = false;
                count++;
                text.text = "";
                //theAudio.Play(enterSound);
                if (count == listSentences.Count) //��ȭ�� �����ٸ� �ڷ�ƾ ����
                {
                    StopAllCoroutines();
                    ExitDialog();
                }
                else
                {
                    StopAllCoroutines();
                    if (onlyText)
                        StartCoroutine(StartTextCoroutine());
                    else
                        StartCoroutine(StartDialogCoroutine());
                }
            }
        }
    }
}
