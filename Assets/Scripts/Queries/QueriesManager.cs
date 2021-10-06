using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QueriesManager : MonoBehaviour {

    public QueriesData queriesData;

    public Transform referenceButton;
    private string referenceButtonString;

    public GameObject queryCategoriesPrefab;
    public GameObject queryPrefab;
    public GameObject sectionPrefab;

    public Transform queriesPanel;

    private Transform categoryList;
    private Transform queriesList;

    private void Awake() {
        InitList();
        InitCategory();
    }

    private void Start() {
        SelectFirstCategory();
    }

    private void SelectFirstCategory() {
        var firstButton = categoryList.transform.GetChild(0).GetComponent<Button>();
        firstButton.Select();
        firstButton.onClick.Invoke();
    }

    private void InitList() {
        categoryList = FindListFrom("Categories");

        if (categoryList == null) {
            Debug.LogError("List in categoryPane not found.");
            return;
        }

        queriesList = FindListFrom("Queries");
        
        if (categoryList == null) {
            Debug.LogError("List in categoryPane not found.");
            return;
        }
    }

    private Transform FindListFrom(string source) {
        return queriesPanel.Find("Frame").transform.Find(source).transform.Find("List");
    }

    private void InitCategory() {

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

    public void OnCategoryClicked(Transform categoryButton) {
        ClearChildren(queriesList);
        CreateQueriesFrom(categoryButton);
        SetReferenceButton(referenceButtonString);
    }

    private void SetReferenceButton(string referenceButtonString) {
        // Setting the text of RoomButtons are special cases, thus you need to manual set the text for this case
        var refText = referenceButton.Find("Text").GetComponent<TextMeshProUGUI>();
        if (referenceButtonString == null || referenceButtonString.Trim().Length == 0) {
            refText.SetText("None");
            return;
        }

        refText.SetText(referenceButtonString);
        referenceButton.GetComponent<RoomButton>().UpdateRoomReferenceWith(referenceButtonString);
    }

    private void CreateQueriesFrom(Transform source) {
        Category catSource = GetCategoryEquivalent(source);

        Category category = queriesData.categories.Find(c => c.title == catSource.title);

        if (category == null) {
            Debug.LogError("Category not found.");
            return;
        }

        referenceButtonString = category.referenceTransformString;

        foreach (var query in category.queries) {
            CreateQuery(query, queriesList, queryPrefab);
        }
    }

    private Category GetCategoryEquivalent(Transform source) {
        Category category = new Category();
        category.title = source.name;
        return category;
    }

    private void CreateQuery(Query query, Transform targetParentList, GameObject prefab) {

        GameObject newQuery = Instantiate(prefab, targetParentList, false);
        newQuery.transform.SetParent(targetParentList);
        newQuery.transform.name = query.title;
        newQuery.transform
            .Find("Title")
            .GetComponent<TextMeshProUGUI>()
            .SetText(query.title);

        foreach (var section in query.sections) {
            CreateSection(section, newQuery.transform, sectionPrefab);
        }
    }

    private void CreateSection(Section section, Transform targetParentList, GameObject prefab) {

        GameObject newSection = Instantiate(prefab, targetParentList, false);
        newSection.transform.SetParent(targetParentList);
        newSection.transform.name = section.title;
        newSection.transform
            .Find("Title")
            .GetComponent<TextMeshProUGUI>()
            .SetText(section.title);
        
        newSection.transform
            .Find("Contents")
            .GetComponent<TextMeshProUGUI>()
            .SetText(section.contents);
    }

    private void ClearChildren(Transform parent) {
        if (parent.transform.childCount == 0) {
            return;
        }

        foreach (Transform child in parent.transform) {
            Destroy(child.gameObject);
        }
    }
}
