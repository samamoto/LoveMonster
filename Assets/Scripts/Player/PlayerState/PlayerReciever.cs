using UnityEngine.EventSystems;
using UnityEngine;

public interface PlayerReciever : IEventSystemHandler {

	// 受信側メッセージ作成用
	void PlayAction(string name);
	void PlayAction(string name, Controller.Button btn);
	void PlayAction(string name, Controller.Button btn, Vector3[] move, int score);

}
