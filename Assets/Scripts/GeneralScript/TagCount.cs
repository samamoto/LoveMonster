using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーン上に存在するタグの数を数える
/// </summary>
static public class TagCount {

	/**
	 * @fn
	 * シーン上に存在するタグの数を数える
	 * @brief タグのカウンタ
	 * @param タグ名
	 * @return 検出したタグ数
	 */
	static public int CountTag(string tagname) {
		GameObject[] tagObjects = GameObject.FindGameObjectsWithTag(tagname);

		if (tagObjects.Length == 0) {
			Debug.Log(tagname + "タグがついたオブジェクトはありません");
			return 0;
		}
		return tagObjects.Length;
	}

}
