using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：プロパティ
  /// </summary>
  internal class ConvertProperty : AbstractConverter, IConvertable
  {
    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, int indent, List<string> otherScripts)
    {
      return Convert(item as IItemProperty, indent, otherScripts);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemProperty item, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // アクセサのメンバー確認とフィールドの作成
      if (item.AccessorList.Where(accessor => accessor.Members.Count() == 0).Any())
      {
        result.Append($"{indentSpace}private _{item.Name}_: {ExpressionsToString(item.PropertyTypes)}");

        // デフォルト設定
        if (item.DefaultValues.Any())
        {
          result.Append($" = {ExpressionsToString(item.DefaultValues)}");
        }
        result.AppendLine(";");
      }

      // コメント
      var comment = GetTypeScriptComments(item, indentSpace);

      // 定義
      var scope = GetScope(item);
      var propertyType = ExpressionsToString(item.PropertyTypes);
      foreach (var accessor in item.AccessorList)
      {
        result.Append(comment);
        result.Append(GetAccessorString(accessor, indent, scope, item.Name, propertyType, otherScripts));
      }

      return result.ToString();
    }

    /// <summary>
    /// プロパティのset/get用Typescriptを返す
    /// </summary>
    /// <param name="accessorItem">set/get用のアイテム</param>
    /// <param name="indent">インデント数</param>
    /// <param name="scope">スコープ名</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="propertyType">プロパティの型</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>set/get用Typescriptの文字列</returns>
    private string GetAccessorString(IAnalyzeItem accessorItem, int indent, string scope, string propertyName, string propertyType, List<string> otherScripts)
    {
      var indentSpace = GetIndentSpace(indent);

      var result = new StringBuilder();
      var notExistsMember = true;
      if(accessorItem.Members.Any()){
        notExistsMember = false;
      }

      // set/getの設定
      var accessorItemName = accessorItem.Name.ToLower(CultureInfo.CurrentCulture);
      switch (accessorItemName)
      {
        case "set":
          result.AppendLine($"{indentSpace}{scope}set {propertyName}(value: {propertyType}) {{");

          if(notExistsMember){
            result.AppendLine($"{GetIndentSpace(indent + 1)}this._{propertyName}_ = value;");
          }
          break;

        case "get":
          result.AppendLine($"{indentSpace}{scope}get {propertyName}(): {propertyType} {{");

          if (notExistsMember)
          {
            result.AppendLine($"{GetIndentSpace(indent + 1)}return this._{propertyName}_;");
          }
          break;

        default:
          return result.ToString();
      }

      // メンバー追加
      foreach (var member in accessorItem.Members)
      {
        result.Append(ConvertUtility.Convert(member, indent + 1, otherScripts));
      }

      result.AppendLine($"{indentSpace}}}");

      return result.ToString();
    }
  }
}
