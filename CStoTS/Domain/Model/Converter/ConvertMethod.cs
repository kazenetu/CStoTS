using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：メソッド
  /// </summary>
  internal class ConvertMethod : AbstractConverter, IConvertable
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
      return Convert(item as IItemMethod, indent, otherScripts);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <param name="otherScripts">その他のスクリプト(内部クラスなど)</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemMethod item, int indent, List<string> otherScripts)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // メソッド名
      string BaseMethodName = item.Name;

      // クラス情報を取得
      var classInstanse = item.Parent as IItemClass;
      var className = classInstanse.Name;
      var targetIndex = 1;
      var targetItems = classInstanse.Members.Where(member => member is IItemMethod && member.Name == BaseMethodName)
                        .Select(member => (index: targetIndex++, item: member as IItemMethod)).ToList();
      var isOverload = targetItems.Count() > 1;
      var returnType = ExpressionsToString(item.MethodTypes);
      var isReturn = returnType != "void";

      // 複数コンストラクタの存在確認
      var indexValue = string.Empty;
      if (isOverload)
      {
        // インデックスの設定
        foreach (var targetItem in targetItems)
        {
          if (targetItem.item == item)
          {
            indexValue = $"{targetItem.index}";
          }
        }
      }

      // コメント
      result.Append(GetTypeScriptComments(item, indentSpace));

      // パラメータ取得
      var args = new List<string>();
      foreach (var arg in item.Args)
      {
        args.Add($"{arg.name}: {ExpressionsToString(arg.expressions)}");
      }

      // 定義
      var scope = GetScope(item);
      if (isOverload)
      {
        scope = "private ";
      }
      if (item.Modifiers.Contains("static"))
      {
        scope += "static ";
      }

      result.Append($"{indentSpace}{scope}{BaseMethodName}{indexValue}");
      // ジェネリックスクラス
      if (item.GenericTypes.Any())
      {
        result.Append("<");
        result.Append(string.Join(", ", item.GenericTypes.Select(typeItem => GetTypeScriptType(typeItem))));
        result.Append(">");
      }
      result.Append("(");
      result.Append(string.Join(", ", args));
      result.Append($"): {returnType}");
      result.AppendLine(" {");

      // メンバー追加
      foreach (var member in item.Members)
      {
        result.Append(ConvertUtility.Convert(member, indent + 1, otherScripts));
      }

      result.AppendLine($"{indentSpace}}}");

      // 複数メソッドで最終メソッドの場合は総括メソッドを作成する
      if (isOverload && targetItems.Last().item == item)
      {
        // 総合メソッド内処理用文字列
        var methodResult = new StringBuilder();

        // パラメータ数のリスト取得
        var paramCounts = targetItems.Select(member => member.item.Args.Count);
        var existsNoneParam = paramCounts.Min() <= 0;
        var maxParamCount = paramCounts.Max();

        // パラメータリストを生成
        var methodArgs = new Dictionary<int, List<string>>();
        for (var index = 1; index <= maxParamCount; index++)
        {
          methodArgs.Add(index, new List<string>());
        }

        // メソッドパラメータの格納とメソッド内処理の作成
        foreach (var targetItem in targetItems)
        {
          // 1メソッドのパラメータ取得
          var argIndex = 1;
          var ifConditions = new List<string>();
          var callMethodParams = new List<string>();
          foreach (var arg in targetItem.item.Args)
          {
            var expressionsValue = ExpressionsToString(arg.expressions);

            // メソッドパラメータに追加
            if (!methodArgs[argIndex].Contains(expressionsValue))
            {
              methodArgs[argIndex].Add(expressionsValue);
            }

            // メソッド内条件
            ifConditions.Add(GetCondition(argIndex, expressionsValue));

            // 呼び出しメソッドのパラメータ
            callMethodParams.Add($"param{argIndex}");

            argIndex++;
          }
          // メソッド内条件の追加
          while (argIndex <= maxParamCount)
          {
            ifConditions.Add(GetCondition(argIndex, "undefined"));
            argIndex++;
          }

          // メソッド内処理の追加
          var methodSpace = GetIndentSpace(indent + 1);
          methodResult.AppendLine($"{methodSpace}if ({string.Join(" && ", ifConditions)}) {{");
          methodResult.Append($"{GetIndentSpace(indent + 2)}this.{BaseMethodName}{targetItem.index}");
          methodResult.Append("(");
          methodResult.Append(string.Join(", ", callMethodParams));
          methodResult.AppendLine(");");
          methodResult.AppendLine($"{GetIndentSpace(indent + 2)}return;");
          methodResult.AppendLine($"{methodSpace}}}");
        }

        // 総合メソッドの作成
        result.Append($"{indentSpace}{GetScope(item)}{BaseMethodName}(");
        for (var index = 1; index <= maxParamCount; index++)
        {
          if (index > 1)
          {
            result.Append(", ");
          }

          result.Append($"param{index}");
          if (existsNoneParam)
          {
            result.Append("?");
          }
          result.Append($": {string.Join(" | ", methodArgs[index])}");
        }
        result.Append($"): {returnType}");
        result.AppendLine(" {");
        if(isReturn){
          result.Append("return ");
        }
        result.Append(methodResult.ToString());
        result.AppendLine($"{indentSpace}}}");
      }

      return result.ToString();
    }
  }
}
