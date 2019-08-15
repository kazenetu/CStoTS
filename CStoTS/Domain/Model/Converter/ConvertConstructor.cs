using CSharpAnalyze.Domain.PublicInterfaces;
using CSharpAnalyze.Domain.PublicInterfaces.AnalyzeItems;
using CStoTS.Domain.Model.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStoTS.Domain.Model.Converter
{
  /// <summary>
  /// TS変換クラス：コンストラクタ
  /// </summary>
  internal class ConvertConstructor : AbstractConverter, IConvertable
  {
    /// <summary>
    /// メソッド名
    /// </summary>
    private const string BaseMethodName = "constructor";

    /// <summary>
    /// エントリメソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    public string Convert(IAnalyzeItem item, int indent)
    {
      return Convert(item as IItemConstructor, indent);
    }

    /// <summary>
    /// 変換メソッド
    /// </summary>
    /// <param name="item">C#解析結果</param>
    /// <param name="indent">インデント数</param>
    /// <returns>TypeScript変換結果</returns>
    private string Convert(IItemConstructor item, int indent)
    {
      var result = new StringBuilder();
      var indentSpace = GetIndentSpace(indent);

      // クラス情報を取得
      var classInstanse = item.Parent as IItemClass;
      var className = classInstanse.Name;
      var targetIndex = 1;
      var targetItems = classInstanse.Members.Where(member => member is IItemConstructor)
                        .Select(member => (index: targetIndex++, item: member as IItemConstructor)).ToList();
      var existSuper = classInstanse.SuperClass.Any();
      var isOverload = targetItems.Count() > 1;

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
      var scope = string.Empty;
      if(isOverload){
        scope = "private ";
      }
      result.Append($"{indentSpace}{scope}{BaseMethodName}{indexValue}");
      result.Append("(");
      result.Append(string.Join(", ", args));
      result.Append(")");
      result.AppendLine(" {");

      // メンバー追加
      if (existSuper) {
        // スーパークラスありの場合はコンストラクタを呼び出す
        result.Append($"{GetIndentSpace(indent + 1)}base(");
        result.Append(string.Join(", ", item.BaseArgs));
        result.AppendLine(");");
      }
      foreach (var member in item.Members)
      {
        result.Append(ConvertUtility.Convert(member, indent + 1));
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
        result.Append($"{indentSpace}{BaseMethodName}(");
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
        result.AppendLine(") {");
        result.Append(methodResult.ToString());
        result.AppendLine($"{indentSpace}}}");
      }

      return result.ToString();
    }

    /// <summary>
    /// 条件用文字列取得
    /// </summary>
    /// <param name="paramIndex">パラメータ番号(1～)</param>
    /// <param name="targetType">TypeScriptの型</param>
    /// <returns>条件用文字列</returns>
    protected string GetCondition(int paramIndex, string targetType) {
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
