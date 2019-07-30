using System.Globalization;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラスのスーパークラス
  /// </summary>
  internal abstract class AbstractConverter
  {
    /// <summary>
    /// インデントスペース取得
    /// </summary>
    /// <param name="indentSpace">インデント数</param>
    protected string GetIndentSpace(int indentSpace)
    {
      var result = new StringBuilder();
      while (indentSpace > 0)
      {
        result.Append("  ");
        indentSpace--;
      }
      return result.ToString();
    }

    /// <summary>
    /// 型のTypeScript変換
    /// </summary>
    /// <param name="src">C#用型</param>
    /// <returns>TypeScript変換後の型</returns>
    protected string GetTypeScriptType(string src)
    {
      var lowerSrc = src.ToLower(CultureInfo.CurrentCulture);
      switch (lowerSrc)
      {
        case "string":
          return "String";
        case "int":
        case "decimal":
        case "long":
          return "Number";
      }

      return src;
    }
  }
}
