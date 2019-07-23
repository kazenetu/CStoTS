using CSharpAnalyze.Domain.PublicInterfaces.Repository;
using System.Collections.Generic;

namespace CStoTS
{
  /// <summary>
  /// テスト用FileRepository : 入力C#ソースコード
  /// </summary>
  internal class InCSFileRepositoryMock : ICSFileRepository
  {
    /// <summary>
    /// ファイルリスト
    /// </summary>
    private List<(string relativePath, string source)> files = new List<(string relativePath, string source)>();

    /// <summary>
    /// ファイルリスト取得
    /// </summary>
    /// <param name="rootPath">対象ファイルルート(未使用)</param>
    /// <param name="exclusionKeywords">除外ファイルリスト(未使用)</param>
    /// <returns>ファイルリスト</returns>
    public List<(string relativePath, string source)> GetCSFileList(string rootPath, List<string> exclusionKeywords)
    {
      return files;
    }

    /// <summary>
    /// ファイル情報追加
    /// </summary>
    /// <param name="filePath">ファイル名</param>
    /// <param name="source">ソースコード</param>
    public void Add(string filePath, string source)
    {
      files.Add((filePath, source));
    }

    /// <summary>
    /// ファイル情報クリア
    /// </summary>
    public void Clear()
    {
      files.Clear();
    }
  }
}
