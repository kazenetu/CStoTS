using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：式
  /// </summary>
  internal class ConvertStatementExpression : AbstractConverter, IConvertable
  {
    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, int indent, List<string> otherScripts)
    {
      return Convert(item as IItemStatementExpression, indent, otherScripts);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemStatementExpression item, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      result.Append($"{indentSpace}");
      
      if(item.LeftSideList.Any()){
        // 左辺の追加
        result.Append(ExpressionsToString(item.LeftSideList));
      }

      if(!string.IsNullOrEmpty(item.AssignmentOperator)){
        // 代入演算子
        result.Append($" {item.AssignmentOperator} ");
      }

      if (item.RightSideList.Any())
      {
        // 右辺の追加
        result.Append(ExpressionsToString(item.RightSideList));
      }
      
      result.AppendLine(";");

      return result.ToString();
    }

  }
}
