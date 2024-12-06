using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject pauseUI;
    public GameObject howUI;
    public GameObject winUI;
    public GameObject loseUI;

    private List<GameObject> uiList;

    void Start()
    {
        uiList = new List<GameObject> { gameUI, pauseUI, howUI, winUI, loseUI };
        LoadUI(gameUI);
    }

    public void LoadUI(GameObject uiToLoad)
    {
        foreach (GameObject go in uiList)
        {
            if(go != null)
            {
                go.SetActive(go == uiToLoad);
            }
        }
    }
    public void LoadGameUI()
    {
        LoadUI(gameUI);
    }
    public void LoadPauseUI()
    {
        LoadUI(pauseUI);
    }
    public void LoadHowUI()
    {
        LoadUI(howUI);
    }
    public void LoadWinUI()
    {
        LoadUI(winUI);
    }
    public void LoadLoseUI()
    {
        LoadUI(loseUI);
    }
}


/// <summary>
/// class used for utility GUI methods
/// </summary>
public static class GUIUtilitys
{
    public static void FadeOutSprite(SpriteRenderer sprite, float time, Action callback = null)
    {

        //using fancy await utils wait for provided time
        Timer t = new(time, sprite.GetInstanceID().ToString() + UnityEngine.Random.Range(0, 9999999));
        t.Countdown(true);
        t.OnEnd(() =>
        {
            //ran once sprite has been faded
            if (callback != null)
                callback(); //callback to invoker script
            t.Destroy();

        });

        t.OnUpdate((elapsed) =>
        {
            //runs every update frame, allowing us to use the elapsed time as t.
            Color spriteColor = sprite.color;
            sprite.color = new Color(spriteColor.r, spriteColor.b, spriteColor.g, elapsed / time);
        });
        t.StartTimer();
    }

    public static void FadeOutSprite(Image image, float time, Action callback = null)
    {

        //using fancy await utils wait for provided time
        Timer t = new(time, image.sprite.GetInstanceID().ToString() + UnityEngine.Random.Range(0, 9999999));
        t.Countdown(true);
        t.OnEnd(() =>
        {
            //ran once sprite has been faded
            if (callback != null)
                callback(); //callback to invoker script
            t.Destroy();

        });

        t.OnUpdate((elapsed) =>
        {
            //runs every update frame, allowing us to use the elapsed time as t.
            Color spriteColor = image.color;
            image.color = new Color(spriteColor.r, spriteColor.b, spriteColor.g, elapsed / time);
        });
        t.StartTimer();
    }

    public static void FadeInSprite(SpriteRenderer sprite, float time, Action callback = null)
    {

        //using fancy await utils wait for provided time
        Timer t = new(time, sprite.GetInstanceID().ToString() + UnityEngine.Random.Range(0, 9999999));
        t.Countdown(false);
        t.OnEnd(() =>
        {
            //ran once sprite has been faded
            if (callback != null)
                callback(); //callback to invoker script
            t.Destroy();
        });

        t.OnUpdate((elapsed) =>
        {
            //runs every update frame, allowing us to use the elapsed time as t.
            Color spriteColor = sprite.color;
            sprite.color = new Color(spriteColor.r, spriteColor.b, spriteColor.g, elapsed / time);
        });
        t.StartTimer();
    }

    public static void FadeInSprite(Image image, float time, Action callback = null)
    {

        //using fancy await utils wait for provided time
        Timer t = new(time, image.sprite.GetInstanceID().ToString() + UnityEngine.Random.Range(0, 9999999));
        t.Countdown(false);
        t.OnEnd(() =>
        {
            //ran once sprite has been faded
            if (callback != null)
                callback(); //callback to invoker script
            t.Destroy();
        });

        t.OnUpdate((elapsed) =>
        {
            //runs every update frame, allowing us to use the elapsed time as t.
            Color spriteColor = image.color;
            image.color = new Color(spriteColor.r, spriteColor.b, spriteColor.g, elapsed / time);
        });
        t.StartTimer();
    }

    public static void FadeOutText(TextMeshProUGUI tmp, float time, Action callback = null)
    {

        //using fancy await utils wait for provided time
        Timer t = new(time, tmp.GetInstanceID().ToString() + UnityEngine.Random.Range(0, 9999999));
        t.Countdown(true);
        t.OnEnd(() =>
        {
            //ran once sprite has been faded
            if (callback != null)
                callback(); //callback to invoker script
            t.Destroy();
        });

        t.OnUpdate((elapsed) =>
        {
            //runs every update frame, allowing us to use the elapsed time as t.
            Color spriteColor = tmp.color;
            tmp.color = new Color(spriteColor.r, spriteColor.b, spriteColor.g, elapsed / time);
        });
        t.StartTimer();
    }

    public static void FadeInText(TextMeshProUGUI tmp, float time, Action callback = null)
    {

        //using fancy await utils wait for provided time
        Timer t = new(time, tmp.GetInstanceID().ToString() + UnityEngine.Random.Range(0, 9999999));
        t.OnEnd(() =>
        {
            //ran once sprite has been faded
            if (callback != null)
                callback(); //callback to invoker script
            t.Destroy();
        });

        t.OnUpdate((elapsed) =>
        {
            //runs every update frame, allowing us to use the elapsed time as t.
            Color spriteColor = tmp.color;
            tmp.color = new Color(spriteColor.r, spriteColor.b, spriteColor.g, elapsed / time);
        });
        t.StartTimer();
    }
}