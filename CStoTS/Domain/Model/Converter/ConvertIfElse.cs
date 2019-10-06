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
  /// TS変換クラス：条件分岐
  /// </summary>
  internal class ConvertIfElse : AbstractConverter, IConvertable
  {
    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, Config config, int indent, List<string> otherScripts)
    {

      if (item is IItemIf)
      {
        return Convert(item as IItemIf, config, indent, otherScripts);
      }
      else
      {
        return Convert(item as IItemElseClause, config, indent, otherScripts);
      }
    }

    /// <summary>
    /// 変換メソッド:if
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemIf item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      result.AppendLine($"{indentSpace}if ({ConvertConditions(item.Conditions)}) {{");

      // メンバー追加
      foreach (var member in item.TrueBlock)
      {
        result.Append(ConvertUtility.Convert(member, config, indent + 1, otherScripts));
      }

      result.AppendLine($"{indentSpace}}}");

      // else構文追加
      foreach (var block in item.FalseBlocks)
      {
        result.Append(ConvertUtility.Convert(block, config, indent, otherScripts));
      }

      return result.ToString();
    }

    /// <summary>
    /// 変換メソッド:else
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemElseClause item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      if (item.Conditions.Any())
      {
        result.AppendLine($"{indentSpace}else if ({ConvertConditions(item.Conditions)}) {{");
      }
      else
      {
        result.AppendLine($"{indentSpace}else {{");
      }

      // メンバー追加
      foreach (var member in item.Block)
      {
        result.Append(ConvertUtility.Convert(member, config, indent + 1, otherScripts));
      }

      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }

  }
}
