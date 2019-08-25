using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("式のテスト", nameof(StatementExpressionTest))]
  public class StatementExpressionTest : TestBase
  {
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private int field;
        public void Method()
        {
          this.field = 10*2+1;
          this.field = this.field/2;
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: number;");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    this.field = 10 * 2 + 1;");
      expectedTS.AppendLine("    this.field = this.field / 2;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "LineCommentTest")]
    public void LineCommentTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private int field;
        public void Method()
        {
          // 1行コメント
          this.field = 10;
          this.field--;
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: number;");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    // 1行コメント");
      expectedTS.AppendLine("    this.field = 10;");
      expectedTS.AppendLine("    this.field--;");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
