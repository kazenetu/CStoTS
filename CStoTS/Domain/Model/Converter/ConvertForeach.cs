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
  /// TS変換クラス：反復処理(foreach)
  /// </summary>
  internal class ConvertForEach : AbstractConverter, IConvertable
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
      return Convert(item as IItemForEach, config, indent, otherScripts);
    }

    /// <summary>
    /// 変換メソッド:foreach
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemForEach item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // 定義
      var isMultiParams = false;
      var multiParamLocalName = string.Empty;
      var localName = ExpressionsToString(item.Local);
      if (item.CollectionTypes.First().Name == "Dictionary")
      {
        multiParamLocalName = localName;
        isMultiParams = true;
        localName = "[k, v]";
      }

      result.AppendLine($"{indentSpace}for (let {localName} of {ExpressionsToString(item.Collection)}) {{");

      // 複数要素の場合はローカル変数作成
      if (isMultiParams)
      {
        result.AppendLine($"{GetIndentSpace(indent + 1)}let {multiParamLocalName} = {{ \"Key\": k, \"Value\": v }};");
      }

      // メンバー追加
      foreach (var member in item.Members)
      {
        result.Append(ConvertUtility.Convert(member, config, indent + 1, otherScripts));
      }

      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }

  }
}
