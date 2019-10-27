using System.Collections.Generic;

namespace CStoTS.Domain.Model.Mode
{
  /// <summary>
  /// ファイル指定パス
  /// </summary>
  public class FilePath : ValueObject
  {
    /// <summary>
    /// ファイルパス
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// ファイルパスの設定確認
    /// </summary>
    public bool HasValue {
      get
      {
        return !string.IsNullOrEmpty(Value);
      }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    private FilePath(string filePath)
    {
      Value = filePath;
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
    /// 確認対象のファイルパスが最後に指定されているか確認
    /// </summary>
    /// <param name="filePath">確認対象のファイルパス</param>
    /// <returns>最後に指定されているか否か</returns>
    public bool EndsWith(string filePath)
    {
      if (!HasValue)
      {
        return false;
      }

      return Value.EndsWith(filePath, System.StringComparison.CurrentCulture);
    }

    /// <summary>
    /// インスタンス取得
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <returns>インスタンス</returns>
    public static FilePath Create(string filePath)
    {
      return new FilePath(filePath);
    }

  }
}
