using CStoTS;
using CStoTS.ApplicationService;
using CStoTSTest.Common;
using Xunit;

namespace CStoTSTest
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      // 一旦実装

      // C#ソース作成
      var testBase = new TestBase();
      testBase.CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }", null);

      // 変換・出力
      var csToTs = new ConvertApplication();
      csToTs.Convert(string.Empty, string.Empty, new TSFileRepositoryMock(), testBase.Files);
    }
  }
}
