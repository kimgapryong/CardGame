﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using Object = UnityEngine.Object;

public class ResourceManager
{
	// 실제 로드한 리소스.
	Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

	// 비동기 리소스 진행 상황.
	Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();
	public int HandlesCount = 0;
	#region 리소스
	public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
	{
		// 캐시 확인.
		if (_resources.TryGetValue(key, out Object resource))
		{
			callback?.Invoke(resource as T);
			return;
		}

		// 로딩은 시작했지만 완료되지 않았다면, 콜백만 추가.
		if (_handles.ContainsKey(key))
		{
			_handles[key].Completed += (op) => { callback?.Invoke(op.Result as T); };
			return;
		}

		// 리소스 비동기 로딩 시작.
		_handles.Add(key, Addressables.LoadAssetAsync<T>(key));
		HandlesCount++;
		_handles[key].Completed += (op) => 
		{
			_resources.Add(key, op.Result as UnityEngine.Object);
			callback?.Invoke(op.Result as T);
			HandlesCount--;
		};
	}

	public void Release(string key)
	{
		if (_resources.TryGetValue(key, out Object resource) == false)
			return;

		_resources.Remove(key);

		if (_handles.TryGetValue(key, out AsyncOperationHandle handle))
			Addressables.Release(handle);

		_handles.Remove(key);
	}

	public void Clear()
	{
		_resources.Clear();

		foreach (var handle in _handles.Values)
			Addressables.Release(handle);

		_handles.Clear();
	}
	#endregion

	#region 프리팹
	public void Instantiate(string key, Transform parent = null, Action<GameObject> callback = null)
	{
		LoadAsync<GameObject>(key, (prefab) =>
		{
			GameObject go = GameObject.Instantiate(prefab, parent);
			go.name = prefab.name;
			go.transform.localPosition = prefab.transform.position;
			callback?.Invoke(go);
		});

		//Addressables.InstantiateAsync(key, parent).Completed += (go) => 
		//{ 
		//	onInstantiate?.Invoke(go.Result); 
		//};
	}

	public void Destroy(GameObject go, float seconds = 0.0f)
	{
		Object.Destroy(go, seconds);

		//if (seconds == 0.0f)
		//	Addressables.ReleaseInstance(go);
		//else
		//	Managers.Instance.StartCoroutine(CoDestroyAfter(go, seconds));
	}

	IEnumerator CoDestroyAfter(GameObject go, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Addressables.ReleaseInstance(go);
	}
    #endregion

    public void LoadSprite(string spriteKey, Action<Sprite> callback)
    {
        if (_resources.TryGetValue(spriteKey, out var cachedResource))
        {
            callback?.Invoke(cachedResource as Sprite);
            return;
        }

        if (_handles.ContainsKey(spriteKey))
        {
            _handles[spriteKey].Completed += (op) =>
            {
                var sprite = op.Result as Sprite;
                if (sprite != null)
                {
                    _resources[spriteKey] = sprite;
                    callback?.Invoke(sprite);
                }
            };
            return;
        }

        var handle = Addressables.LoadAssetAsync<Sprite>(spriteKey);
        _handles[spriteKey] = handle;

        handle.Completed += (op) =>
        {
            var sprite = op.Result;
            if (sprite == null)
            {
                Debug.LogError($"[Addressables] Sprite '{spriteKey}' 로드 실패");
                return;
            }

            _resources[spriteKey] = sprite;
            callback?.Invoke(sprite);
        };
    }
    public void InstantiateSprite(string name, string spriteKey, Transform parent = null, Action<GameObject> callback = null)
    {
        LoadSprite(spriteKey, (sp) =>
        {
            GameObject newObj = new GameObject(name);
            var renderer = newObj.AddComponent<SpriteRenderer>();
            renderer.sprite = sp;

            if (parent != null)
                newObj.transform.SetParent(parent);

            callback?.Invoke(newObj);
        });
    }


}