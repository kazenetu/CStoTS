using CSharpAnalyze.Domain.PublicInterfaces.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model
{
  /// <summary>
  /// TypeScript変換クラス：メイン処理
  /// </summary>
  internal class MainConverter
  {
    public string ConvertTS(IAnalyzed analyzed, int indent = 0)
    {
      var result = new StringBuilder();

      // 対象ファイルのディレクトリのリストを取得
      var thisDirectory = GetDerectoryList(analyzed.FilePath);

      // 外部ファイル参照を組み立て
      var importFileKeys = new Dictionary<string, List<string>>();
      foreach (var importString in analyzed.FileRoot.OtherFiles)
      {
        // ファイルパス取得
        var filePath = importString.Value;
        if(string.IsNullOrEmpty(filePath)){
          // ファイルパスがない場合は定義済みTSクラス(ファイル出力)
          filePath = importString.Key;
        }

        // ファイルパスを現地点からの相対パスに変換
        filePath = GetRelativePath(filePath, thisDirectory).Replace(".cs", string.Empty, StringComparison.CurrentCulture);

        // キー名確認とリスト生成
        if (!importFileKeys.ContainsKey(filePath))
        {
          importFileKeys.Add(filePath, new List<string>());
        }

        // TSクラス名を追加
        var tsClassName = importString.Key;
        var dotIndex = tsClassName.IndexOf(".", StringComparison.CurrentCulture);
        if (dotIndex > 0)
        {
          tsClassName = tsClassName.Substring(0, dotIndex);
        }
        importFileKeys[filePath].Add(tsClassName);
      }

      // 外部ファイル参照設定
      var importTargets = importFileKeys.Where(item => item.Key != "./String").OrderBy(item => item.Key);
      if (importTargets.Any())
      {
        result.Append(string.Join(Environment.NewLine, importTargets.Select(item => $"import {{ {string.Join(", ", item.Value)} }} from '{item.Key}';")));
        result.AppendLine(Environment.NewLine);
      }

      // C#解析結果を取得
      var targetItems = analyzed.FileRoot.Members;

      // TS変換変換結果を取得
      foreach(var targetItem in targetItems){
        var otherScripts = new List<string>();
        result.Append(ConvertUtility.Convert(targetItem, 0, otherScripts));
        result.Append(string.Join(Environment.NewLine, otherScripts));
      }

      // 変換結果を返す
      return result.ToString();
    }

    /// <summary>
    /// ディレクトリのリストを取得
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <returns>相対パスのリスト</returns>
    private List<string> GetDerectoryList(string filePath)
    {
      var directoryCount = filePath.LastIndexOf("/", StringComparison.CurrentCulture);
      if (directoryCount <= 0)
      {
        return new List<string>();
      }

      var directory = filePath.Substring(0, directoryCount);

      return directory.Split("/").ToList();
    }
    
    /// <summary>
    /// 相対パスを取得
    /// </summary>
    /// <param name="targetFilePath">対象のファイルパス</param>
    /// <param name="thisDirectory">現在のディレクトリリスト</param>
    /// <returns>ファイルの相対パス</returns>
    private string GetRelativePath(string targetFilePath,List<string> thisDirectory)
    {
      // 現在のディレクトリがルートの場合はそのまま出力
      if(!thisDirectory.Any()){
        return $"./{targetFilePath}";
      }

      // 対象のディレクトリリストを取得
      var targetDirectory = GetDerectoryList(targetFilePath);

      // 対象のディレクトリがルートの場合は現在のディレクトリの分だけ遡る
      if (!targetDirectory.Any())
      {
        return string.Join(string.Empty, thisDirectory.Select(item => "../")) + targetFilePath;
      }

      var relativePath = new StringBuilder();
      var isEqual = true;
      for (int i = 0; i < thisDirectory.Count; i++)
      {
        // 対象のディレクトリとの比較
        if(targetDirectory.Count > i){
          // ディレクトリが同じれある場合は対象ファイルパスから除外
          if(thisDirectory[i] == targetDirectory[i]){
            if(isEqual){
              targetFilePath = targetFilePath.Replace(targetDirectory[i], String.Empty, StringComparison.CurrentCulture);
            }
          }
          else 
          {
            isEqual = false;
            relativePath.Append("../");
          }
        }
      }

      var result = relativePath.ToString() + targetFilePath;
      if (result.StartsWith("/", StringComparison.CurrentCulture))
      {
        result = ".." + result;
      }

      return result;
    }

  }
}
