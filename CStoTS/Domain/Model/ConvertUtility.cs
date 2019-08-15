using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Converter;
using CStoTS.Domain.Model.Interface;
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
      {typeof(IItemInterface), ()=>new ConverterInterface() },
      {typeof(IItemClass), ()=>new ConverterClass() },
      {typeof(IItemConstructor), ()=>new ConvertConstructor() },
    };

    /// <summary>
    /// C#解析結果からTS変換変換結果を返す
    /// </summary>
    /// <param name="csItem">C#解析結果</param>
    /// <returns>TypeScript変換結果</returns>
    public static string Convert(IAnalyzeItem csItem, int indent)
    {
      // 対象リストからC#解析結果に該当するTS変換クラスを抽出
      var query = table.Where(item => item.Key.IsInstanceOfType(csItem));
      if (!query.Any())
      {
        return string.Empty;
      }

      // 該当したTS変換クラスのインスタンスからTS変換変換結果を返す
      return query.First().Value().Convert(csItem, indent);
    }
  }
}
