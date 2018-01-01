using UnityEngine;
using System.Collections;
using UnityEngine.UI;



/// <summary>
/// プレイヤーがマップのどの辺りにいるのかを一目で把握できるようにする
/// スタートからゴールまでの距離を取得
/// 取得した距離とプレイヤーの現在の距離を比較し、どの程度進んでいるのかをマップ上に表示
/// 
/// ランキングUIとして活用
/// 
/// </summary>
public class Map : MonoBehaviour
{

    private MainGameManager mgm;    //メインゲームマネージャー
    private PlayerManager pm;       //プレイヤーマネージャー
    private AllPlayerManager apm;   //オールプレイヤーマネージャー

    public Vector3[] GetPlayerPosition = new Vector3[4];  //プレイヤーの現在地を受け取るための値

    public float[] MapLength = new float[4];       //マップの長さ(距離)
    public float[] PlayerGoingLength = new float[4];      //プレイヤーの進んでいる距離の格納用の配列 プレイヤー1～4


    public float[] PlayerGoingLength_rank = new float[4];      //プレイヤーの進んでいる距離の格納用の配列 プレイヤー1～4 ランク用

    public int[] PlayerRank = new int[4];   //ランキング格納用

    //GameObject Minimap_pos_1p;
    //GameObject Minimap_pos_2p;
    //GameObject Minimap_pos_3p;
    //GameObject Minimap_pos_4p;

    int count = 0;    //カウント

    // Use this for initialization
    void Start()
    {
        apm = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();
        mgm = GameObject.Find("GameManager").GetComponent<MainGameManager>();

        //Minimap_pos_1p = GameObject.Find("MiniMap_1P").gameObject;      //各プレイヤーのミニマップの位置格納場所
        //Minimap_pos_2p = GameObject.Find("MiniMap_2P").gameObject;      //各プレイヤーのミニマップの位置格納場所
        //Minimap_pos_3p = GameObject.Find("MiniMap_3P").gameObject;      //各プレイヤーのミニマップの位置格納場所
        //Minimap_pos_4p = GameObject.Find("MiniMap_4P").gameObject;      //各プレイヤーのミニマップの位置格納場所
        //プレイヤー位置の初期化,人数分繰り返し
        for (count = 0; count <= 3; count++)
        {
            PlayerGoingLength[count] = 0;       //0で初期化
        }
        for (count = 0; count <= 3; count++)
        {
            GetPlayerPosition[count] = Vector3.zero;       //0で初期化
        }
		for (count = 0; count <= 3; count++)
			MapLength[count] = mgm.getStageLength(count);     //マップの長さを取得
        
        //Debug.Log(MapLength);
        //Debug.Log("マップの長さ:" + MapLength);
    }

    // Update is called once per frame
    void Update()
    {

        //まずはプレイヤーの現在地をx,y,z全て受け取る
        for (count = 0; count <= 3; count++)
        {
            GetPlayerPosition[count] = apm.getPlayerPosision(count + 1);       //
        }

        //プレイヤーの進んでいる距離のみを格納()
        for (count = 0; count <= 3; count++)
        {
            PlayerGoingLength[count] = GetPlayerPosition[count].z;       //zのみを格納
        }


        //Minimap_pos_1p.GetComponent<Slider>().value = PlayerGoingLength[0] / MapLength;       //zのみを格納
        //Minimap_pos_2p.GetComponent<Slider>().value = PlayerGoingLength[1] / MapLength;       //zのみを格納
        //Minimap_pos_3p.GetComponent<Slider>().value = PlayerGoingLength[2] / MapLength;       //zのみを格納
        //Minimap_pos_4p.GetComponent<Slider>().value = PlayerGoingLength[3] / MapLength;       //zのみを格納

        Ranking();

        

    }

    public void Ranking()
    {


        //int work_x = 0;
        //int work_y = 0;


        ////プレイヤー１の距離が現在のどの距離よりも短ければ
        //if(PlayerGoingLength[0]>=PlayerGoingLength[0]
        //    && PlayerGoingLength[1] >= PlayerGoingLength[0]
        //    && PlayerGoingLength[2] >= PlayerGoingLength[0]
        //    && PlayerGoingLength[3] >= PlayerGoingLength[0])
        //{
        //    PlayerRank[0] = 1;  //1p
        //}


        //for(int count=0;count<4;count++)
        //if (PlayerGoingLength[0] >= PlayerGoingLength[count]
        //    && PlayerGoingLength[1] >= PlayerGoingLength[count]
        //    && PlayerGoingLength[2] >= PlayerGoingLength[count]
        //    && PlayerGoingLength[3] >= PlayerGoingLength[count])
        //{
        //    PlayerRank[0] = count;  //1p
        //}




        //頭が混乱したのでいつも通りゴリ押しするわ(´・ω・｀)ｂ


        //1
        if (PlayerGoingLength[0] < PlayerGoingLength[1]
            && PlayerGoingLength[1] < PlayerGoingLength[2]
            && PlayerGoingLength[2] < PlayerGoingLength[3]
            )
        {
            PlayerRank[0] = 1;  //1p
            PlayerRank[1] = 2;  //2p
            PlayerRank[2] = 3;  //3p
            PlayerRank[3] = 4;  //4p
        }

        //2
        if (PlayerGoingLength[0] > PlayerGoingLength[1]
            && PlayerGoingLength[1] > PlayerGoingLength[2]
            && PlayerGoingLength[2] > PlayerGoingLength[3]
            )
        {
            PlayerRank[0] = 1;  //1p
            PlayerRank[1] = 2;  //2p
            PlayerRank[2] = 3;  //3p
            PlayerRank[3] = 4;  //4p
        }


        

    }
}
