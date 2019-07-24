using CStoTS.Infrastructure;
using System.Collections.Generic;

namespace CStoTSTest.Common
{
  /// <summary>
  /// テスト用ファイル出力 : 出力Typescriptソースコード
  /// </summary>
  internal class TSFileRepositoryMock: ITSFileRepository
  {
    /// <summary>
    /// 変換後のTypeScriptリスト
    /// </summary>
    public List<(string filePath, string typeScripts)> Scripts { get; } = new List<(string filePath, string typeScripts)>();

    /// <summary>
    /// TypeScriptの出力
    /// </summary>
    /// <param name="filePath">出力パス</param>
    /// <param name="tsData">変換後のTypeScript文字列</param>
    public void WriteFile(string filePath, string tsData)
    {
      Scripts.Add((filePath, tsData));
    }
  }
}
