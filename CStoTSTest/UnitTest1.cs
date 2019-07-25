using CStoTS;
using CStoTS.ApplicationService;
using CStoTSTest.Common;
using System.Linq;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      // C#ソース作成
      var testBase = new TestBase();
      testBase.CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }");

      // 変換
      var csToTs = new ConvertApplication();
      var tsFiles = new TSFileRepositoryMock();
      csToTs.Convert(string.Empty, string.Empty, tsFiles, testBase.Files);

      // 変換確認
      var actual = tsFiles.Scripts.First();
      Assert.Equal("test.ts", actual.filePath);

      var expectedScript = new StringBuilder();
      expectedScript.AppendLine("class Test{");
      expectedScript.AppendLine("}");

      Assert.Equal(expectedScript.ToString(), actual.typeScripts);
    }
  }
}
