﻿using CStoTS.ApplicationService;
using CStoTS.Domain.Model.Mode;
using CStoTSTest.Common;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CStoTS
{
  public class TestBase
  {
    /// <summary>
    /// C#入力用FileRepository
    /// </summary>
    private InCSFileRepositoryMock csFiles = new InCSFileRepositoryMock();

    /// <summary>
    /// Typescript出力用FileRepository
    /// </summary>
    private TSFileRepositoryMock tsFiles = new TSFileRepositoryMock();

    /// <summary>
    /// C#ソースコードの書式用文字列
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

    /// <summary>
    /// C#入力ソースコード作成
    /// </summary>
    /// <param name="fileName">ファイル名</param>
    /// <param name="addUsing">追加Using</param>
    /// <param name="sourceCode">ソースコード</param>
    public void CreateFileData(string fileName, string addUsing, string sourceCode)
    {
      var source = string.Format(CultureInfo.CurrentCulture, BaseSource, addUsing, sourceCode);
      csFiles.Add(fileName, source);
    }

    /// <summary>
    /// TypeScript変換
    /// </summary>
    /// <param name="withoutMethod">メソッド除外を追加するか否か</param>
    public void ConvertTS(bool withoutMethod = false)
    {
      var mode = OutputMode.Mode.All;
      if(withoutMethod){
        // メソッド除外
        mode = OutputMode.Mode.WithoutMethod;
      }

      var csToTs = new ConvertApplication();
      csToTs.Convert(Config.Create(mode, string.Empty, string.Empty), tsFiles, csFiles);
    }

    /// <summary>
    /// 変換後のTypescriptを取得
    /// </summary>
    /// <param name="filePath">ファイルの相対パス</param>
    /// <returns>Typescriptまたはnull</returns>
    public string GetTypeScript(string filePath)
    {
      var result = tsFiles.Scripts.Where(item => item.filePath == filePath);
      if (!result.Any())
      {
        return null;
      }
      return result.First().typeScripts;
    }
  }
}
