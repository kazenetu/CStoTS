using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("反復処理(for)のテスト", nameof(ForTest))]
  public class ForTest : TestBase
  {
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // TODO C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // TODO 変換後の期待値設定
      var expectedTS = new StringBuilder();
      //expectedTS.AppendLine("export class Test {");
      //expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
