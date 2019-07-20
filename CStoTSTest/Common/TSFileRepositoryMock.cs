using CStoTS.Infrastructure;
using System;

namespace CStoTSTest.Common
{
  internal class TSFileRepositoryMock: ITSFileRepository
  {
    /// <summary>
    /// TypeScriptの出力
    /// </summary>
    /// <param name="filePath">出力パス</param>
    /// <param name="tsData">変換後のTypeScript文字列</param>
    public void WriteFile(string filePath, string tsData)
    {
      Console.WriteLine($"------{filePath}------");
      Console.Write(tsData);
    }
  }
}
