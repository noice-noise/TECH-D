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

    private void CreateCategory(Category categoryInfo, Transform targetParentList, GameObject prefab) {

        GameObject newCategory = Instantiate(prefab, targetParentList, false);
        newCategory.transform.SetParent(targetParentList);
        newCategory.transform.name = categoryInfo.title;
        newCategory.transform
            .Find("Text")
            .GetComponent<TextMeshProUGUI>()
            .SetText(categoryInfo.title);

        newCategory.GetComponent<Button>().onClick.AddListener( 
            delegate { OnCategoryClicked(newCategory.transform); });
    }

    public void OnCategoryClicked(Transform category) {
        Debug.Log(category.name);
        DisplayCategory(category);
    }

    private void DisplayCategory(Transform category) {
        
    }
}
