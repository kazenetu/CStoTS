using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：class
  /// </summary>
  internal class ConverterClass : AbstractConverter, IConvertable
  {
    /// <summary>
    /// クラスタイプ
    /// </summary>
    /// <remarks>内部クラスか否か</remarks>
    private enum ClassType
    {
      Normal,
      Inner
    }

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

      // クラスコメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // クラス定義
      result.Append(indentSpace);
      switch (GetClassType(item))
      { 
        case ClassType.Normal:
          result.Append($"class {item.Name}{GetClassInfo(item)}");
          break;
        case ClassType.Inner:
          result.Append($"public static {item.Name} = class{GetClassInfo(item)}");
          break;
      }
      result.AppendLine(" {");

      // メンバー追加
      foreach(var member in item.Members){
        result.Append(ConvertUtility.Convert(member, indent + 1));
      }

      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }

    /// <summary>
    /// クラスタイプの取得
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <returns>クラスタイプ</returns>
    private ClassType GetClassType(IItemClass item)
    {
      if (item.Parent is IItemClass)
      {
        return ClassType.Inner;
      }
      return ClassType.Normal;
    }

    /// <summary>
    /// クラス情報の取得
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <returns>クラス情報</returns>
    /// <remarks>ジェネリックス、継承の文字列取得</remarks>
    private string GetClassInfo(IItemClass item)
    {
      var result = new StringBuilder();

      // ジェネリックスクラス
      if(item.GenericTypes.Any()){
        result.Append("<");
        result.Append(string.Join(",", item.GenericTypes.Select(typeItem => GetTypeScriptType(typeItem))));
        result.Append(">");
      }

      // スーパークラスあり
      if (item.SuperClass.Any()){
        var superClass = new StringBuilder();
        foreach(var targetItem in item.SuperClass){
          superClass.Append(GetTypeScriptType(targetItem.Name));
        }

        result.Append($" extends {superClass.ToString()}");
      }

      // インターフェイスあり
      if (item.Interfaces.Any())
      {
        var interfaceList = new List<string>();
        foreach (var targetItemList in item.Interfaces)
        {
          // パスを含むインターフェース名格納
          var interfaceItem = string.Empty;
          foreach (var targetItem in targetItemList)
          {
            interfaceItem += GetTypeScriptType(targetItem.Name);
          }
          // インターフェース名追加
          interfaceList.Add(interfaceItem);
        }

        result.Append($" implements {string.Join(",", interfaceList)}");
      }

      return result.ToString();
    }
  }
}
