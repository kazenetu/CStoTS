using System.IO;
using System.Text;

namespace CStoTS.Infrastructure
{
  /// <summary>
  /// TypeScript出力
  /// </summary>
  internal class TSFileRepository: ITSFileRepository
  {
    /// <summary>
    /// TypeScriptの出力
    /// </summary>
    /// <param name="filePath">出力パス</param>
    /// <param name="tsData">変換後のTypeScript文字列</param>
    public void WriteFile(string filePath, string tsData)
    {
      using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
      {
        sw.Write(tsData);
      }
    }
  }
}
