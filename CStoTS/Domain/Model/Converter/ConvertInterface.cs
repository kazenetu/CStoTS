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
  /// TS変換クラス：interface
  /// </summary>
  internal class ConvertInterface : AbstractConverter, IConvertable
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
      return Convert(item as IItemInterface, config, indent, otherScripts);
    }

    public string Convert(IItemInterface item, Config config, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // インターフェイス定義
      result.Append(indentSpace);
      result.Append($"{GetScope(item)}interface {item.Name}");

      // ジェネリックスクラス
      if (item.GenericTypes.Any())
      {
        result.Append("<");
        result.Append(string.Join(", ", item.GenericTypes.Select(typeItem => GetTypeScriptType(typeItem))));
        result.Append(">");
      }

      // インターフェイスあり
      if (item.Interfaces.Any())
      {
        var interfaceList = new List<string>();
        foreach (var targetItemList in item.Interfaces)
        {
          // インターフェース名追加
          interfaceList.Add(ExpressionsToString(targetItemList));
        }

        result.Append(" implements ");
        result.Append(string.Join(", ", interfaceList.Select(typeItem => GetTypeScriptType(typeItem))));
      }

      result.AppendLine(" {");

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
