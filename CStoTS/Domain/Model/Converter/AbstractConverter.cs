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
  }
}
