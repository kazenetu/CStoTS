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
  /// TS変換クラス：複数条件分岐
  /// </summary>
  internal class ConvertSwitch : AbstractConverter, IConvertable
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

      if (item is IItemSwitch)
      {
        return Convert(item as IItemSwitch, config, indent, otherScripts);
      }
      else
      {
        return Convert(item as IItemSwitchCase, config, indent, otherScripts);
      }
    }

    /// <summary>
    /// 変換メソッド:switch
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemSwitch item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // caseがtypeチェックとローカルフィールド設定か否か
      var existsTypes = item.Cases.Where(caseItem => ExistsTypeCase(caseItem as IItemSwitchCase)).Any();


      if (!existsTypes)
      {
        // 定義(switchのみ)
        result.AppendLine($"{indentSpace}switch ({ExpressionsToString(item.Conditions)}) {{");

        // caseはインデントをインクリメント
        indent++;
      }

      // メンバー追加
      foreach (var member in item.Cases)
      {
        result.Append(ConvertUtility.Convert(member, config, indent, otherScripts));
      }

      if (!existsTypes)
      {
        // 定義(switchのみ)
        result.AppendLine($"{indentSpace}}}");
      }

      return result.ToString();
    }

    /// <summary>
    /// 変換メソッド:case
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemSwitchCase item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // typeチェックとローカルフィールド設定か否か
      var existsTypes = ExistsTypeCase(item);

      // 定義 case か if の出力分岐
      if (existsTypes)
      {
        result.Append(CreateCaseIf(item, indent));
      }
      else
      {
        // case
        foreach (var label in item.Labels)
        {
          result.Append($"{indentSpace}");
          if (label.First().TypeName != "Default")
          {
            result.Append("case ");
          }
          result.AppendLine($"{ExpressionsToString(label)}:");
        }
      }

      // メンバー追加
      foreach (var member in item.Members)
      {
        // if構文に変化した場合はbreakを除外
        if (existsTypes && member is IItemBreak)
        {
          continue;
        }

        result.Append(ConvertUtility.Convert(member, config, indent + 1, otherScripts));
      }

      if (existsTypes)
      {
        // if : ブロック終了
        result.AppendLine($"{indentSpace}}}");
      }

      return result.ToString();
    }

    /// <summary>
    /// caseがtypeチェックならびにローカルフィールド設定を行っているか
    /// </summary>
    /// <param name="caseItem">対象インスタンス</param>
    /// <returns>typeチェックとローカルフィールド設定か否か</returns>
    private bool ExistsTypeCase(IItemSwitchCase caseItem)
    {
      if(caseItem is null)
      {
        return false;
      }

      var types = caseItem.Labels.Where(label => label.Exists(exp => exp.Name == " "));
      return types.Any();
    }

    /// <summary>
    /// Caseの代わりにifを出力
    /// </summary>
    /// <param name="caseItem">対象インスタンス</param>
    /// <param name="indent">インデント数</param>
    /// <returns>if構文</returns>
    private string CreateCaseIf(IItemSwitchCase caseItem,int indent)
    {
      var result = new StringBuilder();

      // 親(switch)を取得
      var parent = caseItem.Parent as IItemSwitch;

      // 対象は1行のみ
      var spaceLabelIndex = caseItem.Labels.First().FindIndex(label => label.Name == " ");
      var targetLabels = caseItem.Labels.First().GetRange(0, spaceLabelIndex);

      // 条件を作成
      var conditions = new List<IExpression>();
      // 左辺値
      conditions.AddRange(parent.Conditions);
      // is
      conditions.Add(null);
      // 右辺値
      conditions.AddRange(targetLabels);

      result.Append($"{GetIndentSpace(indent)}if (");

      result.Append(ConvertConditions(conditions));

      result.AppendLine($") {{");

      // ローカルフィールドを作成
      result.AppendLine($"{GetIndentSpace(indent + 1)}let {caseItem.Name}: {ExpressionsToString(targetLabels)} = {ExpressionsToString(parent.Conditions)};");

      return result.ToString();
    }
  }
}
