using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBase {

    public readonly long id;

    public ResourceBase(JSONNode jNode) {
        this.id = jNode["ID"];
    }
}
