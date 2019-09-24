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
  /// TS変換クラス：フィールド
  /// </summary>
  internal class ConvertField : AbstractConverter, IConvertable
  {
    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, Config config, int indent, List<string> otherScripts)
    {
      return Convert(item as IItemField, config, indent, otherScripts);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemField item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      var scope = GetScope(item);
      if (item.Modifiers.Contains("static")){
        scope += "static ";
      }
      result.Append($"{indentSpace}{scope}{item.Name}");
      result.Append($": {ExpressionsToString(item.FieldTypes)}");

      // デフォルト設定
      if (item.DefaultValues.Any())
      {
        result.Append($" = {ExpressionsToString(item.DefaultValues)}");
      }
      else
      {
        if (config.Mode.Value == OutputMode.Mode.WithoutMethod)
        {
          result.Append(GetDefaultString(item.FieldTypes));
        }
      }

      result.AppendLine(";");

      return result.ToString();
    }
  }
}
