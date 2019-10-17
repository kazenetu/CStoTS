using CStoTS;
using System.Text;
using Xunit;

namespace CStoTSTest
{
  [Trait("反復処理(do while)のテスト", nameof(DoWhileTest))]
  public class DoWhileTest : TestBase
  {
    [Fact(DisplayName = "StandardTest")]
    public void StandardTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var i = 0;
          do
          {
            i++;
          }while(i<10);
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let i: number = 0;");
      expectedTS.AppendLine("    do {");
      expectedTS.AppendLine("      i++;");
      expectedTS.AppendLine("    } while (i < 10);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "AdditionTest")]
    public void AdditionTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var i = 0;
          do
          {
          }while(i++<10);
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let i: number = 0;");
      expectedTS.AppendLine("    do {");
      expectedTS.AppendLine("    } while (i++ < 10);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "GenericsTest")]
    public void GenericsTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          var list = new List<string>();
          list.Add(""A"");
          list.Add(""B"");
          list.Add(""C"");
          var i = 0;
          do
          {
            i+=1;
          }while(i<list.Count);
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定(List.tsは別途出力されるArrayラッパークラス)
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("import { List } from './List';");
      expectedTS.AppendLine("");
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let list: List<string> = new List<string>();");
      expectedTS.AppendLine("    list.Add(\"A\");");
      expectedTS.AppendLine("    list.Add(\"B\");");
      expectedTS.AppendLine("    list.Add(\"C\");");
      expectedTS.AppendLine("    let i: number = 0;");
      expectedTS.AppendLine("    do {");
      expectedTS.AppendLine("      i += 1;");
      expectedTS.AppendLine("    } while (i < list.Count);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "ArrayTest")]
    public void ArrayTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        public void Method(){
          int[] list = { 1, 2, 3 };
          var i = 0;
          do
          {
            i++;
          }while (i < list.Length);
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let list: number[] = [1, 2, 3];");
      expectedTS.AppendLine("    let i: number = 0;");
      expectedTS.AppendLine("    do {");
      expectedTS.AppendLine("      i++;");
      expectedTS.AppendLine("    } while (i < list.length);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "InstanceFieldTest")]
    public void InstancePropertyTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private int field = 10;
        public void Method(){
          var i = 0;
          do
          {
            i++;
          }while (i < field);
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private field: number = 10;");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let i: number = 0;");
      expectedTS.AppendLine("    do {");
      expectedTS.AppendLine("      i++;");
      expectedTS.AppendLine("    } while (i < this.field);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

    [Fact(DisplayName = "StaticFieldTest")]
    public void StaticFieldTest()
    {
      // C#ソース作成
      CreateFileData("test.cs", string.Empty,
      @"public class Test
      {
        private static int field = 10;
        public void Method(){
          var i = 0;
          do
          {
            i++;
          }while (i < field);
        }
      }");

      // 変換
      ConvertTS();

      // 変換確認
      var actualTS = GetTypeScript("test.ts");
      Assert.NotNull(actualTS);

      // 変換後の期待値設定
      var expectedTS = new StringBuilder();
      expectedTS.AppendLine("export class Test {");
      expectedTS.AppendLine("  private static field: number = 10;");
      expectedTS.AppendLine("  public Method(): void {");
      expectedTS.AppendLine("    let i: number = 0;");
      expectedTS.AppendLine("    do {");
      expectedTS.AppendLine("      i++;");
      expectedTS.AppendLine("    } while (i < Test.field);");
      expectedTS.AppendLine("  }");
      expectedTS.AppendLine("}");

      Assert.Equal(expectedTS.ToString(), actualTS);
    }

  }
}
