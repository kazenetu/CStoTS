using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：class
  /// </summary>
  internal class ConverterClass : AbstractConverter, IConvertable
  {
    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, int indent)
    {
      return Convert(item as IItemClass, indent);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemClass item, int indent)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      result.AppendLine($"{indentSpace}class {item.Name}{{");
      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }
  }
}
