using System;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model
{
  /// <summary>
  /// TypeScript変換クラス
  /// </summary>
  internal class Converter
  {
    public string ConvertTS(CSharpAnalyze.Domain.PublicInterfaces.Events.IAnalyzed analyzed, int indent = 0)
    {
      var result = new StringBuilder();

      result.Append(ConvertTS((dynamic)analyzed.FileRoot.Members.First(), 0));

      return result.ToString();
    }

    private string ConvertTS(CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems.IItemClass item, int indent = 0)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      result.AppendLine($"{indentSpace}class {item.Name}{{");
      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }

    /// <summary>
    /// インデントスペース取得
    /// </summary>
    /// <param name="indentSpace">インデント数</param>
    private string GetIndentSpace(int indentSpace)
    {
      var result = new StringBuilder();
      while(indentSpace > 0)
      {
        result.Append("  ");
        indentSpace--;
      }
      return result.ToString();
    }
  }
}
