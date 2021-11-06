using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;

public class CardDisplay : MonoBehaviour
{
    public GameObject artworkImage;
    public GameObject cardFace;
    public GameObject cardBack;
    public float duration;
    private Sequence s;
    private bool turned;

    // Start is called before the first frame update
    void Start()
    {
        turned = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator getCard()
    {
        yield return StartCoroutine(turnOff());
        yield return StartCoroutine(GetTexture());
        yield return StartCoroutine(turnOn());
    }

    public IEnumerator turnOn()
    {
        if (!turned)
        {
            Tween myTween = cardBack.transform.DORotate(new Vector3(0, 90, 0), duration);
            yield return myTween.WaitForCompletion();

            myTween = cardFace.transform.DORotate(new Vector3(0, 0, 0), duration);
            yield return myTween.WaitForCompletion();

            turned = true;
        }
    }

    public IEnumerator turnOff()
    {
        if (turned)
        {
            Tween myTween = cardFace.transform.DORotate(new Vector3(0, 90, 0), duration);
            yield return myTween.WaitForCompletion();

            myTween = cardBack.transform.DORotate(new Vector3(0, 0, 0), duration);
            yield return myTween.WaitForCompletion();

            turned = false;
        }
    }

    public IEnumerator GetTexture()
    {
        float imWidth = artworkImage.GetComponent<RectTransform>().rect.width;
        float imHeight = artworkImage.GetComponent<RectTransform>().rect.height;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://picsum.photos/seed/" + Random.Range(0, 1000) + "/" + imWidth + "/" + imHeight);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            artworkImage.GetComponent<Image>().sprite = Sprite.Create(tex as Texture2D, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}
