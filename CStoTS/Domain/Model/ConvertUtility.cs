using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Converter;
using CStoTS.Domain.Model.Interface;
using CStoTS.Domain.Model.Mode;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CStoTS.Domain.Model
{
  /// <summary>
  /// TypeScript変換ユーティリティ
  /// </summary>
  internal static class ConvertUtility
  {
    /// <summary>
    /// C#解析結果とTS変換クラスのマップ
    /// </summary>
    private static Dictionary<Type, Func<IConvertable>> table = new Dictionary<Type, Func<IConvertable>>()
    {
      {typeof(IItemInterface), ()=>new ConvertInterface() },
      {typeof(IItemClass), ()=>new ConvertClass() },
      {typeof(IItemConstructor), ()=>new ConvertConstructor() },
      {typeof(IItemMethod), ()=>new ConvertMethod() },
      {typeof(IItemField), ()=>new ConvertField() },
      {typeof(IItemProperty), ()=>new ConvertProperty() },
      {typeof(IItemReturn), ()=>new ConvertReturn() },
      {typeof(IItemStatementExpression), ()=>new ConvertStatementExpression() },
      {typeof(IItemEnum), ()=>new ConvertEnum() },
      {typeof(IItemStatementLocalDeclaration), ()=>new ConvertLocalDeclaration() },
      {typeof(IItemLocalFunction), ()=>new ConvertLocalFunction() },
      {typeof(IItemIf), ()=>new ConvertIfElse() },
      {typeof(IItemElseClause), ()=>new ConvertIfElse() },
      {typeof(IItemBreak), ()=>new ConvertBreak() },
      {typeof(IItemContinue), ()=>new ConvertContinue() },
    };

    /// <summary>
    /// C#解析結果からTS変換変換結果を返す
    /// </summary>
    /// <param name="csItem">C#解析結果</param>
    /// <param name="config">設定情報</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    public static string Convert(IAnalyzeItem csItem, Config config, int indent, List<string> otherScripts)
    {
      // 対象リストからC#解析結果に該当するTS変換クラスを抽出
      var query = table.Where(item => item.Key.IsInstanceOfType(csItem));
      if (!query.Any())
      {
        return string.Empty;
      }

      // 該当したTS変換クラスのインスタンスからTS変換変換結果を返す
      return query.First().Value().Convert(csItem, config, indent, otherScripts);
    }
  }
}
