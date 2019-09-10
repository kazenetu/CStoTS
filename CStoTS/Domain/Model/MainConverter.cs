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

      // 外部ファイル参照を設定
      var importStrings = analyzed.FileRoot.OtherFiles.
            OrderBy(item => item.Key).
            Select(item => $"import {{ {item.Key} }} from '{item.Value.Replace(".cs",string.Empty,StringComparison.CurrentCulture)}';");

      if(importStrings.Any()){
        result.Append(string.Join(Environment.NewLine, importStrings));
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
  }
}
