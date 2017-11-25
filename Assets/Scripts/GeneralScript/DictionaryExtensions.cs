﻿//  DictionaryExtensions.cs
//  http://kan-kikuchi.hatenablog.com/entry/DictionaryExtensions
//  Created by kan.kikuchi on 2015.08.24.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Dictionaryの拡張クラス
/// </summary>
public static class DictionaryExtensions {

	/// <summary>
	/// 値を取得、keyがなければデフォルト値を設定し、デフォルト値を取得
	/// </summary>
	public static TValue TryGetValueEx<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue) {

		//Dictionary自体がnullの場合はインスタンス作成
		if (source == null) {
			source = new Dictionary<TKey, TValue>();
		}

		//keyが存在しない場合はデフォルト値を設定
		if (!source.ContainsKey(key)) {
			source[key] = defaultValue;
		}

		return source[key];
	}
	/// <summary>
	/// keyがnullならfalseを返す
	/// </summary>
	public static bool ContainsKeyNullable<TKey, TValue>(
		this Dictionary<TKey, TValue> self, TKey key
	) {
		if (key == null) {
			return false;
		}
		return self.ContainsKey(key);
	}
}