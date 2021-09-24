using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QueriesManager : MonoBehaviour {
    public QueriesData queriesData;
    public GameObject queryCategoriesPrefab;

    public Transform categoryPane;

    private void Awake() {
        InitCategory();
    }

    private void InitCategory() {
        var categoryList = categoryPane.Find("List");
        if (categoryList == null) {
            Debug.LogError("List in categoryPane not found.");
            return;
        }

        foreach (Category category in queriesData.categories) {
            CreateCategory(category, categoryList, queryCategoriesPrefab);
        }
    }

    private void CreateCategory(Category categoryInfo, Transform targetParent, GameObject prefab) {

        GameObject newButton = Instantiate(prefab, targetParent, false);
        newButton.transform.SetParent(targetParent);
        newButton.transform.name = categoryInfo.title;
        newButton.transform
            .Find("Text")
            .GetComponent<TextMeshProUGUI>()
            .SetText(categoryInfo.title);


        newButton.GetComponent<Button>().onClick.AddListener( 
            delegate { OnCategoryClicked(newButton.transform); });
    }

    public void OnCategoryClicked(Transform source) {
        Debug.Log(source.name);
    }
}
