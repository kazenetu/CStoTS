using System.Collections.Generic;

namespace CStoTS.Domain.Model.Mode
{
  /// <summary>
  /// ルートパス
  /// </summary>
  public class RootPath : ValueObject
  {
    /// <summary>
    /// パス情報
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="path">ルートパス</param>
    private RootPath(string path)
    {
      Value = path;
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
    /// <param name="path">パス情報</param>
    /// <returns>インスタンス</returns>
    public static RootPath Create(string path)
    {
      return new RootPath(path);
    }

  }
}
