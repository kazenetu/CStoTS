using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
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
    /// 前スペースを置くキーワード
    /// </summary>
    private List<string> beforeSpaceKeywords = new List<string>() {
      "+",
      "-",
      "*",
      "/",
      "==",
      ">",
      "<",
      ">=",
      "<=",
    };

    /// <summary>
    /// 後スペースを置くキーワード
    /// </summary>
    private List<string> afterSpaceKeywords = new List<string>() {
      "+",
      "-",
      "*",
      "/",
      "new",
      ",",
      "==",
      ">",
      "<",
      ">=",
      "<=",
    };

    /// <summary>
    /// スペース対象で型名称が必須
    /// </summary>
    private List<string> existsTypeSpaceKeywords = new List<string>()
    {
      ">",
      "<",
    };

    /// <summary>
    /// C#とTypeScriptの変換リスト
    /// </summary>
    /// <remarks>一部C#側はTypeScriptの型に変換済みを考慮</remarks>
    private readonly Dictionary<string, string> ConvertMethodNames = new Dictionary<string, string>()
    {
      {@"\.ToString\(",".toString(" },
      {@"\.Length",".length" },
      {@"number\.Parse\(","Number(" },
      {@"[s|S]tring\.Empty","''" },
    };

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
    /// TypeScript用スコープを返す
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <returns>TypeScript用スコープ</returns>
    protected string GetScope(IAnalyzeItem item)
    {
      // class/interfaceのexport判定
      if (item is IItemClass || item is IItemInterface)
      {
        if (item.Modifiers.Where(modifier => modifier != "private").Any())
        {
          return "export ";
        }
        return string.Empty;
      }

      // そのほかの要素
      var resultItems = new List<string>();
      foreach (var scope in item.Modifiers)
      {
        switch(scope){
          case "private":
          case "protected":
          case "public":
            resultItems.Add(scope);
            break;
        }
      }

      var result = string.Join(" ", resultItems);
      if (resultItems.Any())
      {
        result += " ";
      }
      return result;
    }

    /// <summary>
    /// C#の型情報をTypeScriptに変換
    /// </summary>
    /// <param name="expressions">C#の型情報</param>
    /// <returns>TypeScriptの型情報</returns>
    protected string ExpressionsToString(List<IExpression> expressions)
    {
      var result = new StringBuilder();

      foreach (var exp in expressions)
      {
        // 前スペースの代入
        if (IsSpaceKeyword(exp, beforeSpaceKeywords))
        {
          result.Append(" ");
        }

        result.Append(GetTypeScriptType(exp.Name));

        // 後ろスペースの代入
        if (IsSpaceKeyword(exp, afterSpaceKeywords))
        {
          result.Append(" ");
        }
      }
      return ReplaceMethodNames(result.ToString());
    }

    /// <summary>
    /// スペース代入確認
    /// </summary>
    /// <param name="targetExp">確認対象</param>
    /// <param name="SpaceKeywords">スペースチェックリスト</param>
    /// <returns>スペース代入結果</returns>
    private bool IsSpaceKeyword(IExpression targetExp, List<string> SpaceKeywords)
    {
      // スペースチェックリストに含まれていない
      if (!SpaceKeywords.Contains(targetExp.Name))
      {
        return false;
      }

      // 型名称必須リストに含まれている なおかつ 型名称が設定されていない
      if (existsTypeSpaceKeywords.Contains(targetExp.Name) && string.IsNullOrEmpty(targetExp.TypeName))
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// デフォルト値を取得
    /// </summary>
    /// <param name="expressions">C#の型情報</param>
    /// <returns>TypeScriptのデフォルト値</returns>
    protected string GetDefaultString(List<IExpression> expressions)
    {
      // 値なしの場合は終了
      if(!expressions.Any()){
        return string.Empty;
      }

      var result = new StringBuilder();
      result.Append(" = ");
      switch (expressions.First().TypeName.ToLower(CultureInfo.CurrentCulture)){
        case "string":
          result.Append("\"\"");
          return result.ToString();
        case "byte":
        case "int16":
        case "int32":
        case "int64":
        case "sbyte":
        case "uint16":
        case "uint32":
        case "uint64":
        case "biginteger":
        case "decimal":
        case "double":
        case "single":
          result.Append("0");
          return result.ToString();
        case "boolean":
          result.Append("false");
          return result.ToString();
        default:
          result.Append("new ");
          break;
      }

      foreach (var exp in expressions)
      {
        // 前スペースの代入
        if (beforeSpaceKeywords.Contains(exp.Name))
        {
          result.Append(" ");
        }

        result.Append(GetTypeScriptType(exp.Name));

        // 後ろスペースの代入
        if (afterSpaceKeywords.Contains(exp.Name))
        {
          result.Append(" ");
        }
      }
      result.Append("()");

      return ReplaceMethodNames(result.ToString());
    }

    /// <summary>
    /// 型が組み込みか否か
    /// </summary>
    /// <param name="expression">対象インスタンス</param>
    /// <returns>組み込みか否か</returns>
    protected bool IsLiteralType(IExpression expression)
    {
      switch (expression.TypeName.ToLower(CultureInfo.CurrentCulture))
      {
        case "string":
        case "byte":
        case "int16":
        case "int32":
        case "int64":
        case "sbyte":
        case "uint16":
        case "uint32":
        case "uint64":
        case "biginteger":
        case "decimal":
        case "double":
        case "single":
        case "boolean":
          return true;
        default:
          return false;
      }
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
        case "bool":
          return "boolean";
        case "int":
        case "decimal":
        case "long":
          return "number";
        case "datetime":
          return "Date";
        case "object":
          return "any";
        case "==":
          return "===";
      }

      return src;
    }

    /// <summary>
    /// 条件式をTypeScriptに変換する
    /// </summary>
    /// <param name="conditions">条件式情報</param>
    /// <returns>TypeScriptに変換した条件式</returns>
    protected string ConvertConditions(List<IExpression> conditions)
    {
      var isKeywordIndex = conditions.FindIndex(item => item.Name == "is" && string.IsNullOrEmpty(item.TypeName));
      if (isKeywordIndex > 0)
      {
        // 左辺値設定
        var leftValue = ExpressionsToString(conditions.GetRange(0, isKeywordIndex));

        // 右辺値設定
        var rightValue = string.Empty;
        var isLiteral = false;
        foreach (var item in conditions.GetRange(isKeywordIndex + 1, conditions.Count - (isKeywordIndex + 1)))
        {
          isLiteral = IsLiteralType(item);
          if (item.TypeName == "Enum")
          {
            isLiteral = true;
            rightValue = "function";
            break;
          }
          rightValue += GetTypeScriptType(item.Name);
        }

        // 条件式組み立て
        var result = new StringBuilder();
        if (isLiteral)
        {
          result.Append($"typeof {leftValue} === \"{rightValue}\"");
        }
        else
        {
          result.Append($"{leftValue} instanceof {rightValue}");
        }
        return result.ToString();
      }

      // isキーワードがない場合はそのままTypeScript変換して返す
      return ExpressionsToString(conditions);
    }

    #region メソッド C#→TypeScript変換

    /// <summary>
    /// メソッドをTypeScript用に置換え
    /// </summary>
    /// <param name="src">ソース文字列</param>
    /// <returns>置き換え後文字列</returns>
    public string ReplaceMethodNames(string src)
    {
      var result = src;

      foreach (var convertMethodName in ConvertMethodNames.Keys)
      {
        if (Regex.IsMatch(result, convertMethodName))
        {
          result = ReplaceMethod(result, convertMethodName, ConvertMethodNames[convertMethodName]);
        }
      }

      return result;
    }

    /// <summary>
    /// メソッドの置き換え
    /// </summary>
    /// <param name="srcText">ソース文字列</param>
    /// <param name="regexText">正規表現</param>
    /// <param name="replaceText">置き換え後の対象</param>
    /// <returns>置き換え後文字列</returns>
    private string ReplaceMethod(string srcText, string regexText, string replaceText)
    {
      var result = srcText;

      var replaceCount = Regex.Matches(replaceText, @"\{[0-9]}").Count;
      if (replaceCount <= 0)
      {
        // 置換文字列にパラメータが設定されていない場合は単純な置換え
        result = Regex.Replace(srcText, regexText, replaceText);
      }
      else
      {
        // パラメータの取得
        var args = new List<string>();
        foreach (Match match in Regex.Matches(srcText, $"^{regexText}\\((.+?)\\)$"))
        {
          if (match.Groups.Count < 2)
          {
            break;
          }
          // カンマ区切りでパラメータリスト作成
          args.AddRange(Regex.Split(match.Groups[1].Value, @"\s*,\s*(?=(?:[^""]*""[^""]*"")*[^""]*$)").Select(arg => arg.Trim()).ToList());
        }

        // 置換文字列のパラメータ数より作成したパラメータ数が少ない場合は元の文字列を返す
        if (replaceCount > args.Count)
        {
          return srcText;
        }

        try
        {
          // 置換を実施
          result = Regex.Replace(srcText, $"{regexText}\\(.*\\)$", string.Format(CultureInfo.InvariantCulture, replaceText, args.ToArray()));
        }
        catch
        {
          throw;
        }
      }

      return result;
    }

    #endregion

    /// <summary>
    /// 親クラスのパスを取得する
    /// </summary>
    /// <param name="item">内部クラスのインスタンス</param>
    /// <returns>親クラスのパス</returns>
    protected string GetParentClessName(IItemClass item)
    {
      var result = new List<string>();

      // 親クラスをさかのぼりながら格納する
      var parent = item?.Parent as IItemClass;
      while (parent != null)
      {
        result.Add(parent.Name);
        parent = parent.Parent as IItemClass;
      }

      return string.Join(".", result);
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

      // 1行コメント
      if (item.Comments.Count() == 1)
      {
        return $"{indentSpace}{item.Comments.First()}{Environment.NewLine}";
      }

      // 複数コメント：コメント情報を一行にする
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

    /// <summary>
    /// 条件用文字列取得
    /// </summary>
    /// <param name="paramIndex">パラメータ番号(1～)</param>
    /// <param name="targetType">TypeScriptの型</param>
    /// <returns>条件用文字列</returns>
    protected string GetCondition(int paramIndex, string targetType)
    {
      var paramName = $"param{paramIndex}";
      switch (targetType)
      {
        case "string":
        case "number":
        case "bigint":
        case "boolean":
        case "symbol":
        case "object":
        case "function":
          return $"typeof {paramName} === '{targetType}'";
        case "undefined":
          return $"{paramName} === {targetType}";
      }
      return $"{paramName} instanceof {targetType}";
    }

  }
}
