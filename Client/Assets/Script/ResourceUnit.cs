using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ResourceUnit : ResourceBase {

    public GameObject prefabUnit;

    public ResourceUnit(JSONNode jNode) :base(jNode){
        prefabUnit = Utility.LoadResource<GameObject>(jNode["Prefab"]);
    }
}
