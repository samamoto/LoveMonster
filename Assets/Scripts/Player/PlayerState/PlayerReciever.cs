using UnityEngine.EventSystems;

public interface PlayerReciever : IEventSystemHandler {

	// 受信側メッセージ作成用
	void PlayAction(string name);
	void PlayAction(string name, Controller.Button btn);

}
