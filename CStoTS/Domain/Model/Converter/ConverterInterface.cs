using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：interface
  /// </summary>
  internal class ConverterInterface : AbstractConverter, IConvertable
  {
    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, int indent)
    {
      return Convert(item as IItemInterface, indent);
    }

    public string Convert(IItemInterface item, int indent)
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
        result.Append(ConvertUtility.Convert(member, indent + 1));
      }

      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }

  }
}
