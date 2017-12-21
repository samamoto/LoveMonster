// Const
// ----------------------------------------------------------------------------
// ゲーム内で使用する定数リスト
// ----------------------------------------------------------------------------

/// <summary>
/// 現在存在するアニメーションのステートリスト＝タグ
/// </summary>
public static class ConstAnimationStateTags {
	public const string PlayerStateDead = "Dead";
	public const string PlayerStateIdle = "Idle";
	public const string PlayerStateWalkRun = "WalkRun";
	public const string PlayerStateJump = "Jump";
	public const string PlayerStateVault = "Vault";
	public const string PlayerStateClimb = "Climb";
	public const string PlayerStateClimbJump = "ClimbJump";
	public const string PlayerStateSlider = "Slide";
	public const string PlayerStateLongSlider = "LongSlider";
	public const string PlayerStateWallRun = "WallRun";
	public const string PlayerStateKongVault = "KongVault";
	public const string PlayerStateLand = "Land";           // 受け身
	public const string PlayerStateBreakFall = "BreakFall";
	public const string PlayerStateClimbOver = "ClimbOver";
}

/// <summary>
/// プレイヤーパラメータ関係
/// </summary>

public static class ConstPlayerParameter {
	public const int PlayerMax = 4;
}

/// <summary>
/// シーンリスト
/// </summary>
public static class ConstSceneNames {
	public const string Title = "TitleScene";
	public const string Tutorial = "TutorialScene";
	public const string GameScene = "GameScene";
	public const string GameSceneBonus = "GameSceneStageBonus";
	public const string StageSelect = "StageSelectScene";
	public const string Result = "ResultScene";

	/// <summary>
	/// この後に数字を繋げて次へ(ステージ続く場合)
	/// </summary>
	public const string GameSceme = "GameScene_Stage";
}

/// <summary>
/// オブジェクトの名前
/// </summary>
public static class ConstObjectNames {
	public const string ActionBlockName = "Block";
	public const string ActionVault = ConstAnimationStateTags.PlayerStateVault + ConstObjectNames.ActionBlockName;
	public const string ActionClimb = ConstAnimationStateTags.PlayerStateClimb + ConstObjectNames.ActionBlockName;
	public const string ActionClimbJump = ConstAnimationStateTags.PlayerStateClimbJump + ConstObjectNames.ActionBlockName;
	public const string ActionSlider = ConstAnimationStateTags.PlayerStateSlider + ConstObjectNames.ActionBlockName;
	public const string ActionWallRun = ConstAnimationStateTags.PlayerStateWallRun + ConstObjectNames.ActionBlockName;
	public const string ActionLand = ConstAnimationStateTags.PlayerStateLand + ConstObjectNames.ActionBlockName;
	public const string ActionBreakFall = ConstAnimationStateTags.PlayerStateBreakFall + ConstObjectNames.ActionBlockName;
}