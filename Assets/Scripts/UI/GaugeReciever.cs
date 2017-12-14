using UnityEngine.EventSystems;
using UnityEngine;

public interface GaugeReciever : IEventSystemHandler {

	// 受信側メッセージ作成用	
	void ReceivePlayerGauge(float incRate );

}
