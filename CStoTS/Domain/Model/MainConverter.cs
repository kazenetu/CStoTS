using CSharpAnalyze.Domain.PublicInterfaces.Events;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model
{
  /// <summary>
  /// TypeScript変換クラス：メイン処理
  /// </summary>
  internal class MainConverter
  {
    public string ConvertTS(IAnalyzed analyzed, int indent = 0)
    {
      var result = new StringBuilder();

      // C#解析結果を取得
      var targetItem = analyzed.FileRoot.Members.First();

      // TS変換変換結果を取得
      result.Append(ConvertUtility.Convert(targetItem, 0));

      // 変換結果を返す
      return result.ToString();
    }
  }
}
