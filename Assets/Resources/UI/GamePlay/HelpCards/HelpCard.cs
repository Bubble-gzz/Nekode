using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Video;
using TMPro;
public class HelpCard : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_FontAsset Font_CH, Font_EN;
    public string title_EN, title_CH;
    [TextArea]
    public string description_EN, description_CH;
    public Vector2 CardSize = new Vector2(1000, 850);
    GameObject VideoResource;
    CanvasGroup canvasGroup;
    TMP_Text title, description;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        VideoResource = transform.Find("VideoResource")?.gameObject;
        title = transform.Find("Title").GetComponent<TMP_Text>();
        description = transform.Find("Description").GetComponent<TMP_Text>();
        if (VideoResource) {
            VideoPlayer[] videoPlayers = VideoResource.GetComponentsInChildren<VideoPlayer>();
            foreach(var videoPlayer in videoPlayers)
                videoPlayer.frame = 0;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Disappear()
    {
        canvasGroup.DOFade(0, 0.2f);
        if (VideoResource) {
            VideoPlayer[] videoPlayers = VideoResource.GetComponentsInChildren<VideoPlayer>();
            foreach(var videoPlayer in videoPlayers)
                videoPlayer.Pause();
        }
    }
    public void Appear()
    {
        if (Settings.language == "EN") {
            title.text = title_EN;
            title.font = Font_EN;
            description.text = description_EN;
            description.font = Font_EN;
        }
        else {
            title.text = title_CH;
            title.font = Font_CH;
            description.text = description_CH;
            description.font = Font_CH;
        }
        if (VideoResource) {
            VideoPlayer[] videoPlayers = VideoResource.GetComponentsInChildren<VideoPlayer>();
            foreach(var videoPlayer in videoPlayers)
                if (!videoPlayer.isPlaying) videoPlayer.Play();
        }
        canvasGroup.DOFade(1, 0.2f);
    }
}
