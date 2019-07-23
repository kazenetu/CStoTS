using CSharpAnalyze.Domain.PublicInterfaces.Events;
using CSharpAnalyze.Domain.PublicInterfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

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
    /// メソッドリスト
    /// </summary>
    /// <remarks>ファイルに紐づくメソッドのりスト</remarks>
    private List<(string relativePath, Action<IAnalyzed> delegateMethod)> actions = new List<(string relativePath, Action<IAnalyzed> delegateMethod)>();

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
    /// <param name="delegateMethod">メソッド</param>
    public void Add(string filePath, string source, Action<IAnalyzed> delegateMethod = null)
    {
      files.Add((filePath, source));
      if (delegateMethod != null)
      {
        actions.Add((filePath, delegateMethod));
      }
    }

    /// <summary>
    /// メソッドを取得する
    /// </summary>
    /// <param name="filePath">ファイル名</param>
    /// <returns>メソッドまたはnull(未定義)</returns>
    public Action<IAnalyzed> GetDelegateMethod(string filePath)
    {
      var target = actions.Where(action => action.relativePath == filePath);

      if (!target.Any()) return null;

      return target.First().delegateMethod;
    }

    /// <summary>
    /// ファイル情報クリア
    /// </summary>
    public void Clear()
    {
      files.Clear();
      actions.Clear();
    }
  }
}
