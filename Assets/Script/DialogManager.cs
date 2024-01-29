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
    private int count; //대화 진행 상황 카운트
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
        // 텍스트 담당 코드
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i]; //각 문장을 1글자씩 추가
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
            // 대화창이 다른 경우 → 캐릭터, 대화창 변경
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
                // 캐릭터 이미지가 다른 경우 → 캐릭터 변경
                if (listSprites[count] != listSprites[count - 1])
                {
                    animSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);
                    rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count]; //rendererSprite.sprite = listSprites[count]와 동일
                    animSprite.SetBool("Change", false);
                }
                else
                {
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
        else // 첫 이미지는 무조건 교체가 발생
        {
            rendererDialogWindow.GetComponent<SpriteRenderer>().sprite = listDialogWindows[count];
            rendererSprite.GetComponent<SpriteRenderer>().sprite = listSprites[count];
        }
        keyActivated = true;
        // 텍스트 담당 코드
        for (int i = 0; i < listSentences[count].Length; i++)
        {
            text.text += listSentences[count][i]; //각 문장을 1글자씩 추가
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
            { //Z키 누르면 다음 대화로 넘어감
                keyActivated = false;
                count++;
                text.text = "";
                //theAudio.Play(enterSound);
                if (count == listSentences.Count) //대화가 끝났다면 코루틴 종료
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
