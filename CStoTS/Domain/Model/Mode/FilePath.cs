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
