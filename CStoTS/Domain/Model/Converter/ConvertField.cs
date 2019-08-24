using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
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
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, int indent)
    {
      return Convert(item as IItemField, indent);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemField item, int indent)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      var scope = GetScope(item);
      result.Append($"{indentSpace}{scope}{item.Name}");
      result.Append($": {ExpressionsToString(item.FieldTypes)}");

      // デフォルト設定
      if(item.DefaultValues.Any()){
        result.Append($" = {ExpressionsToString(item.DefaultValues)}");
      }

      result.AppendLine(";");

      return result.ToString();
    }
  }
}
