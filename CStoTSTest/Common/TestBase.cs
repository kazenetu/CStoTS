using CSharpAnalyze.Domain.PublicInterfaces.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CStoTS
{
  class TestBase
  {
    /// <summary>
    /// テスト用FileRepository
    /// </summary>
    public CSFileRepositoryMock Files { get; } = new CSFileRepositoryMock();

    /// <summary>
    /// ソースコード
    /// </summary>
    private string BaseSource = string.Empty;

    /// <summary>
    /// Setup
    /// </summary>
    public TestBase()
    {
      // 基本ソースの組み立て
      var baseSource = new StringBuilder();
      baseSource.AppendLine("using System;");
      baseSource.AppendLine("using System.Collections.Generic;");
      baseSource.AppendLine("{0}");
      baseSource.AppendLine("namespace CSharpAnalyzeTest{{");
      baseSource.AppendLine("{1}");
      baseSource.AppendLine("}}");
      BaseSource = baseSource.ToString();
    }

    #region テスト情報作成メソッド

    /// <summary>
    /// テスト情報作成
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <param name="addUsing">追加Using</param>
    /// <param name="sourceCode">ソースコード</param>
    /// <param name="delegateMethod">イベント処理</param>
    public void CreateFileData(string fileName, string addUsing, string sourceCode, Action<IAnalyzed> delegateMethod)
    {
      var source = string.Format(CultureInfo.CurrentCulture, BaseSource, addUsing, sourceCode, delegateMethod);
      Files.Add(fileName, source, delegateMethod);
    }

    #endregion
  }
}
