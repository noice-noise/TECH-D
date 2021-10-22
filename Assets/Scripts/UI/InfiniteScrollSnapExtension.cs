using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(ScrollSnap))]
public class InfiniteScrollSnapExtension : MonoBehaviour {
    public ScrollSnap scrollSnap;

    private int pages;
    private ScrollRect scrollRect;
    private Transform listContainterTransform;

    public Button NextButton;
    public Button PrevButton;

    private void Awake() {

        scrollSnap = GetComponent<ScrollSnap>();
        scrollRect = GetComponent<ScrollRect>();

        listContainterTransform = scrollRect.content;
        pages = listContainterTransform.childCount;

        NextButton.onClick.AddListener(delegate { NextScreen(); });
        PrevButton.onClick.AddListener(delegate { PrevScreen(); });

    }

    private void PrevScreen() {
        if (scrollSnap.CurrentPage() == 0) {
            scrollSnap.ChangePage(MaxPageNumber());
        } else {
            scrollSnap.PreviousScreen();
        }
    }

    private void NextScreen() {
        if (scrollSnap.CurrentPage() == MaxPageNumber()) {
            scrollSnap.ChangePage(0);
        } else {
            scrollSnap.NextScreen();
        }
    }

    public int MaxPageNumber() {
        return pages - 1;
    }
}
