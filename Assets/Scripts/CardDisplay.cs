using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using DG.Tweening;

public class CardDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject artworkImage;
    [SerializeField]
    private GameObject cardFace;
    [SerializeField]
    private GameObject cardBack;
    [SerializeField]
    private float duration;
    [SerializeField]
    private bool turned;
    private const string downloadUrl = "https://picsum.photos/seed/";

    void Start()
    {
        Croupier.AddCard(this);
    }

    private void OnDestroy()
    {
        Croupier.RemoveCard(this);
    }

    public IEnumerator TurnOn()
    {
        if (!turned)
        {
            turned = true;

            Tween myTween = cardBack.transform.DORotate(new Vector3(0, 90, 0), duration);
            cardBack.transform.DOScale(new Vector3(1.5f, 1.5f, 1), duration);
            yield return myTween.WaitForCompletion();

            myTween = cardFace.transform.DORotate(new Vector3(0, 0, 0), duration);
            cardFace.transform.DOScale(new Vector3(1, 1, 1), duration);
            yield return myTween.WaitForCompletion();
        }
        Croupier.CardIsTurnedOn();
    }

    public IEnumerator TurnOff()
    {
        if (turned)
        {
            turned = false;

            Tween myTween = cardFace.transform.DORotate(new Vector3(0, 90, 0), duration);
            cardFace.transform.DOScale(new Vector3(1.5f, 1.5f, 1), duration);
            yield return myTween.WaitForCompletion();

            myTween = cardBack.transform.DORotate(new Vector3(0, 0, 0), duration);
            cardBack.transform.DOScale(new Vector3(1, 1, 1), duration);
            yield return myTween.WaitForCompletion();
        }
        Croupier.CardIsTurnedOff();
    }

    public IEnumerator SetTexture()
    {
        float imWidth = artworkImage.GetComponent<RectTransform>().rect.width;
        float imHeight = artworkImage.GetComponent<RectTransform>().rect.height;
        int seed = Random.Range(0, 1000);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture($"{downloadUrl}{seed}/{imWidth}/{imHeight}");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
            artworkImage.GetComponent<Image>().sprite = Sprite.Create(tex as Texture2D, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            Croupier.TextureLoaded(this);
        }
    }
}
