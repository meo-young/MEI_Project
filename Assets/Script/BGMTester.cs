using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMTester : MonoBehaviour
{
    BGMManager theBGM;
    public int playMusicTrack;

    void Start()
    {
        theBGM = FindObjectOfType<BGMManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        theBGM.Play(playMusicTrack);
        StartCoroutine(SoundPlayer());
        this.transform.position = new Vector3(500, 500, 500);
    }

    IEnumerator SoundPlayer()
    {
        yield return new WaitForSeconds(11f);
        theBGM.Play(0);
        this.gameObject.SetActive(false);
    }
}
