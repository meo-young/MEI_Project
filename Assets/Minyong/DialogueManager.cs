using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[System.Serializable]
public class CharacterSprite
{
    public string characterName;
    public Sprite sprite;
}

[System.Serializable]
public class DialogueEntry
{
    public int index;
    public string character;
    public List<string> texts;
}

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public SpriteRenderer dialogueBoxImage;
    public string dialogueFileName; // Resources 폴더에 있는 Json 파일 이름 (반드시 Resources 폴더에 있어야 함)
    public PlayerManager thePlayer;

    private Dictionary<int, DialogueEntry> dialogues = new Dictionary<int, DialogueEntry>();
    private int currentDialogueIndex;
    private int endIndex;
    private int currentTextIndex;
    public bool isTextFullyDisplayed = true;
    private bool isDialogueActive = false;

    public List<CharacterSprite> characterSpritesList;
    private Dictionary<string, Sprite> characterSprites;

    void Awake()
    {
        characterSprites = new Dictionary<string, Sprite>();
        foreach (var item in characterSpritesList)
        {
            characterSprites[item.characterName] = item.sprite;
        }
        LoadDialogues();
        dialogueBoxImage.gameObject.SetActive(false);
        dialogueText.text = "";
    }

    void Start()
    {

    }

    void LoadDialogues()
    {
        TextAsset dialogueData = Resources.Load<TextAsset>(dialogueFileName);

        if (dialogueData != null)
        {
            string dataAsJson = dialogueData.text;
            List<DialogueEntry> loadedDialogues = JsonConvert.DeserializeObject<List<DialogueEntry>>(dataAsJson);
            foreach (var dialogue in loadedDialogues)
            {
                dialogues[dialogue.index] = dialogue;
            }
        }
        else
        {
            Debug.LogError("Dialogue 파일을 찾을 수 없음: " + dialogueFileName);
        }
    }


    public void StartDialogue(int startIndex, int endIndex)
    {

        if (dialogues.ContainsKey(startIndex) && dialogues.ContainsKey(endIndex))
        {
            currentDialogueIndex = startIndex;
            this.endIndex = endIndex;
            currentTextIndex = 0;
            dialogueBoxImage.gameObject.SetActive(true);
            var dialogueEntry = dialogues[currentDialogueIndex];
            UpdateDialogueBox(dialogueEntry.character, dialogueEntry.texts[currentTextIndex]);
            ShowDialogue(dialogueEntry);
            isDialogueActive = true;
            thePlayer.notMove = true;
        }
        else
        {
            Debug.LogError($"Dialogue 범위를 찾을 수 없음: {startIndex} ~ {endIndex}");
        }
    }

    void ShowDialogue(DialogueEntry dialogueEntry)
    {
        if (currentTextIndex < dialogueEntry.texts.Count)
        {
            isTextFullyDisplayed = false;
            StartCoroutine(ShowTextOneByOne(dialogueEntry.texts[currentTextIndex], 0.02f));
        }
    }

    void UpdateDialogueBox(string character, string text)
    {
        dialogueText.text = text;
        if (characterSprites.TryGetValue(character, out Sprite characterSprite))
        {
            dialogueBoxImage.sprite = characterSprite;
        }
    }

    IEnumerator ShowTextOneByOne(string text, float delay)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(delay);
        }
        isTextFullyDisplayed = true;
    }

    public void EndDialogue()
    {
        dialogueBoxImage.gameObject.SetActive(false);
        dialogueText.text = "";
        isDialogueActive = false;
        thePlayer.notMove = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isDialogueActive)
        {
            if (!isTextFullyDisplayed)
            {
                StopAllCoroutines();
                dialogueText.text = dialogues[currentDialogueIndex].texts[currentTextIndex];
                isTextFullyDisplayed = true;
            }
            else if (currentTextIndex < dialogues[currentDialogueIndex].texts.Count - 1)
            {
                currentTextIndex++;
                ShowDialogue(dialogues[currentDialogueIndex]);
            }
            else if (currentDialogueIndex < endIndex)
            {
                currentDialogueIndex++;
                if (dialogues.TryGetValue(currentDialogueIndex, out DialogueEntry nextDialogue))
                {
                    currentTextIndex = 0;
                    UpdateDialogueBox(nextDialogue.character, nextDialogue.texts[currentTextIndex]);
                    ShowDialogue(nextDialogue);
                }
                else
                {
                    EndDialogue();
                }
            }
            else
            {
                EndDialogue();
            }
        }
    }
}
