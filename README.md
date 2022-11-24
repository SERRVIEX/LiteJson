
# LiteJson

![Version](https://img.shields.io/badge/Version-v1.0.0-brightgreen.svg)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/SERRVIEX/SimpleRecyclerCollection/blob/main/LICENSE) 
[![Contact](https://img.shields.io/badge/LinkedIn-blue.svg?logo=LinkedIn)](https://www.linkedin.com/in/sergiu-ciornii-466395220/)

## Requirements
[![Unity 2020+](https://img.shields.io/badge/unity-2020+-black.svg?style=flat&logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)
![.NET 4.x Scripting Runtime](https://img.shields.io/badge/.NET-4.x-blueviolet.svg?style=flat&cacheSeconds=2592000)

## Description

This is a tiny version of SimpleJSON by Bunny83 with some modifications.
[![Author](https://img.shields.io/badge/SimpleJSON-gray.svg?logo=Github)](https://github.com/Bunny83/SimpleJSON)

Which news?
- Removed ``JSONLazyCreator`` and now instead of creating it, an error (with path to root to know from where it is appeared) will be thrown when trying to get a non-existent key.
- Added reference to the parent node.
- Now you can access the ``Key`` of the node, if the node's parent is of ``JSONObject`` type then it will be return a ``string`` key, and if the node's parent is of ``JSONObject`` type then the ``int`` index will be returned.
- Removed the static instance for ``JSONNull`` because the parent reference was added.
- If you call a function or method that is not implemented in the specified type, an error will be thrown.
Here you can see all the available types: ``JSONNode``, ``JSONObject``, ``JSONArray``, ``JSONString``, ``JSONNumber``, ``JSONBool``, ``JSONNull``.

## How to use?

```csharp
JSONNode json = new JSONObject();
// You can set a key directly.
json["obj1"] = 1337;
// You can add a key through the method.
json.Add("obj2", 1448);
Debug.Log(json.Contains("obj2"));

// You can perform a foreach loop.
foreach (var item in json)
	Debug.Log($"{item.Key}: {item.Value}");

json["array1"] = new JSONArray();
JSONArray array1 = json["array1"].AsArray();
array1.Add(1337);
array1.Add("2");

array1[0] += 1448;

JSONNumber arrayFirstNumber = array1[0].AsDouble();
Debug.Log(json["array1"].IndexOf(arrayFirstNumber));

// You can perform a for loop.
for (int i = 0; i < array1.Count; i++)
	Debug.Log(array1[i]);
	
// And so on...
```

## License
[MIT](https://choosealicense.com/licenses/mit/)
