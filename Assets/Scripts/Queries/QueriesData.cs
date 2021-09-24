using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QueriesData", menuName = "TECH-D/QueriesData", order = 0)]
public class QueriesData : ScriptableObject {
    public List<Category> categories;
}

[System.Serializable]
public class Category {
    public string title;
    public List<Query> queries;
}

[System.Serializable]
public class Query {
    public string title;
    public List<Section> sections;
}

[System.Serializable]
public class Section {
    public string title;
    [TextArea]
    public string contents;
}