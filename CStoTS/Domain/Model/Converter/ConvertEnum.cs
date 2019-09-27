using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using CStoTS.Domain.Model.Mode;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：enum
  /// </summary>
  internal class ConvertEnum : AbstractConverter, IConvertable
  {
    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, Config config, int indent, List<string> otherScripts)
    {
      return Convert(item as IItemEnum, config, indent, otherScripts);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemEnum item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();

      // インデントは1固定
      indent = 1;
      var indentSpace = GetIndentSpace(indent);

      // クラスコメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      result.AppendLine($"{indentSpace}export enum {item.Name} {{");

      // 要素定義
      var enumItemSpace = GetIndentSpace(indent + 1);
      var enumItemIndex = 0;
      foreach (var enumItem in item.Items){
        result.Append($"{enumItemSpace}{enumItem.Key}");

        // 設定値
        var val = 0;
        if(!string.IsNullOrEmpty(enumItem.Value) && int.TryParse(enumItem.Value,out val) && enumItemIndex != val)
        {
          result.Append($" = {enumItem.Value}");
        }
        result.AppendLine(",");
        enumItemIndex++;
      }
      result.AppendLine($"{indentSpace}}}");

      // otherScriptsに定義を格納
      var className = GetParentClessName(item.Parent as IItemClass);
      if(!string.IsNullOrEmpty(className)){
        className += ".";
      }
      className += item.Parent?.Name;

      otherScripts.Add($"export namespace {className} {{");
      otherScripts.Add($"{result.ToString()}}}");
      otherScripts.Add($"");

      // 呼び出し元にはEmptyを返す
      return string.Empty;
    }

  }
}
