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
  /// TS変換クラス：反復処理(for)
  /// </summary>
  internal class ConvertFor : AbstractConverter, IConvertable
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
      return Convert(item as IItemFor, config, indent, otherScripts);
    }

    /// <summary>
    /// 変換メソッド:for
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemFor item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      var declarations = GetDeclarations(item);
      var letKeyword = item.IsVar ? "let " : string.Empty;
      var incrementors = item.Incrementors.Select(incrementor => ExpressionsToString(incrementor));
      result.AppendLine($"{indentSpace}for ({letKeyword}{string.Join(", ", declarations)}; {ExpressionsToString(item.Conditions)}; {string.Join(", ", incrementors)}) {{");

      // メンバー追加
      foreach (var member in item.Members)
      {
        result.Append(ConvertUtility.Convert(member, config, indent + 1, otherScripts));
      }

      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }

    /// <summary>
    /// ローカルフィールド群に型情報を追加
    /// </summary>
    /// <param name="item">反復処理(for)インスタンス</param>
    /// <returns>ローカルフィールド情報</returns>
    private List<string> GetDeclarations(IItemFor item)
    {
      var result = new List<string>();

      foreach (var declaration in item.Declarations)
      {
        if (!item.IsVar)
        {
          // ローカルフィールド宣言ではない場合、そのまますべてを文字列に置き換える
          result.Add($"{ExpressionsToString(declaration)}");
          continue;
        }

        // イコールキーワードで各要素に分割
        var equalKeywordIndex = declaration.FindIndex(exp => exp.Name == "=");
        var localField = declaration.GetRange(0, equalKeywordIndex);
        var valueList = declaration.GetRange(equalKeywordIndex + 1, declaration.Count - equalKeywordIndex - 1);

        // 型情報取得
        var declarationTypes = valueList.Select(exp => exp.TypeName);
        var declarationType = GetTypeScriptType(declarationTypes.First());
        if(declarationType == declarationTypes.First())
        {
          declarationType = string.Empty;
        }
        else
        {
          declarationType = $": {declarationType}";
        }

        // 再結合して式を作成
        result.Add($"{ExpressionsToString(localField)}{declarationType} = {ExpressionsToString(valueList)}");
      }

      return result;
    }
  }
}
