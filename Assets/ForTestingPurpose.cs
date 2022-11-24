using UnityEngine;

using LiteJson;

public class ForTestingPurpose : MonoBehaviour
{
    public TextAsset _json;

    [ContextMenu("Call Test")]
    public void TestMethod()
    {
        Debug.Log(JSON.Parse(_json.text));
    }
}
