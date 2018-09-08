using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    private IDictionary<long, ResourceUnit> units;

    public void Initialize() {
        LoadUnitData();
    }

    public void LoadUnitData() {
        TextAsset Json = Resources.Load<TextAsset>("json/ResourceUnits.json");
        string JsonStr = Json.text;

        var JsonData = JSON.Parse(JsonStr);

        units = new Dictionary<long, ResourceUnit>();
        ResourceUnit resUnit = null;
        foreach (var jNode in JsonData.Childs) {
            resUnit = new ResourceUnit(jNode);
            if (resUnit == null)
                continue;

            units.Add(resUnit.id, resUnit);
        }
    }
}
