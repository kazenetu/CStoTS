using CSharpAnalyze.Domain.PublicInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

    /// <summary>
    /// 型のTypeScript変換
    /// </summary>
    /// <param name="src">C#用型</param>
    /// <returns>TypeScript変換後の型</returns>
    protected string GetTypeScriptType(string src)
    {
      var lowerSrc = src.ToLower(CultureInfo.CurrentCulture);
      switch (lowerSrc)
      {
        case "string":
          return "String";
        case "int":
        case "decimal":
        case "long":
          return "Number";
      }

      return src;
    }

    /// <summary>
    /// コメントをTypeScript用に変換する
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indentSpace">インデント文字列</param>
    /// <returns>TypeScript用コメント</returns>
    protected string GetTypeScriptComments(IAnalyzeItem item,string indentSpace)
    {
      if (!item.Comments.Any())
      {
        return string.Empty;
      }

      // コメント情報を一行にする
      var sb = new StringBuilder();
      foreach (var itemComment in item.Comments)
      {
        sb.Append(itemComment.Replace("///", string.Empty, StringComparison.CurrentCulture).Trim());
      }
      var src = sb.ToString();

      var result = new StringBuilder();
      result.AppendLine($"{indentSpace}/**");

      // 正規表現でTypeScript用コメントを追加する
      foreach (var comment in comments)
      {
        var matches = Regex.Matches(src, comment.regx);
        if (matches.Count < 1)
        {
          continue;
        }
        foreach (Match macheItem in matches)
        {
          var matcheGroup = macheItem.Groups;
          if(matcheGroup.Count <= 1)
          {
            continue;
          }

          var replaceSentence = comment.replaceSentence;
          for (var i = 1; i < matcheGroup.Count; i++)
          {
            replaceSentence = replaceSentence.Replace($"${ i}", matcheGroup[i].Value, StringComparison.CurrentCulture);
          }
          result.AppendLine($"{indentSpace} * {replaceSentence}");
        }
      }
      result.AppendLine($"{indentSpace} */");

      return result.ToString();
    }

    /// <summary>
    /// TypeScript用コメント変換情報
    /// </summary>
    private List<(string regx, string replaceSentence)> comments = new List<(string regx, string replaceSentence)>()
    {
        ( "<summary>(.+?)</summary>","$1" ),
        ( "<typeparam name=\"(.+?)\">(.+?)<","@template $1 $2" ),
        ( "<param name=\"(.+?)\">(.+?)<","@param $1 $2" ),
        ( "<returns>(.+?)<","@returns $1" ),
    };

  }
}
