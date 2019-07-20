namespace CStoTS.Infrastructure
{
  /// <summary>
  /// TypeScript出力インターフェイス
  /// </summary>
  public interface ITSFileRepository
  {
    /// <summary>
    /// TypeScriptの出力
    /// </summary>
    /// <param name="filePath">出力パス</param>
    /// <param name="tsData">変換後のTypeScript文字列</param>
    void WriteFile(string filePath,string tsData);
  }
}
