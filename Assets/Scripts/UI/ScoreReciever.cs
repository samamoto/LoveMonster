﻿using UnityEngine.EventSystems;
using UnityEngine;

public interface ScoreReciever : IEventSystemHandler {

	// 受信側メッセージ作成用	
	void ReceivePlayerScore(int id, int score);

}
