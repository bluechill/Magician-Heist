using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMonoBehavior : MonoBehaviour {
	private static IDictionary<int, IList<GameObject>> layersCache = new Dictionary<int, IList<GameObject>> ();

	public static IList<GameObject> FindGameObjectsWithLayer(int layer)
	{
		IList<GameObject> cache;
		if (!layersCache.TryGetValue(layer, out cache)) // Might be a bitwise mask
		{
			cache = new List<GameObject>();

			foreach (KeyValuePair<int, IList<GameObject>> pair in layersCache) {
				if ((pair.Key & layer) != 0)
				{
					foreach (GameObject go in pair.Value)
						cache.Add (go);
				}
			}

			return cache;
		}

		return layersCache [layer];
	}

	public static GameObject FindWithLayer(int layer)
	{
		var cache = layersCache[layer];
		if (cache != null && cache.Count > 0)
			return cache[0];
		else
			return null;
	}

	protected void Awake() {
		IList<GameObject> cache;
		if (!layersCache.TryGetValue(gameObject.layer, out cache))
		{
			cache = new List<GameObject>();
			layersCache.Add(gameObject.layer, cache);
		}

		cache.Add(gameObject);
	}

	protected void OnDestroy()
	{
		layersCache[gameObject.layer].Remove(gameObject);
	}
}
