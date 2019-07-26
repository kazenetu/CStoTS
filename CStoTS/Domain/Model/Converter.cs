﻿using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CSharpAnalyze.Domain.PublicInterfaces.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model
{
  /// <summary>
  /// 変換インターフェース
  /// </summary>
  internal interface IConvertable
  {
    string Convert(IAnalyzeItem item, int indent);
  }

  /// <summary>
  /// TS変換クラスのスーパークラス
  /// </summary>
  internal class ConverterBase
  {
    /// <summary>
    /// C#解析結果とTS変換クラスのマップ
    /// </summary>
    private static Dictionary<Type, Func<IConvertable>> table = new Dictionary<Type, Func<IConvertable>>()
    {
      {typeof(IItemClass), ()=>new ConverterClass() },
    };

    /// <summary>
    /// C#解析結果に該当するTS変換クラスを返す
    /// </summary>
    /// <param name="csItem">C#解析結果</param>
    /// <returns>TS変換クラスのインスタンス または null</returns>
    /// <remarks>該当しない場合はnullを返す</remarks>
    public static IConvertable GetConverter(IAnalyzeItem csItem)
    {
      // 対象リストからC#解析結果に該当するTS変換クラスを抽出
      var query = table.Where(item => item.Key.IsInstanceOfType(csItem));
      if (!query.Any())
      {
        return null;
      }

      // 該当したTS変換クラスのインスタンスを返す
      return query.First().Value();
    }


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

  /// <summary>
  /// TS変換クラス：class
  /// </summary>
  internal class ConverterClass: ConverterBase, IConvertable
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



  /// <summary>
  /// TypeScript変換クラス
  /// </summary>
  internal class Converter
  {
    public Converter()
    {
    }

    public string ConvertTS(IAnalyzed analyzed, int indent = 0)
    {
      var result = new StringBuilder();

      // C#解析結果を取得
      var targetItem = analyzed.FileRoot.Members.First();

      // TS変換クラスを取得
      var targetConverter = ConverterBase.GetConverter(targetItem);
      if (targetConverter is null)
      {
        return string.Empty;
      }

      // 該当したTS変換クラスの変換メソッドを呼び出す
      result.Append(targetConverter.Convert(targetItem, 0));

      // 変換結果を返す
      return result.ToString();
    }
  }


}
