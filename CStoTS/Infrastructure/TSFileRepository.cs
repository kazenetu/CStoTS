using System.Collections.Generic;
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
      // ディレクトリの確認と作成
      var directoryPath = Path.GetDirectoryName(filePath);
      if (!Directory.Exists(directoryPath))
      {
        Directory.CreateDirectory(directoryPath);
      }

      // ファイル作成
      using (var sw = new StreamWriter(filePath, false, Encoding.UTF8))
      {
        sw.Write(tsData);
      }
    }

    /// <summary>
    /// 固定TypeScriptの出力
    /// </summary>
    /// <param name="outputTSRoot">出力：TypeScriptのルートパス</param>
    public void OutputFixedTypeScripts(string outputTSRoot)
    {
      var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
      // 固定TSクラスをコピー
      var tsFileNames = new List<string>() { "Dictionary.ts", "List.ts", "JSONConverter.ts" };
      foreach (var tsFileName in tsFileNames)
      {
        File.Copy($"{baseDirectory}/Infrastructure/TypeScripts/{tsFileName}", Path.Combine(outputTSRoot, tsFileName), true);
      }
    }
  }
}
