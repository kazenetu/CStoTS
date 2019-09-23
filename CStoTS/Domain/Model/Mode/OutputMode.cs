using System.Collections.Generic;

namespace CStoTS.Domain.Model.Mode
{
  /// <summary>
  /// 出力モード
  /// </summary>
  public class OutputMode : ValueObject
  {
    /// <summary>
    /// 出力列挙型
    /// </summary>
    public enum Mode
    {
      All,
      WithoutMethod,
    };

    /// <summary>
    /// 出力情報
    /// </summary>
    public Mode Value { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="mode">出力設定</param>
    private OutputMode(Mode mode)
    {
      Value = mode;
    }

    /// <summary>
    /// 反復子
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return Value;
    }

    /// <summary>
    /// インスタンス取得
    /// </summary>
    /// <param name="mode">出力設定</param>
    /// <returns>インスタンス</returns>
    public static OutputMode Create(Mode mode)
    {
      return new OutputMode(mode);
    }
  }
}
